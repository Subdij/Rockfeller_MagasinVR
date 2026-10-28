using UnityEngine;
using TMPro;

public class NPCInteractionVR : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    public GameObject dialogueCanvas;
    public TMP_Text dialogueText;

    [Header("Paramètres")]
    public float interactDistance = 3f;
    public bool autoLookAtPlayer = true;

    [Header("Debug")]
    public bool enableDebugInputs = false;   // ✅ coche ça si tu veux tester au clavier/souris

    private bool isTalking = false;

    void Start()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    void Update()
    {
        if (isTalking && autoLookAtPlayer && player != null)
        {
            Vector3 lookPos = player.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }

        // ✅ Inputs debug uniquement (PC)
        if (enableDebugInputs)
        {
            if (Input.GetKeyDown(KeyCode.E))
                OnInteract();

            if (Input.GetMouseButtonDown(0))
                OnInteract();
        }
    }

    public void OnInteract()
    {
        // 🔒 Facultatif : on remet la vérif de distance
        if (player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist > interactDistance) return;
        }

        if (!isTalking)
            StartConversation();
        else
            EndConversation();
    }

    void StartConversation()
    {
        isTalking = true;
        dialogueCanvas.SetActive(true);
        dialogueText.text = "Bonjour, en quoi puis-je vous être utile ?";
    }

    public void EndConversation()
    {
        isTalking = false;
        dialogueCanvas.SetActive(false);
    }
}
