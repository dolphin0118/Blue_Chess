using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TextMeshProUGUI playerHpText;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] Image hpGauge;
    int playerHp;
    string playerName;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerHp = playerController.playerHp;
        playerName = playerController.playerName;
        playerHpText.text = playerHp.ToString();
        playerNameText.text = playerName;
        hpGauge.fillAmount = playerHp / 20f;
    }
}
