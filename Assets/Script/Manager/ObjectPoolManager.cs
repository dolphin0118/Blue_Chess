using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;
    public Dictionary<string, ObjectPool<GameObject>> multiPool = new Dictionary<string, ObjectPool<GameObject>>();

    void Awake()
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

        PoolSetup();
    }

    private void PoolSetup()
    {
        UnitCard[] UnitCards = Resources.LoadAll<UnitCard>("Scriptable");
        foreach (var unit in UnitCards)
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(unit.UnitPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            defaultCapacity: 10,
            maxSize: 20
            );
            multiPool.Add(unit.Name, pool);
        }
    }
}
