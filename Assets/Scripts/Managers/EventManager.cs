using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject EventCanvas;

    public TextMeshProUGUI EventTypeText;
    public TextMeshProUGUI DescriptionText;

    private void Awake()
    {
        GameManager.OnGameplayEventAppears += GameManagerEventAppears;
    }

    private void OnDestroy()
    {
        GameManager.OnGameplayEventAppears -= GameManagerEventAppears;
    }

    void GameManagerEventAppears(GameManager.GameplayEvent gamePlayEvent)
    {
        // EventCanvas.SetActive(true);
    }

    public void DoSomethingButtonPressed()
    {
        
    }

    public void IgnoreButtonPressed()
    {
        //
    }
}
