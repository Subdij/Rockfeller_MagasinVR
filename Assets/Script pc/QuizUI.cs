using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Linq;

public class QuizUI : MonoBehaviour
{
    [Header("Références UI - Questions")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons; 

    [Header("Références UI - Résultat (Feedback)")]
    public TextMeshProUGUI resultText;
    [Tooltip("L'image qui doit apparaitre en même temps que le texte de résultat")]
    public GameObject resultImageObject; 

    [Header("Navigation")]
    [Tooltip("L'objet (avec collider) sur lequel on doit cliquer pour passer à la suite")]
    public GameObject nextQuestionButtonObject; 

    private QuizLoaderAndManager manager;
    private Question currentQuestion;
    private int currentQuestionIndex = 0;
    private int correctAnswersCount = 0;
    
    // NOUVEAU : Pour savoir si on doit recommencer
    private bool isQuizFinished = false;

    void Start()
    {
        manager = QuizLoaderAndManager.Instance;

        if (manager == null || manager.loadedQuizData == null)
        {
            Debug.LogError("[QuizUI] ERREUR : Manager non prêt.");
            return;
        }

        // Initialisation propre
        ResetQuizUI();
        SetupButtons();
        DisplayNextQuestion();
    }

    private void ResetQuizUI()
    {
        if (resultImageObject != null) resultImageObject.SetActive(false);
        if (nextQuestionButtonObject != null) nextQuestionButtonObject.SetActive(false);
        if (resultText != null) resultText.text = "";
        isQuizFinished = false;
    }

    private void RestartQuiz()
    {
        Debug.Log("[QuizUI] Redémarrage du Quiz...");
        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        isQuizFinished = false;
        
        ResetQuizUI();
        
        // On réactive l'affichage des questions
        if(questionText != null) questionText.gameObject.SetActive(true);
        // On réactive les boutons (car EndQuiz les a cachés)
        foreach(var button in optionButtons) 
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }

        DisplayNextQuestion();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; 
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SubmitAnswer(index));
        }
    }

    private void DisplayNextQuestion()
    {
        // Nettoyage entre les questions
        if (resultImageObject != null) resultImageObject.SetActive(false);
        if (nextQuestionButtonObject != null) nextQuestionButtonObject.SetActive(false);
        if (resultText != null) resultText.text = "";

        if (currentQuestionIndex < manager.loadedQuizData.badgeQuestions.Length)
        {
            currentQuestion = manager.loadedQuizData.badgeQuestions[currentQuestionIndex];
            
            if(questionText != null)
                questionText.text = $"{currentQuestionIndex + 1}/{manager.loadedQuizData.badgeQuestions.Length}\n{currentQuestion.questionText}";

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].interactable = true; 
                if (i < currentQuestion.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    TextMeshProUGUI btnText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null) btnText.text = currentQuestion.options[i].text;
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            EndQuiz();
        }
    }

    private void SubmitAnswer(int selectedOptionIndex)
    {
        foreach(var button in optionButtons) button.interactable = false;

        if (selectedOptionIndex >= currentQuestion.options.Length) return;

        AnswerOption selectedOption = currentQuestion.options[selectedOptionIndex];

        if (selectedOption.isCorrect)
        {
            correctAnswersCount++;
            if(resultText != null) resultText.text = $"<color=green>CORRECT !</color>\n{selectedOption.rationale}";
        }
        else
        {
            if(resultText != null) resultText.text = $"<color=red>INCORRECT !</color>\n{selectedOption.rationale}";
        }

        if (resultImageObject != null) resultImageObject.SetActive(true);
        if (nextQuestionButtonObject != null) nextQuestionButtonObject.SetActive(true);
    }

    // C'est ici que tout se joue pour le bouton SUIVANT
    public void OnNextButtonClicked()
    {
        // Si le quiz est fini, le bouton "Suivant" sert de bouton "Recommencer"
        if (isQuizFinished)
        {
            RestartQuiz();
        }
        else
        {
            // Sinon, c'est le comportement normal
            Debug.Log("[QuizUI] Question Suivante.");
            currentQuestionIndex++;
            DisplayNextQuestion();
        }
    }

    private void EndQuiz()
    {
        Debug.Log("[QuizUI] Fin du Quiz.");
        bool badgeValidated = manager.CheckBadgeQuiz(correctAnswersCount);
        
        // On cache les questions pour laisser place au résultat final
        if(questionText != null) questionText.gameObject.SetActive(false);
        foreach(var button in optionButtons) button.gameObject.SetActive(false);

        // On affiche le cadre de résultat
        if (resultImageObject != null) resultImageObject.SetActive(true);

        if (badgeValidated)
        {
            if(resultText != null) resultText.text = $"<color=green>FÉLICITATIONS ! BADGE VALIDÉ !</color>\nScore : {correctAnswersCount}/5.";
            // Succès : On peut cacher le bouton suivant (ou l'utiliser pour quitter)
            if (nextQuestionButtonObject != null) nextQuestionButtonObject.SetActive(false);
        }
        else
        {
            string feedback = correctAnswersCount >= 3 ? "Bon début." : "Revoir les bases.";
            // Échec : On affiche le message ET on garde le bouton suivant pour recommencer
            if(resultText != null) resultText.text = $"<color=orange>ÉCHEC</color>\nScore : {correctAnswersCount}/5.\n{feedback}\n\n</size>";
            
            if (nextQuestionButtonObject != null) nextQuestionButtonObject.SetActive(true);
            
            // On marque le quiz comme "Fini" pour que le prochain clic déclenche le Restart
            isQuizFinished = true;
        }
    }
}