using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI playerHpText;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] Image hpGauge;
    int playerHp;
    string playerName;

    void Update()
    {
        playerHp = playerData.playerHp;
        playerName = playerData.playerName;
        playerHpText.text = playerHp.ToString();
        playerNameText.text = playerName;
        hpGauge.fillAmount = playerHp / 20f;
    }
}
