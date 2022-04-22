using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI HappinessText;
    
    public void PauseButtonPressed()
    {
        GameManager.Instance.SwitchState(GameManager.Instance.PausedState);
    }
    public void SomethingButtonPressed()
    {
        GameManager.Instance.IncreaseHappiness(3);
        GameManager.Instance.InvokeOnSomeInteraction(true);
    }

    void Update()
    {
        HappinessText.text = "Happiness: " + GameManager.Instance.Happiness;
    }
}
