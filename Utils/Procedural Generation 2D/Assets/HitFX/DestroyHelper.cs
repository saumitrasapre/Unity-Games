using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject referenceObject;

    public void DestoryObject()
    {
        Destroy(referenceObject);
    }
}
