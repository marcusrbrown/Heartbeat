using UnityEngine;

public class PingReceiver : MonoBehaviour
{
    private Metagame metagame_;

    public Vector2 GetPingPoint()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }

    public void Ping()
    {
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
    }

    private void Start()
    {
        // Register ourselves with the metagame as a ping receiver.
        metagame_.RegisterPingReceiver(this);
    }

    private void OnDestroy()
    {
        // Unregister ourselves as a ping receiver.
        metagame_.UnregisterPingReceiver(this);
    }
}