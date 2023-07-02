using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;


    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateMap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
            RegenerateMap?.Invoke();
        }
    }
    public void GenerateRoomContent(MapData mapData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(mapData);
        SelectEnemySpawnPoints(mapData);

        foreach (GameObject item in spawnedObjects)
        {
            if(item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectPlayerSpawnPoint(MapData mapData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, mapData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = mapData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        //graphTest.RunDijkstraAlgorithm(playerSpawnPoint, mapData.floorPositions);

        Vector2Int roomIndex = mapData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            mapData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            mapData.GetRoomFloorWithoutCorridors(roomIndex)
            );

        FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        mapData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }

    private void SelectEnemySpawnPoints(MapData mapData)
    {
        foreach (KeyValuePair<Vector2Int,HashSet<Vector2Int>> roomData in mapData.roomsDictionary)
        { 
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value, 
                    mapData.GetRoomFloorWithoutCorridors(roomData.Key)
                    )
            );

        }
    }

}
