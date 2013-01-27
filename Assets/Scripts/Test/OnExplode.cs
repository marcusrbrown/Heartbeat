using UnityEngine;
using System.Collections;

public class OnExplode : MonoBehaviour {

    public bool explodeCheck = false;
    public float x;
    public float y;
    public float z;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (explodeCheck == true)
        {
            transform.Translate(x, y, z);
        }

	}

    void Explode()
    {
        x = Random.Range(-0.5f, 0.5f);
        y = Random.Range(-0.5f, 0.5f);
        z = Random.Range(-0.5f, 0.5f);
        explodeCheck = true;
    }
}
