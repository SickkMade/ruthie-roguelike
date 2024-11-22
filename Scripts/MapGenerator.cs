using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] roomPrefabs;
    [SerializeField]
    private GameObject[] hallwayPrefabs;
    [SerializeField, Range(1, 25)]
    private int numberOfRooms;
    [SerializeField, Range(1, 100)]
    private int offsetBetweenRooms;
    private HashSet<Vector2> listOfUsedRoomPositions = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap(){
        for(int i = 0; i < numberOfRooms; i++){
            GenerateRoom();
        }
    }

    //we will define a grid to spawn rooms on.
    //10 x 10 so max of 100 rooms, and ill make sure to count an offset
    //for room size
    private void GenerateRoom(){
        int xPos = Random.Range(0, 10);
        int yPos = Random.Range(0, 10);
        Vector2 location = new(xPos, yPos);
        if(listOfUsedRoomPositions.Contains(location)){
            GenerateRoom();
        }
        else { //if new location is empty
            listOfUsedRoomPositions.Add(location);
            Instantiate(roomPrefabs[0], location * offsetBetweenRooms, Quaternion.identity);
        }
    }
}
