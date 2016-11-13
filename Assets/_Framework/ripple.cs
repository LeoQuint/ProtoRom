using UnityEngine;
using System.Collections;

public class ripple : MonoBehaviour {

    [SerializeField]
    float fadeSpeed;

    float alpha;
    [SerializeField]
    float growthSpeed;

    [SerializeField]
    float lifeTime;

    float size;
    private Material mat;
	// Use this for initialization
	void Awake () {
        size = 0f;
        mat = GetComponent<Renderer>().material;
        alpha = 0.5f;
	}

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
	
	// Update is called once per frame
	void Update () {

        size += growthSpeed * Time.deltaTime;
        transform.localScale = new Vector3(size, size, size);
        alpha -= (fadeSpeed * Time.deltaTime);
        mat.color = new Color(1f,1f,1f,alpha);

	}
}
