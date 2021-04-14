using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSoftTrack : MonoBehaviour
{

    [SerializeField] private Camera mCamera;
    [Range(0, 1.0f)] [SerializeField] private float cameraSmoothingMult = 0.125f;
    [SerializeField] private Vector3 Offset;

    private float positionalDepth = 10;
    private float cameraZ;

    private void Awake()
    {
        if (mCamera == null)
            mCamera = Camera.main;

        positionalDepth = (transform.position.z - mCamera.transform.position.z);
        cameraZ = mCamera.transform.position.z;

    }

    private void LateUpdate()
    {
        cameraDragFollow();
    }

    private void cameraDragFollow()
    {
        Vector3 screenPosDepth = Input.mousePosition;
        screenPosDepth.z = positionalDepth;
        Vector3 mousePosition = mCamera.ScreenToWorldPoint(screenPosDepth);

        Vector3 smoothedMousePos = Vector3.Lerp(transform.position, mousePosition + Offset, cameraSmoothingMult);
        smoothedMousePos.z = cameraZ;
        mCamera.transform.position = smoothedMousePos;

    }

}
