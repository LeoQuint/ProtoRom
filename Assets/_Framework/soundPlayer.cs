using UnityEngine;
using System.Collections;

public class soundPlayer : MonoBehaviour {

    public static soundPlayer instance = null;

    AudioSource musicAud;

    public AudioClip begin;
    public AudioClip chase;

    public static int Language = 0;

    float musicFadeLength = 8f;
    public void SetLanguage(int l)
    {
        Language = l;
    }


    void Awake()
    {
        Cursor.visible = false;
#if UNITY_EDITOR
        Cursor.visible = true;
#endif
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        musicAud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
        {
            Cursor.visible = !Cursor.visible;
        }
        if (isFading)
        {
            musicAud.volume -= Time.deltaTime / musicFadeLength;
        }
    }

    public void Reset()
    {
        isFading = false;
        musicAud.volume = 1f;
        musicAud.clip = begin;
        musicAud.Play();
    }
    public void Chase()
    {
        musicAud.clip = chase;
        musicAud.Play();
    }
    private bool isFading = false;
    public void FadeMusic(float delay)
    {
        musicFadeLength = delay;
        isFading = true;
    }
}
