using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Create the relevant relevant references needed for the mainCamera.
    public Camera mainCamera;
    public FlareLayer mainCameraFlareLayer;
    public GUILayer mainCameraGUILayer;
    public AudioListener mainCameraAudioListener;

    //Create the relevant references needed for the secondayCamera
    public Camera secondaryCamera;
    public FlareLayer secondaryCameraFlareLayer;
    public GUILayer secondaryCameraGUILayer;
    public AudioListener secondaryCameraAudioListener;

    //Create a public string for the up d-pad button, this will be set in the editor.
    public string dPadUp;
    //Create a bool called bDPadUpPressed and set it to false.
    bool bDPadUpPressed = false;

	// Use this for initialization
	void Start ()
    {
        //The mainCamera is active.
        mainCamera.enabled = true;
        mainCameraFlareLayer.enabled = true;
        mainCameraGUILayer.enabled = true;
        mainCameraAudioListener.enabled = true;
        
        //The secondaryCamera is inactive.
        secondaryCamera.enabled = false;
        secondaryCameraFlareLayer.enabled = false;
        secondaryCameraGUILayer.enabled = false;
        secondaryCameraAudioListener.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //If dPadUp has been pressed and bDPadUpPressed is currently set to false.
        if(Input.GetAxis(dPadUp) == 1 && !bDPadUpPressed)
        {
            //Set the mainCamera to the opposite state.
            mainCamera.enabled = !mainCamera.enabled;
            mainCameraFlareLayer.enabled = !mainCameraFlareLayer.enabled;
            mainCameraGUILayer.enabled = !mainCameraGUILayer.enabled;
            mainCameraAudioListener.enabled = !mainCameraAudioListener.enabled;

            //Set the secondaryCamera to the opposite state.
            secondaryCamera.enabled = !secondaryCamera.enabled;
            secondaryCameraFlareLayer.enabled = !secondaryCameraFlareLayer.enabled;
            secondaryCameraGUILayer.enabled = !secondaryCameraGUILayer.enabled;
            secondaryCameraAudioListener.enabled = !secondaryCameraAudioListener.enabled;

            //Set bDPadUpPressed to true.
            bDPadUpPressed = true;
            //Call the ResestBool function in one second.
            Invoke("ResetBool", 1.0f);
        }
	}

    void ResetBool()
    {
        //Set bDPadUpPressed to false so the player change the camera again.
        bDPadUpPressed = false;
    }
}
