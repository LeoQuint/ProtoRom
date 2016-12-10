using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class mate_AI : MonoBehaviour
{

    /// <summary>
    /// Public Variables
    /// </summary>
    public float m_movementSpeed;
    public float m_swimRate;
    public GameObject m_checkPointPrefab;
    public float m_distancePerCheckPoint;
    public float m_maxSideAmplitude;
    public float m_turnRate;
    public float m_detectionRangeRatio = 20;
    /// <summary>
    /// Private Variables
    /// </summary>

    private bool m_IsMoving;
    private float m_lastSwimBurst;
    //The array positions of m_checkpoint & m_destinationToCheckPoint.
    private int[] m_currentDestination = new int[2] {0,0};
    //Contains 5 points between current checkpoint and the enxt to simulate swimming.
    private Vector3[] m_destinationToCheckPoint = new Vector3[5];
    //Holds all checkpoints
    private GameObject[] m_checkPoints = new GameObject[3];
    private float distanceToPlayerRatio;
    private Quaternion m_worldLimits;
    private Transform playerPos;
    private Rigidbody rb;
    private Animator anim;



    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    void Start()
    {
        m_IsMoving = false;
        m_lastSwimBurst = Time.time;
    }

    void SetCheckPoints()
    {
        float count = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < m_checkPoints.Length; i++)
        {
            GameObject point = Instantiate(m_checkPointPrefab, new Vector3(0f, 100f, 0f), Quaternion.identity) as GameObject;
            point.name = i.ToString();
            bool outOfBound;
            Vector3 pointPosition;
            do
            {
                outOfBound = false;
                float xAbs = Random.Range(0f, m_distancePerCheckPoint);
                float xDir = Random.Range(0, 100) > 49 ? -1f : 1f;
                float xPos = pos.x + (xAbs * xDir);
                float yDir = Random.Range(0, 100) > 49 ? -1f : 1f;
                float yPos = yDir * (pos.y + m_distancePerCheckPoint - xAbs);
                pointPosition = new Vector3(xPos, yPos, 0f);
                if (xPos < m_worldLimits.x )
                {
                    outOfBound = true;
                }
                if (xPos > m_worldLimits.y)
                {
                    outOfBound = true;
                }
                if (yPos > m_worldLimits.z)
                {
                    outOfBound = true;
                }
                if (yPos < m_worldLimits.w)
                {
                    outOfBound = true;
                }
                if (Vector3.Distance(pointPosition, pos) < (m_distancePerCheckPoint / 2f))
                {
                    outOfBound = true;
                }

                count++;
                //Fail safe
                if(count > 1000)
                {
                    outOfBound = false;
                }
            } while (outOfBound);
            point.transform.position = pointPosition;
            m_checkPoints[i] = point;
            pos = new Vector3(pointPosition.x, pointPosition.y, pointPosition.z);
        }
        SetInBetweens(-1);
        debugDraw = true;
    }

    void SetInBetweens(int index)
    {
        Vector3 fromPos;
        Vector3 toPos;
        if (index == -1)
        {
            fromPos = transform.position;
            toPos = m_checkPoints[0].transform.position;
        }
        else
        {
            fromPos = m_checkPoints[index].transform.position;
            toPos = m_checkPoints[index + 1].transform.position;
        }
        float xDistance =  fromPos.x - toPos.x ;
        float yDistance = fromPos.y - toPos.y ;
        bool movingOnX = Mathf.Abs(xDistance) > Mathf.Abs(yDistance) ? true : false;
        
        

        m_destinationToCheckPoint[4] = toPos;
        m_destinationToCheckPoint[3] =
             new Vector3(
                    (movingOnX ? toPos.x + (xDistance * 0.2f) : toPos.x + (xDistance * 0.2f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude)),
                    (movingOnX ? toPos.y + (yDistance * 0.2f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude) : toPos.y + (yDistance * 0.2f)),
                    toPos.z);
        m_destinationToCheckPoint[2] = 
            new Vector3((movingOnX? toPos.x + (xDistance * 0.4f) : toPos.x + (xDistance * 0.4f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude)) ,
            (movingOnX ? toPos.y + (yDistance * 0.4f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude)  : toPos.y + (yDistance * 0.4f)), toPos.z);
        m_destinationToCheckPoint[1] =
             new Vector3((movingOnX ? toPos.x + (xDistance * 0.6f) : toPos.x + (xDistance * 0.6f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude)),
            (movingOnX ? toPos.y + (yDistance * 0.6f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude) : toPos.y + (yDistance * 0.6f)), toPos.z);
        m_destinationToCheckPoint[0] =
            new Vector3((movingOnX ? toPos.x + (xDistance * 0.8f) : toPos.x + (xDistance * 0.8f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude)),
            (movingOnX ? toPos.y + (yDistance * 0.8f) + Random.Range(-m_maxSideAmplitude, m_maxSideAmplitude) : toPos.y + (yDistance * 0.8f)), toPos.z);

        //m_destinationToCheckPoint

    }
    bool debugDraw = false;
    void Update()
    {
        anim.SetFloat("velocity", rb.velocity.magnitude);
        if (debugDraw)
        {
            Debug.DrawLine(transform.position, m_checkPoints[0].transform.position, Color.red);
            for (int i = 0; i < m_checkPoints.Length-1; i++)
            {
                Debug.DrawLine(m_checkPoints[i].transform.position, m_checkPoints[i+1].transform.position, Color.red);
            }
        }

    }
    void OnDrawGizmosSelected()
    {
        if (debugDraw)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_destinationToCheckPoint[0], 0.5f);
            Gizmos.DrawSphere(m_destinationToCheckPoint[1], 0.5f);
            Gizmos.DrawSphere(m_destinationToCheckPoint[2], 0.5f);
            Gizmos.DrawSphere(m_destinationToCheckPoint[3], 0.5f);
            Gizmos.DrawSphere(m_destinationToCheckPoint[4], 0.5f);
        }
    }
    void FixedUpdate()
    {
        if (m_IsMoving && m_currentDestination[1] < m_checkPoints.Length)
        {
            float step = 5f * Time.deltaTime;
            //Vector3 newDir = Vector3.RotateTowards(transform.forward, m_checkPoints[m_currentDestination[1]].transform.position, step, 0.0F);
            // Debug.Log(newDir);
            //transform.rotation = Quaternion.LookRotation(newDir);

            var q = Quaternion.LookRotation(m_destinationToCheckPoint[m_currentDestination[0]] - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, m_turnRate * Time.deltaTime);

            if (Time.time >= m_swimRate + m_lastSwimBurst)
            {
                m_lastSwimBurst = Time.time;
                float distance = Vector3.Distance(transform.position, playerPos.position);
                distanceToPlayerRatio = 1f - (distance / m_detectionRangeRatio);
                if (distanceToPlayerRatio < 0f)
                {
                    distanceToPlayerRatio = 0f;
                }
                Debug.Log(distanceToPlayerRatio);
                rb.AddForce(transform.forward * m_movementSpeed * distanceToPlayerRatio);
            }
            if (Vector3.Distance(transform.position, m_destinationToCheckPoint[m_currentDestination[0]]) < 1f)
            {
                m_currentDestination[0]++;
            }
            if (Vector3.Distance(transform.position, m_checkPoints[m_currentDestination[1]].transform.position) < 1f)
            {
                m_currentDestination[0] = 0;
                m_IsMoving = false;                
            }
        }
    }
    //Assign the player to the mate on creation.
    public void SetReferences(Transform p, Quaternion world)
    {
        m_worldLimits = world;
        playerPos = p;
        SetCheckPoints();
    }

    public void CheckPointReached()
    {
       
        m_currentDestination[1]++;
        if (m_currentDestination[1] >= m_checkPoints.Length)
        {
            return;
        }
        SetInBetweens(m_currentDestination[1] - 1);
        m_IsMoving = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_currentDestination[1] == 0)
        {
            m_IsMoving = true;
        }
    }
    
    
}
