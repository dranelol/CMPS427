using UnityEngine;
using System.Collections;

public class LineDraw : MonoBehaviour {
    public LineRenderer lineRenderer;
	// Use this for initialization

    public float alphaDelta = 0.1f;

    public int lineLength = 5;
    public float lineWidthStart = 0.1f;
    public float lineWidthEnd = 1f;

    public float angleFromForward = 45;

    public GameObject player;

    public Color colorStart = Color.white;
    public Color colorEnd = Color.blue;
    private Color currentColor;
    private float currentWidth;
    private float currentTime;


    public float timeToLerp = 1.0f;


	void Start () 
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(lineWidthStart, lineWidthStart);
        lineRenderer.SetVertexCount(lineLength);
        

        player = GameObject.FindGameObjectWithTag("Player");

        currentColor = colorStart;
        currentWidth = lineWidthStart;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = player.transform.position;
        //lineRenderer.SetPosition(0, transform.parent.transform.position);

        lineRenderer.SetPosition(0, Vector3.zero);
        for (int i = 1; i < lineLength; i++)
        {
            //Vector3 pos = player.transform.forward * i;
            //Vector3 pos = new Vector3(player.transform.forward.x * Mathf.Sin(45f) * i, player.transform.forward.y, player.transform.forward.z * Mathf.Cos(45f) * i);
            Vector3 pos = Rotations.RotateAboutY(new Vector3(player.transform.forward.x * i, player.transform.forward.y, player.transform.forward.z * i), angleFromForward);
            lineRenderer.SetPosition(i, pos);
        }

        float lerpTime = Mathf.PingPong(Time.time, timeToLerp) / timeToLerp;

        
        currentWidth = Mathf.Lerp(lineWidthStart, lineWidthEnd, lerpTime);
        //Debug.Log(lerpTime);
        Debug.Log(currentWidth);

        if (currentColor == Color.white)
        {
            StartCoroutine(lerpColor(true));
        }

        

        if (currentColor == Color.blue)
        {
            StopCoroutine("lerpColor");

            StartCoroutine(lerpColor(false));
        }

        if (currentWidth == lineWidthStart)
        {
            StartCoroutine(lerpWidth(true));
        }

        if (currentWidth == lineWidthEnd)
        {
            StopCoroutine("lerpWidth");
            StartCoroutine(lerpWidth(false));
        }

        
        
        //currentColor = Color.Lerp(currentColor, colorEnd, Time.time/100f);
        lineRenderer.SetColors(currentColor, currentColor);
        lineRenderer.SetWidth(currentWidth, currentWidth);

        
	}

    IEnumerator lerpColor(bool fromStart)
    {
        float tColor = 0.0f;
        float lerpRate = 1.0f / timeToLerp;

        while (tColor < 1.0f)
        {
            tColor += (Time.deltaTime * lerpRate);
            if (fromStart == true)
            {
                currentColor = Color.Lerp(colorStart, colorEnd, tColor);
            }

            else
            {
                currentColor = Color.Lerp(colorEnd, colorStart, tColor);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator lerpWidth(bool fromStart)
    {
        float tWidth = 0.0f;
        float lerpRate = 1.0f / timeToLerp;

        while (tWidth < 1.0f)
        {
            tWidth += (Time.deltaTime * lerpRate);
            if (fromStart == true)
            {
                currentWidth = Mathf.Lerp(lineWidthStart, lineWidthEnd, tWidth);
            }

            else
            {
                currentWidth = Mathf.Lerp(lineWidthEnd, lineWidthStart, tWidth);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    


}
