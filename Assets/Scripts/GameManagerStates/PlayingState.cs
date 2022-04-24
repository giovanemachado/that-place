using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayingState : BaseGameState
{
    int decreaseHappinessAmountByTime = 10;
    int timeToDecreaseHappiness = 30;
    float timerHappiness = 0;
    int secondsHappiness = 0;

    int increaseCoinsAmountByTime = 1;
    int timeToIncreaseCoins = 3;
    float timerCoins = 0;
    int secondsCoins = 0;

    float timerPersonEvents = 0;
    int secondsPersonEvents = 0;

    float timerBuildingEvents = 0;
    int secondsBuildingEvents = 0;

    int timingForNewPersonEvents = 1;
    int timingForNewBuildingEvents = 1;

    public override void EnterState(GameManager gameManager)
    {
        GameManager gameManagerInstance = GameManager.Instance;

        // First building 
        Vector3 position = gameManagerInstance.FirstBuildSpot.transform.position;
        position.y += gameManagerInstance.FixBuildingPositionToInstantiateYAmount;
        Quaternion rotation = Quaternion.identity;
        GameObject firstBuilding = GameManager.Instantiate(gameManagerInstance.HouseBuilding, position, rotation);
        gameManagerInstance.FirstBuildSpot.SetActive(false);
        firstBuilding.transform.parent = gameManagerInstance.EnvironmentBuildings.transform;
        gameManagerInstance.InstantiatedBuildings.Add(firstBuilding);

        var sprite = firstBuilding.GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Buildings Back";

        // First people
        gameManagerInstance.InstantiatePeople();
    }

    public override void UpdateState(GameManager gameManager)
    {
        CheckDecreasingHappiness(gameManager);
        CheckIncreasingMoney(gameManager);
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
            GameManager.Instance.IncreaseCoinsWithoutBonus(increaseCoinsAmountByTime);
        }
    }

    void CheckForNewEvents(GameManager gameManager)
    {
        timerPersonEvents += Time.deltaTime;
        secondsPersonEvents = (int)(timerPersonEvents % 60);

        timerBuildingEvents += Time.deltaTime;
        secondsBuildingEvents = (int)(timerBuildingEvents % 60);

        if (secondsPersonEvents > timingForNewPersonEvents)
        {
            timerPersonEvents = 0;
            GameManager.Instance.InstantiateSpeechBubblePersonEvent();
            timingForNewPersonEvents = UnityEngine.Random.Range(15, 25);
        }

        if (secondsBuildingEvents > timingForNewBuildingEvents)
        {
            timerBuildingEvents = 0;
            GameManager.Instance.InstantiateSpeechBubbleBuildingEvent();
            timingForNewBuildingEvents = UnityEngine.Random.Range(15, 25);
        }
    }
}