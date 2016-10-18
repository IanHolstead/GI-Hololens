using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    public float initialDelay = 1f;
    public float stoppedThreashold = .5f;
    float age = 0f;

    //Random random;
    Vector3 initialLocation;
    public float range = 3;

	// Use this for initialization
	void Start () {
        Random.InitState(0);
        initialLocation = FindObjectOfType<Camera>().transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        age += Time.deltaTime;
        //temp();
        if (age > initialDelay && GetComponent<Rigidbody>().velocity.magnitude < stoppedThreashold)
        {
            Destroy(this);
        }
    }

    void temp()
    {
        if (age > initialDelay)
        {
            age = 0;
            transform.position = initialLocation + GetRandVector3();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    Vector3 GetRandVector3()
    {
        return new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
    }
}
