using System.Collections.Generic;

[System.Serializable]
public class ClientData
{
    public string clientName;
    public List<Phase> phases;
}

[System.Serializable]
public class Phase
{
    public string phaseName;
    public string questionText;
    public List<Option> options;
}

[System.Serializable]
public class Option
{
    public string text; // Le texte du bouton
    public bool isCorrect;
    public string feedback; // La réponse du client (ex: "Je préfère Philips")
}