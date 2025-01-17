using UnityEngine;

public class InitUVLight : MonoBehaviour
{
    void Start()
    {
        TagNecro necro = FindFirstObjectByType<TagNecro>();
        if (necro != null)
            necro.GetComponent<Collider>().enabled = false;

        UVLightOld initUv = GetComponentInChildren<UVLightOld>();

        if (initUv != null)
            Destroy(initUv.gameObject);
    }
}
