using UnityEngine;

public class Metagame : MonoBehaviour
{
    public GameObject player;
    public GameObject enemies;
    public GameObject items;

    private Heartbeat heartbeat_;

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
            this.enabled = false;
            return;
        }

        if (this.items == null)
        {
            Debug.LogWarning("Attach a GameObject containing items to the Metagame component.");
        }

        heartbeat_ = this.player.GetComponent<Heartbeat>();
        heartbeat_.SetMetagame(this);
    }
}