using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipTrack
{
    ButtonClick, 
    CircleSpawned, 
    BackgroundMusic, 
    WinSound, 
    LoseSound, 
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private AudioSource circleSpawn;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound; 

    public static AudioManager Instance;

    public void PlayAudioClip(AudioClipTrack soundClip)
    {
        switch (soundClip)
        {
            case AudioClipTrack.ButtonClick:
                buttonClick.Play(); 
                break;

            case AudioClipTrack.CircleSpawned:
                Debug.Log("CircleSpawnSound"); 
                circleSpawn.Play(); 
                break;

            case AudioClipTrack.BackgroundMusic:
                break;

            case AudioClipTrack.WinSound:
                Debug.Log("WinSpawnSound");
                winSound.Play(); 
                break;

            case AudioClipTrack.LoseSound:
                loseSound.Play(); 
                break;

            default:
                break;
        }
    }

    private void Awake()
    {
        if (AudioManager.Instance == null)
        {
            AudioManager.Instance = this;
        }
        else
        {
            if (AudioManager.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
