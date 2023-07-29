using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    GameObject playerGameObject;
    [SerializeField] GameObject keyPrefab = null;
    [SerializeField] GameObject targetChestPrefab = null;

    public Transform itemParent;
    private Vector2Int playerSpawnPoint = Vector2Int.zero;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateMap;

    private void Awake()
    {
        playerGameObject = FindObjectOfType<Player>().gameObject;
    }

    public void GenerateRoomContent(MapData mapData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(mapData);
        SelectKeySpawnPoint(mapData);
        SelectTargetChestSpawnPoint(mapData);
        SelectEnemySpawnPoints(mapData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null && item.GetComponent<Player>() == null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectTargetChestSpawnPoint(MapData mapData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, mapData.roomsDictionary.Count);
        Vector2Int targetChestSpawnPoint = mapData.roomsDictionary.Keys.ElementAt(randomRoomIndex);
        List<GameObject> placedPrefabs = defaultRoom.ProcessRoom(
                   targetChestSpawnPoint,
                   mapData.roomsDictionary[targetChestSpawnPoint],
                   mapData.GetRoomFloorWithoutCorridors(targetChestSpawnPoint),
                   targetChestPrefab
                   );
        
        mapData.roomsDictionary.Remove(targetChestSpawnPoint);
    }

    private void SelectKeySpawnPoint(MapData mapData)
    {
        Vector2Int farthestPoint = graphTest.RunDijkstraAlgorithm(playerSpawnPoint, mapData.floorPositions);
        Vector2Int keySpawnPoint = Vector2Int.one;
        foreach (var roomCenter in mapData.roomsDictionary.Keys)
        {
            if (mapData.roomsDictionary[roomCenter].Contains(farthestPoint))
            {
                keySpawnPoint = roomCenter;
                break;
            }
        }
        if (keySpawnPoint == Vector2Int.zero)
        {
            keySpawnPoint = mapData.roomsDictionary.Keys.ElementAt(UnityEngine.Random.Range(0, mapData.roomsDictionary.Count));
        }
        List<GameObject> placedPrefabs = defaultRoom.ProcessRoom(
                   keySpawnPoint,
                   mapData.roomsDictionary[keySpawnPoint],
                   mapData.GetRoomFloorWithoutCorridors(keySpawnPoint),
                   keyPrefab
                   );
        spawnedObjects.AddRange(placedPrefabs);
        mapData.roomsDictionary.Remove(keySpawnPoint);
    }

    private void SelectPlayerSpawnPoint(MapData mapData)
    {

        if (playerGameObject == null)
        {
            Debug.LogError("Player not found in scene!");
        }
        int randomRoomIndex = UnityEngine.Random.Range(0, mapData.roomsDictionary.Count);
        playerSpawnPoint = mapData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        Vector2Int roomIndex = mapData.roomsDictionary.Keys.ElementAt(randomRoomIndex);        

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            mapData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            mapData.GetRoomFloorWithoutCorridors(roomIndex),
            this.playerGameObject
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
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in mapData.roomsDictionary)
        {
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value,
                    mapData.GetRoomFloorWithoutCorridors(roomData.Key),
                    null
                    )
            );

        }
    }

}
