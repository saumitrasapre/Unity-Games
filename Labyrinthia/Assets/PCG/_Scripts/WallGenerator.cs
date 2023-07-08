using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        CreateBasicWalls(tileMapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tileMapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinary = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                Vector2Int neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tileMapVisualizer.PaintSingleCornerWall(position, neighboursBinary);
        }
    }

    private static void CreateBasicWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> basicWallPositions,HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            //Check neighbours of a wall
            string neighboursBinary = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tileMapVisualizer.PaintSingleBasicWall(position,neighboursBinary);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionsList)
            {
                Vector2Int neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
