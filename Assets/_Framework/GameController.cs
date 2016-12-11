using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public enum EvoState { PROKARYOTE, EUKARYOTE, CLUSTER, PLANARIAN, PTERYX, GAMEOVER, COUNT }
public class GameController : MonoBehaviour {

    [Tooltip("X : Leftmost limit, Y: RightMost limit, Z : Upmost limit, W: bottom limit.")]
    public Quaternion m_worldLimits;
    public GameObject lightBeam;
    public float lightSpawnRate;
    private float timeSinceLight;
    [SerializeField]
    Quaternion lightAngle;

    public static int lastLightMatUsed = 0;

    List<GameObject> nutrients = new List<GameObject>();
    //public bool[] nAvailable = new bool[30];

    private float m_spawnTimer;
    [SerializeField]
    float m_nutrientSpawnRate;
    public GameObject nutrient;
    [SerializeField]
    float m_cellSpawnRate;
    public GameObject cell;
    public GameObject mate;
    public GameObject mate2;
    public GameObject mate3;



    private Vector2[] nPos = new Vector2[100];


    public EvoState state;
    List<GameObject> cells = new List<GameObject>();
    //UI elements and variables
    public GameObject fadingPanel;
    public float fadingPanelSpeed;
    private bool fading = false;
    public Text tipsText;
    public informationPanel infoPanel;
    public string[] tips;
    private bool isTextFading = false;
    private bool isTextDisplaying = false;
    public float textFadeSpeed;
    public float textSolidDuration;
    public fxPlayer fxp;

    public GameObject mateTraker;
    private Transform trackerArrow;
    //private Transform trackerTimer;
    private Transform trackerSprite;
    private bool isTracking = false;
    private Transform trackedMate;
    public float distanceToOutOfScreenChase = 20f;
    public float distanceToPopTracker = 10f;
    public float timerBeforeLosingMateChase = 15f;
    public float showTimerRemaining = 10f;
    private float chaseTimer;

    private bool barLoading = false;
    private float barDirection = 1f;
    private float maxBarHeight;
    [SerializeField][Range(0f,1f)]
    float percentBarHeight = 0.2f;
    [SerializeField]
    float barLoadTime = 3f;
    float barloadTimer;


    //private float 
    //private Vector3 spawnVec = new Vector3(0f,0f,-20f);
    Transform player;
    GameObject mainCam;

    public RectTransform topBar;
    public RectTransform botBar;

    private GameObject[] plants;

    public GameObject wildLife_Prefab_1;
    public GameObject wildLife_Prefab_2;
    public GameObject wildLife_Prefab_3;

    public int totalNumOfWildlifePerType = 20;

    private List<GameObject> wildlifeList = new List<GameObject>();

    public QuotePlayer quotePlayer;


    void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = EvoState.PROKARYOTE;


        trackerArrow = mateTraker.transform.FindChild("arrow");        
        trackerSprite = mateTraker.transform.FindChild("sprite");
        //trackerTimer = trackerSprite.FindChild("timer");

