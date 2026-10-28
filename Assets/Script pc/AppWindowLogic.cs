using UnityEngine;

public class AppWindowLogic : MonoBehaviour
{
    [Header("Les éléments de l'application")]
    [Tooltip("La fenêtre principale de l'application (celle qui s'ouvre)")]
    public GameObject windowObject;

    [Tooltip("L'icône qui doit apparaitre dans la barre des tâches quand l'app est ouverte")]
    public GameObject taskbarIconObject;

    void Start()
    {
        // Au lancement, on cache tout (état éteint)
        if (windowObject != null) windowObject.SetActive(false);
        if (taskbarIconObject != null) taskbarIconObject.SetActive(false);
    }

    // Fonction pour l'icône du Bureau : Lance tout
    public void OpenApp()
    {
        Debug.Log("Lancement de l'application...");
        if (windowObject != null) windowObject.SetActive(true);
        // L'icône de la barre des tâches DOIT apparaître
        if (taskbarIconObject != null) taskbarIconObject.SetActive(true);
    }

    // Fonction pour la Croix Rouge : Ferme tout
    public void CloseApp()
    {
        Debug.Log("Arrêt complet de l'application...");
        if (windowObject != null) windowObject.SetActive(false);
        // Ici, on enlève l'icône car l'app est vraiment fermée
        if (taskbarIconObject != null) taskbarIconObject.SetActive(false);
    }

    // Fonction pour l'icône de la Barre des Tâches : Minimise ou Restaure
    public void ToggleApp()
    {
        // On vérifie d'abord si l'application est bien "lancée" (l'icône barre des tâches est active)
        // Si l'icône barre des tâches n'est pas là, c'est que l'app est fermée, donc on l'ouvre.
        if (taskbarIconObject != null && !taskbarIconObject.activeSelf)
        {
            OpenApp();
            return;
        }

        // Si l'app est lancée, on bascule juste la visibilité de la FENÊTRE
        if (windowObject != null)
        {
            if (windowObject.activeSelf)
            {
                // Si la fenêtre est visible -> On la cache (Minimiser)
                // MAIS on laisse l'icône barre des tâches active !
                Debug.Log("Minimisation de la fenêtre");
                windowObject.SetActive(false);
            }
            else
            {
                // Si la fenêtre est cachée -> On la réaffiche (Restaurer)
                Debug.Log("Restauration de la fenêtre");
                windowObject.SetActive(true);
            }
        }
    }
}