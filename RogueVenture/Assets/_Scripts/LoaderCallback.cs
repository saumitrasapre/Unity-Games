using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    //This script is used to start loading the actual game scene after the first frame of the loading scene has been rendered
    private bool isFirstUpdate = true;
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallBack();
        }
    }
}
