using UnityEngine;
using System.Collections;

public class cell_AI : MonoBehaviour {

    public Color[] colors;
    public float baseMovementSpeed;
    public float detectionRange;
    // Use this for initialization

    public bool isFleeing = false;

    float moveMag;
    float moveTimer;
    float moveSpeed;
    float moveFreq;

    float dirChangeTimer;
    float dirChangeFreq;

    float burstCD;
    float burstTimer;

    Vector3 randomDirection;

    Rigidbody rb;
    Animator anim;

    public int type;

    public bool isMate = false;
    public float m_lifetime= 30f;
    void Awake()
    {
        type = Random.Range(0,3);
        anim = GetComponent<Animator>();
        GetComponent<SphereCollider>().radius = detectionRange;
    }

    void Start () {
        GetComponent<Light>().color = colors[type];
        rb = GetComponent<Rigidbody>();
        moveSpeed = Random.Range(1f, 10f);
        float sizeX = 2f * (moveSpeed/10f);
       
        moveFreq = Random.Range(1f, 10f);
        float sizeY = 2f * (moveSpeed / 10f);
        moveMag = Random.Range(1f, 5f);
        transform.localScale = new Vector3(sizeX, sizeY, sizeX);
        
        dirChangeTimer = Time.time;
        moveTimer = Time.time;
        dirChangeFreq = Random.Range(1f,10f);
        burstCD = Random.Range(1f,5f);

        randomDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
        burstTimer = Time.time;
        Destroy(gameObject, m_lifetime);
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("velocity", rb.velocity.magnitude);
        if (isMate)
        {
            Escape(true);
            return;
        }

        if (isFleeing)
        {
            Escape();
        }
        else
        {
            Wander();
        }
        
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -400f,400f), Mathf.Clamp(transform.position.y, -70f,10f), 0f);
    }
    void Wander()
    {
        if (Time.time >= dirChangeTimer + dirChangeFreq)
        {
            dirChangeTimer = Time.time;
            randomDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f).normalized;
        }
        Vector3 targetDir = randomDirection - transform.position.normalized;
        float step = 1f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
    
        transform.rotation = Quaternion.LookRotation(newDir);

        if (Time.time >= burstCD + burstTimer)
        {
            burstTimer = Time.time;
            rb.AddForce(transform.forward * moveMag * baseMovementSpeed);
        }
       
    }
    Transform playerPos;
    void Escape()
    {
        Vector3 targetDir = playerPos.position - transform.position;
       
        
        float step = 1f * Time.deltaTime;
       
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, -step, 0.0F);

      
        transform.rotation = Quaternion.LookRotation(newDir);

        if (Time.time >= (burstCD/2f) + burstTimer)
        {
            burstTimer = Time.time;
            rb.AddForce(transform.forward * moveMag * baseMovementSpeed);
        }
    }
    public float timeToChase;
    private float timeChased;
    void Escape(bool isT)
    {
        if (gameover)
        {
     
            transform.rotation = Quaternion.LookRotation(Vector3.up);          
            rb.AddForce(transform.forward * 1f * baseMovementSpeed * 2f * Time.deltaTime);
            return;
        }
        Vector3 targetDir = new Vector3(-500f, 500f, 0f) - transform.position;       
        float step = 1f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir.normalized, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
        if (Time.time >= (burstCD / 10f) + burstTimer && isFleeing)
        {
            burstTimer = Time.time;
            rb.AddForce(transform.forward * 1f * baseMovementSpeed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player")
        {
            playerPos = other.transform;
            isFleeing = true;
        }
    }
    private bool gameover = false;
    void OnTriggerStay(Collider other)
    {
        if (gameover)
        {
            return;
        }
        if (isMate && other.tag == "Player")
        {
            timeChased += Time.deltaTime;
            if (timeChased >= timeToChase)
            {
                gameover = true;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GameOver();
            }
            return;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isFleeing = false;
        }
    }
}
