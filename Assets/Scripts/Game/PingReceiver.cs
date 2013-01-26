using UnityEngine;

public class PingReceiver : MonoBehaviour
{
    protected virtual void OnPing()
    {
        Debug.Log(gameObject.name + ": Ping received!");
    }
}