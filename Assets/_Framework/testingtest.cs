using UnityEngine;
using System.Collections;

public class testingtest : MonoBehaviour {

    public float angle;
    public float near;
    public float far;
    public Material m;

	// Use this for initialization
	void Start () {
        //MeshMaker.CreateTriangle(30f, 20f);
	}
    void Update() {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MakeT();
        }
    }
    public void MakeT() {
        GameObject go = MeshMaker.CreateTriangle(angle, near, far, m);
        go.AddComponent<textureScroller>();
        go.GetComponent<textureScroller>().scrollSpeed = 1f;
        go.GetComponent<textureScroller>().MainoffsetY = 4f;
        go.layer = 4;

    }
}
