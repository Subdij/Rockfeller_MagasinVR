using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ProductChoice : MonoBehaviour
{
    ScenarioController scenario;
    bool correct;

    public void Init(ScenarioController sc, bool isCorrect)
    {
        scenario = sc;
        correct = isCorrect;

        var inter = gameObject.AddComponent<XRSimpleInteractable>();
        inter.selectEntered.AddListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (correct) scenario.CorrectProduct();
        else scenario.WrongProduct();
    }
}
