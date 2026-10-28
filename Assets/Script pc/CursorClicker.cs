using UnityEngine;
using UnityEngine.XR; 
using System.Collections.Generic;

public class CursorClicker : MonoBehaviour
{
    [Header("Configuration VR")]
    [Tooltip("Choisis la main qui contrôle le curseur (RightHand ou LeftHand)")]
    public XRNode controllerNode = XRNode.RightHand;

    private bool wasPressed = false;
    private ClickableElement currentHoveredElement;
    private List<InputDevice> devices = new List<InputDevice>();

    void Update()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);

        if (devices.Count > 0)
        {
            InputDevice device = devices[0];
            bool isPressed = false;
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed))
            {
                if (isPressed && !wasPressed)
                {
                    if (currentHoveredElement != null)
                    {
                        currentHoveredElement.OnCursorClick();
                    }
                }
                wasPressed = isPressed;
            }
        }
    }

    // --- GESTION DES COLLISIONS (TRIGGERS) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        ClickableElement element = other.GetComponent<ClickableElement>();
        if (element != null)
        {
            currentHoveredElement = element;
            
            // NOUVEAU : On active l'effet de survol (changement de couleur)
            element.OnHoverEnter(); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ClickableElement element = other.GetComponent<ClickableElement>();
        
        // Si l'objet qu'on quitte avait un script ClickableElement, on remet sa couleur normale
        if (element != null)
        {
            element.OnHoverExit();
        }

        // Si c'était l'objet actif, on le vide
        if (element == currentHoveredElement)
        {
            currentHoveredElement = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning($"[PROBLEME] Collision physique avec : {collision.gameObject.name}. Coche 'Is Trigger' !");
    }
}