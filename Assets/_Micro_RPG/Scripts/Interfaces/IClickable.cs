using UnityEngine;

public interface IClickable
{
    void OnClick(Vector3 clickPosition);
    int Priority { get; }
}
