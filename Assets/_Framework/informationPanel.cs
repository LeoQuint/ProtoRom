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
    public float educationTextDisplayTime = 8f;
    [SerializeField]
    float educationTextFadeSpeed = 2f;
    int lastStageIndex = 0;

    private int currentTip = 0;



    void Awake()
    {
        mainText = transform.FindChild("Text").GetComponent<Text>();
        mainText.gameObject.SetActive(true);
        Debug.Log(transform.position);
        startingX = transform.position.x;
        educationText.color = Vector4.zero;

        MakeNewlines();
        //Check for the language from the menu screen
        if (soundPlayer.Language != 0)
        {
            SwitchLanguage();
        }
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
        //Education text
        if (isEducationFades)
        {
            FadeInOutEducation(educationUp);
        }

    }
    public void PopUp()
    {
        StopAllCoroutines();
        if (m_scalingUp || m_scalingDown) {

            return;
        }
        //helpButton.SetActive(false);
        Debug.Log(currentTip);
        if (!m_English)
        {
            educationText.text = displayEducationTexts[currentTip + lastStageIndex + 2 + 5];
            mainText.text = displayTexts[currentTip + lastStageIndex + 5];
        }
        else
        {
            mainText.text = displayTexts[currentTip];
            educationText.text = displayEducationTexts[currentTip + lastStageIndex];
        }
        color_Education = 0f;
        isEducationFades = true;
        educationUp = true;
        m_scalingUp = true;
        
    }
    public void PopUp(int index)
    {
        
        currentTip = index;

        if (!m_English)
        {
            mainText.text = displayTexts[currentTip + 5];
            educationText.text = displayEducationTexts[currentTip + lastStageIndex + 2 + 5];
        }
        else
        {
            mainText.text = displayTexts[currentTip];
            educationText.text = displayEducationTexts[currentTip + lastStageIndex];
        }

        color_Education = 0f;
        isEducationFades = true;
        educationUp = true;
        m_scalingUp = true;
        StopAllCoroutines();
        
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
            soundPlayer.Language = 1;
            languageButton.sprite = english;
            //currentTip += 5;
            mainText.text = displayTexts[currentTip + 5];

            if (currentTip == 4)
            {
                //education text has 7 length hence the +2
                educationText.text = displayEducationTexts[currentTip + lastStageIndex + 2 + 5];
            }
            else
            {
                educationText.text = displayEducationTexts[currentTip +2 + 5];
            }
        }
        else
        {
            soundPlayer.Language = 0;
            languageButton.sprite = french;
            //currentTip -= 5;
            mainText.text = displayTexts[currentTip];

            if (currentTip == 4)
            {
                educationText.text = displayEducationTexts[currentTip + lastStageIndex];
            }
            else
            {
                educationText.text = displayEducationTexts[currentTip];
            }
        }

    }

    //Education text section
    //Sets the index of the educational text for the last stage base on what animal you have.
    public void SetLastPhaseIndex(int val)
    {
        lastStageIndex = val;
    }
    private float color_Education = 0f;
    private bool isEducationFades = false;
    private bool educationUp = false;
    void FadeInOutEducation(bool up)
    {
        if (up)
        {
            color_Education += Time.deltaTime * educationTextFadeSpeed;
            if (color_Education >= 1f)
            {
                color_Education = 1f;
                isEducationFades = false;
                StartCoroutine(DelayForEducation());
            }
        }
        else
        {
            color_Education -= Time.deltaTime * educationTextFadeSpeed;
            if (color_Education <= 0f)
            {
                color_Education = 0f;
                isEducationFades = false;
            }
        }
        educationText.color = Vector4.one * color_Education;
    }

    IEnumerator DelayForEducation()
    {
        yield return new WaitForSeconds(educationTextDisplayTime);
        isEducationFades = true;
        educationUp = false;
    }
    IEnumerator ClosingDelay()
    {
        yield return new WaitForSeconds(instructionStayTimer);
        ClosePopUp();
    }
}
