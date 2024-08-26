using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SpawnCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [SerializeField] TextMeshProUGUI charaNameText;
    [SerializeField] TextMeshProUGUI charaPriceText;
    [SerializeField] GameObject traitSynergy;
    [SerializeField] GameObject schoolSynergy;
    private Image traitSymbol;
    private Image schoolSymbol;
    private TextMeshProUGUI traitName;
    private TextMeshProUGUI schoolName;
    
    private Color initColor;
    private Image charaImage;
    private CharaCard charaCard;
    private bool mouse_On;
    private bool isSpawn;
    private bool isReroll;

    void Start() {
        initColor = this.GetComponent<Image>().color;
        charaImage = this.GetComponent<Image>();
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
        mouse_On = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        this.GetComponent<Image>().color = initColor;
        mouse_On = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        CardSpawn();
    }

    void CardInit() {
        isSpawn = true;
        isReroll = true;
    }

    void CardSetup() {
        int chardCount = SpawnSystem.instance.charaCards.Length;
        int selectCard = Random.Range(0, chardCount);
        charaCard = SpawnSystem.instance.charaCards[selectCard];
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
        string symbolPath = "Chara/Symbol/";
        string traitName = charaCard.charaData.traitSynergy.ToString();
        string schoolName = charaCard.charaData.schoolSynergy.ToString();

        traitSymbol.sprite = Resources.Load(symbolPath + traitName,typeof(Sprite)) as Sprite;
        schoolSymbol.sprite = Resources.Load(symbolPath + schoolName,typeof(Sprite)) as Sprite;
        this.traitName.text = traitName;
        this.schoolName.text = schoolName;
    }

    void CardSpawn() {
        isSpawn = SpawnSystem.instance.isSpawnable();
        if(isSpawn) {
            SpawnSystem.instance.SpawnChara(charaCard);
            CardDisable();
        }
    }  

    void CardEnable() {
        isSpawn = true;
        charaImage.sprite = charaCard.CharaMemorial;
        charaNameText.text = charaCard.Name;
        charaPriceText.text = charaCard.charaData.charaPrice.ToString();
        traitSynergy.SetActive(true);
        schoolSynergy.SetActive(true);
    }

    void CardDisable() {
        isSpawn = false;
        charaImage.sprite = null;
        charaNameText.text = charaCard.Name;
        charaPriceText.text = charaCard.charaData.charaPrice.ToString();
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
