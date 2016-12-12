using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
