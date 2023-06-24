using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SFXManager : MonoBehaviour
{
    
    
    //SFX
    [SerializeField] public AudioClip jumpSFX;
    [SerializeField] public AudioClip landSFX;
    [SerializeField] public AudioClip dashSFX;
    [SerializeField] public AudioClip moveSFX;
    [SerializeField] public AudioClip pickupSFX;
    [SerializeField] public AudioClip crateSFX;

    

    

    

    //private caches
    private AudioSource myAudioSource;
    private PlayerController playerController;
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource    = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        playerInput      = GetComponent<PlayerInput>();
        
        // playerInput.actions["Jump"].performed += _ => PlaySFX(jumpSFX);
        // playerInput.actions["Dash"].performed += _ => PlaySFX(dashSFX);
    }
    
    public void PlaySFX(AudioClip SFX)
    {
        myAudioSource.clip = SFX;
        myAudioSource.Play();
    }
}
