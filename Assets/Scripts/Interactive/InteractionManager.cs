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
    private AudioSource       _audioSource;

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
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.1f;

        ProcessDependencies();
        AddAudioSource();
        _interactives = null;
    }

    private void ProcessDependencies()
    {
        foreach (Interactive interactive in _interactives)
        {
            foreach (InteractiveData requirementData in interactive.interactiveData.requirements)
            {
                Interactive requirement = FindInteractive(requirementData);
                Debug.Log($"Loading requirement: {requirementData.name}");
                interactive.AddRequirement(requirement);
                Debug.Log($"Adding {interactive.gameObject.name} as dependent of {requirementData.name}");
                requirement.AddDependent(interactive);
            }
        }
    }

    private void AddAudioSource()
    {
        foreach (Interactive interactive in _interactives)
        {
            interactive.SetPickAudioSource(_audioSource);
        }
    }

    private Interactive FindInteractive(InteractiveData interactiveData)
    {
        foreach (Interactive interactive in _interactives)
            if (interactive.interactiveData == interactiveData)
                return interactive;

        return null;
    }
}
