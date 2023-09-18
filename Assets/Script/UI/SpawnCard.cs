using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SpawnCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    Color initColor;
    Image Chara_Image;
    Chara Chara_Clone;
    bool mouse_On;
    bool isSpawn;
    
    void Start() {
        initColor = this.GetComponent<Image>().color;
        Chara_Image = this.GetComponent<Image>();
        Card_Init();
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

    void Card_Init() {
        isSpawn = true;
        int Select_Max = SpawnSystem.instance.Chara_List.Length;
        int Select_Card = Random.Range(0, Select_Max);
        Chara_Clone = SpawnSystem.instance.Chara_List[Select_Card];
        Chara_Image.sprite = Chara_Clone.Chara_Card;
    }

    void CardSpawn() {
        if(isSpawn) {
            isSpawn = false;
            SpawnSystem.instance.Spawn_Chara(Chara_Clone.Chara_Prefab);
            Chara_Image.sprite = null;
        }
    }
}
