using UnityEngine;
using System.Collections;

public class Heartbeat : MonoBehaviour
{
    public float interval = 8.0f;
    public float duration = 5.0f;

    public float radiusStep = 1.0f;

    private Metagame metagame_;
    private bool disabled_;

    private float sonarRadius_;

    public void SetMetagame(Metagame metagame)
    {
        metagame_ = metagame;
    }

	private void Start()
    {
        StartCoroutine(Ping(interval));
	}

    // MRBrown@PM 1/25/2013: TODO: Support a paused state.
    private IEnumerator Ping(float seconds)
    {
        while (!disabled_)
        {
            yield return new WaitForSeconds(seconds);

            Debug.Log("Ping!");

            StartCoroutine(Sonar(this.duration));
        }
    }

    private IEnumerator Sonar(float seconds)
    {
        float currentTime = Time.time;
        float endTime = currentTime + seconds;

        // Start at 1 unity out from the player.
        sonarRadius_ = 1.0f;

        while (currentTime < endTime)
        {
            sonarRadius_ += radiusStep * Time.deltaTime;

            yield return new WaitForEndOfFrame();
            currentTime = Time.time;
        }
    }

	private void Update()
    {
	}
}
