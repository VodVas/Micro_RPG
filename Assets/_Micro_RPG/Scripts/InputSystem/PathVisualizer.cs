using UnityEngine;
using System.Collections.Generic;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _targetMarkerPrefab;
    [SerializeField] private float _markerLifetime = 0.5f;

    private GameObject _currentMarker;
    private readonly Queue<Vector3> _pathPoints = new Queue<Vector3>(50);

    public void ShowTargetPosition(Vector3 position)
    {
        if (_currentMarker != null)
            Destroy(_currentMarker);

        _currentMarker = Instantiate(_targetMarkerPrefab, position, Quaternion.identity);
        Destroy(_currentMarker, _markerLifetime);
    }

    public void UpdatePath(Vector3 currentPosition, Vector3 targetPosition)
    {
        if (_lineRenderer == null) return;

        _pathPoints.Clear();
        _pathPoints.Enqueue(currentPosition);
        _pathPoints.Enqueue(targetPosition);

        _lineRenderer.positionCount = _pathPoints.Count;
        _lineRenderer.SetPositions(_pathPoints.ToArray());
    }

    public void ClearPath()
    {
        if (_lineRenderer != null)
            _lineRenderer.positionCount = 0;

        _pathPoints.Clear();
    }
}