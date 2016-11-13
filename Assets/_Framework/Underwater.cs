using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour
{

    //This script enables underwater effects. Attach to main camera.

    //Define variable
    public int underwaterLevel = 10;

    //The scene's default fog settings
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    //private Material defaultSkybox = RenderSettings.skybox;
    //private Material noSkybox;

    void Start()
    {
        bool defaultFog = RenderSettings.fog;
        Color defaultFogColor = RenderSettings.fogColor;
        float defaultFogDensity = RenderSettings.fogDensity;
    //Set the background color
    //Camera.main.backgroundColor = new Color(0, 0.4f, 0.7f, 1);
    }

    void Update()
    {
        if (transform.position.y < underwaterLevel)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.3f);
            RenderSettings.fogDensity = 0.04f;
            //RenderSettings.skybox = noSkybox;
        }
        else
        {
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
            //RenderSettings.skybox = defaultSkybox;
        }
    }
}