using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class surfaceSound : MonoBehaviour {

    Transform player;
    AudioSource aud;

    public float minDistance = 10f;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aud = GetComponent<AudioSource>();
	}
    float playerDistance;
	// Update is called once per frame
	void Update () {
        playerDistance = (player.position.y + minDistance) - transform.position.y;
        if (playerDistance >  0f)
        {
            aud.volume = playerDistance/ minDistance;
        }
	}
}
