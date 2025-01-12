using UnityEngine;
using System.Collections;

public class ChalkController : MonoBehaviour
{
    [SerializeField] private Material _chalkmaterial;
    [SerializeField] private Material _shiningMaterial;
    [SerializeField] private float    _disappearWait;
    [SerializeField] private float    _disappearSpeed;

    private ChalkPool        _chalkPool;
    private Renderer         _renderer;
    private YieldInstruction _wfs;
    private YieldInstruction _wff;
    
    public bool InSpot = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = _chalkmaterial;
        _wfs = new WaitForSeconds(_disappearWait);
        _wff = new WaitForEndOfFrame();
    }
    private void OnEnable()
    {
        StartCoroutine(Disapear());
    }
    public void SetPool(ChalkPool pool)
    {
        _chalkPool = pool;
    }

    private IEnumerator Disapear()
    {
        yield return _wfs;

        Material  material = _renderer.material;
        Color    initColor = material.color;
        Color    lastColor = initColor;
        Color     newColor = initColor;
        bool      finished = false; 
        float            i = 0;

        lastColor.a = 0.0f;

        while(!finished && !InSpot)
        {
            newColor = Color.Lerp(initColor,lastColor,i);

            material.color = newColor;

            i += _disappearSpeed * Time.deltaTime;
            
            if (newColor.a <= 0)
            {
                finished = true;
                _chalkPool.RemoveChalk(gameObject);
            }

            yield return _wff;
        }
    }
    public void ChangeChalkMaterial()
    {
        InSpot = true;
        StopCoroutine(Disapear());
        Renderer renderer = GetComponent<Renderer>();
        Texture texture = renderer.material.mainTexture;
        renderer.material = _shiningMaterial;
        renderer.material.mainTexture = texture;
    }
}
