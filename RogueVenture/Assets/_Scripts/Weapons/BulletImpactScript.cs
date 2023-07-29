using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactScript : MonoBehaviour
{
   public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}
