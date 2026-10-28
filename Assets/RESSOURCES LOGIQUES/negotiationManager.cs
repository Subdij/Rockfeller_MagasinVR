using UnityEngine;
using UnityEngine.UI; // On garde pour l'affichage texte
using System.IO;
using System.Collections.Generic;
using TMPro; 

public class NegotiationManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject vrCanvas; 
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI feedbackText;
    
    // On référence les TEXTES des boutons, pas les boutons eux-mêmes
    public TextMeshProUGUI[] buttonLabels; 
    // On référence les OBJETS boutons pour pouvoir les cacher/afficher
    public GameObject[] buttonObjects;

    [Header("Configuration")]
    public string jsonFileName = "client1_data.json";

    private ClientData currentData;
    private int currentPhaseIndex = 0;
    private float currentScore = 100f;
    private Phase currentPhase; // Pour stocker la phase en cours

    void Start()
    {
        vrCanvas.SetActive(false);
        LoadJsonData();
    }

    void LoadJsonData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (File.Exists(path))
        {
            currentData = JsonUtility.FromJson<ClientData>(File.ReadAllText(path));
        }
    }

    public void StartNegotiation()
    {
        if (currentData == null) return;
        vrCanvas.SetActive(true);
        currentPhaseIndex = 0;
        DisplayPhase(currentPhaseIndex);
    }

    void DisplayPhase(int index)
    {
        if (index >= currentData.phases.Count)
        {
            EndNegotiation();
            return;
        }

        currentPhase = currentData.phases[index];
        questionText.text = currentPhase.questionText;
        feedbackText.text = ""; 

        // Mise à jour des textes des boutons
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            if (i < currentPhase.options.Count)
            {
                buttonObjects[i].SetActive(true);
                // Met à jour le texte (assure-toi d'avoir relié les TextMeshProUGUI dans l'inspecteur)
                if(buttonLabels[i] != null) buttonLabels[i].text = currentPhase.options[i].text;
            }
            else
            {
                buttonObjects[i].SetActive(false);
            }
        }
    }

    // CETTE FONCTION SERA APPELÉE PAR TES BOUTONS PHYSIQUES
    public void SelectAnswer(int buttonIndex)
    {
        if (currentPhase == null || buttonIndex >= currentPhase.options.Count) return;

        Option selectedOption = currentPhase.options[buttonIndex];
        
        // Feedback et Score
        feedbackText.text = selectedOption.feedback;

        if (!selectedOption.isCorrect)
        {
            if (currentPhase.phaseName == "Découverte") currentScore -= 0;
            else currentScore -= 25;
        }

        Invoke("NextPhase", 3.0f);
    }

    void NextPhase()
    {
        currentPhaseIndex++;
        DisplayPhase(currentPhaseIndex);
    }

    void EndNegotiation()
    {
        questionText.text = "Négociation terminée !";
        string finalResult = (currentScore >= 80) ? "Client validé !" : "Échec.";
        feedbackText.text = "Score final : " + currentScore + "%\n" + finalResult;
        foreach(var btn in buttonObjects) btn.SetActive(false);
    }
}