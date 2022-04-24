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
    [Header("People")]
    public int People;
    int maxPeople = 50;

    public GameObject PeopleGO;
    public GameObject PeopleSpawner;
    public GameObject EnvironmentPeople;
    public List<GameObject> InstantiatedPeople = new List<GameObject>();

    [Header("Happiness")]
    [HideInInspector] public bool IsHappendAnInteraction;
    public int Happiness;
    int maxHappiness = 100;
    int minHappiness = 50;

    int increaseHappinessByAction = 5;

    [Header("Coins")]
    public int Coins;
    int maxCoins = 100;
    int minCoins = 0;

    public int HouseBuildingCoinCost;
    public int FunBuildingCoinCost;
    public int CoinBuildingCoinCost;

    int increasePeopleAmountByHouseBuilding = 5;
    int increaseCoinAmountByCoinBuilding = 1;
    int increaseHappinessAmountByFunBuilding = 1;

    int bonusCoinsByHappinessLevel5 = 4;
    int bonusCoinsByHappinessLevel4 = 3;
    int bonusCoinsByHappinessLevel3 = 2;
    int bonusCoinsByHappinessLevel2 = 1;
    int bonusCoinsByHappinessLevel1 = 0;

    [Header("Editor")]
    public List<GameObject> InstantiatedBuildings = new List<GameObject>();
    public GameObject HouseBuilding;
    public GameObject FunBuilding;
    public GameObject CoinBuilding;
    [HideInInspector] public float FixBuildingPositionToInstantiateYAmount = 0.35f;

    public GameObject FirstBuildSpot;

    int currentHouseBuildings = 0;
    int currentFunBuildings = 0;
    int currentCoinBuildings = 0;

    int maxHouseBuildings = 9;
    int maxFunBuildings = 4;
    int maxCoinBuildings = 4;

    [HideInInspector] public GameObject EditingSpot = null;

    [HideInInspector]
    public enum BuildingType
    {
        HOUSE,
        FUN,
        COIN
    }

    public GameObject EnvironmentBuildings;

    // Gameplay Events
    public struct GameplayEvent
    {
        public string description;
        public EventType eventType;

        public GameplayEvent(string description, EventType eventType)
        {
            this.description = description;
            this.eventType = eventType;
        }
    }

    [HideInInspector]
    public enum EventType
    {
        SPEECH_BUBBLE,
        WEATHER,
        PEOPLE
    }

    [HideInInspector]
    public enum SpeechBubbleType 
    {
        PERSON,
        HOUSE_BUILDING,
        FUN_BUILDING,
        COIN_BUILDING
    }

    [HideInInspector] public GameObject SpeechBubbleSelected;


    // Events
    public static event Action<BaseGameState> OnGameStateChange;
    public static event Action<bool> OnChangeIdle;
    public static event Action<GameObject> OnClickBuildingSpot;

    public static event Action OnHouseBuildingBuilded;
    public static event Action OnFunBuildingBuilded;
    public static event Action OnCoinBuildingBuilded;

    public static event Action<string> OnPlayerError;
    public static event Action<GameplayEvent> OnGameplayEventAppears;

    public static event Action OnIncreasingPeople;
    public static event Action OnObtainCoins;


    void Start()
    {  
        EditingSpot = null;

        InitStateAndInvokeEvent(MainMenuState);
    }

    void Update()
    {
        state.UpdateState(this);
    }

    // State machine
    public void SwitchState(BaseGameState newState)
    {
        state.ExitState(this);

        InitStateAndInvokeEvent(newState);
    }

    void InitStateAndInvokeEvent(BaseGameState initState)
    {
        state = initState;
        OnGameStateChange?.Invoke(state);
        state.EnterState(this);
    }

    // Happiness mechanic
    public void IncreaseHappiness(int amountOfHappiness)
    {
        // Happiness bonus == 1 * fun buildings
        amountOfHappiness += increaseHappinessAmountByFunBuilding * currentFunBuildings;

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

    public void InvokeOnSomeInteraction(bool isHappendAnAction)
    {
        if (isHappendAnAction) IncreaseHappiness(increaseHappinessByAction);

        IsHappendAnInteraction = isHappendAnAction;
        OnChangeIdle?.Invoke(isHappendAnAction);
    }

    // Coins mechanic
    public void IncreaseCoins(int amountOfCoins)
    {
        // Bonus == 1 * coin buildings + bonus coin by happiness
        amountOfCoins += increaseCoinAmountByCoinBuilding * currentCoinBuildings;
        amountOfCoins += BonusCoinsByHappiness();

        IncreaseCoinsWithoutBonus(amountOfCoins);

        OnObtainCoins?.Invoke();
    }

    public void IncreaseCoinsWithoutBonus (int amountOfCoins)
    {
        if (Coins + amountOfCoins <= maxCoins)
        {
            Coins += amountOfCoins;
        }
        else
        {
            Coins = maxCoins;
        }
    }

    public void DecreaseCoins(int amountOfCoins)
    {
        if (Coins - amountOfCoins >= minCoins)
        {
            Coins -= amountOfCoins;
        }
        else
        {
            Coins = minCoins;
        }
    }

    public bool HasCoinsToDoThis(int cost)
    {
        return Coins >= cost;
    }

    int BonusCoinsByHappiness()
    {
        if (Happiness > 90) return bonusCoinsByHappinessLevel5;
        if (Happiness > 80) return bonusCoinsByHappinessLevel4;
        if (Happiness > 70) return bonusCoinsByHappinessLevel3;
        if (Happiness > 60) return bonusCoinsByHappinessLevel2;
       
        return bonusCoinsByHappinessLevel1;
    }

    // People mechanic
    public void IncreasePeople(int amountOfPeople)
    {
        if (People + amountOfPeople <= maxPeople)
        {
            People += amountOfPeople;
        }
        else
        {
            People = maxPeople;
        }

        OnIncreasingPeople?.Invoke();
    }

    public void InstantiatePeople()
    {
        GameObject newPerson = Instantiate(PeopleGO, PeopleSpawner.transform.position, Quaternion.identity);

        newPerson.transform.parent = EnvironmentPeople.transform;
        InstantiatedPeople.Add(newPerson);
    }

    // Building mechanic
    public void InvokeOnClickBuildingSpot(GameObject buildingSpotClicked)
    {
        OnClickBuildingSpot?.Invoke(buildingSpotClicked);
    }

    public void AddABuildAndInvokeOnBuilding(BuildingType buildingType)
    {
        GameObject newBuilding = null;
        Vector3 position = EditingSpot.transform.position;
        position.y += FixBuildingPositionToInstantiateYAmount;

        Quaternion rotation = Quaternion.identity;

        var sprite = HouseBuilding.GetComponent<SpriteRenderer>();

        if (CheckIfBuildingIsBack(EditingSpot.name))
        {
            sprite.sortingLayerName = "Buildings Front";
        } else
        {
            sprite.sortingLayerName = "Buildings Back";
        }

        if (buildingType == BuildingType.HOUSE)
        {
            DecreaseCoins(HouseBuildingCoinCost);
            currentHouseBuildings++;
            IncreasePeople(increasePeopleAmountByHouseBuilding);
            InstantiatePeople();
            newBuilding = Instantiate(HouseBuilding, position, rotation);
            OnHouseBuildingBuilded?.Invoke();
        }

        if (buildingType == BuildingType.FUN)
        {
            DecreaseCoins(FunBuildingCoinCost);
            currentFunBuildings++;
            newBuilding = Instantiate(FunBuilding, position, rotation);
            OnFunBuildingBuilded?.Invoke();
        }

        if (buildingType == BuildingType.COIN)
        {
            DecreaseCoins(CoinBuildingCoinCost);
            currentCoinBuildings++;
            newBuilding = Instantiate(CoinBuilding, position, rotation);
            OnCoinBuildingBuilded?.Invoke();
        }

        InstantiatedBuildings.Add(newBuilding);
        newBuilding.transform.parent = EnvironmentBuildings.transform;

        EditingSpot.SetActive(false);
        EditingSpot = null;

        InvokeOnSomeInteraction(true);
    }

    public bool CanCreateBuilding(BuildingType buildingType)
    {
        int maxBuildingsOfType = 0;
        int currentBuildingsOfType = 0;

        if (buildingType == BuildingType.HOUSE)
        {
            currentBuildingsOfType = currentHouseBuildings;
            maxBuildingsOfType = maxHouseBuildings;
        }

        if (buildingType == BuildingType.FUN)
        {
            currentBuildingsOfType = currentFunBuildings;
            maxBuildingsOfType = maxFunBuildings;
        }

        if (buildingType == BuildingType.COIN)
        {
            currentBuildingsOfType = currentCoinBuildings;
            maxBuildingsOfType = maxCoinBuildings;
        }

        return currentBuildingsOfType + 1 <= maxBuildingsOfType;
    }

    // Gameplay Events mechanic
    public void InvokeOnGameplayEventAppears(GameplayEvent gameplayEvent)
    {
        OnGameplayEventAppears?.Invoke(gameplayEvent);
    }

    public void InstantiateSpeechBubbleBuildingEvent()
    {
        List<GameObject> buildingsList = InstantiatedBuildings;
        GameObject randomBuilding = buildingsList[UnityEngine.Random.Range(0, buildingsList.Count)];
        GameObject speechBubble = randomBuilding.transform.GetChild(1).gameObject;

        speechBubble.SetActive(true);

        SpeechBubbleSelected = speechBubble;

        StartCoroutine(FinishSpeechBubbleEvent(speechBubble));
    }

    public void InstantiateSpeechBubblePersonEvent()
    {
        List<GameObject> peopleList = InstantiatedPeople;
        GameObject randomPerson = peopleList[UnityEngine.Random.Range(0, peopleList.Count)];
        GameObject speechBubble = randomPerson.transform.GetChild(1).gameObject;

        speechBubble.SetActive(true);

        SpeechBubbleSelected = speechBubble;

        StartCoroutine(FinishSpeechBubbleEvent(speechBubble));
    }

    IEnumerator FinishSpeechBubbleEvent(GameObject speechBubble)
    {
        yield return new WaitForSeconds(10);
        speechBubble.SetActive(false);
    }

    public void InvokeOnClickSpeechBubble(GameObject speechBubble, SpeechBubbleType speechBubbleType)
    {
        bool isAHappinesSpeechBubble = speechBubbleType == SpeechBubbleType.PERSON || speechBubbleType == SpeechBubbleType.FUN_BUILDING;
        bool isACoinSpeechBubble = speechBubbleType == SpeechBubbleType.HOUSE_BUILDING || speechBubbleType == SpeechBubbleType.COIN_BUILDING;

        if (isAHappinesSpeechBubble)
        {
            IncreaseHappiness(1);
        }

        if (isACoinSpeechBubble)
        {
            IncreaseCoins(1);
        }

        speechBubble.SetActive(false);
    }

    // General
    public void InvokeOnPlayerErrorWithMessage  (string error)
    {
        OnPlayerError?.Invoke(error);
    }

    bool CheckIfBuildingIsBack(string buildingName)
    {
        bool resp = false;
        string[] frontSpots = new string[] { "9", "10", "11", "12", "13", "14", "15", "16", "17" };
        for(int i = 0; i < frontSpots.Length; i++)
        {
            if (buildingName.Contains(frontSpots[i]))
            {
                resp = true;
                break;
            }
        }
        
        return resp;
    }
}

