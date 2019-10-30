using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The majority of this script was inspired by (Placzek, 2016)

public class BulletPool : MonoBehaviour
{
    public static BulletPool bulletPool;

    public List<GameObject> pooledBullets = new List<GameObject>();
    public GameObject bullet;
    public int totalBulletsToPool;
    public int currentActiveBullets;
    bool bCanExpand = true;    

    void Awake()
    {
        bulletPool = this;
    }

	void Start ()
    {
		for(int i = 0; i < totalBulletsToPool; i++)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.SetActive(false);
            pooledBullets.Add(newBullet);
        }
	}

    public GameObject GetBullet()
    {           
        for(int i = 0; i < pooledBullets.Count; i++)
        {
            if(!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }                
        }
        if(bCanExpand)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.SetActive(false);
            pooledBullets.Add(newBullet);
            return newBullet;
        }
        return null;        
    }
}
