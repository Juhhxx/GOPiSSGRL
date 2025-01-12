#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Globalization;
using System.Text.RegularExpressions;

public class UniformNamesEditor : EditorWindow
{
    private bool replaceSpacesWithUnderscores = true;

    [MenuItem("Tools/Uniform Names")]
    public static void ShowWindow()
    {
        GetWindow<UniformNamesEditor>("Uniform Names");
    }

    private void OnGUI()
    {
        GUILayout.Label("Uniformize GameObject Names", EditorStyles.boldLabel);

        replaceSpacesWithUnderscores = EditorGUILayout.Toggle("Replace Spaces with Underscores", replaceSpacesWithUnderscores);

        if (GUILayout.Button("Uniformize Selected"))
        {
            if (Selection.activeGameObject != null)
            {
                RenameGameObjectAndChildren(Selection.activeGameObject);
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a GameObject in the Hierarchy.");
            }
        }
    }

    private void RenameGameObjectAndChildren(GameObject root)
    {
        if (root == null)
        {
            Debug.LogError("Root GameObject is null.");
            return;
        }

        Undo.RecordObject(root, "Uniformize Names");

        ApplyNameFormatting(root.transform);

        Debug.Log($"Renaming completed for '{root.name}' and its children.");
    }

    private void ApplyNameFormatting(Transform obj)
    {
        // Skip prefab instances
        if (PrefabUtility.IsPartOfPrefabInstance(obj.gameObject))
        {
            Debug.Log($"Skipping prefab instance: {obj.name}");
            return;
        }

        // Format name
        string originalName = obj.name;
        obj.name = FormatName(originalName);

        // Log name change
        Debug.Log($"Renamed '{originalName}' to '{obj.name}'");

        // Recursively process children
        foreach (Transform child in obj)
        {
            ApplyNameFormatting(child);
        }
    }

    private string FormatName(string name)
    {
        // Replace all non-alphanumeric characters with spaces
        name = Regex.Replace(name, @"[^a-zA-Z0-9]+", " ");

        // Capitalize each word
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        name = textInfo.ToTitleCase(name.ToLower());

        // Remove leading, trailing, and extra spaces
        name = Regex.Replace(name, @"\s+", " ").Trim();

        if (replaceSpacesWithUnderscores)
        {
            return name.Replace("_", " ");
        }

        return name; // No changes if neither option is selected
    }
}
#endif
