using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

    public Transform target;
    public float cameraSpeed;
    public GameObject particleEffect;
    private Transform particleHolder;
    void Awake()
    {
        particleHolder = transform.FindChild("particleHolder");
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10f), Time.deltaTime * cameraSpeed);
    }
    public void Transition()
    {
        GameObject pe = Instantiate(particleEffect, particleHolder.position, particleHolder.rotation) as GameObject;
        pe.transform.SetParent(particleHolder);
        Destroy(pe, 3f);
    }
 
}
