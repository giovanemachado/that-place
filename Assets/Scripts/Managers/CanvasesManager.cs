using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasesManager : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject HUDCanvas;
    public GameObject LoreCanvas;
     
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(BaseGameState state)
    {
        MainMenuCanvas.SetActive(state == GameManager.Instance.MainMenuState);
        HUDCanvas.SetActive(state == GameManager.Instance.PlayingState);
        LoreCanvas.SetActive(state == GameManager.Instance.LoreState);
    }
}
