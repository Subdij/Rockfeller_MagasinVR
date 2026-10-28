using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRButton : MonoBehaviour
{
    public UnityEvent onClick;

    void Start()
    {
        // Pour PC (clic souris)
        GetComponent<Button>().onClick.AddListener(() => { onClick.Invoke(); });

        // Pour VR (ray + trigger)
        GetComponent<XRBaseInteractable>().selectEntered.AddListener((_) => { onClick.Invoke(); });
    }
}
