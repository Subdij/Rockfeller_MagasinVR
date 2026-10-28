using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform openPosition;
    public float speed = 3f;
    public AudioSource audioSource;   // référence à l’AudioSource
    public AudioClip openSound;       // son d’ouverture

    private Vector3 closedPosition;
    private bool isOpening = false;
    private bool wasOpening = false;  // pour détecter le changement d’état

    void Start()
    {
        closedPosition = transform.position;
    }

    void Update()
    {
        Vector3 target = isOpening ? openPosition.position : closedPosition;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);

        // Détection du changement d’état
        if (isOpening != wasOpening)
        {
            if (isOpening && openSound != null)
                audioSource.PlayOneShot(openSound);

            wasOpening = isOpening;
        }
    }

    public void SetOpening(bool state)
    {
        isOpening = state;
    }
}