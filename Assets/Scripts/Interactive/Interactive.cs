using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] private InteractiveData _interactiveData;

    private InteractionManager  _interactionManager;
    private PlayerInventory     _playerInventory;
    private List<Interactive>   _requirements;
    private List<Interactive>   _dependents;
    private List<Interactive>   _interactables;
    private Animator            _animator;
    private AudioSource         _audioSource;
    private bool                _requirementsMet;
    public bool RequirementsMet => _requirementsMet;
    private int                 _interactionCount;

    public bool isOn;

    public InteractiveData interactiveData => _interactiveData;

    public string inventoryName => _interactiveData.inventoryName;
    public Sprite inventoryIcon => _interactiveData.inventoryIcon;
    public GameObject holdingObject => _interactiveData.holdingObject;
    public bool deleteRequirementsOnUse => _interactiveData.deleteRequirementsOnUse;
    private bool IsType(InteractiveData.Type type) => _interactiveData.type == type;
    public void SetPickAudioSource(AudioSource source) => _audioSource = source;

    void Awake()
    {
        _interactionManager = InteractionManager.instance;
        _playerInventory    = _interactionManager.playerInventory;
        _requirements       = new List<Interactive>();
        _dependents         = new List<Interactive>();
        _animator           = GetComponent<Animator>();
        _requirementsMet    = _interactiveData.requirements.Length == 0;
        _interactionCount   = 0;
        isOn                = _interactiveData.startsOn;

        _interactionManager.RegisterInteractive(this);
    }

    public void AddRequirement(Interactive requirement) => _requirements.Add(requirement);

    public void AddDependent(Interactive dependent) => _dependents.Add(dependent);

    public void AddInteractable(Interactive interactable) => _interactables.Add(interactable);

    public string GetInteractionMessage()
    {
        if (IsType(InteractiveData.Type.Pickable) && !_playerInventory.Contains(this) && (_requirementsMet && deleteRequirementsOnUse|| PlayerHasRequirement() && !deleteRequirementsOnUse))
            return _interactionManager.pickMessage.Replace("$name", _interactiveData.inventoryName.ToLower());
        else if (!_requirementsMet)
        {
            if (PlayerHasRequirementSelected())
                return _playerInventory.GetSelectedInteractionMessage();
            else
                return _interactiveData.requirementsMessage;
        }
        else if (interactiveData.interactionMessages.Length > 0)
            return interactiveData.interactionMessages[_interactionCount % _interactiveData.interactionMessages.Length];
        else
            return null;
    }

    public void SetInteractionMessage(string message)
    {
        interactiveData.interactionMessages[0] = message;
    }

    private bool PlayerHasRequirementSelected()
    {
        foreach (Interactive requirement in _requirements)
        {
            if (_playerInventory.IsSelected(requirement))
                return true;
        }

        Debug.Log("Not holding requirement");
        return false;
    }
    private bool PlayerHasRequirement()
    {
        foreach (Interactive requirement in _requirements)
        {
            if (_playerInventory.Contains(requirement))
                return true;
        }

        Debug.Log("Not have requirement");
        return false;
    }


    public void Interact()
    {
        bool select = false;

        if (_requirementsMet)
            InteractSelf(true);
        else if (!deleteRequirementsOnUse && PlayerHasRequirement())
        {
            select = true;
            InteractSelf(true);
        }
        else if (deleteRequirementsOnUse && PlayerHasRequirementSelected())
        {
            select = true;
            UseRequirementFromInventory();
        }

        if ((!select || _requirementsMet) && !IsType(InteractiveData.Type.Pickable))
            PlayAnimation("Talk");
    }

    private void InteractSelf(bool direct)
    {
        if (direct && IsType(InteractiveData.Type.Indirect))
            return;
        else if (IsType(InteractiveData.Type.Pickable) && !_playerInventory.IsFull())
            PickUpInteractive();
        else if (IsType(InteractiveData.Type.InteractOnce) || IsType(InteractiveData.Type.InteractMulti))
            DoDirectInteraction();
        else if (IsType(InteractiveData.Type.Indirect))
            PlayAnimation("Interact");
    }

    private void PickUpInteractive()
    {
        Debug.Log($"Picking up {gameObject.name}");
        if (_audioSource != null || interactiveData.pickUpSound != null)
        {
            PlayPickUpSound();
        }
        _playerInventory.Add(this);
        gameObject.SetActive(false);
    }

    public void PlayPickUpSound()
    {
        _audioSource.clip = interactiveData.pickUpSound;
        _audioSource.Play();
    }

    private void DoDirectInteraction()
    {
        ++_interactionCount;

        if (IsType(InteractiveData.Type.InteractOnce))
            isOn = false;

        CheckDependentsRequirements();
        DoIndirectInteractions();

        PlayAnimation("Interact");
    }

    private void CheckDependentsRequirements()
    {
        foreach (Interactive dependent in _dependents)
            dependent.CheckRequirements();
    }

    private void CheckRequirements()
    {
        foreach (Interactive requirement in _requirements)
        {
            if (!requirement._requirementsMet || 
               (!requirement.IsType(InteractiveData.Type.Indirect) && requirement._interactionCount == 0))
               {
                    _requirementsMet = false;
                    return;
               }
        }

        _requirementsMet = true;
        PlayAnimation("Awake");

        CheckDependentsRequirements();
    }

    private void DoIndirectInteractions()
    {
        foreach (Interactive dependent in _dependents)
            if (dependent.IsType(InteractiveData.Type.Indirect) && dependent._requirementsMet)
                dependent.InteractSelf(false);
    }
 
    private void PlayAnimation(string animation)
    {
        Debug.Log($"{gameObject.name} is doing animation {animation}");
        if (_animator != null)
        {
            gameObject.SetActive(true);
            _animator.SetTrigger(animation);
        }
    }

    private void UseRequirementFromInventory()
    {
        Interactive requirement = _playerInventory.GetSelected();

        _playerInventory.Remove(requirement);

        ++requirement._interactionCount;
        
        requirement.PlayAnimation("Interact");

        CheckRequirements();
    }
    public void ResetRequirements()
    {
        _requirementsMet = false;
    }
}
