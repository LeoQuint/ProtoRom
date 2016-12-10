using UnityEngine;
using System.Collections;

public class animal_AI : MonoBehaviour {

    [SerializeField]
    float mean_MovementSpeed = 150f;
    [SerializeField]
    float mean_SwimRate = 0.1f;
    [SerializeField]
    float mean_MaleScale = 2f;
    [SerializeField]
    float mean_FemaleScale = 1f;
    [SerializeField]
    float mean_ChildScale = 0.5f;
    [SerializeField]
    float rateOfDance = 5f;
    [SerializeField][Range(0f,1f)]
    float chance_of_dance = 0.5f;


    //Componeent references
    private Animator anim;
    private Rigidbody rb;
    //Private values
    float m_swimRate;
    float m_Scale;
    float m_Speed;

    float m_SwimTimer;
    float m_danceTimer;

    void Awake()
    {
        anim = transform.FindChild("skel").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
	// Use this for initialization
	void Start () {
        SetPersonalities();
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("velocity", rb.velocity.magnitude);
        if (Time.time >= m_danceTimer)
        {
            m_danceTimer = Time.time + rateOfDance;
            if (Random.Range(0f, 1f) < chance_of_dance)
            {
                anim.SetTrigger("dance");
            }
        }

    }

    void FixedUpdate()
    {
       
        if (Time.time >=  m_SwimTimer)
        {
            m_SwimTimer = Time.time + m_swimRate;            
            rb.AddForce(transform.forward * m_Speed);
        }
       
    }

    void SetPersonalities()
    {
        int type = Random.Range(0,3);
        switch (type)
        {
            case 0:
                m_Scale = Random.Range(0.8f, 1.2f) * mean_MaleScale;
                break;
            case 1:
                m_Scale = Random.Range(0.8f, 1.2f) * mean_FemaleScale;
                break;
            case 2:
                m_Scale = Random.Range(0.8f, 1.2f) * mean_ChildScale;
                break;
        }

        transform.localScale = Vector3.one * m_Scale;
        m_Speed = Random.Range(0.8f, 1.2f) * mean_MovementSpeed;
        rateOfDance *= Random.Range(0.2f,1.2f);
        m_SwimTimer = Time.time + m_swimRate;
        m_danceTimer = Time.time + rateOfDance;
    }
}
