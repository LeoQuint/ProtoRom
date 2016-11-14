using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour {

    [SerializeField]
    float baseSwimForce;

    private Vector3 screenSize;
    private Vector3 target;
    private Vector3 directionVector;
    private float clickDistance;

    Rigidbody rb;

    public GameObject scan;

    public int nutrientRequired;
    private int nutrientCollected;
    public int photonsRequired;
    private float photonsCollected;
    public int cellRequired;
    private int cellCollected;

    private int currentCheckPoint = 0;

    [SerializeField]
    float haloSize;
    [SerializeField]
    float bioLumIntensity;
    private Light bioLum;

    public Mesh[] meshes;
    public Texture[] textures;
    //Animators
    Animator proAnim;
    Animator eukAnim;
    Animator planAnim;
    Animator pteAnim;
    Animator anoAnim;
    Animator nectocarisAnim;

    public float turnRate;
    public float scanCD;
    private float scanTimer;
    GameController gc;
    private float inactivityTimer;
    public float inactivityResetTime = 60f;
    [System.NonSerialized]
    public int finalType = 0;
    [System.NonSerialized]
    float gameOverX;
    [System.NonSerialized]
    float gameOverY;
    public fxPlayer fxp;
    private bool playingLight = false;
    private bool blockPhaseTwo = false;
    private bool isGameover = false;
    public float delayAfterGameover = 15f;

    private float swinFXTimer = 0f;

    private GameObject myMate;

    private int[] cellEatenType = new int[3];
    //public displayFollow displayCounter;
    
    private soundPlayer soundBoard;
    void Awake()
    {
        soundBoard = GameObject.FindGameObjectWithTag("soundboard").GetComponent<soundPlayer>();

        proAnim = transform.FindChild("prokaryote_cell").GetComponent<Animator>();
        eukAnim = transform.FindChild("eukaryote_cell").GetComponent<Animator>();
        planAnim = transform.FindChild("planaria_B").GetComponent<Animator>();
        pteAnim = transform.FindChild("hallucigenia").GetComponent<Animator>();
        anoAnim = transform.FindChild("anomalocaris").GetComponent<Animator>();
        nectocarisAnim = transform.FindChild("nectocaris").GetComponent<Animator>();
        //planAnim;
        //pteAnim;
        bioLum = transform.FindChild("eukaryote_cell").transform.FindChild("light").GetComponent<Light>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        nutrientCollected = 0;
        photonsCollected = 0;
    }
	void Start () {
        screenSize = new Vector3(Screen.width, Screen.height, 0f);
        target = transform.position;
        rb = GetComponent<Rigidbody>();
        scanTimer = Time.time - scanCD;
	}	
	void Update () {
        inactivityTimer += Time.deltaTime;


        if (inactivityTimer >= inactivityResetTime)
        {
            SceneManager.LoadScene(0);
        }
        if (isGameover)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.down), Time.deltaTime * turnRate);
            rb.AddForce(transform.forward * -1f * baseSwimForce * 0.8f * Time.deltaTime);
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {            
            inactivityTimer = 0f;
            if (gc.state == EvoState.CLUSTER)
            {
                return;
            }
            clickDistance = Vector3.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
           
            if (clickDistance < 70f && gc.state == EvoState.PROKARYOTE && Time.time >= scanTimer + scanCD)
            {
                scanTimer = Time.time;
                proAnim.SetTrigger("scan");
                fxp.PlayOnce(2);
                Instantiate(scan, transform.position, Random.rotation);
            }
            else
            {
                
                directionVector = new Vector3(Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x,
                                       Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y, 0f);    
                rb.AddForce(directionVector.normalized * baseSwimForce);
                if (gc.state >= EvoState.PLANARIAN )
                {
                    fxp.PlayOnce(3);
                    
                }
                else
                {
                    fxp.PlayOnce(4);
                }
               
            }
           
        }

        if (Input.GetMouseButton(0))
        {
            inactivityTimer = 0f;
            if (gc.state == EvoState.CLUSTER)
            {
                return;
            }          
            
            directionVector = new Vector3(Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x,
                                    Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y, 0f);
            rb.AddForce(directionVector.normalized * baseSwimForce * Time.deltaTime);
            if (gc.state >= EvoState.PLANARIAN && Time.time > swinFXTimer + 1f)
            {
                fxp.PlayOnce(3);
                swinFXTimer = Time.time;
            }
            else if (Time.time > swinFXTimer + 1f)
            {
                fxp.PlayOnce(4);
                swinFXTimer = Time.time;
            }

            

        }

        switch (gc.state)
        {
            case EvoState.PROKARYOTE:
                proAnim.SetFloat("velocity", rb.velocity.magnitude);
                break;

            case EvoState.EUKARYOTE:
                eukAnim.SetFloat("velocity", rb.velocity.magnitude);
                break;
            case EvoState.PLANARIAN:
                planAnim.SetFloat("velocity", rb.velocity.magnitude);
                break;
            case EvoState.PTERYX:
                if (finalType == 0)
                {
                    pteAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                else if (finalType == 1)
                {
                    anoAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                else
                {
                    nectocarisAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                break;
            case EvoState.GAMEOVER:
                if (finalType == 0)
                {
                    pteAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                else if (finalType == 1)
                {
                    anoAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                else
                {
                    nectocarisAnim.SetFloat("velocity", rb.velocity.magnitude);
                }
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, gameOverX -5f, gameOverX+ 5f), Mathf.Clamp(transform.position.y, gameOverY - 5f, gameOverY +2.8f), 0f);
                
                break;
        }

        if (rb.velocity.magnitude > 0.2f)
        {
            Turn();
        }
        
    }
    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(delayAfterGameover);
        soundBoard.Reset();
        SceneManager.LoadScene(0);
    }
    public void Pounce()
    {
        planAnim.SetTrigger("pounce");
    }
    public void NutrientReceived(int val)
    {
        fxp.PlayOnce(7);
        nutrientCollected += val;
        float scale = 0.7f + (nutrientCollected/3f);
        transform.localScale = new Vector3(scale, scale, scale);
        if (nutrientCollected >= nutrientRequired)
        {
            transform.localScale = new Vector3(1f,1f,1f);
            StartPhaseTwo();
            return;
        }
    }


    public void Photosynthesize(float photon)
    {
        if (!playingLight)
        {
            playingLight = true;
            fxp.PlayScaling();
        }
        if (blockPhaseTwo)
        {
            return;
        }
        photonsCollected += photon;
        eukAnim.SetBool("photosynthesize", true);

        bioLum.intensity = (photonsCollected / photonsRequired)* bioLumIntensity;
        bioLum.range = (photonsCollected / photonsRequired) * haloSize;


        if (photonsCollected >= photonsRequired)
        {
            blockPhaseTwo = true;
            StartPhaseThree();
        }
    }
    public void Photosynthesize(bool hasPhoton)
    {
        if (!hasPhoton)
        {
            playingLight = false;
            eukAnim.SetBool("photosynthesize", false);
            fxp.PlayScaling(false);
        }
    }
    void StartPhaseTwo()
    {
        Camera.main.GetComponent<cameraFollow>().Transition();
        fxp.PlayOnce(8);
        gc.ChangeState(EvoState.EUKARYOTE);
        transform.FindChild("prokaryote_cell").gameObject.SetActive(false);
        transform.FindChild("eukaryote_cell").gameObject.SetActive(true);
    }
    void StartPhaseThree()
    {
        Camera.main.GetComponent<cameraFollow>().Transition();
        fxp.PlayOnce(8);
        Debug.Log("Phase 3");
        gc.ChangeState(EvoState.CLUSTER);
        rb.angularVelocity = new Vector3(0f,0f,0f);
        rb.velocity = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        transform.FindChild("Cluster").gameObject.SetActive(true);
        transform.FindChild("eukaryote_cell").gameObject.SetActive(false);
    }
    public void StartPhaseFour()
    {
        soundBoard.Chase();
        Camera.main.GetComponent<cameraFollow>().Transition();
        fxp.PlayOnce(8);
        Debug.Log("Phase 4");
        gc.ChangeState(EvoState.PLANARIAN);
        transform.FindChild("Cluster").gameObject.SetActive(false);
        transform.FindChild("planaria_B").gameObject.SetActive(true);
    }
    void StartPhaseFive()
    {
        Camera.main.GetComponent<cameraFollow>().Transition();
        Debug.Log("Phase 5");
        fxp.PlayOnce(8);
        int lastType = 0;
        for (int i = 0; i < 3; i++)
        {
            lastType += cellEatenType[i];
        }
        if (lastType == 0)
        {
            finalType = 0;
        }
        else if (lastType == 3)
        {
            finalType = 1;
        }
        else
        {
            finalType = 2;
        }

        if (finalType == 0)
        {
            transform.FindChild("hallucigenia").gameObject.SetActive(true);
        }
        else if(finalType == 1)
        {
            transform.FindChild("anomalocaris").gameObject.SetActive(true);
        }
        else {
            transform.FindChild("nectocaris").gameObject.SetActive(true);
        }

        transform.FindChild("planaria_B").gameObject.SetActive(false);

        gc.ChangeState(EvoState.PTERYX);
    }
    public void Reset()
    {
        photonsCollected = 0;
        nutrientCollected = 0;
    }
    void Turn()
    {
        //transform.Rotate(directionVector.normalized * turnRate);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector.normalized * -1f), Time.deltaTime * turnRate);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cell")
        {
            Pounce();
            fxp.PlayOnce(7);
            
            if (cellCollected == 0)
            {
                //displayCounter.Reset();
            }
            
            int cellType = other.transform.parent.GetComponent<cell_AI>().type;
            cellEatenType[cellCollected] = cellType;
            cellCollected++;
            //displayCounter.SetCounter(cellCollected - 1, cellType+1);
            //displayCounter.Activate();
            if (cellCollected >= cellRequired)
            {
                StartPhaseFive();
            }
            Destroy(other.transform.parent.gameObject);
        }
        if (other.tag == "checkpoint" && other.name == currentCheckPoint.ToString())
        {
            Debug.Log("Hit correct checkpoint");
            currentCheckPoint++;
            myMate.GetComponent<mate_AI>().CheckPointReached();
            if (currentCheckPoint == 3)
            {
                GameOver();
            }
        }
    }
    public void SetMate(GameObject g)
    {
        myMate = g;
    }
    public void GameOver()
    {
        gc.GameOver();
        isGameover = true;
        StartCoroutine(LoadMenu());
    }
}
