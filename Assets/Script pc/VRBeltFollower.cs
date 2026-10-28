using UnityEngine;

public class VRBeltFollower : MonoBehaviour
{
    [Header("Cible")]
    [Tooltip("Glisse ta Main Camera ici")]
    public Transform headTarget;

    [Header("Configuration")]
    [Tooltip("Distance verticale sous la tête (ex: 0.5 pour le ventre)")]
    public float verticalOffset = 0.5f;

    // J'ai retiré la variable "smoothSpeed" car elle ne servait qu'à la rotation

    void LateUpdate()
    {
        if (headTarget == null) return;

        // --- 1. Gérer la POSITION SEULEMENT ---
        // On prend la position de la tête (X et Z)
        // Mais on fixe la hauteur (Y) pour qu'elle soit 'verticalOffset' plus bas que la tête
        Vector3 targetPosition = new Vector3(headTarget.position.x, headTarget.position.y - verticalOffset, headTarget.position.z);
        
        transform.position = targetPosition;

        // --- 2. ROTATION SUPPRIMÉE ---
        // La ceinture gardera maintenant sa rotation actuelle (ou celle définie par son parent XR Origin)
        // Elle ne tournera plus quand tu tournes la tête à gauche/droite.
    }
}