using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    public float lifeSpan = 5f;
    public float stoppedThreashold = .5f;
    float age = 0f;
    
    void Start()
    {
        Random.InitState(0);
    }
    
    void Update()
    {
        age += Time.deltaTime;
        //TODO: Not sure this works
        if (age > lifeSpan && GetComponent<Rigidbody>().velocity.magnitude < stoppedThreashold)
        {
            Destroy(this);
        }
    }
}
