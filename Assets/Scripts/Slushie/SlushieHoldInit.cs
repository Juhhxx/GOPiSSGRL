using UnityEngine;

public class SlushieHoldInit : MonoBehaviour
{
    
    private Material _material;

    void Start()
    {
        GameObject slushieCup = FindAnyObjectByType<SlushieCup>().gameObject;

        _material = GetComponent<MeshRenderer>().material;

        _material = slushieCup.GetComponent<MeshRenderer>().material;
    }
}
