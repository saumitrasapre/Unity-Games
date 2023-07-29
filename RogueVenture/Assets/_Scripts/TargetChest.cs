using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChest : MonoBehaviour
{
    AudioSource audioSource;
    Animator targetChestAnimator;
    private const string TARGETCHESTTRIGGER = "TargetChestTrigger";

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        targetChestAnimator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        //GetComponent<Collider2D>().enabled = false;
        //GetComponent<SpriteRenderer>().enabled = false;
        audioSource.Play();
        targetChestAnimator.SetTrigger(TARGETCHESTTRIGGER);
        yield return new WaitForSeconds(audioSource.clip.length);
        //Destroy(gameObject);
    }
}
