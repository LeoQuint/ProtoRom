using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class language : MonoBehaviour {

    private int lang = 0;

    public Sprite en;
    public Sprite fr;

    public GameObject begin;
    public GameObject commencer;

    void Awake()
    {
        begin.SetActive(true);
        commencer.SetActive(false);
       
    }

    void Start()
    {
        soundPlayer.instance.SetLanguage(0);
    }

    public void SetLanguage()
    {
        ++lang;
        if (lang > 1)
        {
            begin.SetActive(true);
            commencer.SetActive(false);
            lang = 0;
            GetComponent<Image>().sprite = fr;
        }
        else
        {
            begin.SetActive(false);
            commencer.SetActive(true);
            GetComponent<Image>().sprite = en;
        }
        soundPlayer.instance.SetLanguage(lang);
    }
}
