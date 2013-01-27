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

    private Metagame metagame_;
    private PingReceiverState state_;
    private float stateElapsed_;

    private Vector3 pulsePosition_;
    private float pulseRadius_;
    private float pulseMaxRadius_;
    private float pulseWaveSpeed_;
    private float pulseFade_;
    private bool pulsing_;

    private GameObject blipInstance_;
    private Material pulseEchoMaterial_;

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

        if (blipInstance_ != null)
        {
            blipInstance_.transform.position = this.transform.position;
            blipInstance_.renderer.enabled = true;
        }

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
        if ((metagame_ = Metagame.GetInstance()) == null)
        {
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

        blipInstance_ = Instantiate(metagame_.Blip) as GameObject;

        if (blipInstance_)
        {
            blipInstance_.transform.parent = this.transform;
            pulseEchoMaterial_ = blipInstance_.renderer.material;
        }
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

        if (pulseEchoMaterial_ != null)
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

        Material material = pulseEchoMaterial_;

        material.SetVector("_Position", pulsePosition_);
        material.SetFloat("_Radius", pulseRadius_);
        material.SetFloat("_MaxRadius", pulseMaxRadius_);
        material.SetFloat("_Fade", pulseFade_);
    }
}
