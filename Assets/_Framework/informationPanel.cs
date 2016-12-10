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

    public bool m_HasEatenFirstFood = false;
    private float startingX;

    private bool m_English = true;

    public Sprite english;
    public Sprite french;
    public Image languageButton;

    public float instructionStayTimer = 10f;

    public Text educationText;
    public string[] displayEducationTexts;

    private int currentTip = 0;
    void Awake()
    {
        mainText = transform.FindChild("Text").GetComponent<Text>();
        mainText.gameObject.SetActive(true);
        Debug.Log(transform.position);
        startingX = transform.position.x;
        educationText.color = Vector4.zero;
        MakeNewlines();
    }

    void MakeNewlines()
    {
        for (int i = 0; i < displayTexts.Length; ++i)
        {
            displayTexts[i] = displayTexts[i].Replace("NEWLINE", "\n");
        }
        for (int i = 0; i < displayEducationTexts.Length; ++i)
        {
            displayEducationTexts[i] = displayEducationTexts[i].Replace("NEWLINE", "\n");
        }
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {

        if (m_scalingUp)
        {
            float newX = transform.position.x + (2000f * Time.deltaTime);
            if (transform.position.x >= 550f)
            {
                newX = 550f;
                m_scalingUp = false;
                //mainText.gameObject.SetActive(true);
                if (m_HasEatenFirstFood)
                {
                    StartCoroutine(ClosingDelay());
                }
                
            }
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        }
        else if (m_scalingDown)
        {
            float newX = transform.position.x - (2000f * Time.deltaTime);
            if (transform.position.x <= startingX)
            {
                newX = startingX;
                m_scalingDown = false;
                //mainText.gameObject.SetActive(false);
                //helpButton.SetActive(true);
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
        
        currentTip = index;
        if (!m_English)
        {
            currentTip += 5;
        }
        mainText.text = displayTexts[currentTip];
        m_scalingUp = true;
    }

    IEnumerator ClosingDelay()
    {
        yield return new WaitForSeconds(instructionStayTimer);
        ClosePopUp();
    }

    public void ClosePopUp()
    {
        m_scalingDown = true;
    }

    public void SetFirstTipOff()
    {
        m_HasEatenFirstFood = true;
        StartCoroutine(ClosingDelay());
    }

    public void SwitchLanguage()
    {
        m_English = !m_English;
        if (!m_English)
        {
            languageButton.sprite = english;
            currentTip += 5;
            mainText.text = displayTexts[currentTip];
        }
        else
        {
            languageButton.sprite = french;
            currentTip -= 5;
            mainText.text = displayTexts[currentTip];
        }

    }
}
