using UnityEngine;

public class InitUVLight : MonoBehaviour
{
    void Start()
    {
        TAG_Necro necro = FindAnyObjectByType<TAG_Necro>();
        if (necro != null)
            necro.GetComponent<Collider>().enabled = false;

        Shader.SetGlobalFloat("_Lighted", 0);
    }
}
