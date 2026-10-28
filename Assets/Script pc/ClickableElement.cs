using UnityEngine;
using UnityEngine.UI; 

public class ClickableElement : MonoBehaviour
{
    public enum ButtonType
    {
        SimpleDisable,
        SimpleEnable,
        SimpleToggle,
        OpenApp,
        CloseApp,
        ToggleApp,
        UnityUIButton,
        QuizNextButton
    }

    [Header("Configuration")]
    public ButtonType buttonFunction = ButtonType.SimpleDisable;

    [Header("Feedback Visuel (Hover)")]
    [Tooltip("Couleur normale quand le curseur n'est pas dessus")]
    public Color normalColor = Color.white;
    [Tooltip("Couleur quand le curseur survole l'élément")]
    public Color hoverColor = new Color(0.9f, 0.9f, 0.9f, 1f); // Gris clair par défaut
    private Image targetImage; // L'image à colorier (automatique)

    [Header("Objets Simples")]
    public GameObject targetObject; 

    [Header("Application Manager")]
    public AppWindowLogic connectedAppLogic;

    [Header("Quiz UI")]
    [Tooltip("Glisse ici l'objet qui contient le script QuizUI (uniquement pour le mode QuizNextButton)")]
    public QuizUI connectedQuizUI;

    void Start()
    {
        // On essaie de trouver une image sur cet objet pour gérer la couleur
        targetImage = GetComponent<Image>();
        if (targetImage != null)
        {
            // On sauvegarde la couleur de départ définie dans Unity comme "couleur normale"
            // Si tu préfères forcer le blanc, décommente la ligne suivante :
            // targetImage.color = normalColor;
            normalColor = targetImage.color;
        }
    }

    // --- GESTION DU SURVOL (HOVER) ---
    public void OnHoverEnter()
    {
        if (targetImage != null) targetImage.color = hoverColor;
    }

    public void OnHoverExit()
    {
        if (targetImage != null) targetImage.color = normalColor;
    }

    // --- GESTION DU CLIC ---
    public void OnCursorClick()
    {
        switch (buttonFunction)
        {
            case ButtonType.SimpleDisable:
                if (targetObject != null) targetObject.SetActive(false);
                break;
            case ButtonType.SimpleEnable:
                if (targetObject != null) targetObject.SetActive(true);
                break;
            case ButtonType.SimpleToggle:
                if (targetObject != null) targetObject.SetActive(!targetObject.activeSelf);
                break;
            case ButtonType.OpenApp:
                if (connectedAppLogic != null) connectedAppLogic.OpenApp();
                break;
            case ButtonType.CloseApp:
                if (connectedAppLogic != null) connectedAppLogic.CloseApp();
                break;
            case ButtonType.ToggleApp:
                if (connectedAppLogic != null) connectedAppLogic.ToggleApp();
                break;
            case ButtonType.UnityUIButton:
                Button btn = GetComponent<Button>();
                if (btn != null && btn.interactable) btn.onClick.Invoke();
                break;
            case ButtonType.QuizNextButton:
                if (connectedQuizUI != null)
                {
                    connectedQuizUI.OnNextButtonClicked();
                }
                else
                {
                    Debug.LogError($"[ClickableElement] ERREUR : Mode 'QuizNextButton' sans 'Connected Quiz UI' !");
                }
                break;
        }
    }
}