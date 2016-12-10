using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class informationPanel : MonoBehaviour {

    public float m_scalingSpeed = 1f;
    public string[] displayTexts;
    public GameObject helpButton;

    private bool m_scalingDown = false;
    private bool m_scalingUp = false;

    private float m_scale = 0f;
    private Text mainText;


    private float startingX;

    private int currentTip = 0;
    void Awake()
    {
        mainText = transform.FindChild("Text").GetComponent<Text>();
        mainText.gameObject.SetActive(false);
        Debug.Log(transform.position);
        startingX = transform.position.x;
        MakeNewlines();
    }

    void MakeNewlines()
    {
        for (int i = 0; i < displayTexts.Length; ++i)
        {
            displayTexts[i] = displayTexts[i].Replace("NEWLINE", "\n");
        }

    }

    void Start() {
        //transform.localScale = new Vector3(m_scale, m_scale, m_scale);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PopUp(0);
        }
        if (m_scalingUp)
        {
            //m_scale += m_scalingSpeed * Time.deltaTime;
            //if (m_scale >= 1f)
            //{
            //    m_scale = 1f;
            //    m_scalingUp = false;
            //    mainText.gameObject.SetActive(true);
            //    StartCoroutine(ClosingDelay());
            //}
            //transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            float newX = transform.position.x + (2000f * Time.deltaTime);
            if (transform.position.x >= 550f)
            {
                newX = 550f;
                m_scalingUp = false;
                mainText.gameObject.SetActive(true);
                StartCoroutine(ClosingDelay());
            }
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        }
        if (m_scalingDown)
        {
            //m_scale -= m_scalingSpeed * Time.deltaTime;
            //if (m_scale <= 0f)
            //{
            //    helpButton.SetActive(true);
            //    m_scale = 0f;
            //    mainText.gameObject.SetActive(false);
            //    m_scalingDown = false;
            //}
            //transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            float newX = transform.position.x - (2000f * Time.deltaTime);
            if (transform.position.x <= startingX)
            {
                newX = startingX;
                m_scalingDown = false;
                mainText.gameObject.SetActive(false);
                helpButton.SetActive(true);
            }
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
    public void PopUp()
    {
        if (m_scalingUp || m_scalingDown) {
            return;
        }
        //helpButton.SetActive(false);
        mainText.text = displayTexts[currentTip];
        m_scalingUp = true;
    }
    public void PopUp(int index)
    {
        //helpButton.SetActive(false);
        currentTip = index;
        mainText.text = displayTexts[index];
        m_scalingUp = true;
    }

    IEnumerator ClosingDelay()
    {
        yield return new WaitForSeconds(5f);
        ClosePopUp();
    }

    public void ClosePopUp()
    {
        m_scalingDown = true;
    }
}
