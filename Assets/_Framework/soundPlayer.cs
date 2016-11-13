using UnityEngine;
using System.Collections;

public class soundPlayer : MonoBehaviour {

    public static soundPlayer instance = null;

    AudioSource musicAud;

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
}
