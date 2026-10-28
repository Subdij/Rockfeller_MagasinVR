using System;
using UnityEngine;

// Structure pour les options de réponse (lue directement depuis le JSON)
[Serializable]
public struct AnswerOption
{
    public string text;
    public bool isCorrect;
    [TextArea(1, 3)]
    public string rationale; // Justification ou explication de la réponse
}

// Structure pour une question (lue directement depuis le JSON)
[Serializable]
public class Question
{
    public string id;
    public string topic;
    public string questionText;
    // La pénalité n'est pas utilisée ici, mais pourrait servir pour d'autres types de questions
    // public float penaltyPercentage; 
    public AnswerOption[] options;
}

// Structure conteneur pour l'ensemble du fichier JSON
[Serializable]
public class QuizData
{
    public string quizTitle;
    [Tooltip("Nombre de bonnes réponses requises pour valider le badge (doit être 5)")]
    public int passRateBadge;
    public Question[] badgeQuestions;
    public Question[] reviewQuestions; // Questions de la banque supplémentaire
}