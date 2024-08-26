using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControll : MonoBehaviour {
    protected GameObject ObjectHitPosition;
    protected GameObject previousParent;
    protected Vector3 previousPos;

    public virtual void OnObjectControll() {
        previousPos = this.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitRay)) {
            ObjectHitPosition = new GameObject("HitPosition");
            previousParent = this.transform.parent.gameObject;
            ObjectHitPosition.transform.position = hitRay.point;
            this.transform.SetParent(ObjectHitPosition.transform);   
            this.transform.localPosition = new Vector3(0, 0.1f, 0);
        }    
    }
    public virtual void OnObjectMove() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Stage");
        if(Physics.Raycast(ray, out RaycastHit hitLayerMask, Mathf.Infinity, layerMask)) {
            float H = Camera.main.transform.position.y;
            float h = ObjectHitPosition.transform.position.y;
            Vector3 newPos = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            ObjectHitPosition.transform.position = newPos;
        }
    }



}