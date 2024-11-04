using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;

public class SpawnCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private SpawnSystem spawnSystem;

    [SerializeField] TextMeshProUGUI UnitNameText;
    [SerializeField] TextMeshProUGUI UnitPriceText;
    [SerializeField] GameObject traitSynergy;
    [SerializeField] GameObject schoolSynergy;
    private Image traitSymbol;
    private Image schoolSymbol;
    private TextMeshProUGUI traitName;
    private TextMeshProUGUI schoolName;
    
    private Color initColor;
    private Image UnitImage;
    private UnitCard unitCard;
    private bool isSpawn;
    private bool isReroll;

    void Start() {
        spawnSystem = GetComponentInParent<SpawnSystem>();
        initColor = this.GetComponent<Image>().color;
        UnitImage = this.GetComponent<Image>();
        CardInit();
        CardSetup();
        SynergyInit();
        SynergySetup();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R) && isReroll) {
            StartCoroutine(CardReroll());
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        this.GetComponent<Image>().color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData) {
        this.GetComponent<Image>().color = initColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        CardSpawn();
    }

    void CardInit() {
        isSpawn = true;
        isReroll = true;
    }

    void CardSetup() {
        int chardCount = spawnSystem.UnitCards.Length;
        int selectCard = Random.Range(0, chardCount);
        unitCard = spawnSystem.UnitCards[selectCard];
        CardEnable();
    }

    void SynergyInit() {
        traitSymbol = traitSynergy.GetComponentInChildren<Image>();
        schoolSymbol = schoolSynergy.GetComponentInChildren<Image>();
        traitName = traitSynergy.GetComponentInChildren<TextMeshProUGUI>();
        schoolName = schoolSynergy.GetComponentInChildren<TextMeshProUGUI>();
        traitSymbol.transform.SetAsLastSibling();
        schoolSymbol.transform.SetAsLastSibling();
    }

    void SynergySetup() {
        string symbolPath = "Unit/Symbol/";
        string traitName = unitCard.UnitData.traitSynergy.ToString();
        string schoolName = unitCard.UnitData.schoolSynergy.ToString();

        traitSymbol.sprite = Resources.Load(symbolPath + traitName,typeof(Sprite)) as Sprite;
        schoolSymbol.sprite = Resources.Load(symbolPath + schoolName,typeof(Sprite)) as Sprite;
        this.traitName.text = traitName;
        this.schoolName.text = schoolName;
    }

    void CardSpawn() {
        isSpawn = spawnSystem.isSpawnable();
        if(isSpawn) {
            //spawnSystem.SpawnUnit(unitCard.name);
            spawnSystem.SpawnUnitIncludePhotonView(unitCard.name);
            CardDisable();
        }
    }  

    void CardEnable() {
        isSpawn = true;
        UnitImage.sprite = unitCard.UnitMemorial;
        UnitNameText.text = unitCard.Name;
        UnitPriceText.text = unitCard.UnitData.UnitPrice.ToString();
        traitSynergy.SetActive(true);
        schoolSynergy.SetActive(true);
    }

    void CardDisable() {
        isSpawn = false;
        UnitImage.sprite = null;
        UnitNameText.text = unitCard.Name;
        UnitPriceText.text = unitCard.UnitData.UnitPrice.ToString();
        traitSynergy.SetActive(false);
        schoolSynergy.SetActive(false);
    }


    IEnumerator CardReroll() {
        isReroll = false;
        CardSetup();
        SynergySetup();
        yield return new WaitForSeconds(0.3f);
        isReroll = true;
    }
}