        plants = GameObject.FindGameObjectsWithTag("plant");
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].SetActive(false);
        }
    }
    void Start()
    {
        PreSpawnWildLife();
        maxBarHeight = Screen.height * percentBarHeight * 2f;
        m_spawnTimer = Time.time -m_nutrientSpawnRate;
        StartCoroutine(FirstTip());
    }
    public void ChangeState(EvoState s)
    {
        state = s;
        switch (state)
        {
            case EvoState.EUKARYOTE:
                foreach (GameObject n in nutrients)
                {
                    Destroy(n);
                }
                nutrients.Clear();
                m_spawnTimer = Time.time;
                ShowTip(1);
                Grayscale gs = mainCam.GetComponent<Grayscale>();
                gs.enabled = false;
                
                break;
            case EvoState.CLUSTER:
                mainCam.GetComponent<Camera>().fieldOfView = 30f;
                ShowTip(2);
                break;
            case EvoState.PLANARIAN:
                mainCam.GetComponent<cameraFollow>().ZoomOut();
                for (int i = 0; i < plants.Length; i++)
                {
                    plants[i].SetActive(true);
                }
                ShowTip(3);
                break;
            case EvoState.PTERYX://Chase your mate/stay single.

                foreach (GameObject c in cells)
                {
                    Destroy(c);
                }
                cells.Clear();

                Vector3 pos;
                bool tooClose;
                bool outOfBound;
                do
                {
                    outOfBound = false;
                    tooClose = false;
                    float xRng = Random.Range(player.position.x - 2f, player.position.x + 2f);
                    float yRng = Random.Range(player.position.y - 2f, player.position.y + 2f);
                    pos = new Vector3(xRng, yRng, 0f);
                    if (Vector3.Distance(pos, player.position) < 2f)
                    {
                        tooClose = true;
                    }
                    if (pos.x < m_worldLimits.x || pos.x > m_worldLimits.y || pos.y > m_worldLimits.z || pos.y < m_worldLimits.w)
                    {
                        outOfBound = true;
                    }
                } while (tooClose || outOfBound);


                switch (player.GetComponent<player>().finalType)
                {             
                    case 0:
                        GameObject m = Instantiate(mate, pos, Quaternion.identity) as GameObject;
                        player.GetComponent<player>().SetMate(m);
                        m.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);
                        trackedMate = m.transform;
                        StartTrackingMate(true);
                        infoPanel.SetLastPhaseIndex(2);
                        break;
                    case 1:
                        GameObject m1 = Instantiate(mate2, pos, Quaternion.identity) as GameObject;
                        player.GetComponent<player>().SetMate(m1);
                        m1.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);
                        trackedMate = m1.transform;
                        StartTrackingMate(true);
                        infoPanel.SetLastPhaseIndex(1);
                        break;
                    case 2:
                        GameObject m2 = Instantiate(mate3, pos, Quaternion.identity) as GameObject;
                        player.GetComponent<player>().SetMate(m2);
                        m2.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);
                        trackedMate = m2.transform;
                        StartTrackingMate(true);
                        infoPanel.SetLastPhaseIndex(0);
                        break;
                }
                
                
                ShowTip(4);
                break;
            case EvoState.GAMEOVER:
                StartTrackingMate(false);
                //player.GetComponent<player>().GameOver();
                //ShowTip(5);
                break;

        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetBar(true, true, 40f);
        }
        if (barLoading)
        {
            MovingBars();
        }
        if (isTracking)
        {
            TrackMate();
        }
        if (isTextDisplaying)
        {
            tipsText.color = new Color(1f, 1f, 1f, tipsText.color.a + (textFadeSpeed * Time.deltaTime));
            if (tipsText.color.a >= 1f)
            {
                isTextDisplaying = false;
                StartCoroutine(TextDisplaying());
            }
        }
        if (isTextFading)
        {
            tipsText.color = new Color(1f, 1f, 1f, tipsText.color.a - (textFadeSpeed * Time.deltaTime));
            if (tipsText.color.a <= 0f)
            {
                isTextFading = false;
            }
        }
        if (fading)
        {
            fadingPanel.GetComponent<Image>().color = new Color(0f,0f,0f, fadingPanel.GetComponent<Image>().color.a + (fadingPanelSpeed * Time.deltaTime));
            //fadingPanel.transform.FindChild("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, fadingPanel.transform.FindChild("Text").GetComponent<Text>().color.a + (fadingPanelSpeed * Time.deltaTime));
            return;
        }
        if (state == EvoState.PROKARYOTE)//state 1
        {
            if (Time.time >= m_spawnTimer + m_nutrientSpawnRate)
            {
                m_spawnTimer = Time.time;
                SpawnNutrient();
            }
            return;
        }
        if (state == EvoState.EUKARYOTE)//state 2
        {
            if (Time.time >= m_spawnTimer + m_cellSpawnRate)
            {
                m_spawnTimer = Time.time;
                SpawnLight();
            }
            return;
        }
        if (state == EvoState.PLANARIAN)//state 4
        {
            if (Time.time >= m_spawnTimer + lightSpawnRate)//Using same variable than light here!!!
            {
                m_spawnTimer = Time.time;
                SpawnCell();
            }
            return;
        }
    }

    void PreSpawnWildLife()
    {
        for (int i = 0; i < totalNumOfWildlifePerType; ++i)
        {
            GameObject go1 = Instantiate(wildLife_Prefab_1, Vector3.back * 100f, Quaternion.identity) as GameObject;
            GameObject go2 = Instantiate(wildLife_Prefab_2, Vector3.back * 100f, Quaternion.identity) as GameObject;
            GameObject go3 = Instantiate(wildLife_Prefab_3, Vector3.back * 100f, Quaternion.identity) as GameObject;
            go1.SetActive(false);
            go2.SetActive(false);
            go3.SetActive(false);
            wildlifeList.Add(go1);
            wildlifeList.Add(go2);
            wildlifeList.Add(go3);
        }
    }
    void SpawnNutrient()
    {
        Vector3 pos;
        bool tooClose;
        bool outOfBound;
        do
        {
            outOfBound = false;
            tooClose = false;
            float xRng = Random.Range(player.position.x - 10f, player.position.x + 10f);
            float yRng = Random.Range(player.position.y - 10f, player.position.y + 10f);
            pos = new Vector3(xRng, yRng, 0f);
            if (Vector3.Distance(pos, player.position) < 5f)
            {
                tooClose = true;
            }
            if (pos.x < m_worldLimits.x || pos.x > m_worldLimits.y || pos.y > m_worldLimits.z || pos.y < m_worldLimits.w)
            {
                outOfBound = true;
            }
        } while (tooClose|| outOfBound);
        GameObject newFood = Instantiate(nutrient, pos, Quaternion.identity) as GameObject;
        nutrients.Add(newFood);
        
    }
    void SpawnCell()
    {
        Vector3 pos;
        bool tooClose;
        bool outOfBound;
        do {
            outOfBound = false;
            tooClose = false;
            float xRng = Random.Range(player.position.x - 10f, player.position.x + 10f);
            float yRng = Random.Range(player.position.y - 10f, player.position.y + 10f);
            pos = new Vector3(xRng, yRng, 0f);
            if (Vector3.Distance(pos, player.position) < 5f)
            {
                tooClose = true;
            }
            if (pos.x < m_worldLimits.x || pos.x > m_worldLimits.y || pos.y > m_worldLimits.z || pos.y < m_worldLimits.w)
            {
                outOfBound = true;
            }
        } while (tooClose|| outOfBound);
        GameObject newCell = Instantiate(cell, pos, Quaternion.identity) as GameObject;
        cells.Add(newCell);

    }
    void SpawnLight()
    {

        float xRng = Random.Range(player.position.x -10f ,player.position.x +5f);
        float yRng = 24f;
        GameObject beam = Instantiate(lightBeam, new Vector3(xRng,yRng,0f), lightAngle) as GameObject;
       
    }
    public void GameOver()
    {

        quotePlayer.isFadingIn = true;
        fading = true;
        StartCoroutine(EndGameSequence());
    }
    public void ShowTip(int index)
    {
        infoPanel.PopUp(index);
        //tipsText.text = tips[index];
        //tipsText.color = new Color(1f,1f,1f, 0f);
        //isTextDisplaying = true;
    }
    //Function to set the sprite and status of follow arrow.
    public void StartTrackingMate(bool val)
    {
        chaseTimer = Time.time + timerBeforeLosingMateChase;
        isTracking = val;
        mateTraker.SetActive(val);
    }
    public Vector3 lookVec = Vector3.forward;
    //function to follow and rotate and update timer.
    void TrackMate()
    {
        if (chaseTimer < Time.time)
        {
            mateTraker.SetActive(false);
            player.GetComponent<player>().GameOver();
            isTracking = false;
            return;
        }
        float distanceToMate = Vector3.Distance(player.position, trackedMate.position);
        if (distanceToMate > distanceToOutOfScreenChase)
        {
            float timeRemaining = chaseTimer - Time.time;
            /*if (timeRemaining < showTimerRemaining)
            {
                trackerTimer.gameObject.SetActive(true);
                trackerTimer.GetComponent<Text>().text = timeRemaining.ToString("F0");
            }
            else if(trackerTimer.gameObject.activeSelf)
            {
                trackerTimer.gameObject.SetActive(false);
            }*/
        }
        else
        {
            chaseTimer = Time.time + timerBeforeLosingMateChase;
           /* if (trackerTimer.gameObject.activeSelf)
            {
                trackerTimer.gameObject.SetActive(false);
            }*/
        }



        if (distanceToMate > distanceToPopTracker)
        {
            if (!mateTraker.activeSelf) {
                mateTraker.SetActive(true);
            }            
        }
        else if(mateTraker.activeSelf)
        {
            mateTraker.SetActive(false);
        }

        float angle = 90f + (Mathf.Atan(   ( player.position.y - trackedMate.position.y ) /
                                    (player.position.x - trackedMate.position.x) ) * Mathf.Rad2Deg);
        if (player.position.x < trackedMate.position.x)
        {
            angle += 180f;
        } 
        Quaternion rotationMate = Quaternion.Euler(lookVec * angle);
        mateTraker.transform.rotation = rotationMate;
        trackerSprite.rotation = Quaternion.identity;
    }
    public void SetBar(bool status, bool fovChange, float valFOV)
    {
        mateTraker.SetActive(false);
        //barloadTimer = Time.time + barLoadTime;
        barloadTimer = 0f;
        barLoading = true;
        isLerpingFOV = true;
        targetFOV = valFOV;
    }
    public void ActivateWildlife()
    {
        Vector3 playerPos = player.position;
        float leftRight = 1f;
        for (int i = 0; i < wildlifeList.Count; ++i)
        {
            wildlifeList[i].transform.position = new Vector3((playerPos.x) + 70f*leftRight, Mathf.Clamp(playerPos.y + Random.Range(-20f,20f), m_worldLimits.w + 5f, m_worldLimits.z -5f), playerPos.z + Random.Range(5f, 35f)  );
            leftRight *= -1f;
            wildlifeList[i].transform.rotation = Quaternion.Euler(0f, 90f * leftRight, 0f);
            
            
            wildlifeList[i].SetActive(true);
        }
    }
    //Variables to lerp FOV
    private bool isLerpingFOV = false;
    private float targetFOV = 40f;

    void MovingBars()
    {
        if (isLerpingFOV)
        {
            Camera.main.fieldOfView -= 5f * Time.deltaTime;
            if (Camera.main.fieldOfView <= targetFOV)
            {
                Camera.main.fieldOfView = targetFOV;
                isLerpingFOV = false;
            }
        }
        float size = 0f;
        barloadTimer +=  Time.deltaTime/barLoadTime;
        if (barDirection == 1f)
        {
            size = Mathf.Lerp(topBar.sizeDelta.y, maxBarHeight, barloadTimer);
        }
        else{
            size = Mathf.Lerp(topBar.sizeDelta.y, 0f, barloadTimer);
        }


        if (size >= maxBarHeight-2f && barDirection == 1f)
        {
            size = maxBarHeight;
            barLoading = false;
            barDirection *= -1f;
        }
        if (size-2f < 0f && barDirection == -1f)
        {
            barLoading = false;
            size = 0f;
            barDirection *= -1f;
        }
        topBar.sizeDelta =  new Vector2(Screen.width, size);
        botBar.sizeDelta = new Vector2(Screen.width, size);
    }
    //Quick hack function to set a bool to true in popup to keep the first tips on until first creature eaten.
    public void SetFirstTipOff()
    {
        infoPanel.SetFirstTipOff();
    }
    IEnumerator TextDisplaying()
    {
        yield return new WaitForSeconds(textSolidDuration);
        isTextFading = true;
    }
    IEnumerator FirstTip()
    {
        yield return new WaitForSeconds(1f);
        ShowTip(0);
    }
    IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(3f);
        fading = true;
        
    }


   

}
