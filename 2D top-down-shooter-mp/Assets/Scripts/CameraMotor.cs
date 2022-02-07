using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraMotor : NetworkBehaviour
{
    public GameObject CameraMountPoint;
    void Start()
    {
        if (hasAuthority)
        {
            Transform cameraTransform = Camera.main.gameObject.transform;
            cameraTransform.parent = CameraMountPoint.transform;
            cameraTransform.position = CameraMountPoint.transform.position;
            cameraTransform.rotation = CameraMountPoint.transform.rotation;
            Camera.main.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        Camera.main.gameObject.transform.position = new Vector3(CameraMountPoint.transform.position.x, CameraMountPoint.transform.position.y, -10);
        //Debug.Log(new Vector3(CameraMountPoint.transform.position.x, CameraMountPoint.transform.position.y, -10));
    }
}
