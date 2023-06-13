using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsCounterVisual : MonoBehaviour
{
    [SerializeField] private CupsCounter cupsCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform cupVisualPrefab;

    private List<GameObject> cupVisualGameObjectList;

    private void Awake()
    {
        cupVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        cupsCounter.OnCupSpawned += CupsCounter_OnCupSpawned;
        cupsCounter.OnCupRemoved += CupsCounter_OnCupRemoved;
    }

    private void CupsCounter_OnCupRemoved(object sender, System.EventArgs e)
    {
        GameObject cupGameObject = cupVisualGameObjectList[cupVisualGameObjectList.Count - 1];
        cupVisualGameObjectList.Remove(cupGameObject);
        Destroy(cupGameObject);
    }

    private void CupsCounter_OnCupSpawned(object sender, System.EventArgs e)
    {
        Transform cupVisualTransform = Instantiate(cupVisualPrefab, counterTopPoint);

        float cupOffsetY = .1f;
        cupVisualTransform.localPosition = new Vector3(0, cupOffsetY * cupVisualGameObjectList.Count, 0);
        cupVisualGameObjectList.Add(cupVisualTransform.gameObject);
    }

}
