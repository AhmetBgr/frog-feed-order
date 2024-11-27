using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    public List<AudioSource> audioSources = new List<AudioSource>();
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

        if (sound == null | audioSource == null) return;

        StartCoroutine(PlaySoundWithDelay(sound, delay, playReverse)); 
        //audioSource.PlayOneShot(sound);
    }


    private IEnumerator PlaySoundWithDelay(SoundEffect sound,
        float delay, bool playReverse = false) {

        yield return new WaitForSeconds(delay);

        sound.Play();

        //PlaySound(sound, playReverse);
    }

    private AudioSource GetAudioSource() {
        for (int i = 0; i < audioSources.Count; i++) {
            if (audioSources[i].isPlaying)
                continue;
            else
                return audioSources[i];
        }

        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        audioSource.transform.SetParent(this.transform);
        audioSources.Add(audioSource);
        return null;
    }

}
