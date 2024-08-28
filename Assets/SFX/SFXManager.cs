using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance; ///THERE CAN ONLY BE ONE OF THESE. DONT ADD MORE TO ONE SCENE!!!
    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance= this; 
        }
    }
    public void playSFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject,spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.transform.SetParent(GameObject.Find("Sounds/Effects").transform);
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject,clipLength);
    }

    public void playRandSFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {

        int rand = Random.Range(0, audioClip.Length);


        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.transform.SetParent(GameObject.Find("Sounds/Effects").transform);
        audioSource.Play();
        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

    }
}
