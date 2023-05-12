using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //SFX
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landSFX;
    [SerializeField] private AudioClip dashSFX;

    //private caches
    private AudioSource myAudioSource;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myAudioSource == null) { return; }
        if (playerController == null) { return;}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!playerController.IsJumping) return;
            PlaySFX(jumpSFX);   
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!playerController.IsDashing) return;
            PlaySFX(dashSFX);
        }
        if(!playerController.LandingLock && playerController.FallingTimer > 0.3f)
        {
            PlaySFX(landSFX);
        }
    }

    private void PlaySFX(AudioClip SFX)
    {
        myAudioSource.clip = SFX;
        myAudioSource.Play();
    }
}
