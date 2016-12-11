using UnityEngine;
using System.Collections;

public class PlantGrow : MonoBehaviour {

    private bool grow = false;
    private float scale = 0f;

    [SerializeField]
    float delayToGrow = 1f;
    [SerializeField]
    bool hasRandomTime = true;

    void Awake()
    {
        if (hasRandomTime) {
            delayToGrow *= Random.Range(0.5f, 1.5f);
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnEnable()
    {
        StartCoroutine(DelayToGrow());
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

    IEnumerator DelayToGrow()
    {
        yield return new WaitForSeconds(delayToGrow);
        grow = true;
    }
}
