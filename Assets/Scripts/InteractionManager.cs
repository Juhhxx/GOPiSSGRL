using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private static InteractionManager _instance;

    public static InteractionManager instance
    {
        get { return _instance; }
    }

    [SerializeField] private PlayerInventory    _playerInventory;
    [SerializeField] private string             _pickMessage;

    private List<Interactive> _interactives;

    private InteractionManager()
    {
        _instance       = this;
        _interactives   = new List<Interactive>();
    }

    public PlayerInventory playerInventory
    {
        get { return _playerInventory; }
    }

    public string pickMessage
    {
        get { return _pickMessage; }
    }    

    public void RegisterInteractive(Interactive interactive)
    {
        _interactives.Add(interactive);
    }

    void Start()
    {
        ProcessDependencies();
        _interactives = null;
    }

    private void ProcessDependencies()
    {
        foreach (Interactive interactive in _interactives)
        {
            foreach (InteractiveData requirementData in interactive.interactiveData.requirements)
            {
                Interactive requirement = FindInteractive(requirementData);
                interactive.AddRequirement(requirement);
                requirement.AddDependent(interactive);
            }
        }
    }

    public Interactive FindInteractive(InteractiveData interactiveData)
    {
        foreach (Interactive interactive in _interactives)
            if (interactive.interactiveData == interactiveData)
                return interactive;

        return null;
    }
}
