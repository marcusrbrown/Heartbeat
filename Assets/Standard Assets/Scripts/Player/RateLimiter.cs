using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RateThreshold
{
    public float value;
    public GameObject target;
    public string message;
}

public class RateLimiter : MonoBehaviour
{
    // The amount of time it takes to reach the rate limit.
    public float timeToLimit = 5.0f;

    // The amount of time it takes to drain (return to normal).
    public float timeToDrain = 2.5f;

    // The heart rate limit.
    public float heartRateLimit = 100.0f;

    // Thresholds.
    public List<RateThreshold> thresholds = new List<RateThreshold>();

    private float currentRate_;
    private float limiterHeldTime_;
    private float limiterDrainTime_;

    private float lastThresholdValue_;
    private bool disabledUntilDrain_;

    private void Awake()
    {
        // Sort the thresholds.
        this.thresholds.Sort(CompareRateThresholds);
    }

    private static int CompareRateThresholds(RateThreshold a, RateThreshold b)
    {
        return (int) (a.value - b.value);
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;

        if (Input.GetButton("Jump"))
        {
            limiterDrainTime_ = 0.0f;

            if (!disabledUntilDrain_)
            {
                limiterHeldTime_ += deltaTime;

                float t = Mathf.Clamp01(limiterHeldTime_ / this.timeToLimit);

                currentRate_ = Mathf.Lerp(0, this.heartRateLimit, t);

                if (currentRate_ >= this.heartRateLimit)
                {
                    currentRate_ = this.heartRateLimit;
                    lastThresholdValue_ = this.heartRateLimit;

                    // Kill the player.
                    Metagame.GetInstance().Die();

                    // Don't bother with the "held" limiter until the key is let go and begins to "drain".
                    disabledUntilDrain_ = true;
                }
                else
                {
                    foreach (RateThreshold threshold in this.thresholds)
                    {
                        if ((lastThresholdValue_ != threshold.value) && (currentRate_ >= threshold.value))
                        {
                            lastThresholdValue_ = threshold.value;

                            if ((threshold.target != null) && !string.IsNullOrEmpty(threshold.message))
                            {
                                threshold.target.BroadcastMessage(threshold.message);
                            }
                        }
                    }
                }
            }
        }
        else if (currentRate_ > 0.0f)
        {
            disabledUntilDrain_ = false;
            limiterHeldTime_ = 0.0f;
            limiterDrainTime_ += deltaTime;

            float t = Mathf.Clamp01(limiterDrainTime_ / this.timeToDrain);

            currentRate_ = this.heartRateLimit - Mathf.Lerp(0, this.heartRateLimit, t);

            if (currentRate_ <= 0.0f)
            {
                currentRate_ = 0.0f;
                lastThresholdValue_ = 0.0f;
            }
            else
            {
                for (int i = this.thresholds.Count - 1; i > 0; --i)
                {
                    RateThreshold threshold = this.thresholds[i];

                    if ((lastThresholdValue_ != threshold.value) && (currentRate_ < threshold.value))
                    {
                        lastThresholdValue_ = threshold.value;

                        // Broadcast to the threshold before this one.
                        RateThreshold prevThreshold = this.thresholds[i - 1];

                        if ((prevThreshold.target != null) && !string.IsNullOrEmpty(prevThreshold.message))
                        {
                            prevThreshold.target.BroadcastMessage(prevThreshold.message);
                        }
                    }
                }
            }
        }
    }
}
