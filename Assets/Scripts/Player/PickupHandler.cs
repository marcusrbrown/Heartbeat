using UnityEngine;

[RequireComponent(typeof(Heartbeat))]
public class PickupHandler : MonoBehaviour
{
    private Metagame metagame_;

    public void HandlePickup(Pickup pickup)
    {
        metagame_.IncrementPickupCount();
    }

    private void Awake()
    {
        if ((metagame_ = Metagame.GetInstance()) == null)
        {
            this.enabled = false;
            return;
        }
    }
}
