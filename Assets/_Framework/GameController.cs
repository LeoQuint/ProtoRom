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

    //private float 
    //private Vector3 spawnVec = new Vector3(0f,0f,-20f);
    Transform player;
    GameObject mainCam;

    private GameObject[] plants;

    void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = EvoState.PROKARYOTE;

        plants = GameObject.FindGameObjectsWithTag("plant");
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].SetActive(false);
        }
    }
    void Start()
    {
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
                        break;
                    case 1:
                        GameObject m1 = Instantiate(mate2, pos, Quaternion.identity) as GameObject;
                        player.GetComponent<player>().SetMate(m1);
                        m1.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);
                        break;
                    case 2:
                        GameObject m2 = Instantiate(mate3, pos, Quaternion.identity) as GameObject;
                        player.GetComponent<player>().SetMate(m2);
                        m2.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);
                        break;
                }
                
                
                ShowTip(4);
                break;
            case EvoState.GAMEOVER:
                //player.GetComponent<player>().GameOver();
                //ShowTip(5);
                break;

        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject m = Instantiate(mate, transform.position, Quaternion.identity) as GameObject;
            player.GetComponent<player>().SetMate(m);
            m.GetComponent<mate_AI>().SetReferences(player, m_worldLimits);

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
        float yRng = Random.Range(player.position.y + 12f, player.position.y + 20f);
        GameObject beam = Instantiate(lightBeam, new Vector3(xRng,yRng,0f), lightAngle) as GameObject;
       
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        //ChangeState(EvoState.GAMEOVER);        
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
