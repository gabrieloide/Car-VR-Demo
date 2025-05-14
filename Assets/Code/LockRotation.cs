using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class LockRotation : MonoBehaviour
{
    private Quaternion initialRotation;
    void Start() {
        initialRotation = transform.rotation;
    }

    void Update() {
        // Bloquear rotación a eje Z
        transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
