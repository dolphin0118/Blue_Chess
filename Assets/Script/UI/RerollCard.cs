using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LevelingSystem : MonoBehaviour, IPointerClickHandler {
    private int playerLevel;
    int[,] levelTopercent = {
        {100, 0, 0, 0, 0},
        {100, 0, 0, 0, 0},
        {75, 25, 0, 0, 0},
        {55, 30, 15, 0, 0},
        {45, 33, 20, 2, 0},
        {25, 40, 30, 5, 0},
        {19, 30, 35, 15, 1},
        {16, 20, 35, 25, 4},
    };
    int[] levelToXp = {2, 2, 6, 10, 24, 40, 60};
    void Start() {
        
    }

    void Update() {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        int playerGold = 100;
        

    }
}

public class RerollCard : MonoBehaviour, IPointerClickHandler {
    private int playerLevel;
    int[,] levelTopercent = {
        {100, 0, 0, 0, 0},
        {100, 0, 0, 0, 0},
        {75, 25, 0, 0, 0},
        {55, 30, 15, 0, 0},
        {45, 33, 20, 2, 0},
        {25, 40, 30, 5, 0},
        {19, 30, 35, 15, 1},
        {16, 20, 35, 25, 4},
    };
    void Start() {
        
    }

    void Update() {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        int playerGold = 100;
        

    }
}
