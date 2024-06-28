using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Boundary : MonoBehaviour, IBoundary
{
    [SerializeField] private BoundaryDirection direction;
    private Collider2D _collider;
    private Camera _mainCam;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        _mainCam = Camera.main;
    }

    private void Start()
    {
        _collider.isTrigger = true;

        float offset = (int)direction * _mainCam.orthographicSize;
        transform.position = _mainCam.transform.position + offset * Vector3.right;
    }
}
