using System.Collections.Generic;
using UnityEngine;

public class ChalkPool : MonoBehaviour
{
    [SerializeField] private GameObject    _objectPrefab;
    [SerializeField] private List<Texture> _chalkTextures;
    [SerializeField] private int           _intialPool;
    private Stack<GameObject>              _pool;

    private void Start()
    {
        _pool = new Stack<GameObject>();

        if (_intialPool > 0)
        {
            for (int i = 0; i < _intialPool; i++)
            {
                GameObject newChalk = CreateObject();
                newChalk.SetActive(false);
            }
        }
    }
    private GameObject CreateObject()
    {
        GameObject newObject = Instantiate(_objectPrefab);
        newObject.GetComponent<ChalkController>().SetPool(this);
        _pool.Push(newObject);
        return newObject;
    }

    public GameObject SpawnChalk(RaycastHit hit)
    {
        if (_pool.Count == 0)
            return CreateObject();

        GameObject chalk = _pool.Pop();

        Transform chalkTrans = chalk.transform;
        Renderer  chalkMeshR = chalk.GetComponent<Renderer>();

        // Rotate Chalk to be facing up from te perspective of the surface where it's drawn
        // chalkTrans.LookAt(-hit.normal);
        // Debug.Log($"Chalk looking at ({-hit.normal})");
        // Position the Chalk to be 0.05 unity units above the surface where it's drawn
        Vector3 chalkPos = chalkTrans.forward;
        chalkPos *= -0.01f;
        chalkPos += hit.point;
        chalkTrans.position = chalkPos;
        // Give the Chalk a random rotation in the Y axis
        Vector3 chalkRotation = chalkTrans.localEulerAngles;
        chalkRotation.y = Random.Range(0f,360f);
        chalkTrans.rotation = Quaternion.Euler(chalkRotation);

        chalkMeshR.material.mainTexture = ChooseChalkTexture();
        Color resetColor = chalkMeshR.material.color;
        resetColor.a = 1;
        chalkMeshR.material.color = resetColor;

        Debug.Log($"chalk position is ({chalkTrans.position} + {chalkTrans.up * 0.01f} =){chalkPos}");

        chalk.SetActive(true);
        return chalk;
    }
    public void RemoveChalk(GameObject chalk)
    {
        _pool.Push(chalk);
        chalk.SetActive(false);
    }
    private Texture ChooseChalkTexture()
    {
        int textureIdx = Random.Range(0,_chalkTextures.Count);
        return _chalkTextures[textureIdx];
    }

}
