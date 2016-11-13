using UnityEngine;
using System.Collections;

public class displayFollow : MonoBehaviour {


    public float lerpSpeed = 10f;
    Transform player;
    public Vector3 offset;
    public Material[] materials;

    public  GameObject[] img = new GameObject[3];
	// Use this for initialization
	void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        img[0] = transform.FindChild("1").gameObject;
        img[1] = transform.FindChild("2").gameObject;
        img[2] = transform.FindChild("3").gameObject;
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf)
        {
            transform.position = Vector3.Lerp(transform.position, player.position+ offset, lerpSpeed*Time.deltaTime);
        }
	}

    public void Activate()
    {
        transform.position = player.position + offset;
        gameObject.SetActive(true);
        StartCoroutine(Hide());
    }
    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void SetCounter(int counterIndex, int materialIndex)
    {
        img[counterIndex].GetComponent<MeshRenderer>().material = materials[materialIndex];
        img[counterIndex].gameObject.SetActive(true);
    }
    public void Reset()
    {
        img[0].SetActive(false);
        img[1].SetActive(false);
        img[2].SetActive(false);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(2f);
        DeActivate();
    }
}
