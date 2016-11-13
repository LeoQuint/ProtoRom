using UnityEngine;
using System.Collections;

public class scan : MonoBehaviour {

    public float growSpeed;
    public float maxScale;

    private float currentScale;
    public GameObject particleFlash;
    public GameObject rip;

    // Use this for initialization
    void Start () {
        transform.localScale = Vector3.zero;
        currentScale = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentScale += growSpeed * Time.deltaTime;
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if (currentScale >= maxScale)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "food")
        {
            Instantiate(rip, other.transform.position, Quaternion.identity);
            Instantiate(particleFlash, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z -0.4f), Quaternion.identity);
        }
    }
}
