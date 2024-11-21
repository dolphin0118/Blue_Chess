using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UnitDetailUI : UI_Base
{
    enum Texts
    {
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

    enum Images
    {
        UnitImage,
        Skill,
        TraitSynergy,
        SchoolSynergy,
        Item_1,
        Item_2,
        Item_3
    }

    private UnitCard UnitCard;

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));  // 텍스트 오브젝트들 가져와 dictionary인 _objects에 바인딩. 
        Bind<Image>(typeof(Images));
    }

    public void CardEnable(UnitCard UnitCard)
    {
        this.UnitCard = UnitCard;
        ImageSetup();
        TextSetup();
        StatSetup();
    }

    void StatSetup()
    {
        GetText((int)Texts.HP).text = UnitCard.UnitStat.HP.ToString();
        GetText((int)Texts.MP).text = UnitCard.UnitStat.MP.ToString();
        GetText((int)Texts.ATK).text = UnitCard.UnitStat.ATK.ToString();
        GetText((int)Texts.AP).text = UnitCard.UnitStat.AP.ToString();
        GetText((int)Texts.AR).text = UnitCard.UnitStat.AR.ToString();
        GetText((int)Texts.MR).text = UnitCard.UnitStat.MR.ToString();
        GetText((int)Texts.ATKSpeed).text = UnitCard.UnitStat.AR.ToString();
        GetText((int)Texts.Range).text = UnitCard.UnitStat.Range.ToString();
    }

    void ImageSetup()
    {
        GetText((int)Texts.Name).text = UnitCard.Name;
        GetText((int)Texts.Price).text = UnitCard.UnitData.UnitPrice.ToString();
        GetText((int)Texts.TraitName).text = UnitCard.UnitData.traitSynergy.ToString();
        GetText((int)Texts.SchoolName).text = UnitCard.UnitData.schoolSynergy.ToString();
    }

    void TextSetup()
    {
        string symbolPath = "Unit/Symbol/";
        GetImage((int)Images.UnitImage).sprite = UnitCard.UnitMemorial;
        GetImage((int)Images.TraitSynergy).sprite = Resources.Load(symbolPath + UnitCard.UnitData.traitSynergy, typeof(Sprite)) as Sprite;
        GetImage((int)Images.SchoolSynergy).sprite = Resources.Load(symbolPath + UnitCard.UnitData.schoolSynergy, typeof(Sprite)) as Sprite;

        //traitSymbol.transform.SetAsLastSibling();
        //schoolSymbol.transform.SetAsLastSibling();
    }
}
