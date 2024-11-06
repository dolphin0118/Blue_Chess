using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class UnitLocate : MonoBehaviour
{
    private PhotonView photonView;
    private TeamManager TeamManager;
    private SynergyManager SynergyManager;
    private Tilemap tilemap;
    private GameObject UnitLocateController;
    private GameObject previousParent;
    private Vector3 previousPos;
    private Quaternion benchRotate, battleRotate;
    private int battleLayer, benchLayer, unitLayer;

    void Start()
    {
        tilemap = GameManager.instance.tilemap;
        benchRotate = Quaternion.Euler(-20, 180, 0);
        battleRotate = Quaternion.Euler(0, 0, 0);
        unitLayer = 1 << LayerMask.NameToLayer("Unit");
        battleLayer = 1 << LayerMask.NameToLayer("Battle");
        benchLayer = 1 << LayerMask.NameToLayer("Bench");
        photonView = GetComponent<PhotonView>();
    }

    public void Initialize(TeamManager TeamManager, SynergyManager SynergyManager)
    {
        this.TeamManager = TeamManager;
        this.SynergyManager = SynergyManager;
        this.UnitLocateController = this.TeamManager.UnitLocateController;

    }


    public void ForceLocate()
    {
        if (UnitLocateController != null)
        {
            this.transform.SetParent(previousParent.transform);
            UnitLocateController.transform.localPosition = Vector3.zero;
        }
    }
    public bool IsBattleLayer() { return this.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"); }

    public void OnUnitControll()
    {
        previousPos = this.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitRay))
        {
            previousParent = this.transform.parent.gameObject;
            UnitLocateController.transform.position = hitRay.point;
            if (previousParent.layer == LayerMask.NameToLayer("Battle"))
            {
                //this.transform.GetComponent<UnitInfo>().SynergyRemove();
                photonView.RPC("UnitSynergyUpdate", RpcTarget.All, "Remove");
            }
            this.transform.SetParent(UnitLocateController.transform);
            this.transform.localPosition = new Vector3(0, 0.1f, 0);
        }
    }

    public void OnUnitMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Stage");
        if (Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, layerMask))
        {
            float H = Camera.main.transform.position.y;
            float h = UnitLocateController.transform.position.y;
            Vector3 newPos = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            UnitLocateController.transform.position = newPos;
        }
    }

    public void OnUnitUpdate()
    {
        NotIncludeTileCheckLayer();
        //CheckLayer();
        //OutLayer();
    }

    [PunRPC]
    public void UnitSynergyUpdate(string type)
    {
        if (type.Equals("Add"))
        {
            this.transform.GetComponent<UnitInfo>().SynergyAdd();
        }
        else if (type.Equals("Remove"))
        {
            this.transform.GetComponent<UnitInfo>().SynergyRemove();
        }
    }

    //BattleLocate
    [PunRPC]
    public void UnitLocateUpdate(int row, int col)
    {
        GameObject unitParent = TeamManager.BattleArea.transform.GetChild(row).GetChild(col).gameObject;
        this.transform.SetParent(unitParent.transform);
        this.transform.localPosition = new Vector3(0, 0.1f, 0);
        this.transform.rotation = benchRotate;

    }

    //BenchLocate
    [PunRPC]
    public void UnitLocateUpdate(int col)
    {
        GameObject unitParent = TeamManager.BenchArea.transform.GetChild(col).gameObject;
        this.transform.SetParent(unitParent.transform);
        this.transform.localPosition = new Vector3(0, 0.1f, 0);
        this.transform.rotation = benchRotate;
    }

    public void NotIncludeTileCheckLayer()
    {
        Vector3 currentPos = this.transform.position;
        //벤치레이어일 경우
        RaycastHit hitLayer;
        if (Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, benchLayer))
        {
            GameObject hitObject = hitLayer.collider.transform.gameObject;
            int row = hitObject.transform.parent.GetSiblingIndex();
            int col = hitObject.transform.GetSiblingIndex();
            if (hitObject.transform.childCount == 0)
            {
                previousParent = hitObject;
                this.transform.SetParent(previousParent.transform);
                this.transform.localPosition = new Vector3(0, 0.1f, 0);
                this.transform.rotation = benchRotate;
                photonView.RPC("UnitLocateUpdate", RpcTarget.Others, col);

            }
            //Swap 필요
            else
            {
                GameObject swapUnit = hitObject.transform.GetChild(0).gameObject;
                swapUnit.transform.SetParent(previousParent.transform);
                swapUnit.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                if (previousParent.layer == LayerMask.NameToLayer("Bench"))
                {
                    int swapcol = swapUnit.transform.parent.GetSiblingIndex();
                    photonView.RPC("UnitLocateUpdate", RpcTarget.Others, swapcol);//SwapUnit
                }
                else
                {
                    int swaprow = swapUnit.transform.parent.parent.GetSiblingIndex();
                    int swapcol = swapUnit.transform.parent.GetSiblingIndex();
                    photonView.RPC("UnitLocateUpdate", RpcTarget.Others, swaprow, swapcol);//SwapUnit
                }

                previousParent = hitObject;
                this.transform.SetParent(previousParent.transform);
                this.transform.localPosition = new Vector3(0, 0.1f, 0);
                this.transform.rotation = benchRotate;
                photonView.RPC("UnitLocateUpdate", RpcTarget.Others, col);//CurrentUnit

            }

        }
        //배틀레이어일 경우
        else if (Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, battleLayer))
        {
            GameObject hitObject = hitLayer.collider.transform.gameObject;
            int row = hitObject.transform.parent.GetSiblingIndex();
            int col = hitObject.transform.GetSiblingIndex();

            if (hitObject.transform.childCount == 0)
            {
                previousParent = hitObject;
                this.transform.SetParent(previousParent.transform);
                this.transform.localPosition = Vector3.zero;
                this.transform.rotation = battleRotate;
                //this.transform.GetComponent<UnitInfo>().SynergyAdd();
                photonView.RPC("UnitSynergyUpdate", RpcTarget.All, "Add");
                photonView.RPC("UnitLocateUpdate", RpcTarget.Others, row, col);//CurrentUnit
            }
            else
            {
                GameObject swapUnit = hitObject.transform.GetChild(0).gameObject;
                swapUnit.transform.SetParent(previousParent.transform);
                swapUnit.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                if (previousParent.layer == LayerMask.NameToLayer("Bench"))
                {
                    int swapcol = swapUnit.transform.parent.GetSiblingIndex();
                    photonView.RPC("UnitLocateUpdate", RpcTarget.Others, swapcol);//SwapUnit
                }
                else
                {
                    int swaprow = swapUnit.transform.parent.parent.GetSiblingIndex();
                    int swapcol = swapUnit.transform.parent.GetSiblingIndex();
                    photonView.RPC("UnitLocateUpdate", RpcTarget.Others, swaprow, swapcol);//SwapUnit
                }

                previousParent = hitObject;
                this.transform.SetParent(previousParent.transform);
                this.transform.localPosition = Vector3.zero;
                this.transform.rotation = battleRotate;
                photonView.RPC("UnitSynergyUpdate", RpcTarget.All, "Add");
                photonView.RPC("UnitLocateUpdate", RpcTarget.Others, row, col);//CurrentUnit
            }
            previousParent = hitObject;
            Debug.Log("battle");
        }
        else
        {
            this.transform.SetParent(previousParent.transform);
            this.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void CheckLayer()
    {
        GameObject swapUnit = null;
        bool isSwap = false;

        Vector3 currentPos = this.transform.position;
        Vector3Int tilePos = tilemap.LocalToCell(currentPos);
        Vector3 checkPos = tilemap.GetCellCenterLocal(tilePos);
        checkPos = new Vector3(checkPos.x, 0.1f, checkPos.z);

        //유닛 to 유닛 Swap
        if (Physics.CheckSphere(checkPos, 0.1f, unitLayer))
        {
            isSwap = true;
            Collider[] hitColliders = Physics.OverlapSphere(checkPos, 0.1f, unitLayer);
            swapUnit = hitColliders[0].transform.gameObject;
            this.transform.position = swapUnit.transform.position;
            swapUnit.transform.position = new Vector3(previousPos.x, 0.1f, previousPos.z);
            Debug.Log("Swap");
        }

        //벤치레이어일 경우
        RaycastHit hitLayer;
        if (Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, benchLayer))
        {
            GameObject hitObject = hitLayer.collider.transform.gameObject;
            if (hitObject.transform.childCount == 0)
            {
                previousParent = hitObject;
            }
            else if (isSwap)
            {
                hitObject.transform.GetChild(0).SetParent(previousParent.transform);
                previousParent.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                previousParent = hitObject;
                // swapUnit.transform.SetParent(previousParent.transform);
                // swapUnit.transform.localPosition = new Vector3(0, 0, 0);
                // previousParent = hitObject;
            }
            Debug.Log("Bench");
        }
        //배틀레이어일 경우
        else if (Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, battleLayer))
        {
            //현재 타일이 배틀레이어 내부의 유효한 타일인지
            if (!CheckBattleTile())
            {
                this.transform.position = previousPos;
                return;
            }

            GameObject hitObject = hitLayer.collider.transform.gameObject;
            this.transform.position = checkPos;
            if (!isSwap)
            {
                TeamManager.UnitLocateDelete(CheckTilePosition(previousPos));
                TeamManager.UnitLocateSave(CheckTilePosition(this.gameObject), this.gameObject);
            }
            if (isSwap && swapUnit != null)
            {
                TeamManager.UnitLocateSwap(CheckTilePosition(this.gameObject), CheckTilePosition(swapUnit));
                swapUnit.transform.SetParent(previousParent.transform);
            }
            previousParent = hitObject;
            Debug.Log("battle");
        }
        else
        {
            this.transform.position = previousPos;
        }
    }

    public void OutLayer()
    {
        this.transform.SetParent(previousParent.transform);
        Vector3 currentPos = this.transform.position;
        if (Physics.Raycast(currentPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, benchLayer))
        {
            Vector3 hitPos = hit.transform.position;
            transform.localPosition = new Vector3(0, 0.1f, 0);
            this.transform.rotation = benchRotate;
        }
        else if (Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, battleLayer))
        {
            this.transform.SetParent(TeamManager.BattleArea.transform);
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(this.transform.position));
            transform.position = new Vector3(pos.x, 0.1f, pos.z);
            this.transform.rotation = battleRotate;
            this.transform.GetComponent<UnitInfo>().SynergyAdd();
        }
        else
        {
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(previousPos));
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            transform.localPosition = new Vector3(0, transform.position.y, 0);
        }
    }

    Vector2Int CheckTilePosition(Vector3 checkPos)
    {
        //row -3 -2 -1 0 1 2 3
        //col -3 -4 -5 -6
        Vector3Int tilePos = tilemap.LocalToCell(checkPos - TeamManager.LerpPos);
        int row = tilePos.x + 3;
        int col = tilePos.y * -1 - 3;
        Vector2Int convertTilePos = new Vector2Int(row, col);

        return convertTilePos;
    }

    Vector2Int CheckTilePosition(GameObject checkUnit)
    {
        //row -3 -2 -1 0 1 2 3
        //col -3 -4 -5 -6
        Vector3Int tilePos = tilemap.LocalToCell(checkUnit.transform.position - TeamManager.LerpPos);
        int row = tilePos.x + 3;
        int col = tilePos.y * -1 - 3;
        Vector2Int convertTilePos = new Vector2Int(row, col);

        return convertTilePos;
    }

    bool CheckBattleTile()
    {
        Vector2Int tilePos = CheckTilePosition(this.gameObject);
        int row = tilePos.x;
        int col = tilePos.y;
        //bool isUnitLocate = TeamManager.instance.IsUnitLocate(row, col);
        if (row > 6 || row < 0 || col > 3 || col < 0) return false;
        return true;
    }
}
