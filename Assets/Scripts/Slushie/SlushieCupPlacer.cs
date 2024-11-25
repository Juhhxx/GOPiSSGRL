using UnityEngine;
using UnityEngine.UI;

public class SlushieCupPlacer : MonoBehaviour
{
    private SlushieCup _cup;
    private Interactive _cupInteractive;
    private TakeItem _takeItem;
    private BoxCollider _collider;

    private void Start()
    {
        _cup = FindAnyObjectByType<SlushieCup>();
        _cupInteractive = _cup.GetComponent<Interactive>();
        _takeItem = GetComponent<TakeItem>();
        _collider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        ToggleCollider();
    }
    public void PlaceCup()
    {
        Debug.Log("Placing Cup");
        if (_takeItem.TakeItemFromPlayer(_cupInteractive))
        {
            _cup.gameObject.SetActive(true);
            _cup.transform.position = transform.position + new Vector3(0f,0.15f,0f);
        }
    }
    private void ToggleCollider()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position,transform.up,out hit,0.2f))
        {
            Debug.DrawLine(transform.position,hit.point,Color.red);
            if (hit.collider.gameObject.GetComponent<SlushieCup>() != null)
                _collider.enabled = false;
            else
                _collider.enabled = true;
        }
        else
            _collider.enabled = true;
    }
}
