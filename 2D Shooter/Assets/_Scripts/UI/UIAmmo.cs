using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAmmo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    public void UpdateBulletText(int bulletCount)
    {
        if (bulletCount == 0)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }

        text.SetText(bulletCount.ToString());
    }
}
