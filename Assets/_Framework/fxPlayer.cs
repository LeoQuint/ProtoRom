using UnityEngine;
using System.Collections;

public class fxPlayer : MonoBehaviour {
    public AudioClip[] clips;
    public float scalingSpeed = 0.01f;
    AudioSource aud;
    AudioSource scaledAud;
    void Awake()
    {
        aud = GetComponent<AudioSource>();
        scaledAud = transform.FindChild("scaling").GetComponent<AudioSource>();
    }

    public void PlayOnce(int c)
    {
        aud.PlayOneShot(clips[c]);
    }

   
    bool scaling = false;
    void Update()
    {
        Debug.Log(scaledAud.volume);
        if (scaling)
        {
            scaledAud.volume += scalingSpeed;
        }
        else
        {
           
            if (scaledAud.volume <= 0f)
            {
                scaledAud.Stop();
                return;
            }
            scaledAud.volume -= scalingSpeed;
        }
    }
    public void PlayScaling()
    {
        scaledAud.volume = 0.5f;
        scaling = true;
        scaledAud.Play();
    }
    public void PlayScaling(bool ud)
    {        
        scaling = false;
    }

}
