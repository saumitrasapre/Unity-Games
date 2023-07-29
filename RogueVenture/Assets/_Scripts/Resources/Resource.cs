using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class Resource : MonoBehaviour
{
    [field: SerializeField]
    public ResourceDataSO ResourceData { get; set; }
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PickupResource()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Light2D light = GetComponent<Light2D>();
        if (light != null)
        {
            light.enabled = false;
        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}
