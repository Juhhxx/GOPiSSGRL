using UnityEngine;

public class InitUVLight : MonoBehaviour
{
    void Start() => Shader.SetGlobalFloat("_Lighted", 0);
}
