using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Camera mainCamera;
    private Transform mainCameraTr;
    public Transform viewTarget;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTr = mainCamera.transform;
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
        mainCameraTr.position = new Vector3(viewTarget.position.x, mainCameraTr.position.y, mainCameraTr.position.z);
   
        //tr.LookAt(targetTr.position);
    }
}
