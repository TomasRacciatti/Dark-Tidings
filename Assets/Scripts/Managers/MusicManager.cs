using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Initial Music")] 
    [SerializeField] private AudioClip initialTrack;
    [SerializeField] private bool initialLoop = true;
    [SerializeField] private float initialFadeTime = 1f;
    [SerializeField] private float initialVolume = 0.5f;

    [Header("Cross-fade settings")] 
    [SerializeField] private float defaultFadeDuration = 1f;

    private AudioSource _activeSource;
    private AudioSource _idleSource;
    private Coroutine _fadeCoroutine;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        _activeSource = gameObject.AddComponent<AudioSource>();
        _idleSource = gameObject.AddComponent<AudioSource>();

        _activeSource.playOnAwake = false;
        _idleSource.playOnAwake = false;
        
        if (initialTrack != null)
            PlayMusic(initialTrack, initialLoop, initialFadeTime, initialVolume);
    }


    public void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = -1f, float volume = 1f)
    {
        if (clip == _activeSource.clip) return;

        if (fadeDuration < 0) fadeDuration = defaultFadeDuration;

        // swap roles
        (_idleSource, _activeSource) = (_activeSource, _idleSource);
        _activeSource.clip = clip;
        _activeSource.loop = loop;
        _activeSource.volume = 0f;
        _activeSource.Play();

        // do the fade
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(CrossFade(fadeDuration, volume));
    }


    public void SetVolume(float targetVolume, float fadeDuration = -1f)
    {
        if (fadeDuration < 0) fadeDuration = defaultFadeDuration;
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeVolume(_activeSource, targetVolume, fadeDuration));
    }

    private IEnumerator CrossFade(float duration, float targetVolume)
    {
        float elapsed = 0f;
        float startIdleVol  = _idleSource.volume;
        _activeSource.volume = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _activeSource.volume = Mathf.Lerp(0f, targetVolume, t);
            _idleSource.volume = Mathf.Lerp(startIdleVol, 0f, t);
            yield return null;
        }

        _idleSource.Stop();
        _idleSource.volume = startIdleVol;
        _activeSource.volume = targetVolume;
    }

    private IEnumerator FadeVolume(AudioSource src, float target, float duration)
    {
        float start = src.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            src.volume = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        src.volume = target;
    }
    
    
    public void SetPausedMusic(bool paused)
    {
        if (paused)
        {
            _activeSource.Pause();
            _idleSource  .Pause();
        }
        else
        {
            _activeSource.UnPause();
            _idleSource  .UnPause();
        }
    }
}