using UnityEngine;
using System.Collections;

public class soundPlayer : MonoBehaviour {

    public static soundPlayer instance = null;

    AudioSource musicAud;

    public AudioClip begin;
    public AudioClip chase;

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
        if (Input.GetMouseButtonUp(2) || Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !Cursor.visible;
        }
    }

    public void Reset()
    {
        musicAud.clip = begin;
        musicAud.Play();
    }
    public void Chase()
    {
        musicAud.clip = chase;
        musicAud.Play();
    }
}
