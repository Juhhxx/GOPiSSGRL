using UnityEngine;

public class ChalkDrawingPoint : MonoBehaviour
{
    [SerializeField] float _pointFrequency;
    public float PointFrequency => _pointFrequency;
    [SerializeField] float _maxDetectDistance = 0.4f;
    private SphereCollider _collider;
    private bool _isDrawn = false;
    public bool IsDrawn => _isDrawn;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = _maxDetectDistance;
        if (_isDrawn)
        {

        }
    }
    private void CheckIfChalkDrawn(GameObject drawn, Collider chalkObject)
    {
        ChalkController chalk = drawn.GetComponent<ChalkController>();

        if (chalk != null && !_isDrawn)
        {
            Debug.Log("CHALK WAS DRAWN IN THE RIGHT SPOT");
            _isDrawn = true;
            chalk.ChangeChalkMaterial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIfChalkDrawn(other.gameObject, other);
    }
}
