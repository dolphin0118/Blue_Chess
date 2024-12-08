using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera subCamera;
    private Vector3 homePosition;
    private Vector3 awayPosition;
    private Quaternion homeRotation;
    private Quaternion awayRotation;

    public bool isAwayViewTarget;
    public Transform viewTarget;

    // Start is called before the first frame update
    void Start()
    {
        isAwayViewTarget = false;
        homePosition = mainCamera.transform.position;
        awayPosition = subCamera.transform.position;
        homeRotation = mainCamera.transform.rotation;
        awayRotation = subCamera.transform.rotation;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraZoom();
        LerpTarget();
    }

    void CameraZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * -1;
        if(scroll > 0 && mainCamera.fieldOfView >= 50.0f) {
            mainCamera.fieldOfView = 50.0f;
        }
        else if(scroll < 0 && mainCamera.fieldOfView <=25.0f){
            mainCamera.fieldOfView = 25.0f;
        }
        else mainCamera.fieldOfView += scroll * 10.0f;
    }

    void LerpTarget() {
        
        if(!mainCamera.enabled || viewTarget == null) return;
        if(!isAwayViewTarget) {
            Vector3 newPosition = new Vector3(viewTarget.position.x, homePosition.y, homePosition.z + viewTarget.position.z);
            mainCamera.transform.position = newPosition;
            mainCamera.transform.rotation = homeRotation;
        }
        else {
            Vector3 newPosition = new Vector3(viewTarget.position.x, awayPosition.y, awayPosition.z + viewTarget.position.z + 4);
            mainCamera.transform.position = newPosition;
            mainCamera.transform.rotation = awayRotation;
        }

    }
}
