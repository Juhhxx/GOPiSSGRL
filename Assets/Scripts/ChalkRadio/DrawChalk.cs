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

        // Rotate Chalk to be facing up from te perspective of the surface where it's drawn
        chalkTrans.LookAt(-hit.normal);
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

        Debug.Log($"chalk position is ({chalkTrans.position} + {chalkTrans.up * 0.01f} =){chalkPos}");
    }
    private Texture ChooseChalkTexture()
    {
        int textureIdx = Random.Range(0,_chalkTextures.Count);
        return _chalkTextures[textureIdx];
    }
}
