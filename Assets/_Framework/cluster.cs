using UnityEngine;
using System.Collections;

public class cluster : MonoBehaviour {

    public Vector3[] cellPos;
    public GameObject[] cells = new GameObject[16];

    public GameObject shell;
    public fxPlayer soundPlayer;

    Animator clusAnim;
    int clickCount;
    public int clicksBeforeSplit;
    int splits = 0;
    Vector3 targetShellScale;
    GameController gc;
	// Use this for initialization
	void Awake () {
        clusAnim = transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").GetComponent<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        clickCount = 0;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = transform.FindChild("cells").transform.FindChild((i + 1).ToString()).gameObject;
        }
        targetShellScale = shell.transform.localScale;
    }

    bool isLerpingFOV = false;
    private float targetFOV = 60f;
    public float cameraLerpSpeed = 1f;
    public float[] targetFOVS;
	// Update is called once per frame
	void Update () {

        if (isLerpingFOV)
        {
            Camera.main.fieldOfView += 5f * Time.deltaTime;
            if (Camera.main.fieldOfView >= targetFOV)
            {
                Camera.main.fieldOfView = targetFOV;
                isLerpingFOV = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && gc.state == EvoState.CLUSTER)
        {
            clickCount++;
            soundPlayer.PlayOnce(10);
            if (clickCount >= clicksBeforeSplit)
            {
                clickCount -= clicksBeforeSplit;
                splits++;
                switch (splits)
                {
                    case 1:
                        soundPlayer.PlayOnce(11);
                        clusAnim.SetTrigger("grow");
                        transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").localPosition = new Vector3(-0.06f,0.086f,-0.1f);//BAD CODE!
                        cells[1].SetActive(true);
                        cells[0].gameObject.GetComponent<randomRotation>().Split(cellPos[0]);
                        cells[1].gameObject.GetComponent<randomRotation>().Split(cellPos[1]);
                        targetShellScale = new Vector3(2f,2f,3f);
                        targetFOV = targetFOVS[0];
                        isLerpingFOV = true;
                        break;
                    case 2:
                        soundPlayer.PlayOnce(11);
                        clusAnim.SetTrigger("grow");
                        transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").localPosition = new Vector3(-0.06f, -0.027f, -0.047f);//BAD CODE!
                        transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").localScale = new Vector3(0.65f, 0.65f, 0.65f);//BAD CODE!
                        cells[2].SetActive(true);
                        cells[3].SetActive(true);
                        cells[2].gameObject.GetComponent<randomRotation>().Split(cellPos[2]);
                        cells[3].gameObject.GetComponent<randomRotation>().Split(cellPos[3]);
                        targetShellScale = new Vector3(3f, 3f, 3f);
                        targetFOV = targetFOVS[1];
                        isLerpingFOV = true;
                        break;
                    case 3:
                        soundPlayer.PlayOnce(11);
                        clusAnim.SetTrigger("grow");
                        transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").localPosition = new Vector3(-0.06f, -0.005f, -0.012f);//BAD CODE!
                        transform.FindChild("shell").transform.FindChild("eukaryote_cell_03_Anim").localScale = new Vector3(0.6f, 0.6f, 0.6f);//BAD CODE!
                        cells[4].SetActive(true);
                        cells[5].SetActive(true);
                        cells[6].SetActive(true);
                        cells[7].SetActive(true);
                        cells[4].gameObject.GetComponent<randomRotation>().Split(cellPos[4]);
                        cells[5].gameObject.GetComponent<randomRotation>().Split(cellPos[5]);
                        cells[6].gameObject.GetComponent<randomRotation>().Split(cellPos[6]);
                        cells[7].gameObject.GetComponent<randomRotation>().Split(cellPos[7]);
                        targetShellScale = new Vector3(4f, 4f, 4f);
                        targetFOV = targetFOVS[2];
                        isLerpingFOV = true;
                        break;
                    case 4:
                        soundPlayer.PlayOnce(11);
                        clusAnim.SetTrigger("grow");
                        cells[8].SetActive(true);
                        cells[9].SetActive(true);
                        cells[10].SetActive(true);
                        cells[11].SetActive(true);
                        cells[12].SetActive(true);
                        cells[13].SetActive(true);
                        cells[14].SetActive(true);
                        cells[15].SetActive(true);
                        cells[8].gameObject.GetComponent<randomRotation>().Split(cellPos[8]);
                        cells[9].gameObject.GetComponent<randomRotation>().Split(cellPos[9]);
                        cells[10].gameObject.GetComponent<randomRotation>().Split(cellPos[10]);
                        cells[11].gameObject.GetComponent<randomRotation>().Split(cellPos[11]);
                        cells[12].gameObject.GetComponent<randomRotation>().Split(cellPos[12]);
                        cells[13].gameObject.GetComponent<randomRotation>().Split(cellPos[13]);
                        cells[14].gameObject.GetComponent<randomRotation>().Split(cellPos[14]);
                        cells[15].gameObject.GetComponent<randomRotation>().Split(cellPos[15]);
                        targetShellScale = new Vector3(5f, 5f, 5f);
                        targetFOV = targetFOVS[3];
                        isLerpingFOV = true;
                        break;
                    case 5:
                        transform.parent.GetComponent<player>().StartPhaseFour();
                        targetFOV = 60f;
                        isLerpingFOV = true;
                        break;

                }
            }
        }

        shell.transform.localScale = Vector3.Lerp(shell.transform.localScale, targetShellScale,Time.deltaTime * 2f);
	}
}
