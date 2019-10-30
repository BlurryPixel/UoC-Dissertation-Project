using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //Create a public CharacterController called controller.
    CharacterController controller;
    //Create a public int called playerHealth and set it to 5.
    public int playerHealth = 5;
    //Create a public float called speed and set it to 7.5.
    public float speed = 7.5f;
    //Create multiple public strings that will be used for the various controls.
    public string LeftXAxis, LeftYAxis, RightXAxis, RightYAxis, AButton, rbButton;
    //Create two Vector3s that will be used for the left and right stick of the controller.
    Vector3 leftStick, rightStick;
    //Create a float called deadzone and set it to 0.25.
    float deadzone = 0.25f;

    // Use this for initialization
    void Start()
    {
        //Set controller to the CharacterController on the gameobject this script is attached to.
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //leftStick is set to the current input value of LeftXAxis and LeftYAxis in the appropriate fields.
        leftStick = new Vector3(Input.GetAxis(LeftXAxis), 0, Input.GetAxis(LeftYAxis));
        //If leftStick magnitude is less than the deadzone...
        if (leftStick.magnitude < deadzone)
        {
            //left stick is zeroed out.
            leftStick = Vector3.zero;
        }
        //If leftStick is not zeroed out...
        if (leftStick != new Vector3(0, 0, 0))
        {
            //Move the controller based on the current value of leftStick, multipled by speed and deltaTime.
            controller.Move(leftStick * speed * Time.deltaTime);
        }
        //rightStick is set to the current input value of RightXAxis and RightYAxis in the appropriate fields.
        rightStick = new Vector3(Input.GetAxis(RightXAxis), 0, -Input.GetAxis(RightYAxis));

        //if rightStick has not been zeroed out...
        if (rightStick != Vector3.zero)
        {
            //Create a quaternion called rotation and set it to look in the direction of rightStick.
            Quaternion rotation = Quaternion.LookRotation(rightStick, Vector3.up);
            //gameObject rotation is set to rotation.
            transform.rotation = rotation;
        }
        //If the rbButton is pressed down...
        if (Input.GetButtonDown(rbButton))
        {
            //Create a GameObject called tempBullet and get a bullet from the bullet pool
            GameObject tempBullet = BulletPool.bulletPool.GetBullet();
            //(if tempBullet is not null...
            if (tempBullet != null)
            {
                //Set the bullet speed to 7.5.
                tempBullet.GetComponent<BulletBehaviour>().bulletSpeed = 7.5f;
                //Set the bullet position to the player position with a small offset.
                tempBullet.transform.position = gameObject.transform.position + (transform.forward * 0.5f);
                //set the bullet rotation to that of the player.
                tempBullet.transform.rotation = gameObject.transform.rotation;
                //Set the bullet to be active.
                tempBullet.SetActive(true);
            }
        }
    }
}