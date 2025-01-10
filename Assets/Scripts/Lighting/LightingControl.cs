using UnityEditor;
using UnityEngine;

public class LightingControl : MonoBehaviour
{
    public void ChangeLighting(LightingPresets preset)
    {
        Lightmapping.lightingSettings = preset.LightingSettings;
    }
}