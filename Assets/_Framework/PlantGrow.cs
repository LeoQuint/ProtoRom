using UnityEngine;
using System.Collections;

public class PlantGrow : MonoBehaviour {

    private bool grow = false;
    private float scale = 0f;

    void Awake()
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnEnable()
    {
        grow = true;
    }

    void OnDisable()
    {
        scale = 0f;
    }

    void Update()
    {
        if (grow)
        {
            scale += 50f * Time.deltaTime;
           
            if (scale >= 10f)
            {
                scale = 10f;
                grow = false;
            }
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
