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

    private int currentTip = 0;
    void Awake()
    {
        mainText = transform.FindChild("Text").GetComponent<Text>();
        mainText.gameObject.SetActive(false);
    }

	void Start () {
        transform.localScale = new Vector3(m_scale, m_scale, m_scale);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PopUp(0);
        }
        if (m_scalingUp)
        {
            m_scale += m_scalingSpeed * Time.deltaTime;
            if (m_scale >= 1f)
            {
                m_scale = 1f;
                m_scalingUp = false;
                mainText.gameObject.SetActive(true);
                StartCoroutine(ClosingDelay());
            }
            transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            
        }
        if (m_scalingDown)
        {
            m_scale -= m_scalingSpeed * Time.deltaTime;
            if (m_scale <= 0f)
            {
                helpButton.SetActive(true);
                m_scale = 0f;
                mainText.gameObject.SetActive(false);
                m_scalingDown = false;
            }
            transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            
        }
    }
    public void PopUp()
    {
        helpButton.SetActive(false);
        mainText.text = displayTexts[currentTip];
        m_scalingUp = true;
    }
    public void PopUp(int index)
    {
        helpButton.SetActive(false);
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
