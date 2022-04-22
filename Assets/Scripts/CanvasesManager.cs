using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasesManager : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject PausedMenuCanvas;
    public GameObject HUDCanvas;

    public GameObject SomethingHappend;
     
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
        GameManager.OnChangeIdle += OnSomethingHappend;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
        GameManager.OnChangeIdle -= OnSomethingHappend; 
    }

    void OnSomethingHappend(bool isSomethingHappend)
    {
        if (isSomethingHappend)
        {
            SomethingHappend.SetActive(true);
        } else
        {
            SomethingHappend.SetActive(false);
        }
    }

    void GameManagerOnGameStateChanged(BaseGameState state)
    {
        MainMenuCanvas.SetActive(state == GameManager.Instance.MainMenuState);
        HUDCanvas.SetActive(state == GameManager.Instance.PlayingState);
        PausedMenuCanvas.SetActive(state == GameManager.Instance.PausedState);
    }
}
