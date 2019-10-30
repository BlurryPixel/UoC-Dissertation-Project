using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomAnnouncementScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //Announce to the Level Manager that this room has been spawned by adding it to the lCurrentRooms list.
        LevelManager.levelManager.lCurrentRooms.Add(gameObject);
    }
}
