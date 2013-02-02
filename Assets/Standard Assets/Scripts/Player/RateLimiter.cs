using UnityEngine;

public class RateLimiter : MonoBehaviour
{
    // The amount of time it takes to reach the rate limit.
    public float timeToLimit = 5.0f;

    // The amount of time it takes to drain (return to normal).
    public float timeToDrain = 2.5f;

    // The heart rate limit.
    public float heartRateLimit = 100.0f;

    private float currentRate_;
    private float limiterHeldTime_;
    private float limiterDrainTime_;

    private bool disabledUntilDrain_;

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
                    currentRate_ = heartRateLimit;
                    // TODO: Broadcast the signal that the limit has been reached.

                    // Don't bother with the "held" limiter until the key is let go and begins to "drain".
                    disabledUntilDrain_ = true;
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
            }
        }
    }
}
