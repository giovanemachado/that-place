using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayingState : BaseGameState
{
    int decreaseHappinessAmountByTime = 10;
    int timeToDecreaseHappiness = 30;
    float timerHappiness = 0.0f;
    int secondsHappiness = 0;

    int increaseCoinsAmountByTime = 1;
    int timeToIncreaseCoins = 3;
    float timerCoins = 0.0f;
    int secondsCoins = 0;

    float timerEvents = 0.0f;
    int secondsEvents = 0;

    public override void EnterState(GameManager gameManager)
    {
        // First building 
        Vector3 position = GameManager.Instance.FirstBuildSpot.transform.position;
        position.y += GameManager.Instance.FixBuildingPositionToInstantiateYAmount;
        Quaternion rotation = Quaternion.identity;
        GameManager.Instantiate(GameManager.Instance.HouseBuilding, position, rotation);
        GameManager.Instance.FirstBuildSpot.SetActive(false);
    }

    public override void UpdateState(GameManager gameManager)
    {
        CheckDecreasingHappiness(gameManager);
        // CheckIncreasingMoney(gameManager);
        CheckForNewEvents(gameManager);
    }

    void CheckDecreasingHappiness(GameManager gameManager)
    {
        timerHappiness += Time.deltaTime;
        secondsHappiness = (int)(timerHappiness % 60);

        // if some interaction happend, change timer to zero again
        if (gameManager.IsHappendAnInteraction)
        {
            timerHappiness = 0;
            gameManager.IsHappendAnInteraction = false;
        }

        // if nothing happends for timeToDecreaseHappiness timing, change timer to zero again and decrease happiness
        if (secondsHappiness >= timeToDecreaseHappiness)
        {
            timerHappiness = 0;
            GameManager.Instance.InvokeOnSomeInteraction(false);
            GameManager.Instance.DecreaseHappiness(decreaseHappinessAmountByTime);
        }
    }

    void CheckIncreasingMoney(GameManager gameManager)
    {
        timerCoins += Time.deltaTime;
        secondsCoins = (int)(timerCoins % 60);

        if (secondsCoins >= timeToIncreaseCoins)
        {
            timerCoins = 0;
            GameManager.Instance.IncreaseCoins(increaseCoinsAmountByTime);
        }
    }

    void CheckForNewEvents(GameManager gameManager)
    {
        timerEvents += Time.deltaTime;
        secondsEvents = (int)(timerEvents % 60);
    }
}