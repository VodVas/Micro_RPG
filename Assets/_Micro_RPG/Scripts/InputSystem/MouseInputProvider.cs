using UnityEngine;
using UnityEngine.InputSystem;
using System;

public sealed class MouseInputProvider : InputProviderBase, IGroundClickHandler
{
    private readonly Camera _camera;
    private readonly LayerMask _clickableLayers;
    private readonly LayerMask _groundLayer;
    private readonly float _stoppingDistance;
    private readonly float _rotationSpeed;

    private Vector3? _targetPosition;
    private IClickable _currentTarget;
    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];

    private readonly Mouse _mouse;
    private readonly PlayerInputActions _inputActions;

    public MouseInputProvider(
        PlayerInputActions inputActions,
        Camera camera,
        LayerMask clickableLayers,
        LayerMask groundLayer,
        float stoppingDistance = 0.1f,
        float rotationSpeed = 720f)
    {
        _inputActions = inputActions ?? throw new ArgumentNullException(nameof(inputActions));
        _camera = camera ?? throw new ArgumentNullException(nameof(camera));
        _clickableLayers = clickableLayers;
        _groundLayer = groundLayer;
        _stoppingDistance = stoppingDistance;
        _rotationSpeed = rotationSpeed;

        _mouse = Mouse.current;
        _inputActions.Enable();
    }

    protected override void UpdateInput()
    {
        HandleMouseClick();
        UpdateMovement();
    }

    private void HandleMouseClick()
    {
        if (_mouse == null)
        {
            Debug.Log("Mouse is NULL!");
            return;
        }

        Debug.Log($"Mouse position: {_mouse.position.ReadValue()}");

        if (!_mouse.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse button NOT pressed");
            return;
        }

        Debug.Log("HandleMouseClick");
        var mousePosition = _mouse.position.ReadValue();
        var ray = _camera.ScreenPointToRay(mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        // Check for clickable objects first
        int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, 100f, _clickableLayers);

        if (hitCount > 0)
        {
            // Sort by distance and priority
            Array.Sort(_raycastHits, 0, hitCount, new RaycastHitComparer());

            for (int i = 0; i < hitCount; i++)
            {
                var clickable = _raycastHits[i].collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    _currentTarget = clickable;
                    _targetPosition = _raycastHits[i].point;
                    clickable.OnClick(_raycastHits[i].point);

                    // If it's attackable, trigger attack
                    if (clickable is IAttackable attackable && attackable.IsAlive)
                    {
                        SetAttack(true);
                    }
                    return;
                }
            }
        }

        // Check for ground click
        if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, _groundLayer))
        {
            OnGroundClick(groundHit.point);
        }
    }

    public void OnGroundClick(Vector3 worldPosition)
    {
        _targetPosition = worldPosition;
        _currentTarget = null;
        SetAttack(false);
    }

    private void UpdateMovement()
    {
        if (!_targetPosition.HasValue)
        {
            SetMovement(Vector2.zero);
            return;
        }

        var playerPosition = GetPlayerPosition();
        var directionToTarget = _targetPosition.Value - playerPosition;
        directionToTarget.y = 0; // Ignore vertical difference

        var distance = directionToTarget.magnitude;

        if (distance <= _stoppingDistance)
        {
            SetMovement(Vector2.zero);

            // Clear target if it's just ground movement
            if (_currentTarget == null)
            {
                _targetPosition = null;
            }
            return;
        }

        // Convert world direction to camera-relative direction for isometric view
        var cameraForward = _camera.transform.forward;
        var cameraRight = _camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        var direction = directionToTarget.normalized;
        var movement = new Vector2(
            Vector3.Dot(direction, cameraRight),
            Vector3.Dot(direction, cameraForward)
        );

        SetMovement(movement.normalized);
    }

    private Vector3 GetPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player"); //TODO: убрать
        return player != null ? player.transform.position : Vector3.zero;
    }

    public override void Dispose()
    {
        _inputActions?.Dispose();
    }

    private class RaycastHitComparer : System.Collections.Generic.IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            int distanceComparison = x.distance.CompareTo(y.distance);

            if (distanceComparison != 0) return distanceComparison;

            var xClickable = x.collider?.GetComponent<IClickable>();
            var yClickable = y.collider?.GetComponent<IClickable>();

            if (xClickable != null && yClickable != null)
            {
                return yClickable.Priority.CompareTo(xClickable.Priority);
            }

            return 0;
        }
    }
}
