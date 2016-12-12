using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuotePlayer : MonoBehaviour {


    [SerializeField]
    string[] quotes;
    [SerializeField]
    string[] quotesFrancais;
    [SerializeField]
    float fadeInSpeed = 2f;

    private Text textField;
    private float m_color = 0f;

    private bool isFadingIn = false;
    private int selectedQuote = 0;

    void Awake()
    {
        textField = GetComponent<Text>();
        textField.color = new Vector4(1f,1f,1f,m_color);
    }
    // Use this for initialization
    void Start () {
        for (int i = 0; i < quotes.Length; ++i)
        {
            quotes[i] = quotes[i].Replace("NEWLINE", "\n");
        }
        for (int i = 0; i < quotesFrancais.Length; ++i)
        {
            quotesFrancais[i] = quotesFrancais[i].Replace("NEWLINE", "\n");
        }
        selectedQuote = Random.Range(0, quotes.Length);
    }
	
	// Update is called once per frame
	void Update () {
        if (isFadingIn)
        {
            FadeUp();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            isFadingIn = true;
        }
	}

    public void StartQuote()
    {
        isFadingIn = true;
        if (soundPlayer.Language == 0)
        {
            textField.text = quotes[selectedQuote];
        }
        else
        {
            textField.text = quotesFrancais[selectedQuote];
        }
    }

    void FadeUp()
    {
        
        m_color += Time.deltaTime / fadeInSpeed;
        if (m_color > 1f)
        {
            m_color = 1f;
            isFadingIn = false;           
        }
        textField.color = new Vector4(1f, 1f, 1f, m_color);
    }
}