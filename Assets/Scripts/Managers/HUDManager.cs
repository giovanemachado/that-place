using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI PeopleText;
    string peopleDefaultText = "People:";
    public TextMeshProUGUI HappinessText;
    string happinessDefaultText = "Happiness:";
    public TextMeshProUGUI CoinsText;
    string coinsDefaultText = "Coins:";

    [Header("Editor")]
    public GameObject Editor;

    public TextMeshProUGUI BuildingTypeText;

    string buildingTypeHouse = "House";
    string buildingTypeFunBuilding = "Fun Building";
    string buildingTypeCoinBuilding = "Coin Building";

    string buildingTypeHouseDescription = "home of someone";
    string buildingTypeFunBuildingDescription = "a place to have some fun";
    string buildingTypeCoinBuildingDescription = "where Coins appears";

    public TextMeshProUGUI DescriptionBuildingText;
    string descriptionBuildingDefault = "Description:";

    public GameObject ConfirmBuildingButton;
    public GameObject CancelBuildingButton;

    Color SelectedBuildingSpot = new Color(0, 0, 0, 0.7f);
    Color UnselectedBuildingSpot = new Color(0, 0, 0, 0.3f);

    GameManager.BuildingType buildingSelectedType;

    void Awake()
    {
        GameManager.OnClickBuildingSpot += GameManagerOnClickBuildingSpot;
    }

    void OnDestroy()
    {
        GameManager.OnClickBuildingSpot -= GameManagerOnClickBuildingSpot;
    }

    void Update()
    {
        PeopleText.text = $"{peopleDefaultText} {GameManager.Instance.People}";
        HappinessText.text = $"{happinessDefaultText} { GameManager.Instance.Happiness}";
        CoinsText.text = $"{coinsDefaultText} {GameManager.Instance.Coins}";
    }

    public void GameManagerOnClickBuildingSpot(GameObject buildingSpot)
    {
        DeselectBuldingSpot();

        SpriteRenderer sprite = buildingSpot.GetComponent<SpriteRenderer>();
        sprite.color = SelectedBuildingSpot;

        GameManager.Instance.EditingSpot = buildingSpot;
        Editor.SetActive(true);
    }

    public void CloseEditorButtonPressed()
    {
        DeselectBuldingSpot();
        Editor.SetActive(false);
    }

    void DeselectBuldingSpot()
    {
        GameObject buildingSpot = GameManager.Instance.EditingSpot;

        if (buildingSpot == null) return;

        SpriteRenderer sprite = buildingSpot.GetComponent<SpriteRenderer>();
        sprite.color = UnselectedBuildingSpot;


        BuildingTypeText.text = "";
        DescriptionBuildingText.text = "";

        ShouldShowConfirmCancelBuildingButton(false);

        GameManager.Instance.EditingSpot = null;
    }

    public void HouseEditorButtonPressed()
    {
        BuildingTypeText.text = $"{buildingTypeHouse}";
        DescriptionBuildingText.text = $"{descriptionBuildingDefault} {buildingTypeHouseDescription}";
        buildingSelectedType = GameManager.BuildingType.HOUSE;

        ShouldShowConfirmCancelBuildingButton(true);
    }

    public void FunEditorButtonPressed()
    {
        BuildingTypeText.text = $"{buildingTypeFunBuilding}";
        DescriptionBuildingText.text = $"{descriptionBuildingDefault} {buildingTypeFunBuildingDescription}";
        buildingSelectedType = GameManager.BuildingType.FUN;

        ShouldShowConfirmCancelBuildingButton(true);
    }

    public void CoinEditorButtonPressed()
    {
        BuildingTypeText.text = $"{buildingTypeCoinBuilding}";
        DescriptionBuildingText.text = $"{descriptionBuildingDefault} {buildingTypeCoinBuildingDescription}";
        buildingSelectedType = GameManager.BuildingType.COIN;

        ShouldShowConfirmCancelBuildingButton(true);
    }

    void ShouldShowConfirmCancelBuildingButton(bool should)
    {
        ConfirmBuildingButton.SetActive(should);
        CancelBuildingButton.SetActive(should);
    }

    public void ConfirmBuildingButtonPressed() {
        int cost = 0;

        if (buildingSelectedType == GameManager.BuildingType.HOUSE)
        {
            cost = GameManager.Instance.HouseBuildingCoinCost;
        }

        if (buildingSelectedType == GameManager.BuildingType.FUN)
        {
            cost = GameManager.Instance.FunBuildingCoinCost;
        }

        if (buildingSelectedType == GameManager.BuildingType.COIN)
        {
            cost = GameManager.Instance.CoinBuildingCoinCost;
        }

        if (!GameManager.Instance.HasCoinsToDoThis(cost))
        {
            GameManager.Instance.InvokeOnPlayerErrorWithMessage("I have no Coins for this now.");
            return;
        };

        if (!GameManager.Instance.CanCreateBuilding(buildingSelectedType))
        {
            GameManager.Instance.InvokeOnPlayerErrorWithMessage("I have too much of this kind of buildings now.");
            return;
        };

        GameManager.Instance.AddABuildAndInvokeOnBuilding(buildingSelectedType);
        DeselectBuldingSpot();

        Editor.SetActive(false);
    }

    public void CancelBuildingButtonPressed()
    {
        BuildingTypeText.text = "";
        DescriptionBuildingText.text = "";

        ShouldShowConfirmCancelBuildingButton(false);
    }
}
