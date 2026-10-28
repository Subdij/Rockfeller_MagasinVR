using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public NegotiationManager manager;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie que c'est bien le joueur (assure-toi que ton Player VR a le tag "Player")
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            manager.StartNegotiation();
        }
    }
}