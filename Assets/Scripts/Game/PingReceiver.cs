using UnityEngine;

public enum PingReceiverState
{
    // The receiver has been initialized.
    Awake,

    // The receiver is registered and active.
    Active,

    // The receiver was detected by a ping.
    Detected,
}

public class PingReceiver : MonoBehaviour
{
    // Specifies how long the receiver should remain detected.
    public float detectedDuration = 1.0f;

    public float pulseFadeSpeed = 1.0f;
    public Material pulseEchoMaterial;

    private Metagame metagame_;
    private PingReceiverState state_;
    private float detectedElapsed_;

    private Vector3 pulsePosition_;
    private float pulseRadius_;
    private float pulseMaxRadius_;
    private float pulseWaveSpeed_;
    private float pulseFade_;
    private bool pulsing_;

    public PingReceiverState GetState()
    {
        return state_;
    }

    public Vector2 GetPingPoint()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }

    public void Ping(Vector2 pulseCenter, float pulseRadius, float pulseMaxRadius, float pulseSpeed)
    {
        state_ = PingReceiverState.Detected;
        detectedElapsed_ = 0.0f;

        // Use this transform's Y so that the pulse appears to come from the same plane as the object.
        pulsePosition_ = new Vector3(pulseCenter.x, pulseCenter.y, this.transform.position.z);
        pulseRadius_ = pulseRadius;
        pulseMaxRadius_ = pulseMaxRadius;
        pulseWaveSpeed_ = pulseSpeed;
        pulseFade_ = 0.0f;
        pulsing_ = true;

        OnPing();
    }

    protected virtual void OnPing()
    {
        string debugString = string.Format("{0} ({1}, {2}): Ping received!", gameObject.name, GetPingPoint().x, GetPingPoint().y);

        Debug.Log(debugString);
    }

    private void Awake()
    {
        GameObject metagameObject = GameObject.FindGameObjectWithTag("Metagame");

        if ((metagameObject == null) || ((metagame_ = metagameObject.GetComponent<Metagame>()) == null))
        {
            Debug.LogError("Couldn't locate a GO with the \"Metagame\" tag or it's missing the Metagame script.");
            this.enabled = false;
            return;
        }

        state_ = PingReceiverState.Awake;
    }

    private void Start()
    {
        state_ = PingReceiverState.Active;

        // Register ourselves with the metagame as a ping receiver.
        metagame_.RegisterPingReceiver(this);
    }

    private void OnDestroy()
    {
        // Unregister ourselves as a ping receiver.
        metagame_.UnregisterPingReceiver(this);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (state_ == PingReceiverState.Detected)
        {
            detectedElapsed_ += deltaTime;

            if (detectedElapsed_ >= this.detectedDuration)
            {
                // This receiver can be detected again.
                state_ = PingReceiverState.Active;
                return;
            }
        }

        if (pulsing_)
        {
            UpdatePulseEcho(deltaTime);
        }

        if (this.pulseEchoMaterial != null)
        {
            UpdatePulseEchoMaterial(deltaTime);
        }
    }

    private void UpdatePulseEcho(float deltaTime)
    {
        pulseRadius_ += deltaTime * pulseWaveSpeed_;

        if (pulseRadius_ >= pulseMaxRadius_)
        {
            pulsing_ = false;
        }
    }

    private void UpdatePulseEchoMaterial(float deltaTime)
    {
        pulseFade_ += deltaTime * this.pulseFadeSpeed;

        Material material = this.pulseEchoMaterial;

        material.SetVector("_Position", pulsePosition_);
        material.SetFloat("_Radius", pulseRadius_);
        material.SetFloat("_MaxRadius", pulseMaxRadius_);
        material.SetFloat("_Fade", pulseFade_);
    }
}
