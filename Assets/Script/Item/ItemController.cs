using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ItemController : ObjectControll
{

    public override void OnObjectControll()
    {
        base.OnObjectControll();
    }

    //-----------Mouse Input-----------//
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        { //Left
            OnObjectControll();
        }
    }

    void OnMouseDrag()
    {
        OnObjectMove();
    }

    void OnMouseUp()
    {
        OnObjectUndo();
        Destroy(ObjectHitPosition);
    }

    private void OnObjectUndo()
    {
        Vector3 currentPos = this.transform.position;
        RaycastHit hit;
        int ItemAreaLayer = 1 << LayerMask.NameToLayer("Item");
        int UnitLayer = 1 << LayerMask.NameToLayer("Unit");
        if (Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, ItemAreaLayer))
        {
            Vector3 hitPos = hit.transform.position;
            transform.SetParent(hit.transform);
            transform.localPosition = new Vector3(0, 0, -0.51f);
            this.transform.localRotation = Quaternion.identity;
        }
        else if (Physics.CheckSphere(currentPos, 1, UnitLayer))
        {
            Collider[] hitColliders = Physics.OverlapSphere(currentPos, 0.1f, UnitLayer);
            GameObject unit = hitColliders[0].gameObject;
            ItemAsset itemAsset = GetComponent<ItemBase>().itemAsset;
            Debug.Log(unit.name);
            unit.GetComponent<UnitManager>().AddItem(itemAsset);
            Destroy(this.gameObject);
        }

        else
        {
            this.transform.SetParent(previousParent.transform);
            transform.localPosition = new Vector3(0, 0, -0.51f);
            this.transform.localRotation = Quaternion.identity;
        }
    }
}
