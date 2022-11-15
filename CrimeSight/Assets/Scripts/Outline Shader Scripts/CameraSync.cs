using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraSync : MonoBehaviour
{
    public Camera targetCamera;
    private PostProcessVolume targetCamPPV;
    private Camera camera;
    private PostProcessVolume ppv;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        ppv = GetComponent<PostProcessVolume>();
        targetCamPPV = targetCamera.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sync cameras
        camera.fieldOfView = targetCamera.fieldOfView;
        camera.transform.SetPositionAndRotation(targetCamera.transform.position, targetCamera.transform.rotation);

        // Sync post-process effects
        //ppv.weight = targetCamPPV.weight;
    }
}
