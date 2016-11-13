using UnityEngine;
using System.Collections;

public class textureScroller : MonoBehaviour {

    public float scrollSpeed = 1.0f;
    public float MainoffsetX = 0.0f;
    public float MainoffsetY = 0.0f;

    // Use this for initialization
    void Start () {
	
	}

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(MainoffsetX * offset, MainoffsetY * offset));        
    }
}


