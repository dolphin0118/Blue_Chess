using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CardUI : MonoBehaviour{
    [SerializeField] TextMeshProUGUI charaNameText;
    [SerializeField] TextMeshProUGUI charaPriceText;
    [SerializeField] GameObject traitSynergy;
    [SerializeField] GameObject schoolSynergy;
    [SerializeField] Image charaImage;
    private Image traitSymbol;
    private Image schoolSymbol;
    private TextMeshProUGUI traitName;
    private TextMeshProUGUI schoolName;
    
    private CharaCard charaCard;

    void Start() {
        charaImage = this.GetComponent<Image>();
        traitSymbol = traitSynergy.GetComponent<Image>();
        schoolSymbol = schoolSynergy.GetComponent<Image>();
    }
    
    public void CardEnable(CharaCard charaCard) {
        this.charaCard = charaCard;
        CardSetup();
        SynergySetup();
    }

    void CardSetup() {
        charaImage.sprite = charaCard.CharaMemorial;
        charaNameText.text = charaCard.Name;
        charaPriceText.text = charaCard.charaData.charaPrice.ToString();
    }

    void SynergySetup() {
        string symbolPath = "Chara/Symbol/";
        string traitName = charaCard.charaData.traitSynergy.ToString();
        string schoolName = charaCard.charaData.schoolSynergy.ToString();

        traitSymbol.sprite = Resources.Load(symbolPath + traitName,typeof(Sprite)) as Sprite;
        schoolSymbol.sprite = Resources.Load(symbolPath + schoolName,typeof(Sprite)) as Sprite;
        this.traitName.text = traitName;
        this.schoolName.text = schoolName;

        traitSymbol.transform.SetAsLastSibling();
        schoolSymbol.transform.SetAsLastSibling();
    }

    void StatusSetup() {
        
    }

}
