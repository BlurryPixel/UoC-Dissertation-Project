using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerBehaviour : MonoBehaviour
{
    //Create a public GameObect called firingSphere that will be set in the editor.
    public GameObject firingSphere;
    //Create a public GameObject called player.
    public GameObject player;
    //Create a public int called towerHealth.
    public int towerHealth = 5;
    //Create a bool called bFired and set it to false, this will be used to control the rate of fire.
    bool bFired = false;

    void Start()
    {
        //Player will be set to the TempPlayer object in the scene by locating it based on the tag.
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //While there is an object in the SphereCollider...
    void OnTriggerStay(Collider other)
    {
        //If the colliding object is the player...
        if(other.tag == "Player")
        {
            //Make the firingSphere look at the player.
            firingSphere.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y * 0.5f, player.transform.position.z));
            //Get an inactive bullet from the BulletPool.
            GameObject tempBullet = BulletPool.bulletPool.GetBullet();
            //If tempBullet is not null and bFired is currently false.
            if (tempBullet != null && !bFired)
            {
                //Set the bulletSpeed to 5.
                tempBullet.GetComponent<BulletBehaviour>().bulletSpeed = 5.0f;
                //Set the position of the bullet to the firingSphere position with a small offset.
                tempBullet.transform.position = firingSphere.transform.position + (transform.forward * 0.5f);
                //Set the rotation of the bullet to the firingSphere rotation.
                tempBullet.transform.rotation = firingSphere.transform.rotation;
                //Set tempBullet to be active.
                tempBullet.SetActive(true);
                //Set bFired to true.
                bFired = true;
                //Call the ResetBool function after 1 second.
                Invoke("ResetBool", 1.0f);
            }
        }
    }

    void Update()
    {
        //if towerHealth is less than or equal to 0...
        if(towerHealth <= 0)
        {
            //Set this instance to false.
            gameObject.SetActive(false);
        }
    }

    //Create a function called ResetBool.
    void ResetBool()
    {
        //Set bFired to false.
        bFired = false;
    }
}
