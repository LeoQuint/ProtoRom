using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Play();
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
