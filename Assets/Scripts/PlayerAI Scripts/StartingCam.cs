using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCam : MonoBehaviour
{
    public Camera cam;
//    public Camera Pcam;

    void Start()
    {
        
        cam.enabled = false;
      //  Pcam.enabled = true;


    }

    void Update()
    {
        cam.enabled = true;

    }
}
