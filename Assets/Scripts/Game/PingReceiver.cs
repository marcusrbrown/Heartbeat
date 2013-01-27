using UnityEngine;

public enum PingReceiverState
{
    // The receiver has been initialized.
    Awake,

    // The receiver is registered and active.
    Active,

    // The receiver was detected by a ping.
    Detected,

    // The reciever is in a cooling down after detection.
    Cooldown,
}

public class PingReceiver : MonoBehaviour
{
    // Specifies how long the receiver should remain detected.
    public float detectedDuration = 1.0f;
    public float cooldownDuration = 2.0f;

    public float pulseFadeSpeed = 1.0f;
    public Material pulseEchoMaterial;

    private Metagame metagame_;
    private PingReceiverState state_;
    private float stateElapsed_;

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

    public void Ping(Heartbeat.Pulse pulse)
    {
        SetState(PingReceiverState.Detected);

        // Use this transform's Y so that the pulse appears to come from the same plane as the object.
        pulsePosition_ = new Vector3(pulse.Center.x, pulse.Center.y, this.transform.position.z);
        pulseRadius_ = pulse.Radius;
        pulseMaxRadius_ = pulse.MaxRadius;
        pulseWaveSpeed_ = pulse.Speed;
        pulseFade_ = 0.0f;
        pulsing_ = true;

        OnPing();
    }

    protected virtual void OnPing()
    {
        string debugString = string.Format("{0} ({1}, {2}): Ping received!", gameObject.name, GetPingPoint().x, GetPingPoint().y);

        Debug.Log(debugString);
    }

    private void SetState(PingReceiverState state)
    {
        state_ = state;
        stateElapsed_ = 0.0f;
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

        SetState(PingReceiverState.Awake);
    }

    private void Start()
    {
        SetState(PingReceiverState.Active);

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

        stateElapsed_ += deltaTime;

        if ((state_ == PingReceiverState.Detected) && (stateElapsed_ >= this.detectedDuration))
        {
            // Put the receiver into cooldown.
            SetState(PingReceiverState.Cooldown);
        }

        if ((state_ == PingReceiverState.Cooldown) && (stateElapsed_ >= this.cooldownDuration))
        {
            // This receiver can be detected again.
            SetState(PingReceiverState.Active);
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
