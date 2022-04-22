using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    BaseGameState state;

    [HideInInspector] public MainMenuState MainMenuState = new MainMenuState();
    [HideInInspector] public PlayingState PlayingState = new PlayingState();
    [HideInInspector] public PausedState PausedState = new PausedState();
    [HideInInspector] public QuitState QuitState = new QuitState();

    // Gameplay variables

    [Header("Happiness")]
    public int Happiness;
    public bool isHappendAnInteraction;

    int maxHappiness;
    int minHappiness;

    [Header("Coins")]
    public int Coins;

    public static event Action<BaseGameState> OnGameStateChange;
    public static event Action<bool> OnChangeIdle;

    void Start()
    {
        Happiness = 50;
        maxHappiness = 100;
        minHappiness = 50;
        isHappendAnInteraction = false;

        InitStateAndInvokeEvent(MainMenuState);
    }

    void Update()
    {
        state.UpdateState(this);
    }

    public void SwitchState(BaseGameState newState)
    {
        state.ExitState(this);

        InitStateAndInvokeEvent(newState);
    }

    public void IncreaseHappiness(int amountOfHappiness)
    {
        if (Happiness + amountOfHappiness <= maxHappiness)
        {
            Happiness += amountOfHappiness;
        } else
        {
            Happiness = maxHappiness;
        }
    }
    public void DecreaseHappiness(int amountOfHappiness)
    {
        if (Happiness - amountOfHappiness >= minHappiness)
        {
            Happiness -= amountOfHappiness;
        }
        else
        {
            Happiness = minHappiness;
        }
    }

    void InitStateAndInvokeEvent(BaseGameState initState)
    {
        state = initState;
        OnGameStateChange?.Invoke(state);
        state.EnterState(this);
    }

    public void InvokeOnSomeInteraction(bool isHappendAnAction)
    {
        isHappendAnInteraction = isHappendAnAction;
        OnChangeIdle?.Invoke(isHappendAnAction);
    }
}

