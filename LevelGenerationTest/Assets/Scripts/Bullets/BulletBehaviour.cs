using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //Create a public float called bulletSpeed. This will be set by the object that is firing it.
    public float bulletSpeed;
    //Create a boolean called bPlayerHit and set it to false.
    bool bPlayerHit = false;
	
    void Start()
    {
        //Call the DeactivateAfterTimeLimit function after 5 seconds.
        Invoke("DeactivateAfterTimeLimit", 5.0f);
    }

	void Update ()
    {
        //Move the bullet forwards relative to the bulletSpeed and deltaTime.
        gameObject.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
	}

    //When the bullet hits something...
    void OnCollisionEnter(Collision other)
    {
        //If the other object was a wall...
        if(other.gameObject.tag == "Wall")
        {
            //Set the bullet to be inactive.
            gameObject.SetActive(false);            
        }
        //Else if the other object was an enemy...
        else if(other.gameObject.tag == "Enemy")
        {
            //Reduce the enemy health by 1.
            other.gameObject.GetComponent<EnemyTowerBehaviour>().towerHealth -= 1;
            //Set the bullet to be inactive.
            gameObject.SetActive(false);
        }
        //Else if the other object was the player...
        else if (other.gameObject.tag == "Player" && !bPlayerHit)
        {
            //Set bPlayerHit to true.
            bPlayerHit = true;
            //Reduce the enemy health by 1.
            other.gameObject.GetComponent<PlayerBehaviour>().playerHealth -= 1;
            //Set the bullet to be inactive.
            gameObject.SetActive(false);
        }
    }
    //Create a function called DeactivateAfterTimeLimit.
    void DeactivateAfterTimeLimit()
    {
        //Set the bullet to be inactive.
        gameObject.SetActive(false);        
    } 
}
