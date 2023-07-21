using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSystem : MonoBehaviour {
    public Sprite[] Level_sprites;
    public Image Level_Image;

    void Awake() {
        Level_Image = this.transform.GetComponent<Image>();
    }
    void Update() {
        swap_sprite();
    }
    void swap_sprite() {
        int Chara_Level = GetComponentInParent<CharaInfo>().Player_Level;
        Level_Image.sprite = Level_sprites[Chara_Level];
    }
}
