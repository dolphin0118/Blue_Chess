using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject SynergyUI;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject UserUI;

    public void SetShopUIActive(bool isActive)
    {
        if (isActive)
        {
            ShopUI.SetActive(true);
        }
        else
        {
            ShopUI.SetActive(false);
        }

    }
}
