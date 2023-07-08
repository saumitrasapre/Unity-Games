using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoomFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;
    [SerializeField] private int mapWidth = 20;
    [SerializeField] private int mapHeight = 20;
    [SerializeField][Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;

    //PCG Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> floorPositions, corridorPositions;

    //Events
    public UnityEvent<MapData> OnMapFloorReady;


    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        MapData data = new MapData
        {
            roomsDictionary = this.roomsDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnMapFloorReady?.Invoke(data);
    }

    private void CreateRooms()
    {
        List<BoundsInt> roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(mapWidth, mapHeight, 0)), minRoomWidth, minRoomHeight);
        floorPositions = new HashSet<Vector2Int>();
        if (randomWalkRooms)
        {
            floorPositions = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floorPositions = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>(0);
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        HashSet<Vector2Int> wideCorridors = IncreaseCorridorBrush3by3(corridors);
        floorPositions.UnionWith(wideCorridors);

        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    private void ClearRoomData()
    {
        roomsDictionary.Clear();
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {        
        roomsDictionary[roomPosition] = roomFloor;
    }

    private HashSet<Vector2Int> IncreaseCorridorBrush3by3(HashSet<Vector2Int> corridor)
    {
        HashSet<Vector2Int> newCorridor = new HashSet<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor.ElementAt(i - 1) + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        ClearRoomData();
        for (int i = 0; i < roomsList.Count; i++)
        {
            BoundsInt roomBounds = roomsList[i];
            Vector2Int roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            HashSet<Vector2Int> roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x>=(roomBounds.xMin+offset) && position.x<=(roomBounds.xMax-offset) && position.y>=(roomBounds.yMin+offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
            SaveRoomData(roomCenter, floor);
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>(0);
        Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);
        this.corridorPositions = new HashSet<Vector2Int>();

        while (roomCenters.Count > 0)
        {
            Vector2Int closestRoomCenter = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closestRoomCenter);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closestRoomCenter);
            currentRoomCenter = closestRoomCenter;
            corridors.UnionWith(newCorridor);
            this.corridorPositions.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Vector2Int position = currentRoomCenter;
        corridor.Add(position);
        while (position.y!=destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        ClearRoomData();
        foreach (var room in roomsList)
        {
            Vector2Int roomCenter = new Vector2Int(Mathf.RoundToInt(room.center.x), Mathf.RoundToInt(room.center.y));
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
            SaveRoomData(roomCenter, floor);
        }

        return floor;
    }
}
