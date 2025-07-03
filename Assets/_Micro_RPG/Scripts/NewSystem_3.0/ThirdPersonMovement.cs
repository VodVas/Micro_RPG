////public class PlayerController1 : MonoBehaviour
////{
////    [SerializeField] private float _gravityMultiplier = 2f;
////    [SerializeField] private float _moveSpeed = 5f;
////    [SerializeField] private Rigidbody _rb;

////    private string _horizontal = "Horizontal";
////    private string _vertical = "Vertical";

////    private void FixedUpdate()
////    {
////        HandleMovement();
////        ApplyGravity();
////    }

////    private void HandleMovement()
////    {
////        Vector2 input = new Vector2(Input.GetAxisRaw(_horizontal), Input.GetAxisRaw(_vertical));
////        Vector3 targetVelocity = (transform.forward * input.y + transform.right * input.x).normalized * _moveSpeed;

////        _rb.velocity = new Vector3(targetVelocity.x, _rb.velocity.y, targetVelocity.z);
////    }

////    private void ApplyGravity()
////    {
////        if (!_rb.useGravity) return;

////        _rb.velocity += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
////    }
////}

////public class PlayerMovement : MonoBehaviour
////{
////    [SerializeField] private float _moveSpeed = 5f;
////    private const float _gravity = -9.81f;
////    private float _verticalVelocity;
////    private CharacterController _characterController;

////    private void Awake() => _characterController = GetComponent<CharacterController>();

////    private void Update()
////    {
////        Vector3 moveDirection = GetInputDirection();
////        ApplyGravity(ref moveDirection);
////        Move(moveDirection);
////    }

////    private void OnValidate() => _moveSpeed = Mathf.Max(0, _moveSpeed);

////    private Vector3 GetInputDirection()
////    {
////        float horizontal = Input.GetAxisRaw("Horizontal");
////        float vertical = Input.GetAxisRaw("Vertical");
////        return (transform.forward * vertical + transform.right * horizontal).normalized;
////    }

////    private void ApplyGravity(ref Vector3 direction)
////    {
////        _verticalVelocity = _characterController.isGrounded ? 0 : _verticalVelocity + _gravity * Time.deltaTime;
////        direction.y = _verticalVelocity;
////    }

////    private void Move(Vector3 direction) => _characterController.Move(direction * _moveSpeed * Time.deltaTime);
////}

////public class CharacterAnimation : MonoBehaviour
////{
////    [SerializeField] private Animator _animator;

////    private void Update()
////    {
////        bool forwardPressed = Input.GetKey(KeyCode.W);
////        bool runPressed = Input.GetKey(KeyCode.LeftShift);

////        _animator.SetBool("isWalking", forwardPressed);
////        _animator.SetBool("isRunning", forwardPressed && runPressed);
////    }
////}


////public class CharacterAnimation1 : MonoBehaviour
////{
////    [SerializeField] private Animator _animator;

////    private int _isWalking => Animator.StringToHash("isWalking");
////    private int _isRunning => Animator.StringToHash("isRunning");

////    private void Update()
////    {
////        bool forwardPressed = Input.GetKey(KeyCode.W);
////        bool runPressed = Input.GetKey(KeyCode.LeftShift);

////        _animator.SetBool(_isWalking, forwardPressed);
////        _animator.SetBool(_isRunning, forwardPressed && runPressed);
////    }
////}


//using UnityEngine;

//public class ThirdPersonMovement : MonoBehaviour
//{
//    [SerializeField] private float _speed = 5f;
//    [SerializeField] private float _runSpeed = 10f;
//    [SerializeField] private KeyCode _runningKey = KeyCode.LeftShift;
//    [SerializeField] private bool _canRun = true;
//    [SerializeField] private float _rotationSpeed = 100f;
//    [SerializeField] private Rigidbody _rigidbody;
//    [SerializeField] private Transform _visualRoot;
//    [SerializeField] private bool _FPSControlType = false;

//    private bool _isRunning;
//    private float _targetMovingSpeed;
//    private float _horizontalInput;
//    private float _verticalInput;

//    private void FixedUpdate()
//    {
//        GetPlayerInput();
//        UpdateRunningState();

//        if (_FPSControlType)
//        {
//            UpdateCharacterVelocityShooterMov();
//        }
//        else
//        {
//            UpdateCharacterVelocity();
//            UpdateCharacterRotation();
//        }

//    }

//    private void UpdateRunningState()
//    {
//        _isRunning = _canRun && Input.GetKey(_runningKey);

//        if (_isRunning)
//        {
//            _targetMovingSpeed = _runSpeed;
//        }
//        else
//        {
//            _targetMovingSpeed = _speed;
//        }
//    }

//    private void UpdateCharacterVelocity()
//    {
//        Vector3 movementDirection = transform.rotation * new Vector3(_horizontalInput, 0f, _verticalInput * _targetMovingSpeed);

//        _rigidbody.velocity = movementDirection;
//    }

//    private void GetPlayerInput()
//    {
//        _horizontalInput = Input.GetAxis("Horizontal");
//        _verticalInput = Input.GetAxis("Vertical");
//    }


//    private void UpdateCharacterVelocityShooterMov()
//    {
//        // Движение вперед/назад и вбок (в локальных координатах)
//        Vector3 localMovement = new Vector3(_horizontalInput * _targetMovingSpeed, 0f, _verticalInput * _targetMovingSpeed);

//        // Преобразуем локальное направление в мировое
//        Vector3 worldMovement = transform.rotation * localMovement;

//        _rigidbody.velocity = worldMovement;
//    }

//    private void UpdateCharacterRotation()
//    {
//        _visualRoot.Rotate(Vector3.up * _horizontalInput * _rotationSpeed * Time.deltaTime, Space.Self);
//    }
//}