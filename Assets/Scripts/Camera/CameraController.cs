using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //cinemachine does the followTarget thing
    //this script is for unfocused camera, just to look around map

    private WarlordController inputAction;
    private CinemachineBrain cinemachineBrain;


    private float screenWidth;
    private float boundary = 20f;
    private float speed = 70f;

    private void Awake()
    {
        inputAction = new WarlordController();
       
        cinemachineBrain = GetComponent<CinemachineBrain>();

        screenWidth = Screen.width;
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
        if(cinemachineBrain.enabled == false)
        {
           
            CameraMove();
        }
      
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

    private void CameraMove()
    {
        //move camera in x direction if mouse is pushing out of screen boundaries
        //mouse position, main camera x transform

        if(Input.mousePosition.x > screenWidth - boundary)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            
        }
        if (Input.mousePosition.x < boundary)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

        }

    }


}
