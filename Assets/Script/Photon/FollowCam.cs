using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 basePosition;
    private Transform mainCameraTr;
    public Transform viewTarget;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        basePosition = mainCamera.transform.position;
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
        //mainCameraTr.position = new Vector3(viewTarget.position.x, mainCameraTr.position.y, mainCameraTr.position.z);
        // 현재 카메라의 고도와 각도를 유지하기 위해 카메라의 상대적인 위치를 조정
        Vector3 newPosition = new Vector3(viewTarget.position.x, basePosition.y, basePosition.z + viewTarget.position.z);
        //Vector3 newPosition = new Vector3(viewTarget.position.x, mainCameraTr.position.y,  Mathf.Tan(Mathf.Deg2Rad * 55f) * viewTarget.position.z);
        mainCameraTr.position = newPosition;
        //tr.LookAt(targetTr.position);
    }
}
