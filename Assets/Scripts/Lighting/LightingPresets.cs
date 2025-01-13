using UnityEngine;

[CreateAssetMenu(fileName = "LightingPresets", menuName = "Scriptable Objects/LightingPresets")]
public class LightingPresets : ScriptableObject
{
    public Material SkyboxMaterial;
    
    public bool FogEnabled;
    public FogMode FogMode;
    public Color FogColor;
    public float FogDensity;
}
