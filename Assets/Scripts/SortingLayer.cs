using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    private void Start()
    {
        Renderer[] meshes = GetComponentsInChildren<Renderer>();

        foreach(Renderer ren in meshes)
        {
            ren.sortingOrder = 200;
        }
    }
}
