using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Heartbeat : MonoBehaviour
{
    public float interval = 8.0f;
    public float pulseWaveDuration = 5.0f;
    public float pulseWaveSpeed = 1.0f;
    public float pulseStartRadius = 1.0f;
    public GameObject pulseWave;

    private Metagame metagame_;
    private float elapsed_;

    private Dictionary<int, Pulse> activePulses_ = new Dictionary<int, Pulse>();

    public void SetMetagame(Metagame metagame)
    {
        metagame_ = metagame;
    }

    private void Start()
    {
	}

    // MRBrown@PM 1/25/2013: TODO: Support a paused state.
    private IEnumerator Ping()
    {
        Debug.Log("Ping!");

        // The center is wherever the player currently is located.
        Vector2 center = new Vector2(this.transform.position.x, this.transform.position.z);

        CreatePulse(center);
        yield break;
    }

    private Pulse CreatePulse(Vector2 center)
    {
        GameObject sonarWave = null;

        if (this.pulseWave != null)
        {
            sonarWave = Instantiate(this.pulseWave, this.transform.position, this.transform.rotation) as GameObject;
        }

        Pulse wave = new Pulse(sonarWave, center, this.pulseWaveDuration, this.pulseStartRadius, this.pulseWaveSpeed);

        activePulses_.Add(wave.Id, wave);
        return wave;
    }

    private void RemovePulse(Pulse pulse)
    {
        if (pulse.SonarWave != null)
        {
            Destroy(pulse.SonarWave.gameObject);
        }

        activePulses_.Remove(pulse.Id);
    }

	private void Update()
    {
        float deltaTime = Time.deltaTime;

        elapsed_ += deltaTime;

        if (elapsed_ >= this.interval)
        {
            elapsed_ = 0.0f;
            StartCoroutine(Ping());
        }

        UpdatePulses(deltaTime);
	}

    private void UpdatePulses(float deltaTime)
    {
        Pulse[] pulses = activePulses_.Values.ToArray();

        foreach (Pulse pulse in pulses)
        {
            pulse.Elapsed += deltaTime;

            if (pulse.Elapsed >= pulse.Duration)
            {
                RemovePulse(pulse);
                continue;
            }

            // If there's a SonarWave instance attached, use its radius, otherwise keep track of our own.
            SonarWave sonarWave = pulse.SonarWave;
            float radius = sonarWave != null ? sonarWave.GetPulseRadius() : pulse.Radius;

            radius += pulse.Speed * deltaTime;

            // MRBrown@PM 1/26/2013: TODO: Ask the metagame who got hit by the ping.

            pulse.Radius = radius;
        }
    }

    #region Pulse class

    private class Pulse
    {
        public readonly int Id;
        public readonly Vector2 Center;
        public readonly SonarWave SonarWave;
        public float Duration;
        public float Radius;
        public float Speed;
        public float Elapsed;

        private static int nextId_;

        internal Pulse(GameObject waveObject, Vector2 center, float duration, float radius, float speed)
        {
            Center = center;
            Duration = duration;
            Radius = radius;
            Speed = speed;
            Id = ++nextId_;

            if (waveObject != null)
            {
                SonarWave = waveObject.GetComponent<SonarWave>();
            }

            if (SonarWave != null)
            {
                SonarWave.duration = Duration;
                SonarWave.radius = Radius;
                SonarWave.speed = Speed;

                if (!SonarWave.autoPulse)
                {
                    SonarWave.Pulse();
                }
            }
        }
    }

    #endregion
}
