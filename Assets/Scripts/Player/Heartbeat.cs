using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heartbeat : MonoBehaviour
{
    private class SonarWave
    {
        public readonly int Id;
        public readonly Vector2 Center;
        public float Duration;
        public float Radius;
        public float RadiusStep;

        private static int nextId_;

        internal SonarWave(Vector2 center, float duration, float radius, float radiusStep)
        {
            Center = center;
            Duration = duration;
            Radius = radius;
            RadiusStep = radiusStep;
            Id = ++nextId_;
        }
    }

    public float interval = 8.0f;
    public float duration = 5.0f;

    public float radiusStep = 1.0f;

    private Metagame metagame_;
    private bool disabled_;

    private Dictionary<int, SonarWave> sonarWaves_ = new Dictionary<int, SonarWave>();

    public void SetMetagame(Metagame metagame)
    {
        metagame_ = metagame;
    }

    protected void OnPong(Object sender)
    {
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

            // The center is wherever the player currently is located.
            Vector2 center = new Vector2(this.transform.position.x, this.transform.position.y);

            StartCoroutine(Sonar(CreateSonarWave(center)));
        }
    }

    private IEnumerator Sonar(SonarWave sonarWave)
    {
        float currentTime = Time.time;
        float endTime = currentTime + sonarWave.Duration;

        while (currentTime < endTime)
        {
            float deltaTime = Time.deltaTime;

            sonarWave.Radius += sonarWave.RadiusStep * Time.deltaTime;

            yield return new WaitForEndOfFrame();
            currentTime += deltaTime;
        }

        RemoveSonarWave(sonarWave);
    }

    private SonarWave CreateSonarWave(Vector2 center)
    {
        // Start the radius at one unit out from the player.
        SonarWave wave = new SonarWave(center, this.duration, 1.0f, this.radiusStep);

        sonarWaves_.Add(wave.Id, wave);
        return wave;
    }

    private void RemoveSonarWave(SonarWave sonarWave)
    {
        sonarWaves_.Remove(sonarWave.Id);
    }

	private void Update()
    {
	}
}
