using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CardUI : MonoBehaviour{
    [SerializeField] TextMeshProUGUI UnitNameText;
    [SerializeField] TextMeshProUGUI UnitPriceText;
    [SerializeField] GameObject traitSynergy;
    [SerializeField] GameObject schoolSynergy;
    [SerializeField] Image UnitImage;
    private Image traitSymbol;
    private Image schoolSymbol;
    private TextMeshProUGUI traitName;
    private TextMeshProUGUI schoolName;
    
    private UnitCard UnitCard;

    void Start() {
        UnitImage = this.GetComponent<Image>();
        traitSymbol = traitSynergy.GetComponent<Image>();
        schoolSymbol = schoolSynergy.GetComponent<Image>();
    }
    
    public void CardEnable(UnitCard UnitCard) {
        this.UnitCard = UnitCard;
        CardSetup();
        SynergySetup();
    }

    void CardSetup() {
        UnitImage.sprite = UnitCard.UnitMemorial;
        UnitNameText.text = UnitCard.Name;
        UnitPriceText.text = UnitCard.UnitData.UnitPrice.ToString();
    }

    void SynergySetup() {
        string symbolPath = "Unit/Symbol/";
        string traitName = UnitCard.UnitData.traitSynergy.ToString();
        string schoolName = UnitCard.UnitData.schoolSynergy.ToString();

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
