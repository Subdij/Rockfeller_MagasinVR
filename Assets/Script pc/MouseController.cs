using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseController : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [Header("Paramètres souris")]
    public float fixedY = 0.75f;

    [Header("Curseur UI")]
    public RectTransform screenCursor;

    [Header("Canvas (World Space)")]
    public RectTransform canvasRect;

    [Header("Vitesse du curseur")]
    public float cursorSpeed = 1.0f;   // 🎯 Réglable dans l’inspecteur

    private Quaternion lockedRotation;

    private Vector3 initialHandPos;
    private Vector2 initialCursorPos;
    private Vector2 canvasHalfSize;

    protected override void Awake()
    {
        base.Awake();
        lockedRotation = transform.rotation;

        if (canvasRect != null)
            canvasHalfSize = canvasRect.sizeDelta / 2f;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        Transform hand = args.interactorObject.transform;

        // Calibration au grab
        initialHandPos = hand.position;
        initialCursorPos = screenCursor.localPosition;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (!isSelected) return;
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic) return;

        Transform hand = firstInteractorSelecting.transform;

        // Déplacement physique souris
        Vector3 pos = transform.position;
        pos.x = hand.position.x;
        pos.z = hand.position.z;
        pos.y = fixedY;
        transform.position = pos;

        transform.rotation = lockedRotation;

        // Déplacement curseur UI
        if (screenCursor != null && canvasRect != null)
        {
            Vector3 handDelta = hand.position - initialHandPos;

            // 🎯 Application de la vitesse
            Vector2 newCursorPos =
                initialCursorPos +
                new Vector2(handDelta.x * cursorSpeed, handDelta.z * cursorSpeed);

            newCursorPos.x = Mathf.Clamp(newCursorPos.x, -canvasHalfSize.x, canvasHalfSize.x);
            newCursorPos.y = Mathf.Clamp(newCursorPos.y, -canvasHalfSize.y, canvasHalfSize.y);

            screenCursor.localPosition = newCursorPos;
        }
    }
}
