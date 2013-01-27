using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Metagame : MonoBehaviour
{
    private enum ReceiverState
    {
        // The receiver is registered and active.
        Active,

        // The receiver was detected by a ping.
        Detected,
    }

    public GameObject player;
    public GameObject enemies;
    public GameObject items;

    private Heartbeat heartbeat_;
    private Dictionary<PingReceiver, ReceiverInfo> receivers_ = new Dictionary<PingReceiver, ReceiverInfo>();

    public void RegisterPingReceiver(PingReceiver receiver)
    {
        receivers_.Add(receiver, new ReceiverInfo(receiver));
    }

    public void UnregisterPingReceiver(PingReceiver receiver)
    {
        receivers_.Remove(receiver);
    }

    public void CheckPulseCollisions(Vector2 pulseCenter, float pulseRadius)
    {
        // Look at all ping receivers that are active, but not already detected.
        var activeReceivers = receivers_.Values.Where(r => r.State == ReceiverState.Active);

        foreach (ReceiverInfo active in activeReceivers)
        {
            PingReceiver receiver = active.Receiver;

            if (IsPointInCircle(receiver.GetPingPoint(), pulseCenter, pulseRadius))
            {
                receiver.Ping();
                active.State = ReceiverState.Detected;
            }
        }
    }

    private void Awake()
    {
        if (this.player == null)
        {
            Debug.LogError("Attach the Player GameObject to the Metagame component.");
            this.enabled = false;
            return;
        }

        if (this.enemies == null)
        {
            Debug.LogWarning("Attach a GameObject containing enemies to the Metagame component.");
        }

        if (this.items == null)
        {
            Debug.LogWarning("Attach a GameObject containing items to the Metagame component.");
        }

        heartbeat_ = this.player.GetComponent<Heartbeat>();
        heartbeat_.SetMetagame(this);
    }

    private bool IsPointInCircle(Vector2 testPoint, Vector3 center, float radius)
    {
        float xDistance = testPoint.x - center.x;
        float yDistance = testPoint.y - center.y;
        float distanceSq = (xDistance * xDistance) + (yDistance * yDistance);

        return distanceSq < (radius * radius);
    }

    #region ReceiverInfo nested class

    class ReceiverInfo
    {
        public readonly PingReceiver Receiver;
        public ReceiverState State;

        internal ReceiverInfo(PingReceiver receiver)
        {
            Receiver = receiver;
            State = ReceiverState.Active;
        }
    }

    #endregion
}