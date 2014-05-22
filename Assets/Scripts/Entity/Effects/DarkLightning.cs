using UnityEngine;
using System.Collections;

public class DarkLightning : MonoBehaviour 
{
    public LineRenderer _lineRenderer;
    public GameObject _source;
    public GameObject _target;

    public int Frequency = 3; // The number of vertices per Unity meter.
    public float Deviation = 0.2f; // The maximum distance a vertex can deviate.
    public float Speed = 0.02f; // The time in between vertex updates.

	void Awake () 
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Activate(_source, _target, 5);
        }
    }

    public void Activate(GameObject start, GameObject end, float time)
    {
        if (start != null && end != null && time > 0)
        {
            _source = start;
            _target = end;

            StartCoroutine("Lightning");
            Invoke("Deactivate", time);
        }
    }

    private IEnumerator Lightning()
    {
        while (true)
        {
            Vector3 _vectorToTarget = _target.transform.position - _source.transform.position; // Get the vector between source and target.
            int _vertexCount = (int)_vectorToTarget.magnitude * Frequency; // Calculate the number of vertices for this line.
            float _segmentDistance = _vectorToTarget.magnitude / (float)_vertexCount; // Calculate the distance along _vectorToTarget in between each vertex.
            _vectorToTarget.Normalize(); // Normalize the vector.
            _lineRenderer.SetVertexCount(_vertexCount); // Update the vertex count.

            for (int i = 0; i < _vertexCount; i++)
            {
                Vector3 _vertexOrigin = _source.transform.position + _vectorToTarget * (_segmentDistance * i); // Calculate the position along the vector where this vertex should originate.
                Vector3 _localDeviation = Random.insideUnitSphere * Deviation; // Get a random position around the vertex position for the deviation.

                _lineRenderer.SetPosition(i, _vertexOrigin + _localDeviation);
            }

            yield return new WaitForSeconds(Speed);
        }
    }

    private void Deactivate()
    {
        StopCoroutine("Lightning");
        _lineRenderer.SetVertexCount(0);
        //Destroy(this.gameObject);
    }
}
