using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayingState : BaseGameState
{
    int decreaseHappinessAmountByTime = 1;
    int timeToDecrease = 12;
    float timer = 0.0f;
    int seconds = 0;

    public override void EnterState(GameManager gameManager)
    {
        //
    }

    public override void UpdateState(GameManager gameManager)
    {
        CheckDecreasingHappiness(gameManager);
    }

    void CheckDecreasingHappiness(GameManager gameManager)
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);

        // if some interaction happend, change timer to zero again
        if (gameManager.isHappendAnInteraction)
        {
            timer = 0;
            gameManager.isHappendAnInteraction = false;
        }

        // if nothing happends for timeToDecrease timing, change timer to zero again and decrease happiness
        if (seconds >= timeToDecrease)
        {
            timer = 0;
            GameManager.Instance.InvokeOnSomeInteraction(false);
            GameManager.Instance.DecreaseHappiness(decreaseHappinessAmountByTime);
        }
    }
}