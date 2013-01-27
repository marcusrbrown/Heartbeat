using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Metagame : MonoBehaviour
{
    public GameObject player;
    public GameObject enemies;
    public GameObject items;
    public Transform playerSpawn;
    public float respawnDuration = 3.5f;

    private Heartbeat heartbeat_;
    private GameObject blip_;
    private LinkedList<PingReceiver> receivers_ = new LinkedList<PingReceiver>();
    private Vector3 playerSpawnPosition_;
    private Quaternion playerSpawnRotation_;

    private int pickupCount_;
    private float respawnTimer_;
    private bool respawning_;

    private static Metagame instance_;

    internal GameObject Blip
    {
        get { return blip_; }
    }

    public void RegisterPingReceiver(PingReceiver receiver)
    {
        receivers_.AddLast(receiver);
    }

    public void UnregisterPingReceiver(PingReceiver receiver)
    {
        receivers_.Remove(receiver);
    }

    public bool IsRespawning()
    {
        return respawning_;
    }

    public void CheckPulseCollisions(Heartbeat.Pulse pulse)
    {
        // Look at all ping receivers that are active, but not already detected.
        var activeReceivers = receivers_.Where(r => r.GetState() == PingReceiverState.Active);

        foreach (PingReceiver receiver in activeReceivers)
        {
            Rect receiverRect = receiver.GetPingRect();
            Vector2 receiverCenter = receiver.GetPingPoint();

            // Skip enemies that are inside of the player's visible circle.
            if (IsRectInCircle(receiverRect, heartbeat_.GetCenter(), heartbeat_.visbleAreaRadius)
                || IsPointInCircle(receiverCenter, heartbeat_.GetCenter(), heartbeat_.visbleAreaRadius))
            {
                continue;
            }

            if (IsRectInCircle(receiverRect, pulse.Center, pulse.Radius)
                || IsPointInCircle(receiverCenter, pulse.Center, pulse.Radius))
            {
                receiver.Ping(pulse);
            }
        }
    }

    public void IncrementPickupCount()
    {
        pickupCount_++;
    }

    public void CheckPlayerCollsion(Collider other)
    {
        if ((other.tag == "Enemy") && !respawning_)
        {
            this.player.BroadcastMessage("Explode");
            respawnTimer_ = 0.0f;
            respawning_ = true;
        }
    }

    public static Metagame GetInstance()
    {
        if (instance_ == null)
        {
            GameObject metagameObject = GameObject.FindGameObjectWithTag("Metagame");

            if ((metagameObject == null) || ((instance_ = metagameObject.GetComponent<Metagame>()) == null))
            {
                Debug.LogError("Couldn't locate a GO with the \"Metagame\" tag or it's missing the Metagame script.");
                return null;
            }
        }

        return instance_;
    }

    private void Awake()
    {
        if (this.player == null)
        {
            Debug.LogError("Attach the Player GameObject to the Metagame component.");
            this.enabled = false;
            return;
        }

        if (this.playerSpawn == null)
        {
            playerSpawnPosition_ = this.player.transform.position;
            playerSpawnRotation_ = this.player.transform.rotation;
        }
        else
        {
            playerSpawnPosition_ = this.playerSpawn.position;
            playerSpawnRotation_ = this.playerSpawn.rotation;
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

        // Load the Blip object used for displaying pings on the radar.
        blip_ = Resources.Load("Blip") as GameObject;

        if (blip_ == null)
        {
            Debug.LogWarning("Couldn't load the Blip resource.");
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (respawning_)
        {
            respawnTimer_ += deltaTime;

            if (respawnTimer_ >= this.respawnDuration)
            {
                respawning_ = false;

                this.player.BroadcastMessage("UnExplode");

                //this.player = Instantiate(oldPlayer, playerSpawnPosition_, playerSpawnRotation_) as GameObject;
                this.player.active = true;
            }
        }
    }

    private static bool IsPointInCircle(Vector2 testPoint, Vector2 center, float radius)
    {
        float xDistance = testPoint.x - center.x;
        float yDistance = testPoint.y - center.y;
        float distanceSq = (xDistance * xDistance) + (yDistance * yDistance);

        return distanceSq < (radius * radius);
    }

    private static bool IsRectInCircle(Rect rect, Vector2 center, float radius)
    {
        Vector3 circleDistance = new Vector2(Mathf.Abs(center.x - rect.x), Mathf.Abs(center.y - rect.y));

        if (circleDistance.x > ((rect.width / 2) + radius))
        {
            return false;
        }

        if (circleDistance.y > ((rect.height / 2) + radius))
        {
            return false;
        }

        if (circleDistance.x <= (rect.width / 2))
        {
            return true;
        }

        if (circleDistance.y <= (rect.width / 2))
        {
            return true;
        }

        float cornerDistanceSq = Mathf.Pow(circleDistance.x - (rect.width / 2), 2)
                                           + Mathf.Pow(circleDistance.y - (rect.height / 2), 2);

        return cornerDistanceSq < (radius * radius);
    }
}
