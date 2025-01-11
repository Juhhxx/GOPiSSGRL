using UnityEngine;
using System.Collections;

public class ChalkController : MonoBehaviour
{
    [SerializeField] private Material _shiningMaterial;
    [SerializeField] private float    _disappearSpeed;

    private ChalkPool        _chalkPool;
    private Renderer         _renderer;
    private YieldInstruction _wfs;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _wfs = new WaitForSeconds(2.0f);
        StartCoroutine(Disapear());
    }
    private IEnumerator Disapear()
    {
        yield return _wfs;

        Material  material = _renderer.material;
        Color    initColor = material.color;
        Color    lastColor = initColor;
        Color     newColor = initColor; 
        float            i = 0;

        lastColor.a = 0.0f;

        while(true)
        {
            newColor = Color.Lerp(initColor,lastColor,i);

            material.color = newColor;

            i += _disappearSpeed * Time.deltaTime;
            
            if (newColor.a <= 0)
            {
                _chalkPool.RemoveChalk(gameObject);
            }
        }
    }
    public void SetPool(ChalkPool pool)
    {
        _chalkPool = pool;
    }
    public void ChangeChalkMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        Texture texture = renderer.material.mainTexture;
        renderer.material = _shiningMaterial;
        renderer.material.mainTexture = texture;
        StopCoroutine(Disapear());
    }
}
