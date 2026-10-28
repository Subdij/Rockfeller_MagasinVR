using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public SlidingDoor door; // référence à la porte

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetOpening(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetOpening(false);
        }
    }
}