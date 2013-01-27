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

    public float pulseFadeDuration = 1.0f;

    private Metagame metagame_;
    private PingReceiverState state_;
    private float stateElapsed_;

    private Vector3 pulsePosition_;
    private float pulseRadius_;
    private float pulseMaxRadius_;
    private float pulseWaveSpeed_;
    private float pulseFade_;
    private float pulseFadeTime_;
    private int lastPulseId_;
    private bool pulsing_;
    private bool fading_;

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

    public Rect GetPingRect()
    {
        Collider collider = GetComponent<Collider>();

        if (collider == null)
        {
            return new Rect(-10000.0f, -10000.0f, 1.0f, 1.0f);
        }

        Bounds bounds = collider.bounds;
        Vector3 worldMin = this.transform.TransformPoint(bounds.min);
        Vector3 worldMax = this.transform.TransformPoint(bounds.max);
        float left = worldMin.x;
        float top = worldMin.z;
        float width = worldMax.x - left;
        float height = worldMax.z - top;

        return new Rect(left, top, width, height);
    }

    public void Ping(Heartbeat.Pulse pulse)
    {
        if (pulse.Id == lastPulseId_)
        {
            return;
        }

        SetState(PingReceiverState.Detected);

        // Use this transform's Y so that the pulse appears to come from the same plane as the object.
        pulsePosition_ = new Vector3(pulse.Center.x, pulse.Center.y, this.transform.position.z);
        pulseRadius_ = pulse.Radius;
        pulseMaxRadius_ = pulse.MaxRadius;
        pulseWaveSpeed_ = pulse.Speed;
        pulseFade_ = 0.0f;
        pulseFadeTime_ = 0.0f;
        pulsing_ = true;
        lastPulseId_ = pulse.Id;

        DestroyBlipInstance();

        blipInstance_ = Instantiate(metagame_.Blip, this.transform.position, this.transform.rotation) as GameObject;

        if (blipInstance_ != null)
        {
            pulseEchoMaterial_ = blipInstance_.renderer.material;

            Camera mainCamera = this.camera ? this.camera : Camera.main;
            Quaternion cameraRotation = mainCamera.transform.rotation;

            blipInstance_.transform.LookAt(blipInstance_.transform.position
                                                     + (cameraRotation * Vector3.back), cameraRotation * Vector3.up);

            blipInstance_.renderer.enabled = true;
        }

        OnPing();
    }

    protected virtual void OnPing()
    {
//        string debugString = string.Format("{0} ({1}, {2}): Ping received!", gameObject.name, GetPingPoint().x, GetPingPoint().y);

//        Debug.Log(debugString);
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
    }

    private void OnDestroy()
    {
        DestroyBlipInstance();

        // Unregister ourselves as a ping receiver.
        metagame_.UnregisterPingReceiver(this);
    }

    private void DestroyBlipInstance()
    {
        pulseEchoMaterial_ = null;

        if (blipInstance_ != null)
        {
            blipInstance_.renderer.enabled = false;
            Destroy(blipInstance_);
            blipInstance_ = null;
        }
    }

    private void FixedUpdate()
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

        UpdatePulseEcho(deltaTime);

        if (pulseEchoMaterial_ != null)
        {
            UpdatePulseEchoMaterial(deltaTime);
        }
    }

    private void UpdatePulseEcho(float deltaTime)
    {
        if (pulsing_)
        {
            pulseRadius_ += deltaTime * pulseWaveSpeed_;

            if (pulseRadius_ >= pulseMaxRadius_)
            {
                pulsing_ = false;
                fading_ = true;
            }
        }
        else if (fading_)
        {
            pulseFadeTime_ += deltaTime;

            float t = Mathf.Clamp01(pulseFadeTime_ / this.pulseFadeDuration);

            pulseFade_ = Mathf.Lerp(0, pulseWaveSpeed_, t);

            if (pulseFade_ >= this.pulseWaveSpeed_)
            {
                fading_ = false;
                DestroyBlipInstance();
            }
        }
    }

    private void UpdatePulseEchoMaterial(float deltaTime)
    {
        Material material = pulseEchoMaterial_;

        material.SetVector("_Position", pulsePosition_);
        material.SetFloat("_Radius", pulseRadius_);
        material.SetFloat("_MaxRadius", pulseMaxRadius_);
        material.SetFloat("_Fade", pulseFade_);
    }
}
