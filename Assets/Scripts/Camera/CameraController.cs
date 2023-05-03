using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //cinemachine does the followTarget thing
    //this script is for unfocused camera, just to look around map

    private WarlordController inputAction;
    
    private CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        inputAction = new WarlordController();
       
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    private void OnEnable()
    {
        inputAction.W_Controller.Enable();
    }

    private void OnDisable()
    {
        inputAction.W_Controller.Disable();
    }


    private void Update()
    {
        //if y is pressed, the camera should switch between focused and unfocused (cinemachine brain on or off)

        if(inputAction != null)
        {
            if(inputAction.W_Controller.CameraFocus.WasPressedThisFrame() && cinemachineBrain.enabled == true)
            {
                cinemachineBrain.enabled = false;
            }
            else if(inputAction.W_Controller.CameraFocus.WasPressedThisFrame() && cinemachineBrain.enabled == false)
            {
                cinemachineBrain.enabled = true;
            }
        }

    }
}
