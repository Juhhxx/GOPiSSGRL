using UnityEngine;
using System.Collections.Generic;

public class DrawChalk : MonoBehaviour
{
    [SerializeField] private GameObject _chalkPrefab;
    [SerializeField] private LayerMask _drawingLayer;
    [SerializeField] private float _drawingDistance;

    private ChalkPool _chalkPool;
    private Transform _cameraTrans;

    private void Start()
    {
        _cameraTrans = GetComponentInParent<Camera>().gameObject.transform;
        _chalkPool   = FindFirstObjectByType<ChalkPool>();
    }
    private void Update()
    {
        CheckForDrawingSpot();
    }
    private void CheckForDrawingSpot()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTrans.position,_cameraTrans.forward,out hit,_drawingDistance, _drawingLayer))
            if (Input.GetButtonDown("Interact"))
                _chalkPool.SpawnChalk(hit);
    }
}
