using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CorridorFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    //PCG parameters
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    //PCG Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary
        = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    private HashSet<Vector2Int> floorPositions, corridorPositions;


    //Events
    public UnityEvent<MapData> OnMapFloorReady;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
        MapData data = new MapData
        {
            roomsDictionary = this.roomsDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnMapFloorReady?.Invoke(data);
    }

    private void CorridorFirstGeneration()
    {
        floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        //tilemapVisualizer.PaintFloorTiles(floorPositions);
        //WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

        GenerateRooms(potentialRoomPositions, corridors);
        //StartCoroutine(GenerateRoomsCoroutine(potentialRoomPositions));
    }

    private void GenerateRooms(HashSet<Vector2Int> potentialRoomPositions, List<List<Vector2Int>> corridors)
    {
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);
        
        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }
    private List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }
    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>(0);
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
            {
                //handle corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                //Add a single cell in the direction of +90 degrees
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
                previousDirection = directionFromCell;
            }
        }
        return newCorridor;
    }
    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return Vector2Int.right;
        }
        if (direction == Vector2Int.right)
        {
            return Vector2Int.down;
        }
        if (direction == Vector2Int.down)
        {
            return Vector2Int.left;
        }
        if (direction == Vector2Int.left)
        {
            return Vector2Int.up;
        }
        else
        {
            //Should never run this case
            return Vector2Int.zero;
        }
    }

    private IEnumerator GenerateRoomsCoroutine(HashSet<Vector2Int> potentialRoomPositions, List<List<Vector2Int>> corridors)
    {
        yield return new WaitForSeconds(2);
        tileMapVisualizer.Clear();
        GenerateRooms(potentialRoomPositions, corridors);
        MapData data = new MapData
        {
            roomsDictionary = this.roomsDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnMapFloorReady?.Invoke(data);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                SaveRoomData(position, room);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;

            }
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        ClearRoomData();
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);

            SaveRoomData(roomPosition, roomFloor);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void ClearRoomData()
    {
        roomsDictionary.Clear();
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPosition] = roomFloor;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        this.corridorPositions = new HashSet<Vector2Int>(floorPositions);
        return corridors;
    }
}
