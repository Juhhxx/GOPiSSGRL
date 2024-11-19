using UnityEngine;

public class ChalkDrawingPoint : MonoBehaviour
{
    [SerializeField] float _pointFrequency;
    [SerializeField] float _maxDetectDistance = 0.4f;
    private SphereCollider _collider;
    private Transform _playerTrans;
    private bool _isDrawn;
    public bool IsDrawn => _isDrawn;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _maxDetectDistance;
    }
    private void CheckIfChalkDrawn(GameObject drawn)
    {
        if (drawn.GetComponent<TAG_Chalk>() != null)
        {
            Debug.Log("CHALK WAS DRAWN IN THE RIGHT SPOT");
            _isDrawn = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckIfChalkDrawn(other.gameObject);
    }
}
