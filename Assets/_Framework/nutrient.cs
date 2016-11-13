using UnityEngine;
using System.Collections;

public class nutrient : MonoBehaviour {

    [SerializeField]
    GameObject m_eatenFX;
    public float m_lifetime = 20f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, m_lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {    
            other.GetComponent<player>().NutrientReceived(1);
            Instantiate(m_eatenFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
