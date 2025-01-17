using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Camera[] cameras;
    private int currentCameraIndex = 0;
    public Camera playerCamera;



    void Start()
    {
      
       cameras = FindObjectsOfType<Camera>();


        if (cameras.Length == 0)
        {
            Debug.LogError("No cameras with the tag 'WallCamera' found!");
            cameras = new Camera[0];
            return;
        }
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        //-----


        playerCamera.gameObject.SetActive(true);

    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
                Debug.Log("camera is swtiched");

                SwitchCamera();
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("camera is swtiched");
            cameras[currentCameraIndex].gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
        }
    }

    void SwitchCamera()
    {
        Debug.Log("run");
        cameras[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;  

        while (cameras[currentCameraIndex] == playerCamera)
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        }

        Debug.Log("finsihed running");
        cameras[currentCameraIndex].gameObject.SetActive(true);
        Debug.Log("finsihed running 2");

    }
}