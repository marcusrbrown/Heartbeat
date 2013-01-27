using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickupHandler handler = other.gameObject.GetComponent<PickupHandler>();

            if (handler != null)
            {
                handler.HandlePickup(this);
            }

            Destroy(this.gameObject);
        }
    }
}
