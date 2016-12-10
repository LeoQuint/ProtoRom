using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

    public Transform target;
    public float cameraSpeed;
    public GameObject particleEffect;
    private Transform particleHolder;

    public float zoomOutTo = -20f;
    private bool zoomingOut = false;

    void Awake()
    {
        particleHolder = transform.FindChild("particleHolder");
    }
    void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), Time.deltaTime * cameraSpeed);
        if (zoomingOut)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zoomOutTo), Time.deltaTime * cameraSpeed);
            if (transform.position.z <= zoomOutTo)
            {
                zoomingOut = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, zoomOutTo);
            }
        }
    }
    public void Transition()
    {
        GameObject pe = Instantiate(particleEffect, particleHolder.position, particleHolder.rotation) as GameObject;
        pe.transform.SetParent(particleHolder);
        Destroy(pe, 3f);
    }
    //For after the cluster's growth to zoom out.
    public void ZoomOut()
    {
        zoomingOut = true;
    }

    
 
}
