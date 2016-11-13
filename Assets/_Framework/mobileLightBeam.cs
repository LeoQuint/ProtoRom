using UnityEngine;
using System.Collections;

public class mobileLightBeam : MonoBehaviour {

    public float angle;
    public float near;
    public float far;
    public Material[] m;
    public float scrollSpeed;

    // Use this for initialization
    void Awake()
    {
        MakeT();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MakeT();
        }
    }
    void MakeT()
    {
        if (GameController.lastLightMatUsed >= 5)
        {
            GameController.lastLightMatUsed = 0;
        }
        GameObject go = MeshMaker.CreateTriangle(angle, near, far, m[GameController.lastLightMatUsed]);
        GameController.lastLightMatUsed++;
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = transform.rotation;
        go.AddComponent<textureScroller>();
        go.GetComponent<textureScroller>().scrollSpeed = 1f;
        go.GetComponent<textureScroller>().MainoffsetY = scrollSpeed;
        go.layer = 0;
    }
}
