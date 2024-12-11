using UnityEngine;

public class ChalkDrawingPoint : MonoBehaviour
{
    [SerializeField] float _pointFrequency;
    public float PointFrequency => _pointFrequency;
    [SerializeField] float _maxDetectDistance = 0.4f;
    [SerializeField] private Material _shiningMaterial;
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
        if (drawn.GetComponent<TAG_Chalk>() != null && !_isDrawn)
        {
            Debug.Log("CHALK WAS DRAWN IN THE RIGHT SPOT");
            _isDrawn = true;
            ChangeChalkMaterial(chalkObject);
        }
    }
    private void ChangeChalkMaterial(Collider other)
    {
        Renderer renderer = other.gameObject.GetComponent<Renderer>();
        Texture texture = renderer.material.mainTexture;
        renderer.material = _shiningMaterial;
        renderer.material.mainTexture = texture;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIfChalkDrawn(other.gameObject, other);

    }
}
