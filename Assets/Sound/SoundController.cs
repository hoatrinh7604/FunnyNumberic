using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _instance;
    public static SoundController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //
    [SerializeField] GameObject prefab;

    public AudioClip bg;
    private AudioSource bgSource;
    public AudioClip gameOver;
    private AudioSource gameOverSource;   
    public AudioClip click;
    private AudioSource clickSource;    
    public AudioClip bang;
    private AudioSource bangSource;

    private AudioSource aud;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayAudio(this.click, 0.5f, false);
        }
    }

    public void PlayAudio(AudioClip audio, float volume, bool isLoopback)
    {
        if (audio == this.bg)
        {
            Play(audio, ref bgSource, volume, isLoopback);
            return;
        }
        if (audio == this.gameOver)
        {
            Play(audio, ref gameOverSource, volume, isLoopback);
            return;
        }
        if (audio == this.click)
        {
            Play(audio, ref clickSource, volume, isLoopback);
            return;
        }
        if (audio == this.bang)
        {
            Play(audio, ref bangSource, volume, isLoopback);
            return;
        }
    }

    private void Play(AudioClip audio, ref AudioSource audioSource, float volume, bool isLoopback = false)
    {
        audioSource = Instantiate(Instance.prefab).GetComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.loop = isLoopback;
        audioSource.clip = audio;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void StopAudio(AudioClip audio)
    {
        if (audio == this.bg)
        {
            bgSource?.Stop();
            return;
        }
        if (audio == this.gameOver)
        {
            gameOverSource?.Stop();
            return;
        }
        if (audio == this.click)
        {
            clickSource?.Stop();
            return;
        }
        if (audio == this.bang)
        {
            bangSource?.Stop();
            return;
        }
    }
}
