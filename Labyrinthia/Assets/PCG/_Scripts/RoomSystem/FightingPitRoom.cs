using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightingPitRoom : RoomGenerator
{
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public List<EnemyPlacementData> enemyPlacementData;
    public List<ItemPlacementData> itemData;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors, GameObject specialGameObject)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);
        if (specialGameObject != null)
        {
            GameObject keyObject = prefabPlacer.CreateObject(specialGameObject, roomCenter + new Vector2(0.5f, 0.5f));
        }

        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));

        return placedObjects;
    }
}
