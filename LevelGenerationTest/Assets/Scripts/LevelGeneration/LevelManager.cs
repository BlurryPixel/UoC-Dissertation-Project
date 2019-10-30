using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Create a public static of the LevelManager and call it levelManager.
    public static LevelManager levelManager;
    //Create a public GameObject called player, this will be set in the editor.
    public GameObject player;
    //Create a public reference to the PlayerBehaviour script.
    public PlayerBehaviour playerScript;

    //Create a list of GameObjects called lStartingRooms, this will contain all the potential starting rooms.
    public List<GameObject> lStartingRooms = new List<GameObject>();
    //Create a list of GameOjects called lCurrentRooms, this will contain all the currently spawned rooms in the level.
    public List<GameObject> lCurrentRooms = new List<GameObject>();
    //Create a list of GameObjects called lCurrentNodes, this will contain all the currently spawned nodes in the level.
    public List<GameObject> lCurrentNodes = new List<GameObject>();
    //Create a list of GameObjects called lRoomsToDelete, this will contain all of the rooms that need to be deleted.
    public List<GameObject> lRoomsToDelete = new List<GameObject>();
    //Create a list of GameObjects called lRoomCaps, this will contain all of the rooms used to cap the ends of the level.
    public List<GameObject> lRoomCaps = new List<GameObject>();

    //Create a public int called currentLevels and set it to zero.
    public int currentLevels = 0;
    //Create a public int called maxLevels and set it to zero.
    public int maxLevels = 0;
    //Create a public int called randomModifier.
    public int randomModifier;
    //Create a public int called currentRoomCount.
    public int currentRoomCount;
    //Create a public int called currentNodeCount.
    public int currentNodeCount;

    //Create a public string called dPadDown.
    public string dPadDown;
    //Create a public string called nodeTag.
    public string nodeTag;
    //Create a public GameObject called nodeObject.
    public GameObject nodeObject;
    //Create a public Vector3 called nodeObjectPosition.
    public Vector3 nodeObjectPosition;
    //Create a public TestRoomSpawn called nodeScript.
    public TestRoomSpawn nodeScript;

    //Create a private int called iRandom
    int iRandom;

    //Created a public bool called bGeneratedLevel and set it to false.
    public bool bGeneratedLevel = false;
    //Create a public bool called bLevelCapped and set it to false.
    public bool bLevelCapped = false;
    //Create a public bool called bGoalRoomPlaced and set it to false.
    public bool bGoalRoomPlaced = false;

    bool bDPadDownPressed = false;

    public GameObject itemChest;

	// Use this for initialization
	void Start ()
    {
        SpawnFirstRoom();
    }

	void FixedUpdate ()
    {
        //currentRoomCount is equal to the current number of elements in lCurrentRooms.
        currentRoomCount = lCurrentRooms.Count;
        //currentNodeCount is equal to the current number of elements in lCurrentNodes.
        currentNodeCount = lCurrentNodes.Count;
        
        //if currentLevels is greater than or equal to maxLevels...
        if(currentLevels >= maxLevels && !bGeneratedLevel)
        {            
            //Run the CapEndsOfMap function
            CapEndsOfMap();
            //Run the DeleteClashingRooms function
            DeleteClashingRooms();
            //Run the RemoveNullRoomsFromList function
            RemoveNullRoomsFromList();
            //Run the RemoveNullNodesFromlist function
            RemoveNullNodesFromList();                                                        
        }

        //If the dPadDown button is pressed and DPadDownPressed is currently false.
        if(Input.GetAxis(dPadDown) == -1 && !bDPadDownPressed)
        {
            //Call the ResetLevelGeneration function.
            ResetLevelGeneration();
            //Set bDPadDownPressed to true.
            bDPadDownPressed = true;
            //Call the ResetDPadBool function after half a second.
            Invoke("ResetDPadBool", 0.5f);
        }

        //If the player is dead...
        if(playerScript.playerHealth <= 0)
        {
            //Set the player to be inactive.
            player.SetActive(false);
            //Call the ResetLevelGeneration function.
            ResetLevelGeneration();
        }
    }

    void SpawnFirstRoom()
    {
        //Set levelManager to this script.
        levelManager = this;
        //iRandom is set to a random number between zero (inclusive) and the value of randomModifier (exclusive).
        iRandom = Random.Range(0, randomModifier);
        //Create a starting room in the centre of the grid, chosen at random from the lStartingRooms list.
        Instantiate(lStartingRooms[iRandom], new Vector3(0, 0, 0), transform.rotation);
        //Create the player at the centre of the new room
        player.transform.position = new Vector3(0, 0.5f, 0);
        playerScript.playerHealth = 5;
        player.SetActive(true);
        //Call the GenerationSafetyCheck after 5 seconds.
        Invoke("GenerationSafetyCheck", 5.0f);

        //NOTE: Currently there is only one room in lStartingRooms but this has been set up in a way that new starting rooms can be added with ease.
    }

    void GenerationSafetyCheck()
    {
        if(currentLevels < maxLevels)
        {
            currentLevels = maxLevels + 1;
        }
    }

    //Create a function called RemoveNullRoomsFromList
    void RemoveNullRoomsFromList()
    {
        //Create a for loop that runs while i is less than currentRoomCount.
        for(int i = 0; i < currentRoomCount; i++)
        {
            //if the current element is empty...
            if(lCurrentRooms[i] == null)
            {
                //Remove the current element from the list.
                lCurrentRooms.Remove(lCurrentRooms[i]);
                //Break from the current loop.
                break;
            }
        }
    }

    //Create a function called RemoveNullNodesFromList
    void RemoveNullNodesFromList()
    {
        //Create a for loop that runs while i is less than currentNodeCount.
        for (int i = 0; i < currentNodeCount; i++)
        {
            //if the current element is empty...
            if (lCurrentNodes[i] == null)
            {
                //Remove the current element from the list.
                lCurrentNodes.Remove(lCurrentNodes[i]);
                //Break from the current loop.
                break;
            }
        }
    }

    //NOTE: The RemoveNullRoomsFromList function and RemoveNullNodesFromList function were a solution to an issue where rooms being deleted would cause a NullReferenceException and stop any loops from continuing.
    //      I have since found a simplier solution to that issue making these mostly irrelevant but I have left them in as an extra safety net just in case anything sneaks through.

    //Create a function called CapEndsOfMap.
    void CapEndsOfMap()
    {
        //Create a for loop that runs while i is less than currentNodeCount.
        for(int i = 0; i < currentNodeCount; i++)
        {
            //if bSpawnedRooms is true in the current element...
            if (lCurrentNodes[i].gameObject.GetComponent<TestRoomSpawn>().bSpawnedRooms == true)
            {
                //nodeObject is set to the current element.
                nodeObject = lCurrentNodes[i];
                //nodeScipt is set to the TestRoomSpawn script on the nodeObject.
                nodeScript = nodeObject.GetComponent<TestRoomSpawn>();

                //If the space array is empty...
                if (nodeScript.space.Length <= 0)
                {
                    //bSpawnedRoom is set to false.
                    nodeScript.bSpawnedRooms = false;
                }
                //Else if the closeSpace array is empty...
                else if (nodeScript.closeSpace.Length <= 0)
                {
                    //bSpawnedRooms is set to false.
                    nodeScript.bSpawnedRooms = false;
                }
            }

            //NOTE: The above if statements may seem odd or unnecessary but they act as a safety to prevent uncapped rooms/corridors. Very rarely and for reasons I am not quite certain of, it was possible for a node to set
            //      bSpawnedRooms to true but without a room or corridor spawning (it is possible that it spawned as was deleted but I was unable to say with 100% certainty that this was the case. Doing the above checks simply
            //      fixes that problem.

            //else if bSpawnedRooms is false in the current element...
            else if (lCurrentNodes[i].gameObject.GetComponent<TestRoomSpawn>().bSpawnedRooms == false)
            {
                //nodeTag is set to the tag of the current element.
                nodeTag = lCurrentNodes[i].gameObject.tag;
                //nodeObject is set to the current element.
                nodeObject = lCurrentNodes[i];
                //nodeObjectPosition is set to the the position of nodeObject.
                nodeObjectPosition = nodeObject.transform.position;
                //nodeScript is set to the TestRoomSpawn script on the nodeObject.
                nodeScript = nodeObject.GetComponent<TestRoomSpawn>();

                //If the space array is empty (indicating there is space for a full room)...
                if(nodeScript.space.Length <= 0)
                {
                    if(!bGoalRoomPlaced)
                    {
                        switch (nodeTag)
                        {
                            //If nodeTag is "NorthConnection"...
                            case "NorthConnection":
                                //Spawn the first element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[8], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z + 5), lRoomCaps[8].transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                //Set bGoalRoomPlaced to true.
                                bGoalRoomPlaced = true;
                                break;
                            //If nodeTag is "SouthConnection"...
                            case "SouthConnection":
                                //Spawn the second element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[9], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z - 5), lRoomCaps[9].transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                //Set bGoalRoomPlaced to true.
                                bGoalRoomPlaced = true;
                                break;
                            //If nodeTage is "EastConnection"...
                            case "EastConnection":
                                //Spawn the third element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[10], new Vector3(nodeObjectPosition.x + 5, nodeObjectPosition.y, nodeObjectPosition.z), lRoomCaps[10].transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                //Set bGoalRoomPlaced to true.
                                bGoalRoomPlaced = true;
                                break;
                            //If nodeTag is "WestConnection"...
                            case "WestConnection":
                                //Spawn the fourth element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[11], new Vector3(nodeObjectPosition.x - 5, nodeObjectPosition.y, nodeObjectPosition.z), lRoomCaps[11].transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                //Set bGoalRoomPlaced to true.
                                bGoalRoomPlaced = true;
                                break;
                        }
                    }

                    else
                    {
                        //Start a switch statement using nodeTag.
                        switch(nodeTag)
                        {
                            //If nodeTag is "NorthConnection"...
                            case "NorthConnection":
                                //Spawn the first element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[0], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z + 5), gameObject.transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                break;
                            //If nodeTag is "SouthConnection"...
                            case "SouthConnection":
                                //Spawn the second element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[1], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z - 5), gameObject.transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                break;
                            //If nodeTage is "EastConnection"...
                            case "EastConnection":
                                //Spawn the third element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[2], new Vector3(nodeObjectPosition.x + 5, nodeObjectPosition.y, nodeObjectPosition.z), gameObject.transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                break;
                            //If nodeTag is "WestConnection"...
                            case "WestConnection":
                                //Spawn the fourth element of lRoomCaps at the desired location and rotation.
                                Instantiate(lRoomCaps[3], new Vector3(nodeObjectPosition.x - 5, nodeObjectPosition.y, nodeObjectPosition.z), gameObject.transform.rotation);
                                //Set bSpawnedRooms to true.
                                nodeScript.bSpawnedRooms = true;
                                break;
                        }
                    }                    
                }

                //Else if the closeSpace array is empty (indicating there is only room for a small cap)...
                else if (nodeScript.closeSpace.Length <= 0)
                {
                    //Start a switch statement using nodeTag.
                    switch (nodeTag)
                    {
                        //If nodeTag is "NorthConnection"...
                        case "NorthConnection":
                            //Spawn the fifth element of lRoomCaps at the desired location and rotation.
                            Instantiate(lRoomCaps[4], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z + 0.75f), lRoomCaps[4].transform.rotation);
                            //Set bSpawnedRooms to true.
                            nodeScript.bSpawnedRooms = true;
                            break;
                        //If nodeTag is "SouthConnection"...
                        case "SouthConnection":
                            //Spawn the sixth element of lRoomCaps at the desired location and rotation.
                            Instantiate(lRoomCaps[5], new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z - 0.75f), lRoomCaps[5].transform.rotation);
                            //Set bSpawnedRooms to true.
                            nodeScript.bSpawnedRooms = true;
                            break;
                        //If nodeTag is "EastConnection"...
                        case "EastConnection":
                            //Spawn the seventh element of lRoomCaps at the desired location and rotation.
                            Instantiate(lRoomCaps[6], new Vector3(nodeObjectPosition.x + 0.75f, nodeObjectPosition.y, nodeObjectPosition.z), lRoomCaps[6].transform.rotation);
                            //Set bSpawnedRooms to true.
                            nodeScript.bSpawnedRooms = true;
                            break;
                        //If nodeTag is "WestConnection"...
                        case "WestConnection":
                            //Spawn the eighth element of lRoomCaps at the desired location and rotation.
                            Instantiate(lRoomCaps[7], new Vector3(nodeObjectPosition.x - 0.75f, nodeObjectPosition.y, nodeObjectPosition.z), lRoomCaps[7].transform.rotation);
                            //Set bSpawnedRooms to true.
                            nodeScript.bSpawnedRooms = true;
                            break;
                    }
                }

                else if(nodeScript.closeSpace.Length > 0)
                {
                    for(int j = 0; j < nodeScript.closeSpace.Length; j++)
                    {
                        if(nodeScript.closeSpace[j].tag == "Wall")
                        {
                            switch (nodeTag)
                            {
                                //If nodeTag is "NorthConnection"...
                                case "NorthConnection":
                                    nodeScript.itemChest = Instantiate(itemChest, new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z - 0.5f), Quaternion.Euler(-90, 0, 180));
                                    //Set bSpawnedRooms to true.
                                    nodeScript.bSpawnedRooms = true;
                                    break;
                                //If nodeTag is "SouthConnection"...
                                case "SouthConnection":
                                    //Spawn the sixth element of lRoomCaps at the desired location and rotation.
                                    nodeScript.itemChest = Instantiate(itemChest, new Vector3(nodeObjectPosition.x, nodeObjectPosition.y, nodeObjectPosition.z + 0.5f), Quaternion.Euler(-90, 0, 0));
                                    //Set bSpawnedRooms to true.
                                    nodeScript.bSpawnedRooms = true;
                                    break;
                                //If nodeTag is "EastConnection"...
                                case "EastConnection":
                                    //Spawn the seventh element of lRoomCaps at the desired location and rotation.
                                    nodeScript.itemChest = Instantiate(itemChest, new Vector3(nodeObjectPosition.x - 0.5f, nodeObjectPosition.y, nodeObjectPosition.z), Quaternion.Euler(-90, 0, 270));
                                    //Set bSpawnedRooms to true.
                                    nodeScript.bSpawnedRooms = true;
                                    break;
                                //If nodeTag is "WestConnection"...
                                case "WestConnection":
                                    //Spawn the eighth element of lRoomCaps at the desired location and rotation.
                                    nodeScript.itemChest = Instantiate(itemChest, new Vector3(nodeObjectPosition.x + 0.5f, nodeObjectPosition.y, nodeObjectPosition.z), Quaternion.Euler(-90, 0, 90));
                                    //Set bSpawnedRooms to true.
                                    nodeScript.bSpawnedRooms = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    //Create a function called DeleteClashingRooms.
    void DeleteClashingRooms()
    {
        //Create a for loop that runs while i is less than currentRoomCount.
        for (int i = 0; i < currentRoomCount; i++)
        {
            //If the current element is empty (acts as a safety to prevent NullReferenceExceptions)...
            if(lCurrentRooms[i] == null)
            {
                //Break and move on to the next element.
                break;
            }

            //Else if the current element is not empty...
            else if(lCurrentRooms[i] != null)
            {
                //Create a for loop that runs while j is less than currentRoomCount.
                for (int j = i + 1; j < currentRoomCount; j++)
                {
                    //If the current element is empty (acts as a safety to prevent NullReferenceExceptions)...
                    if (lCurrentRooms[j] == null)
                    {
                        //Break and move on to the next element
                        break;
                    }
                    //Else if the position of element i is the same as the position of element j (Implying there are two rooms in the same location)...
                    else if (lCurrentRooms[i].transform.position.x == lCurrentRooms[j].transform.position.x && lCurrentRooms[i].transform.position.y == lCurrentRooms[j].transform.position.y && lCurrentRooms[i].transform.position.z == lCurrentRooms[j].transform.position.z)
                    {
                        //Add element j to the lRoomsToDelete list.
                        lRoomsToDelete.Add(lCurrentRooms[j]);
                    }
                }
            }
        }

        //Create a for loop that runs while i is less than the current elements in lRoomsToDelete.
        for (int i = 0; i < lRoomsToDelete.Count; i++)
        {
            //Destroy the current element.
            Destroy(lRoomsToDelete[i]);
        }
        //Clear the list of all elements (which at this point should all be null anyway).
        lRoomsToDelete.Clear();       
    }

    //Create a function called ResetLevelGeneration.
    void ResetLevelGeneration()
    {
        //Reset all the relevant variables.
        player.SetActive(false);

        currentLevels = 0;
        currentNodeCount = 0;
        currentRoomCount = 0;

        bGeneratedLevel = false;
        bLevelCapped = false;
        bGoalRoomPlaced = false;

        //Delete all of the rooms.
        for(int i = 0; i < lCurrentRooms.Count; i++)
        {
            Destroy(lCurrentRooms[i]);
        }

        //Clear the relevant lists.
        lCurrentRooms.Clear();
        lCurrentNodes.Clear();

        //Call the SpawnFirstRoom function, restarting the level generation.
        SpawnFirstRoom();
    }

    void ResetDPadBool()
    {
        bDPadDownPressed = false;
    }
}
