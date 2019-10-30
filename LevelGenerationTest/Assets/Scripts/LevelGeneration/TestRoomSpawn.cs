using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomSpawn : MonoBehaviour
{
    //Create a public list of GameObjects called lRooms. This will be used to hold all the rooms this node can spawn.
    public List<GameObject> lRooms = new List<GameObject>();

    //Create four integer values to be used when spawning a new room.
    public int xModifier, yModifier, zModifier, randomModifier;
    //Create three integer values to be used for the position of the far OverlapSphere.
    public int xSphere, ySphere, zSphere;
    //Create Three integer values to be used for the position of the near OverlapSphere.
    public int xCloseSphere, yCloseSphere, zCloseSphere;

    //Create a public bool called bSpawnedRooms that will be set to true when a room has been spawned. This will prevent more than one room being spawned in the same place by the same node.
    public bool bSpawnedRooms = false;

    //Create an integer called iRandom that will be used when deciding what room to spawn.
    int iRandom;

    //Create a Vector3 called vecCurrentPosition, this will be used to get the current position of the GameObject.
    Vector3 vecCurrentPosition;

    //Create an array of Colliders called space, this will contain anything the far OverlapSphere overlaps with.
    public Collider[] space;
    //Create an array of Colliders called closeSpace, this will contain anything the near OverlapSphere overlaps with.
    public Collider[] closeSpace;
    //Create a public GameObject called itemChest, this will be set in the editor.
    public GameObject itemChest;

    void Update()
    {
        //vecCurrentPosition is equal to the current position of the gameobject.
        vecCurrentPosition = gameObject.transform.position;

        //Create both OverlapSpheres at the specified co-ordinates and at the specified size.
        space = Physics.OverlapSphere(new Vector3(vecCurrentPosition.x + xSphere, vecCurrentPosition.y + ySphere, vecCurrentPosition.z + zSphere), 0.05f);
        closeSpace = Physics.OverlapSphere(new Vector3(vecCurrentPosition.x + xCloseSphere, vecCurrentPosition.y + yCloseSphere, vecCurrentPosition.z + zCloseSphere), 0.05f);

        //If currentLevels is less than maxLevels (both found in the LevelManager script) and bSpawnedRooms is false and the space collider array is empty (implying the space being checked is empty)...
        if (LevelManager.levelManager.currentLevels < LevelManager.levelManager.maxLevels && !bSpawnedRooms && space.Length <= 0)
        {
            //iRandom equals a random number between 0 (inclusive) and the value of randomModifier (exclusive).
            iRandom = Random.Range(0, randomModifier);

            //Create a random room at the specified location and rotation.
            Instantiate(lRooms[iRandom], new Vector3(vecCurrentPosition.x + xModifier, vecCurrentPosition.y + yModifier, vecCurrentPosition.z + zModifier), transform.rotation);
            //Increment the currentLevels value by one.
            LevelManager.levelManager.currentLevels += 1;
            //Set bSpawnedRooms to true, this will prevent multiple rooms being spawned in the same location by the same node.
            bSpawnedRooms = true;
        }
    }

    // Use this for initialization
    void Start ()
    {
        //Add this node to the lCurrentNodes list in the LevelManager.
        LevelManager.levelManager.lCurrentNodes.Add(gameObject);      
	}

    //Because the OverlapSpheres are not visible in either the scene view or game view, using OnDrawGizmosSelected will let me draw a sphere to represent it for debuggin purposes.
    private void OnDrawGizmosSelected()
    {
        //Draw a red wireframe sphere with the same position and dimensions as the far OverlapSphere.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(vecCurrentPosition.x + xSphere, vecCurrentPosition.y + ySphere, vecCurrentPosition.z + zSphere), 0.05f);

        //Draw a blue wireframe sphere with the same position and dimensions as the near OverlapSphere.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(vecCurrentPosition.x + xCloseSphere, vecCurrentPosition.y + yCloseSphere, vecCurrentPosition.z + zCloseSphere), 0.05f);
    }
}
