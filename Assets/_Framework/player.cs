using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    private List<int> cellEatenType = new List<int>();
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

    //Hack positioning variables for mates
    bool atFinalPositions = false;
    bool finalTrigger = false;
	void Update () {
        inactivityTimer += Time.deltaTime;


        if (inactivityTimer >= inactivityResetTime && !isGameover)
        {
            soundBoard.Reset();
            SceneManager.LoadScene(0);
        }
        if (isGameover)
        {
            if (!atFinalPositions)
            {
                MoveMatesIntoPosition();
            }
            else
            {
                if (finalType == 0)
                {
                    pteAnim.SetTrigger("dance");
                }
                else if (finalType == 1)
                {
                    anoAnim.SetTrigger("dance");
                }
                else
                {
                    nectocarisAnim.SetTrigger("dance");
                }
                myMate.GetComponent<Animator>().SetTrigger("dance");
                finalTrigger = true;
            }
            if (finalTrigger)
            {
                return;
            }
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.down), Time.deltaTime * turnRate);
            //rb.AddForce(transform.forward * -1f * baseSwimForce * 0.8f * Time.deltaTime);
            return;
        }
        if (Input.GetMouseButtonDown(0) &&  !EventSystem.current.IsPointerOverGameObject())
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

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            inactivityTimer = 0f;
            if (gc.state == EvoState.CLUSTER)
            {
                return;
            }          
            
            
            if (gc.state >= EvoState.PLANARIAN && Time.time > swinFXTimer + 0.3f)
            {
                fxp.PlayOnce(3);
                swinFXTimer = Time.time;
                directionVector = new Vector3(Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x,
                                    Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y, 0f);
                rb.AddForce(directionVector.normalized * baseSwimForce);
            }
            else if (Time.time > swinFXTimer + 0.3f)
            {
                fxp.PlayOnce(4);
                swinFXTimer = Time.time;
                directionVector = new Vector3(Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x,
                                    Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y, 0f);
                rb.AddForce(directionVector.normalized * baseSwimForce);
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
        gc.SetFirstTipOff();
        float scale = 0.7f + (nutrientCollected/3f);
        transform.localScale = new Vector3(scale, scale, scale);
        if (nutrientCollected >= nutrientRequired)
        {
            transform.localScale = new Vector3(1f,1f,1f);
            StartPhaseTwo();
            return;
        }
    }

    //hack to fix scaling sound issue
    private bool isPastLightLevel = false;
    public void Photosynthesize(float photon)
    {
        if (isPastLightLevel) {
            return;
        }
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
        StartCoroutine(transform.FindChild("prokaryote_cell").gameObject.SetActive(false, 0.5f));
        StartCoroutine(transform.FindChild("eukaryote_cell").gameObject.SetActive(true, 0.5f));
    }
    void StartPhaseThree()
    {
        isPastLightLevel = true;
        Photosynthesize(false);
        Camera.main.GetComponent<cameraFollow>().Transition();
        fxp.PlayOnce(8);
        Debug.Log("Phase 3");
        gc.ChangeState(EvoState.CLUSTER);
        rb.angularVelocity = new Vector3(0f,0f,0f);
        rb.velocity = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        StartCoroutine(transform.FindChild("Cluster").gameObject.SetActive(true, 0.5f));
        StartCoroutine(transform.FindChild("eukaryote_cell").gameObject.SetActive(false, 0.5f));
    }
    public void StartPhaseFour()
    {
        soundBoard.Chase();
        Camera.main.GetComponent<cameraFollow>().Transition();
        fxp.PlayOnce(8);
        Debug.Log("Phase 4");
        gc.ChangeState(EvoState.PLANARIAN);
        transform.localScale = Vector3.one * 3f;
        StartCoroutine(transform.FindChild("Cluster").gameObject.SetActive(false, 0.5f));
        StartCoroutine(transform.FindChild("planaria_B").gameObject.SetActive(true, 0.5f));
    }
    void StartPhaseFive()
    {
        Camera.main.GetComponent<cameraFollow>().Transition();
        Debug.Log("Phase 5");
        fxp.PlayOnce(8);
        int lastType = 0;
        int[] typeSorted = new int[3]; 
        for (int i = 0; i < cellEatenType.Count; i++)
        {
            ++typeSorted[cellEatenType[i]];       
        }
        if (typeSorted[0] > typeSorted[1] && typeSorted[0] > typeSorted[2])
        {
            finalType = 1;
            StartCoroutine(transform.FindChild("anomalocaris").gameObject.SetActive(true, 0.5f));
        }
        else if (typeSorted[1] > typeSorted[0] && typeSorted[1] > typeSorted[2])
        {
            finalType = 0;
            StartCoroutine(transform.FindChild("hallucigenia").gameObject.SetActive(true, 0.5f));
        }
        else if (typeSorted[2] > typeSorted[0] && typeSorted[2] > typeSorted[1])
        {
            finalType = 2;
            StartCoroutine(transform.FindChild("nectocaris").gameObject.SetActive(true, 0.5f));
        }
        //Randomize if a tie
        else if (typeSorted[0] == typeSorted[1] && typeSorted[0] > typeSorted[2])
        {
            if (Random.Range(0, 2) == 0)
            {
                finalType = 1;
                StartCoroutine(transform.FindChild("anomalocaris").gameObject.SetActive(true, 0.5f));
            }
            else
            {
                finalType = 0;
                StartCoroutine(transform.FindChild("hallucigenia").gameObject.SetActive(true, 0.5f));
            }
        }
        else if (typeSorted[0] == typeSorted[2] && typeSorted[0] > typeSorted[1])
        {
            if (Random.Range(0, 2) == 0)
            {
                finalType = 1;
                StartCoroutine(transform.FindChild("anomalocaris").gameObject.SetActive(true, 0.5f));
            }
            else
            {
                finalType = 2;
                StartCoroutine(transform.FindChild("nectocaris").gameObject.SetActive(true, 0.5f));
            }
        }
        else if (typeSorted[1] == typeSorted[2] && typeSorted[1] > typeSorted[0])
        {
            if (Random.Range(0, 2) == 0)
            {
                finalType = 0;
                StartCoroutine(transform.FindChild("hallucigenia").gameObject.SetActive(true, 0.5f));
            }
            else
            {
                finalType = 2;
                StartCoroutine(transform.FindChild("nectocaris").gameObject.SetActive(true, 0.5f));
            }
        }
        else//Catch all in case
        {
            finalType = 2;
            StartCoroutine(transform.FindChild("nectocaris").gameObject.SetActive(true, 0.5f));
        }



        StartCoroutine(transform.FindChild("planaria_B").gameObject.SetActive(false, 0.5f));

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
            cellEatenType.Add( cellType);
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
        finalPOSMate = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z);
        finalPOSPlayer = new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z);
        
        Camera.main.GetComponent<cameraFollow>().SetNewTargetForView(new Vector3(transform.position.x, transform.position.y, Camera.main.GetComponent<cameraFollow>().zoomOutTo));
        EndGameCinematic();
        StartCoroutine( DelayBeforeReset());
        isGameover = true;
    }

    public void EndGameCinematic()
    {
        
        myMate.GetComponent<mate_AI>().isGameOver = true;
        gc.StartTrackingMate(false);
        gc.SetBar(true, true, 40f);
        gc.ActivateWildlife();
    }

    IEnumerator DelayBeforeReset()
    {
        yield return new WaitForSeconds(10f);
        soundBoard.FadeMusic(delayAfterGameover);
        gc.GameOver();
        isGameover = true;
        StartCoroutine(LoadMenu());
    }

    //End game positionning
    private Vector3 finalPOSPlayer;
    private Vector3 finalPOSMate;
    private float finalSetupPosDuration = 0f;
    private Quaternion finalRotPlayer = Quaternion.Euler(0f, 90f, 0f);
    private Quaternion finalRotMate = Quaternion.Euler(0f, 90f, 0f);


    void MoveMatesIntoPosition()
    {
        finalSetupPosDuration += Time.deltaTime * 0.33f;
        if (finalSetupPosDuration >= 1f)
        {
            atFinalPositions = true;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, finalPOSPlayer, finalSetupPosDuration);
        myMate.transform.position = Vector3.Lerp(myMate.transform.position, finalPOSMate, finalSetupPosDuration);
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotPlayer, finalSetupPosDuration);
        myMate.transform.rotation = Quaternion.Lerp(myMate.transform.rotation, finalRotMate, finalSetupPosDuration);
        

    }
    
}
