using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public abstract class AudioPlayer : MonoBehaviour
{
    protected AudioSource audioSource;
    [SerializeField] protected float pitchRandomness = 0.05f;
    protected float basePitch;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        basePitch = audioSource.pitch;
    }

    protected void PlayClipWithVariablePitch(AudioClip audioClip)
    {
        float randomPitch = Random.Range(-pitchRandomness, pitchRandomness);
        audioSource.pitch = basePitch + randomPitch;
        PlayClip(audioClip);
    }

    protected void PlayClip(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
