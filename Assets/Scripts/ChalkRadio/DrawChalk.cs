using UnityEngine;
using System.Collections.Generic;

public class DrawChalk : MonoBehaviour
{
    [SerializeField] private GameObject _chalkPrefab;
    [SerializeField] private LayerMask _drawingLayer;
    [SerializeField] private float _drawingDistance;
    [SerializeField] private List<Texture> _chalkTextures;
    private Transform _cameraTrans;

    void Start()
    {
        _cameraTrans = GetComponentInParent<Camera>().gameObject.transform;
    }
    void Update()
    {
        CheckForDrawingSpot();
    }
    private void CheckForDrawingSpot()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTrans.position,_cameraTrans.forward,out hit,_drawingDistance, _drawingLayer))
            if (Input.GetButtonDown("Interact"))
                DrawChalkSpot(hit);
    }
    private void DrawChalkSpot(RaycastHit hit)
    {
        GameObject newChalk = Instantiate(_chalkPrefab);
        Transform chalkTrans = newChalk.transform;
        MeshRenderer chalkMeshR = newChalk.GetComponent<MeshRenderer>();

        chalkTrans.LookAt(-hit.normal);
        Vector3 chalkPos = chalkTrans.forward;
        chalkPos *= -0.05f;
        chalkPos += hit.point;
        chalkTrans.position = chalkPos;

        chalkMeshR.material.mainTexture = ChooseChalkTexture();

        Debug.Log($"chalk position is ({chalkTrans.position} + {chalkTrans.up * 0.05f} =){chalkPos}");
    }
    private Texture ChooseChalkTexture()
    {
        int textureIdx = Random.Range(0,_chalkTextures.Count);
        return _chalkTextures[textureIdx];
    }
}
