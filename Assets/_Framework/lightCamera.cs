using UnityEngine;
using System.Collections;

public class lightCamera : MonoBehaviour {
    player pScript;
    Transform target;
    Camera spot;

    public Vector2 intensityRange;
    
    public float rateOfFlux;
    public Material spotLight;

    private bool isOff = false;
    private bool goingBrighter = true ;

    public bool isAmbiant = false;
    public float isOnDuration = 2f;

    private Transform m_mainObj;
    private float m_scale = 0.01f;
	// Use this for initialization
	void Start () {
        spotLight = transform.parent.transform.FindChild("Triangle").GetComponent<Renderer>().material;
        m_mainObj = transform.parent;

       
        if (isAmbiant)
        {
            m_scale = 1f;
            spotLight.color = new Vector4(1f, 1f, 1f, Random.Range(intensityRange.x, intensityRange.y));
        }
        else
        {
            spotLight.color = new Vector4(1f, 1f, 1f, 0.1f);
            spot = GetComponent<Camera>();
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            target = p.transform;
            pScript = p.GetComponent<player>();
        }
        m_mainObj.localScale = new Vector3(m_scale, 1f, 1f);

    }
	
	// Update is called once per frame
	void Update () {

       
       
        if (isAmbiant)
        {
            return;
        }
        Vector3 screenPoint = spot.WorldToViewportPoint(target.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen)
        {
            pScript.Photosynthesize(1f * Time.deltaTime);
        }
        else
        {
            pScript.Photosynthesize(false);
        }

        if (isOff)
        {
            return;
        }

        Fluctuate();

    }

    void FadingFromPlayer()
    {
        if (spotLight.color.a >= 0f)
        {
            spotLight.color = new Vector4(1f, 1f, 1f, spotLight.color.a - (2f* Time.deltaTime));
        }
        
    }

    void Fluctuate()
    {
        if (m_scale >= intensityRange.y)
        {
            goingBrighter = false;
            isOff = true;
            if (isAmbiant)
            {
                StartCoroutine(LeaveOn(Random.Range(0f, 2f)));
            }
            else
            {
                StartCoroutine(LeaveOn(isOnDuration));
            }
        }
        if (m_scale <= intensityRange.x)
        {
            if (!isAmbiant)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                goingBrighter = true;
                isOff = true;
                StartCoroutine(LeaveOff(Random.Range(5f, 10f)));
            }

        }
        if (goingBrighter)
        {
            m_scale += rateOfFlux * Time.deltaTime;
            m_mainObj.localScale = new Vector3(m_scale, 1f, 1f);
        }
        else
        {
            m_scale -= rateOfFlux * Time.deltaTime;
            m_mainObj.localScale = new Vector3(m_scale, 1f, 1f);
        }

        //if (spotLight.color.a >= intensityRange.y)
        //{
        //    goingBrighter = false;
        //    isOff = true;
        //    if (isAmbiant)
        //    {
        //        StartCoroutine(LeaveOn(Random.Range(0f, 2f)));
        //    }
        //    else
        //    {
        //        StartCoroutine(LeaveOn(isOnDuration));
        //    }
        //}
        //if (spotLight.color.a <= intensityRange.x)
        //{
        //    if (!isAmbiant)
        //    {
        //        Destroy(transform.parent.gameObject);
        //    }
        //    else
        //    {
        //        goingBrighter = true;
        //        isOff = true;
        //        StartCoroutine(LeaveOff(Random.Range(5f, 10f)));
        //    }

        //}
        //if (goingBrighter)
        //{
        //    spotLight.color = new Vector4(1f, 1f, 1f, spotLight.color.a + (rateOfFlux * Time.deltaTime));
        //}
        //else
        //{
        //    spotLight.color = new Vector4(1f, 1f, 1f, spotLight.color.a - (rateOfFlux * Time.deltaTime));
        //}


    }



    IEnumerator LeaveOff(float dur)
    {
        yield return new  WaitForSeconds(dur);
        isOff = false;
    }

    IEnumerator LeaveOn(float dur)
    {
        yield return new WaitForSeconds(dur);
        isOff = false;
    }


}
