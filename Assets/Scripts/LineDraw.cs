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


    public float timeToLerp = 100f;


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
        }

        
        
        //currentColor = Color.Lerp(currentColor, colorEnd, Time.time/100f);
        lineRenderer.SetColors(currentColor, currentColor);
        lineRenderer.SetWidth(currentWidth * Time.time, currentWidth * Time.time);

        
	}

    IEnumerator lerpColor(bool fromStart)
    {
        float tColor = 0.0f;
        float lerpRate = 1.0f / timeToLerp;

        while (tColor < 1.0f)
        {
            Debug.Log(tColor);
            tColor += (Time.deltaTime * lerpRate);
            currentColor = Color.Lerp(colorStart, colorEnd, tColor);
            //yield return new WaitForEndOfFrame();
        }

        yield return null;


    }

    IEnumerator lerpWidth(bool fromStart)
    {
        yield return null;
    }

    


}
