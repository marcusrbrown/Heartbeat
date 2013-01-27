using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Metagame : MonoBehaviour
{
    public GameObject player;
    public GameObject enemies;
    public GameObject items;

    private Heartbeat heartbeat_;
    private LinkedList<PingReceiver> receivers_ = new LinkedList<PingReceiver>();

    public void RegisterPingReceiver(PingReceiver receiver)
    {
        receivers_.AddLast(receiver);
    }

    public void UnregisterPingReceiver(PingReceiver receiver)
    {
        receivers_.Remove(receiver);
    }

    public void CheckPulseCollisions(Vector2 pulseCenter, float pulseRadius, float pulseMaxRadius)
    {
        // Look at all ping receivers that are active, but not already detected.
        var activeReceivers = receivers_.Where(r => r.GetState() == PingReceiverState.Active);

        foreach (PingReceiver receiver in activeReceivers)
        {
            if (IsPointInCircle(receiver.GetPingPoint(), pulseCenter, pulseRadius))
            {
                receiver.Ping(pulseCenter, pulseRadius, pulseMaxRadius);
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
}
