using UnityEngine;
using System.Collections;

public class DarkLightning : MonoBehaviour 
{
    private LineRenderer _lineRenderer;
    private GameObject _source;
    private GameObject _target;

    private int Frequency = 3; // The number of vertices per Unity meter.
    private float Deviation = 0.2f; // The maximum distance a vertex can deviate.
    private float Speed = 0.02f; // The time in between vertex updates.

	void Awake () 
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
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
        Destroy(this.gameObject);
    }
}
