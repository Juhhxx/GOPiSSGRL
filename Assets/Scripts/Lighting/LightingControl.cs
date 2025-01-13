using UnityEditor;
using UnityEngine;

public class LightingControl : MonoBehaviour
{
    public void ChangeLighting(LightingPresets preset)
    {
        if (preset.LightingSettings != null)
        {
            // Lightmapping.lightingSettings = preset.LightingSettings;
            // Lightmappping só pode ser usado no Editor, nas builds dá erro
        }

        if (preset.SkyboxMaterial != null)
        {
            RenderSettings.skybox = preset.SkyboxMaterial;
        }

        if (preset.FogEnabled)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = preset.FogMode;
            RenderSettings.fogColor = preset.FogColor;
            RenderSettings.fogDensity = preset.FogDensity;
        }
        else
        {
            RenderSettings.fog = false;
        }
    }
}