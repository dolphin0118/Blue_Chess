using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;



public class PriceColor
{
    Color32 Price1 = new Color32(21, 40, 49, 255);
    Color32 Price2 = new Color32(12, 101, 45, 255);
    Color32 Price3 = new Color32(72, 190, 227, 255);
    Color32 Price4 = new Color32(211, 0, 215, 255);
    Color32 Price5 = new Color32(244, 163, 68, 255);

    public Color32 GetColor(int index)
    {
        Color32 resultColor = Price1;
        switch (index)
        {
            case 1:
                resultColor = Price1;
                break;
            case 2:
                resultColor = Price2;
                break;
            case 3:
                resultColor = Price3;
                break;
            case 4:
                resultColor = Price4;
                break;
            case 5:
                resultColor = Price5;
                break;
        }
        return resultColor;
    }
}

public class SpawnCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpawnSystem spawnSystem;
    private PlayerData playerData;

    [SerializeField] Image UnitImage;
    [SerializeField] TextMeshProUGUI UnitNameText;
    [SerializeField] TextMeshProUGUI UnitPriceText;
    [SerializeField] GameObject traitSynergy;
    [SerializeField] GameObject schoolSynergy;
    [SerializeField] Image attackImage;
    [SerializeField] Image defenseImage;

    private Image traitSymbol;
    private Image schoolSymbol;
    private TextMeshProUGUI traitName;
    private TextMeshProUGUI schoolName;

    [SerializeField] Image PriceBackground;
    [SerializeField] Image NameBackground;
    private PriceColor priceColor = new PriceColor();


    private Color initColor;
    private UnitCard unitCard;
    private bool isSpawn;
    private bool isReroll;

    void Start()
    {
        spawnSystem = GetComponentInParent<SpawnSystem>();
        playerData = GetComponentInParent<PlayerData>();
        initColor = UnitImage.color;


        Setup();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitImage.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitImage.color = initColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardSpawn();
    }

//-------------------------- Setup--------------------------//
    void Setup() {
        CardInit();
        CardSetup();
        SynergyInit();
        SynergySetup();
        TypeSetup();
    }

    void CardInit()
    {
        isSpawn = true;
        isReroll = true;
    }

    void CardSetup()
    {
        int chardCount = spawnSystem.UnitCards.Length;
        int selectCard = Random.Range(0, chardCount);
        unitCard = spawnSystem.UnitCards[selectCard];
        CardEnable();
    }

    void CardSetup2()
    {
        int[,] levelToPercent = {
        {100, 0, 0, 0, 0},
        {100, 0, 0, 0, 0},
        {75, 25, 0, 0, 0},
        {55, 30, 15, 0, 0},
        {45, 33, 20, 2, 0},
        {25, 40, 30, 5, 0},
        {19, 30, 35, 15, 1},
        {16, 20, 35, 25, 4},
        };

        int cardCount = spawnSystem.UnitCardTier[playerData.playerLevel].Count;
        int selectCard = Random.Range(0, 100);
        for (int i = 0; i < 5; i++)
        {
            int add = levelToPercent[playerData.playerLevel, i];
        }

        unitCard = spawnSystem.UnitCardTier[playerData.playerLevel][selectCard];
    }

    void SynergyInit()
    {
        traitSymbol = traitSynergy.GetComponentInChildren<Image>();
        schoolSymbol = schoolSynergy.GetComponentInChildren<Image>();
        traitName = traitSynergy.GetComponentInChildren<TextMeshProUGUI>();
        schoolName = schoolSynergy.GetComponentInChildren<TextMeshProUGUI>();
        traitSymbol.transform.SetAsLastSibling();
        schoolSymbol.transform.SetAsLastSibling();
    }

    void SynergySetup()
    {
        string symbolPath = "Unit/Symbol/";
        string traitName = unitCard.UnitData.traitSynergy.ToString();
        string schoolName = unitCard.UnitData.schoolSynergy.ToString();

        traitSymbol.sprite = Resources.Load(symbolPath + traitName, typeof(Sprite)) as Sprite;
        schoolSymbol.sprite = Resources.Load(symbolPath + schoolName, typeof(Sprite)) as Sprite;
        this.traitName.text = traitName;
        this.schoolName.text = schoolName;
    }

    void TypeSetup() {
        string TypePath = "Unit/Type/";
        string attackName = unitCard.UnitStat.attackType.ToString();
        string defenseName = unitCard.UnitStat.defenseType.ToString();
        attackImage.sprite = Resources.Load(TypePath + "Attack" + attackName, typeof(Sprite)) as Sprite;
        defenseImage.sprite = Resources.Load(TypePath + "Defense" + defenseName, typeof(Sprite)) as Sprite;
        
    }

//-------------------------- CardSpawn -------------------------------//
    void CardSpawn()
    {
        int unitPrice = unitCard.UnitData.UnitPrice;
        if (isSpawn && spawnSystem.isSpawnable() && unitPrice <= playerData.playerGold)
        {
            playerData.PayGold(unitPrice);
            //spawnSystem.SpawnUnit(unitCard.name);
            spawnSystem.SpawnUnitIncludePhotonView(unitCard.name);
            CardDisable();
        }
    }

    void CardEnable()
    {
        isSpawn = true;

        UnitImage.sprite = unitCard.UnitMemorial;
        UnitNameText.text = unitCard.Name;
        UnitPriceText.text = unitCard.UnitData.UnitPrice.ToString();
        PriceBackground.color = priceColor.GetColor(unitCard.UnitData.UnitPrice);
        NameBackground.color = priceColor.GetColor(unitCard.UnitData.UnitPrice);
        traitSynergy.SetActive(true);
        schoolSynergy.SetActive(true);
    }

    void CardDisable()
    {
        isSpawn = false;
        UnitImage.sprite = null;
        UnitNameText.text = unitCard.Name;
        UnitPriceText.text = unitCard.UnitData.UnitPrice.ToString();
        traitSynergy.SetActive(false);
        schoolSynergy.SetActive(false);
    }


    public void CardReroll()
    {
        CardSetup();
        SynergySetup();
    }
}
