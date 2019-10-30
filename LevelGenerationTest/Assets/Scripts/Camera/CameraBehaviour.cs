using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //Create a public transform called tPlayer that will be set in the editor.
    public Transform tPlayer;

    void Update()
    {
        //Move the camera with the player while maintaining a vertical height of 10 units.
        transform.position = new Vector3(tPlayer.position.x, 10.0f, tPlayer.position.z);
    }
}
