using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using GoogleSheet.Protocol.v2.Res;
using GoogleSheet.Protocol.v2.Req;
using UGS;
using System;
using UGS.IO;
using GoogleSheet;
using System.IO;
using GoogleSheet.Type;
using System.Reflection;
using AutoMapper;
using BlueChessDataBase;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<string> UnitList;
    public PhotonView photonView;
    [System.NonSerialized] public static bool isBattle = false;
    public Tilemap tilemap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this) Destroy(this.gameObject);
        }
        photonView = GetComponent<PhotonView>();
        UnityGoogleSheet.LoadAllData();
        DataBind();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            photonView.RPC("BattleControll", RpcTarget.All, true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            photonView.RPC("BattleControll", RpcTarget.All, false);
        }

    }

    [PunRPC]
    void BattleControll(bool isBattle)
    {

        GameManager.isBattle = isBattle;
    }

    void DataBind()
    {
        //ItemDataBind();
        UnitDataBind();
    }

    ///////////////// -----------ItemData Setting-------------------------//////////

    void ItemDataBind()
    {
        var itemConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BlueChessDataBase.Item, ItemData>();
        });

        var itemMapper = itemConfig.CreateMapper();

        var item = BlueChessDataBase.Item.ItemList;

        for (int i = 0; i < item.Count; i++)
        {
            ItemData itemData = itemMapper.Map<ItemData>(item[i]);
            CreateItemAsset(itemData);
        }
    }

    void CreateItemAsset(ItemData itemData)
    {
        string Name = itemData.Name;
        string path = "Assets/Resources/Item/Scriptable/" + Name + ".asset";

        ItemAsset previousItem = AssetDatabase.LoadAssetAtPath<ItemAsset>(path);
        ItemAsset currentItem = ScriptableObject.CreateInstance<ItemAsset>();

        currentItem.ItemPrefab = Resources.Load("Item/Prefab/" + Name) as GameObject;
        currentItem.ItemImage = Resources.Load("Item/Sprites/" + Name, typeof(Sprite)) as Sprite;
        currentItem.ItemData = itemData;
        currentItem.Name = Name;


        if (previousItem != null)
        {
            if (CheckChangeData(currentItem, previousItem))
            {
                return;
            }
            else
            {
                bool deleteSuccess = AssetDatabase.DeleteAsset(path);
                if (!deleteSuccess)
                {
                    Debug.LogError("Failed to delete existing asset at " + path);
                    return;
                }
            }
        }
        AssetDatabase.CreateAsset(currentItem, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //////////----------------------UnitData-----------------------------/////////
    void UnitDataBind()
    {
        var dataConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BlueChessDataBase.Data, UnitData>();
        });
        var statConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BlueChessDataBase.Stat, UnitStat>();
        });

        var dataMapper = dataConfig.CreateMapper();
        var statMapper = statConfig.CreateMapper();

        var Data = BlueChessDataBase.Data.DataList;
        var Stat = BlueChessDataBase.Stat.StatList;
        for (int i = 0; i < Data.Count; i++)
        {
            UnitData UnitData = dataMapper.Map<UnitData>(Data[i]);
            UnitStat UnitStat = statMapper.Map<UnitStat>(Stat[i]);
            // ※유닛 데이터베이스 동기화
            // CreateUnitAsset(UnitData, UnitStat);

            UnitList.Add(UnitData.Name);

        }

    }



    void CreateUnitAsset(UnitData UnitData, UnitStat UnitStat)
    {
        string UnitName = UnitData.Name;
        string path = "Assets/Resources/Scriptable/" + UnitName + ".asset";
        UnitList.Add(UnitName);

        UnitCard previousUnit = AssetDatabase.LoadAssetAtPath<UnitCard>(path);
        UnitCard currentUnit = ScriptableObject.CreateInstance<UnitCard>();

        currentUnit.UnitPrefab = Resources.Load("Unit/Prefab/" + UnitName) as GameObject;
        currentUnit.UnitMemorial = Resources.Load("Unit/Memorial/" + UnitName + "_memo", typeof(Sprite)) as Sprite;
        currentUnit.UnitData = UnitData;
        currentUnit.UnitStat = UnitStat;
        currentUnit.Name = UnitName;


        if (previousUnit != null)
        {
            if (CheckChangeData(currentUnit, previousUnit))
            {
                return;
            }
            else
            {
                bool deleteSuccess = AssetDatabase.DeleteAsset(path);
                if (!deleteSuccess)
                {
                    Debug.LogError("Failed to delete existing asset at " + path);
                    return;
                }
            }
        }
        AssetDatabase.CreateAsset(currentUnit, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    //---------------------------------------------------------------------------------------//

    private bool CheckChangeData<T>(T currentData, T previousData)
    {
        FieldInfo[] currentFields = currentData.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo[] previousFields = previousData.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < currentFields.Length; i++)
        {
            var currentValue = currentFields[i].GetValue(currentData);
            var previousValue = previousFields[i].GetValue(previousData);

            if (currentValue == null || previousValue == null)
            {
                return false;
            }
            else if (currentValue.GetType().Namespace == "BlueChessDataBase")
            {
                bool ischeck = CheckChangeData(currentValue, previousValue);
                if (!ischeck)
                {
                    return false;
                }
            }
            else if (!Equals(currentValue, previousValue))
            {

                return false;
            }

        }
        return true;
    }
}
