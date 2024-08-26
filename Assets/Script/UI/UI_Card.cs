using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UI_Card : UI_Base
{
    enum Texts {
        Name,
        Price,
        TraitName,
        SchoolName,
        HP,
        MP,
        ATK,
        AP,
        AR,
        MR,
        ATKSpeed,
        Range,   
    }

    enum Images {
        CharaImage,
        Skill,
        TraitSynergy,
        SchoolSynergy,
        Item_1,
        Item_2,
        Item_3
    }

    private CharaCard charaCard;

    private void Start() {
        UIManager.instance.UIcard = this;
        Init();
        gameObject.SetActive(false);
    }

    public override void Init() {
		Bind<TextMeshProUGUI>(typeof(Texts));  // 텍스트 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
		Bind<Image>(typeof(Images)); 
    }

     public void CardEnable(CharaCard charaCard) {
        this.charaCard = charaCard;
        ImageSetup();
        TextSetup();
        StatSetup();
    }

    void StatSetup() {
        GetText((int)Texts.HP).text = charaCard.charaStat.HP.ToString();
        GetText((int)Texts.MP).text = charaCard.charaStat.MP.ToString();
        GetText((int)Texts.ATK).text = charaCard.charaStat.ATK.ToString();
        GetText((int)Texts.AP).text = charaCard.charaStat.AP.ToString();
        GetText((int)Texts.AR).text = charaCard.charaStat.AR.ToString();
        GetText((int)Texts.MR).text = charaCard.charaStat.MR.ToString();
        GetText((int)Texts.ATKSpeed).text = charaCard.charaStat.AR.ToString();
        GetText((int)Texts.Range).text = charaCard.charaStat.Range.ToString();
    }

    void ImageSetup() {    
        GetText((int)Texts.Name).text = charaCard.Name;
        GetText((int)Texts.Price).text = charaCard.charaData.charaPrice.ToString();
        GetText((int)Texts.TraitName).text = charaCard.charaData.traitSynergy.ToString();
        GetText((int)Texts.SchoolName).text = charaCard.charaData.schoolSynergy.ToString();
    }

    void TextSetup() {
        string symbolPath = "Chara/Symbol/";
        GetImage((int)Images.CharaImage).sprite = charaCard.CharaMemorial;
        GetImage((int)Images.TraitSynergy).sprite = Resources.Load(symbolPath + charaCard.charaData.traitSynergy,typeof(Sprite)) as Sprite;
        GetImage((int)Images.SchoolSynergy).sprite = Resources.Load(symbolPath + charaCard.charaData.schoolSynergy,typeof(Sprite)) as Sprite;

        //traitSymbol.transform.SetAsLastSibling();
       //schoolSymbol.transform.SetAsLastSibling();
    }
}
