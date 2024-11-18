using UnityEngine;

public class DrawChalk : MonoBehaviour
{
    [SerializeField] private bool _hasChalk;
    [SerializeField] private GameObject _chalkPrefab;
    [SerializeField] private LayerMask _drawingLayer;
    [SerializeField] private float _drawingDistance;
    private Transform _cameraTrans;

    void Start()
    {
        _cameraTrans = GetComponentInChildren<Camera>().gameObject.transform;
    }
    void Update()
    {
        if (_hasChalk)
            CheckForDrawingSpot();
    }
    private void CheckForDrawingSpot()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTrans.position,_cameraTrans.forward,out hit,_drawingDistance, _drawingLayer))
            if (Input.GetKeyDown(KeyCode.E))
                DrawChalkSpot(hit);
    }
    private void DrawChalkSpot(RaycastHit hit)
    {
        GameObject newChalk = Instantiate(_chalkPrefab);
        Transform chalkTrans = newChalk.transform;

        chalkTrans.LookAt(-hit.normal);
        Vector3 chalkPos = chalkTrans.forward;
        chalkPos *= -0.05f;
        chalkPos += hit.point;
        chalkTrans.position = chalkPos;

        Debug.Log($"chalk position is ({chalkTrans.position} + {chalkTrans.up * 0.05f} =){chalkPos}");
    }
}
