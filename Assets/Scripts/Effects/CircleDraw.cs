using UnityEngine;
using System.Collections;

public class CircleDraw : MonoBehaviour
{
    public LineRenderer lineRenderer;
    // Use this for initialization

    public float alphaDelta = 0.1f;

    public int vertices = 0;
    public float radius = 2.0f;
    public float lineWidthStart = 0.1f;
    public float lineWidthEnd = 1f;

    public GameObject player;

    public Color colorStart = Color.white;
    public Color colorEnd = Color.blue;
    private Color currentColor;
    private float currentWidth;
    private float currentTime;


    public float timeToLerp = 1.0f;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(lineWidthStart, lineWidthStart);
        lineRenderer.SetVertexCount(vertices+1);


        player = GameObject.FindGameObjectWithTag("Player");

        currentColor = colorStart;
        currentWidth = lineWidthStart;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        //lineRenderer.SetPosition(0, transform.parent.transform.position);
        float angle = 0.0f;
        for (int i = 0; i < vertices + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = Mathf.Cos(Mathf.Deg2Rad * angle);

            Vector3 pos = new Vector3(x, 0, z) * radius;
            lineRenderer.SetPosition(i, pos);
            angle += (360.0f / vertices);
        }

        float lerpTime = Mathf.PingPong(Time.time, timeToLerp) / timeToLerp;


        currentWidth = Mathf.Lerp(lineWidthStart, lineWidthEnd, lerpTime);
        //Debug.Log(lerpTime);
        //Debug.Log(currentWidth);

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
