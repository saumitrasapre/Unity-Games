using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHittable
{
    public UnityEvent OnGetHit { get; set; }
    void GetHit(int damage, GameObject damageDealer);
}
