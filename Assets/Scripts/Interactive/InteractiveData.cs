using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveData", menuName = "Scriptable Objects/InteractiveData")]
public class InteractiveData : ScriptableObject
{
    public enum Type { Pickable, InteractOnce, InteractMulti, Indirect };

    public Type                 type;
    public bool                 startsOn = true;
    public string               inventoryName;
    public Sprite               inventoryIcon;
    public InteractiveData[]    requirements;
    public bool                 deleteRequirementsOnUse = true;
    public string               requirementsMessage;
    public string[]             interactionMessages;
    public bool                 activatesUI;
    public GameObject           holdingObject;
}

