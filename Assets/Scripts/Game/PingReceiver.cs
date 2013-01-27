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

    private Metagame metagame_;
    private PingReceiverState state_;
    private float detectedElapsed_;

    public PingReceiverState GetState()
    {
        return state_;
    }

    public Vector2 GetPingPoint()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }

    public void Ping()
    {
        state_ = PingReceiverState.Detected;
        detectedElapsed_ = 0.0f;

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
    }
}
