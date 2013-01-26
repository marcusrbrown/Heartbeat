using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class SonarWave : MonoBehaviour
{
    public bool autoPulse;
    public int segments = 30;
    public float y = 0.0f;

    // The values for these are intended to be set by the calling script.
    public float duration = 5.0f;
    public float radius = 1.0f;
    public float speed = 1.0f;

    private LineRenderer lineRenderer_;
    private float elapsed_;
    private float pulseRadius_;
    private bool pulsing_;

    public void Pulse()
    {
        elapsed_ = 0.0f;
        pulseRadius_ = this.radius;
        lineRenderer_.enabled = true;
        pulsing_ = true;
        Debug.Log("Pulsing!");
    }

    private void Awake()
    {
        lineRenderer_ = this.gameObject.GetComponent<LineRenderer>();
    }

    private IEnumerator Start()
    {
        lineRenderer_.SetVertexCount(this.segments + 1);
        lineRenderer_.useWorldSpace = false;
        lineRenderer_.enabled = false;

        while (this.autoPulse)
        {
            Pulse();
            yield return new WaitForSeconds(duration);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        elapsed_ += deltaTime;

        if (elapsed_ >= this.duration)
        {
            pulsing_ = false;
        }

        if (!pulsing_)
        {
            return;
        }

        pulseRadius_ += this.speed * deltaTime;
        DrawWave();
    }

    private void DrawWave()
    {
        float angle = 0.0f;

        for (int i = 0; i < (segments + 1); ++i)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float z = Mathf.Cos(angle * Mathf.Deg2Rad);

            lineRenderer_.SetPosition(i, new Vector3(x, this.y, z) * pulseRadius_);
            angle += (360.0f / segments);
        }
    }
}