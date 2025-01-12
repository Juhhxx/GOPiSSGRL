using UnityEngine;

public class ReplaceBlockoutModelMeshes : MonoBehaviour
{
    public GameObject blockoutModel; // Drag the blockout model here
    public string blockoutMeshName;  // Name of the mesh in the blockout to replace
    public GameObject newModel;      // Drag the model containing the new mesh here
    public string newMeshName;       // Name of the new mesh

    void Start()
    {
        // Get the new mesh from the new model
        Mesh newMesh = FindMeshInModel(newModel, newMeshName);
        if (newMesh == null)
        {
            Debug.LogError($"New mesh '{newMeshName}' not found in the new model.");
            return;
        }

        // Find and replace meshes in the blockout model
        MeshFilter[] meshFilters = blockoutModel.GetComponentsInChildren<MeshFilter>(true);
        int replacements = 0;

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh != null && mf.sharedMesh.name == blockoutMeshName)
            {
                mf.sharedMesh = newMesh;
                replacements++;
                Debug.Log($"Replaced mesh '{blockoutMeshName}' with '{newMeshName}' on {mf.gameObject.name}");
            }
        }

        Debug.Log($"Mesh replacement completed! Total replacements: {replacements}");
    }

    // Helper to find a mesh by name inside a model
    private Mesh FindMeshInModel(GameObject model, string meshName)
    {
        MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>(true);
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh != null && mf.sharedMesh.name == meshName)
            {
                return mf.sharedMesh;
            }
        }
        return null;
    }
}
