using UnityEngine;

public class Pickup : MonoBehaviour
{
public GameObject target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickupHandler handler = other.gameObject.GetComponent<PickupHandler>();

            if (handler != null)
            {
                handler.HandlePickup(this);
            }
			target.BroadcastMessage("yay");
            Destroy(this.gameObject);
        }
    }
}
