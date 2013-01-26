using UnityEngine;
using System.Collections;

public class Heartbeat : MonoBehaviour
{
    public float interval = 8.0f;

    private Metagame metagame_;
    private bool disabled_;

    public void SetMetagame(Metagame metagame)
    {
        metagame_ = metagame;
    }

	private void Start()
    {
        StartCoroutine(Ping(interval));
	}

    // MRBrown@PM 1/25/2013: TODO: Support a paused state.
    private IEnumerator Ping(float seconds)
    {
        while (!disabled_)
        {
            yield return new WaitForSeconds(seconds);

            Debug.Log("Ping!");

            // Ping all enemies and items.
            if (metagame_.enemies != null)
            {
                metagame_.enemies.SendMessage("OnPing", this);
            }

            if (metagame_.items != null)
            {
                metagame_.items.SendMessage("OnPing", this);
            }
        }
    }
	
	private void Update()
    {
	}
}
