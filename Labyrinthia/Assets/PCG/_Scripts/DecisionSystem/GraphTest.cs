using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GraphTest : MonoBehaviour
{
    Graph graph;

    //bool graphReady = false;

    Dictionary<Vector2Int, int> dijkstraResult;
    int highestValue;

    public Vector2Int RunDijkstraAlgorithm(Vector2Int playerPosition,IEnumerable<Vector2Int> floorPositions)
    {
        //graphReady = false;
        graph = new Graph(floorPositions);
        dijkstraResult = DijkstraAlgorithm.Dijkstra(graph, playerPosition);
        //Debug.Log("Player start " + playerPosition);
        //foreach (var key in dijkstraResult.Keys)
        //{
        //    Debug.Log("Key - " + key + " Value - " + dijkstraResult[key]);
        //}
        highestValue = dijkstraResult.Values.Max();
        Vector2Int maxKey = dijkstraResult.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        //graphReady = true;
        return maxKey;
    }
}
