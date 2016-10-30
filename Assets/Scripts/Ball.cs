using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    public float lifeSpan = 5f;
    public float stoppedThreashold = .5f;
    float age = 0f;

    // Use this for initialization
    void Start()
    {
        Random.InitState(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (age > lifeSpan && GetComponent<Rigidbody>().velocity.magnitude < stoppedThreashold)
        {
            Destroy(this);
        }
    }
}
