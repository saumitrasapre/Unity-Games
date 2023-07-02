using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    public void CreatePrefab()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
