using UnityEngine;
using System.Collections;

public class checkPoint : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
