using UnityEngine;
using TMPro;

public class ScenarioController : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvasConversation;
    public TMP_Text dialogueText;

    public GameObject choicesPanel;
    public VRButton buttonA;
    public VRButton buttonB;
    public VRButton buttonC;
    public VRButton buttonD;  // optionnel

    [Header("Cafetieres 3D (option 1)")]
    public GameObject cafePhilips;
    public GameObject cafeKrups;
    public GameObject cafeMoulinex;

    private int state = 0;

    void Start()
    {
        canvasConversation.SetActive(false);
    }

    // ----------- ACTIVATION PAR LE PNJ (OnInteract) -------------
    public void StartScenario()
    {
        canvasConversation.SetActive(true);
        state = 0;
        Step1_QCM_Accueil();
    }

    public void NextState() // avancée universelle si tu veux ajouter plus tard
    {
        state++;
    }

    // ------------------- ETAPES DU SCENARIO ---------------------

    // 🟦 ETAPE 1 — QCM d'accueil (a, b, c)
    void Step1_QCM_Accueil()
    {
        dialogueText.text =
            "Bonjour Monsieur,\n" +
            "a) Puis-je vous aider ?\n" +
            "b) Vous voulez des informations ?\n" +
            "c) En quoi puis-je vous être utile ?";

        choicesPanel.SetActive(true);

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(WrongAccueil);
        buttonB.onClick.AddListener(WrongAccueil);
        buttonC.onClick.AddListener(CorrectAccueil);
    }

    void WrongAccueil()
    {
        dialogueText.text =
            "Client : « Non merci, je n’ai besoin d’aucune aide. »\n" +
            "Fin de la conversation.";

        choicesPanel.SetActive(false);
    }

    void CorrectAccueil()
    {
        dialogueText.text =
            "Bravo, bonne réponse !\n" +
            "Une question ouverte permet d’initier le dialogue.\n\n" +
            "Client : « Je cherche une cafetière à filtre. »";

        Invoke(nameof(Step2_QuestionsEntonnoir), 2f);
    }

    // 🟩 ETAPE 2 — Entonnoir (ordre : usage > capacité > marque > budget)
    void Step2_QuestionsEntonnoir()
    {
        dialogueText.text =
            "Classe les questions dans l'ordre (entonnoir) :\n" +
            "A)  Usage\n" +
            "B)  Capacité\n" +
            "C)  Marques\n" +
            "D)  Budget";

        // On utilise A → B → C → D
        choicesPanel.SetActive(true);

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(ChooseUsage);
    }

    void ChooseUsage()
    {
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.AddListener(ChooseCapacity);
        dialogueText.text = "Bien ! Ensuite ?";
    }

    void ChooseCapacity()
    {
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.AddListener(ChooseBrand);
        dialogueText.text = "Parfait, puis ?";
    }

    void ChooseBrand()
    {
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.AddListener(ChooseBudget);
        dialogueText.text = "Très bien, et enfin ?";
    }

    void ChooseBudget()
    {
        dialogueText.text =
            "Bravo !\n\nLe client répond :\n" +
            "Usage quotidien, 6 personnes, filtre permanent,\n" +
            "Pas de marque, budget max 30€.";

        Invoke(nameof(Step3_ChooseProduct), 2f);
    }

    // 🟧 ETAPE 3 — Choisir la bonne cafetière (objets 3D)
    void Step3_ChooseProduct()
    {
        dialogueText.text =
            "Choisissez la cafetière adaptée.\n" +
            "(Clique la vraie cafetière dans la scène)";

        choicesPanel.SetActive(false);

        // On ajoute les scripts sur les objets
        cafePhilips.AddComponent<ProductChoice>().Init(this, true);
        cafeKrups.AddComponent<ProductChoice>().Init(this, false);
        cafeMoulinex.AddComponent<ProductChoice>().Init(this, false);
    }

    public void CorrectProduct()
    {
        dialogueText.text =
            "Excellent choix ! C'est la seule avec filtre permanent\n" +
            "et un prix sous les 30€.";

        Invoke(nameof(Step4_Objections), 2f);
    }

    public void WrongProduct()
    {
        dialogueText.text = "Ce produit ne correspond pas au besoin. Réessaie.";
    }

    // 🟥 ETAPE 4 — Objections (origine + année)
    void Step4_Objections()
    {
        dialogueText.text =
            "Client : « Je n'ai pas confiance en la marque.\n" +
            "C'est une marque européenne ? »\n\n" +
            "Quelle est l'origine de Philips ?";

        choicesPanel.SetActive(true);

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(() => WrongObjection());
        buttonB.onClick.AddListener(() => CorrectOrigin());
        buttonC.onClick.AddListener(() => WrongObjection());
        buttonD.onClick.AddListener(() => WrongObjection());
    }

    void CorrectOrigin()
    {
        dialogueText.text =
            "Bonne réponse : Philips est une marque néerlandaise.\n\n" +
            "Année de fondation ?";

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();

        // 1891 = correct
        buttonA.onClick.AddListener(CorrectYear);
        buttonB.onClick.AddListener(WrongObjection);
        buttonC.onClick.AddListener(WrongObjection);
        buttonD.onClick.AddListener(WrongObjection);
    }

    void CorrectYear()
    {
        dialogueText.text =
            "Exact ! Fondée en 1891, marque fiable depuis 130 ans.\n\n" +
            "Client : « Très bien, je prends ce modèle. »";

        Invoke(nameof(Step5_Conclusion), 2f);
    }

    void WrongObjection()
    {
        dialogueText.text = "Mauvaise réponse. Réessaie.";
    }

    // 🟩 ETAPE 5 — Conclusion
    void Step5_Conclusion()
    {
        dialogueText.text =
            "Le vendeur remet la cafetière au client.\n" +
            "Fin du scénario. Merci !";

        choicesPanel.SetActive(false);
    }
}
