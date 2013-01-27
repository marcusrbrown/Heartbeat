using UnityEngine;
using System.Collections;

public class OnExplode : MonoBehaviour {

    public bool explodeCheck = false;
    public float x;
    public float y;
    public float z;

    private float oldX_;
    private float oldY_;
    private float oldZ_;

    void Awake()
    {
    }

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
        oldX_ = transform.localPosition.x;
        oldY_ = transform.localPosition.y;
        oldZ_ = transform.localPosition.z;
        x = Random.Range(-0.5f, 0.5f);
        y = Random.Range(-0.5f, 0.5f);
        z = Random.Range(-0.5f, 0.5f);
        explodeCheck = true;
    }

    void UnExplode()
    {
        explodeCheck = false;
        transform.localPosition = new Vector3(oldX_, oldY_, oldZ_);
    }
}
