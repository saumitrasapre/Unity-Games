using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private List<TileBase> floorTile;
    [SerializeField] private List<TileBase> wallTopTiles;
    [SerializeField] private List<TileBase> wallSideRightTiles;
    [SerializeField] private List<TileBase> wallSideLeftTiles;
    [SerializeField] private List<TileBase> wallBottomTiles;
    [SerializeField] private List<TileBase> wallFullTiles;
    [SerializeField] private List<TileBase> wallInnerCornerDownLeftTiles;
    [SerializeField] private List<TileBase> wallInnerCornerDownRightTiles;
    [SerializeField] private List<TileBase> wallDiagonalCornerDownRightTiles;
    [SerializeField] private List<TileBase> wallDiagonalCornerDownLeftTiles;
    [SerializeField] private List<TileBase> wallDiagonalCornerUpLeftTiles;
    [SerializeField] private List<TileBase> wallDiagonalCornerUpRightTiles;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTileMap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TileBase> tileVariants)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tileVariants[Random.Range(0,tileVariants.Count)], position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binary)
    {
        int typeAsInt = Convert.ToInt32(binary, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTopTiles[Random.Range(0,wallTopTiles.Count)];
        } 
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRightTiles[Random.Range(0, wallSideRightTiles.Count)];
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeftTiles[Random.Range(0, wallSideLeftTiles.Count)];
        }
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottomTiles[Random.Range(0, wallBottomTiles.Count)];
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFullTiles[Random.Range(0, wallFullTiles.Count)];
        }
        if (tile != null)
        {
            PaintSingleTile(wallTileMap, tile, position);
        }

    }

    internal void PaintSingleCornerWall(Vector2Int position, string binary)
    {
        int typeAsInt = Convert.ToInt32(binary, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeftTiles[Random.Range(0, wallInnerCornerDownLeftTiles.Count)];
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRightTiles[Random.Range(0, wallInnerCornerDownRightTiles.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeftTiles[Random.Range(0, wallDiagonalCornerDownLeftTiles.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRightTiles[Random.Range(0, wallDiagonalCornerDownRightTiles.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeftTiles[Random.Range(0, wallDiagonalCornerUpLeftTiles.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRightTiles[Random.Range(0, wallDiagonalCornerUpRightTiles.Count)];
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFullTiles[Random.Range(0, wallFullTiles.Count)];
        }
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = wallBottomTiles[Random.Range(0, wallBottomTiles.Count)];
        }

        if (tile != null)
        {
            PaintSingleTile(wallTileMap, tile, position);
        }
    }
}
