using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public bool boosted = false;
    public CinemachineVirtualCamera movingCamera, stopCamera, zoomCamera;

    public int PriorityBoostAmount = 10;
    public GameObject Reticle;

    public void StopCamera(bool cam)
    {
        movingCamera.gameObject.SetActive(!cam);
        stopCamera.gameObject.SetActive(cam);
    }

    public void Zoom(bool isZoom)
    {
        if (movingCamera != null)
        {
                if (isZoom)
                {
                    movingCamera.Priority += PriorityBoostAmount;
                    boosted = true;
                    movingCamera.gameObject.SetActive(false);
                    zoomCamera.gameObject.SetActive(true);
                }

            else if (!isZoom)
            {
                movingCamera.Priority -= PriorityBoostAmount;
                boosted = false;
                movingCamera.gameObject.SetActive(true);
                zoomCamera.gameObject.SetActive(false);
            }
        }
        if (Reticle != null)
            Reticle.SetActive(boosted);
    }
}
