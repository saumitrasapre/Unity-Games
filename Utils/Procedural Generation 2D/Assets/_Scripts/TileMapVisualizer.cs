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
    [SerializeField] private List<TileBase> wallTop;
    [SerializeField] private List<TileBase> wallSideRight;
    [SerializeField] private List<TileBase> wallSideLeft;
    [SerializeField] private List<TileBase> wallBottom;
    [SerializeField] private List<TileBase> wallFull;
    [SerializeField] private List<TileBase> wallInnerCornerDownLeft;
    [SerializeField] private List<TileBase> wallInnerCornerDownRight;
    [SerializeField] private List<TileBase> wallDiagonalCornerDownRight;
    [SerializeField] private List<TileBase> wallDiagonalCornerDownLeft;
    [SerializeField] private List<TileBase> wallDiagonalCornerUpLeft;
    [SerializeField] private List<TileBase> wallDiagonalCornerUpRight;


    private void Awake()
    {
        wallTop = new List<TileBase>();
        wallSideRight = new List<TileBase>();
        wallSideLeft = new List<TileBase>();
        wallBottom = new List<TileBase>();
        wallFull = new List<TileBase>();
        wallInnerCornerDownLeft = new List<TileBase>();
        wallInnerCornerDownRight = new List<TileBase>();
        wallDiagonalCornerDownRight = new List<TileBase>();
        wallDiagonalCornerDownLeft = new List<TileBase>();
        wallDiagonalCornerUpLeft = new List<TileBase>();
        wallDiagonalCornerUpRight = new List<TileBase>();
    }

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
            tile = wallTop[Random.Range(0,wallTop.Count)];
        } 
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight[Random.Range(0, wallSideRight.Count)];
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft[Random.Range(0, wallSideLeft.Count)];
        }
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom[Random.Range(0, wallBottom.Count)];
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull[Random.Range(0, wallFull.Count)];
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
            tile = wallInnerCornerDownLeft[Random.Range(0, wallInnerCornerDownLeft.Count)];
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight[Random.Range(0, wallInnerCornerDownRight.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft[Random.Range(0, wallDiagonalCornerDownLeft.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight[Random.Range(0, wallDiagonalCornerDownRight.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft[Random.Range(0, wallDiagonalCornerUpLeft.Count)];
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight[Random.Range(0, wallDiagonalCornerUpRight.Count)];
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull[Random.Range(0, wallFull.Count)];
        }
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom[Random.Range(0, wallBottom.Count)];
        }

        if (tile != null)
        {
            PaintSingleTile(wallTileMap, tile, position);
        }
    }
}
