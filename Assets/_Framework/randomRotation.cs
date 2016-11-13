using UnityEngine;
using System.Collections;

public class randomRotation : MonoBehaviour {

    float dirChangeTimer;
    float dirChangeFreq;
    float rotationSpeed;

    float moveMag;
    float moveTimer;
    float moveSpeed;
    float moveFreq;

    Quaternion targetRotation;
    Vector3 targetPosition;
    Vector3 origin;
    bool backToOrigin = false;
    bool hasSplit = false;
    // Use this for initialization
 
	void Start () {
        rotationSpeed = Random.Range(1f,10f);
        dirChangeFreq = Random.Range(1f,10f);

        moveSpeed = Random.Range(1f, 10f);
        moveFreq = Random.Range(1f,10f);

        dirChangeTimer = Time.time;
        moveTimer = Time.time;
        targetRotation = Random.rotation;
        //origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //targetPosition = new Vector3(origin.x, origin.y, origin.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= dirChangeTimer + dirChangeFreq)
        {
            dirChangeTimer = Time.time;
            targetRotation = Random.rotation;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (!hasSplit)
        {
            return;
        }
    

        if (Time.time >= moveTimer + moveFreq)
        {
            
            moveTimer = Time.time;
            if (backToOrigin)
            {
                targetPosition = new Vector3(origin.x, origin.y, origin.z);
                backToOrigin = false;
            }
            else
            {
                targetPosition = new Vector3(   Random.Range(origin.x -0.1f, origin.x + 0.1f), 
                                                Random.Range(origin.y - 0.1f, origin.y + 0.1f), 
                                                Random.Range(origin.z - 0.1f, origin.z + 0.1f));
                backToOrigin = true;
            }
           
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
        

	}

    public void Split(Vector3 targetLoc)
    {
        targetPosition = new Vector3(targetLoc.x, targetLoc.y, targetLoc.z);
        origin = new Vector3(targetLoc.x, targetLoc.y, targetLoc.z);
        backToOrigin = true;
        moveTimer = Time.time + 5f;
        hasSplit = true;
    }
}
