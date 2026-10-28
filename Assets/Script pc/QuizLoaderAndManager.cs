using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class QuizLoaderAndManager : MonoBehaviour
{
    [Header("Questions Chargées")]
    public QuizData loadedQuizData;

    // Singleton pour accès facile depuis d'autres scripts
    public static QuizLoaderAndManager Instance { get; private set; }

    void Awake()
    {
        // Configuration du Singleton
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Décommentez si besoin de garder entre les scènes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // CORRECTION IMPORTANTE : On charge les données dès le Awake (avant le Start des autres scripts)
        Debug.Log("[QuizManager] Initialisation du Manager...");
        LoadQuizData();
    }

    private void LoadQuizData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "questions.json");
        Debug.Log($"[QuizManager] Tentative de lecture du fichier à : {filePath}");

        if (File.Exists(filePath))
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                Debug.Log($"[QuizManager] Fichier trouvé ! Contenu (aperçu) : {jsonText.Substring(0, Mathf.Min(jsonText.Length, 50))}...");

                loadedQuizData = JsonUtility.FromJson<QuizData>(jsonText);

                if (loadedQuizData != null && loadedQuizData.badgeQuestions != null)
                {
                    Debug.Log($"[QuizManager] SUCCÈS ! {loadedQuizData.badgeQuestions.Length} questions chargées pour le Badge.");
                }
                else
                {
                    Debug.LogError("[QuizManager] ERREUR : Le JSON a été lu mais semble vide ou mal formaté.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[QuizManager] ERREUR CRITIQUE lors de la lecture du JSON : {e.Message}");
            }
        }
        else
        {
            Debug.LogError($"[QuizManager] ERREUR : Fichier introuvable ! Assurez-vous d'avoir créé le dossier 'Assets/StreamingAssets' et d'y avoir mis 'questions.json'.");
        }
    }
    
    public bool CheckBadgeQuiz(int correctAnswersCount)
    {
        if (loadedQuizData == null) return false;

        int requiredScore = loadedQuizData.passRateBadge;
        Debug.Log($"[QuizManager] Vérification Score : {correctAnswersCount}/{loadedQuizData.badgeQuestions.Length} (Requis: {requiredScore})");

        return correctAnswersCount >= requiredScore;
    }
    
    public Question[] GetReviewQuestions()
    {
        if (loadedQuizData != null) return loadedQuizData.reviewQuestions;
        return new Question[0];
    }
}