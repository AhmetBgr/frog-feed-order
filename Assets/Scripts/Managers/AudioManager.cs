using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    public GameObject audioSourcePrefab;

    public static AudioManager instance;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySound(SoundEffect sound, float delay = 0f, bool playReverse = false) {

        AudioSource audioSource = GetAudioSource();

        if (sound == null | audioSource == null) {
            Debug.LogWarning("sound did not played");
            return;
        } 

        StartCoroutine(PlaySoundWithDelay(sound, delay, audioSource, playReverse)); 
    }


    private IEnumerator PlaySoundWithDelay(SoundEffect sound,
        float delay, AudioSource audioSource, bool playReverse = false) {

        yield return new WaitForSecondsRealtime(delay);

        sound.Play(audioSource);
        StartCoroutine(WaitForSound(audioSource, () => ObjectPooler.instance.AddToPool(audioSource.name, audioSource.gameObject)));
    }

    private IEnumerator WaitForSound(AudioSource source, Action onCompleteCallBack) {
        //Wait Until Sound has finished playing
        yield return new WaitForSeconds(source.clip.length);

        source.clip = null;

        onCompleteCallBack?.Invoke();
    }

    private AudioSource GetAudioSource() {
        const int maxAttempts = 100; // Avoid infinite looping by limiting attempts
        int attempts = 0;

        while (attempts < maxAttempts) {
            AudioSource audioSource = ObjectPooler.instance.SpawnFromPool(audioSourcePrefab.name).GetComponent<AudioSource>();

            if (!audioSource.isPlaying && audioSource.clip == null) {
                audioSource.transform.SetParent(transform);
                return audioSource;
            }

            // If the AudioSource is not valid, add it back to the pool and try again.
            ObjectPooler.instance.AddToPool(audioSourcePrefab.name, audioSource.gameObject);

            attempts++;
        }

        // If no valid AudioSource is found after maxAttempts, log an error.
        Debug.LogError("Failed to find a valid AudioSource after multiple attempts.");
        return null; // Return null to indicate failure.
    }

}
