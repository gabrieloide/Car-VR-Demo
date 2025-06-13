using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public abstract class InteractableButton : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    protected AudioSource _audioSource;
    protected bool TouchingInteractable = false;
    public abstract void OnEnterInteract(SelectEnterEventArgs selectEnterEventArgs);
    public abstract void OnExitInteract(SelectExitEventArgs selectEnterEventArgs);


    private void Start()
    {
        if (GetComponent<XRSimpleInteractable>() != null)
        {
            GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnEnterInteract);
            GetComponent<XRSimpleInteractable>().selectExited.AddListener(OnExitInteract);
        }
        else if (GetComponent<XRGrabInteractable>() != null)
        {
            GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnEnterInteract);
            GetComponent<XRGrabInteractable>().selectExited.AddListener(OnExitInteract);
            AudioSource.Destroy(GetComponent<AudioSource>());
            
        }

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        if (clickSound == null)
            clickSound = Resources.Load<AudioClip>("Sounds/ClickSound");

        _audioSource.clip = clickSound;
    }
}