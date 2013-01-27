using UnityEngine;

public class Pickup : MonoBehaviour
{
public GameObject target;
public GameObject targetExploder;
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
			targetExploder.BroadcastMessage ("Explode");
            Destroy(this.gameObject);
        }
    }
}
