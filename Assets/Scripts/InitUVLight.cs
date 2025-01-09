using UnityEngine;

public class InitUVLight : MonoBehaviour
{
    void Start()
    {
        TagNecro necro = FindAnyObjectByType<TagNecro>();
        if (necro != null)
            necro.GetComponent<Collider>().enabled = false;

        Shader.SetGlobalFloat("_Lighted", 0);
    }
}
