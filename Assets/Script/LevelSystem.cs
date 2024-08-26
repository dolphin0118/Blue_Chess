using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSystem : MonoBehaviour {
    public Sprite[] Level_sprites;
    Image Level_Image;

    void Awake() {
        Level_Image = this.transform.GetComponent<Image>();
    }

    void Update() {
        swapSprite();
    }

    void swapSprite() {
        int Chara_Level = GetComponentInParent<CharaInfo>().charaStat.Level;
        Level_Image.sprite = Level_sprites[Chara_Level - 1];
    }
}
