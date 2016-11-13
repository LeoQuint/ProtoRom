//#define DEBUG
using UnityEngine;
using System.Collections;

public static class MeshMaker {
    //create a "triangle" view with a far and near plane.
    public static GameObject CreateTriangle(float angle, float near, float far, Material mt)
    {
        GameObject go = new GameObject("Triangle");
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        Mesh m = new Mesh();
        m.vertices = new Vector3[]
            {
                new Vector3( -(near *  Mathf.Tan(Mathf.Deg2Rad* angle/2f)),-near,  0f),
                new Vector3( (near * Mathf.Tan(Mathf.Deg2Rad* angle/2f)),-near, 0f),
                new Vector3( -(far *  Mathf.Tan(Mathf.Deg2Rad* angle/2f)),-far,  0f),
                new Vector3( (far * Mathf.Tan(Mathf.Deg2Rad* angle/2f)),-far, 0f),
                new Vector3( 0f , -(near + ((far-near)/2f)),  0f)
            };
#if DEBUG
        Debug.Log(m.vertices[0]);
        Debug.Log(m.vertices[1]);
        Debug.Log(m.vertices[2]);
        Debug.Log(m.vertices[3]);
        Debug.Log(m.vertices[4]);
        Debug.Log((0.5f * near / far));
        Debug.Log(0.5f - (0.5f * near / far));
        Debug.Log(0.5f + (0.5f * near / far));
#endif
        m.uv = new Vector2[]
           {
                new Vector2(0.5f - (0.5f*near/far) , 1f),
                new Vector2(0.5f + (0.5f*near/far), 1f),  
                new Vector2(0f, 0f),
                new Vector2(1f, 0f),
                new Vector2(0.5f, 0.5f)
           };
        m.triangles = new int[] {
                                    0,4,2,
                                    4,0,1,
                                    1,3,4,
                                    2,4,3                                       
                                };
        mr.material = mt;
        mf.mesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();
        

        return go;
    }

}
