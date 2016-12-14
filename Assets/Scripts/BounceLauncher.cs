using UnityEngine;
using HoloToolkit.Unity;

public class BounceLauncher : MonoBehaviour { 

    public float velocity = 100;
    
    public GameObject sphereRef;
    public GameObject ballPrefab;
    public GameObject cameraRef;
    
    private bool toggle = true; 
    private float timeSinceLastShot = 0f;
    private uint? currentHandID = null;

    void Start ()
    {

    }

    void Update()
    {

    }

#if UNITY_EDITOR || UNITY_WSA
    void LateUpdate ()
    {
        timeSinceLastShot += Time.deltaTime;
        HandsManager handManager = HandsManager.Instance;
        if (handManager.NumberOfTrackedHands == 1)
        {
            if (currentHandID == null || !handManager.IsHandTracked(currentHandID.Value))
            {
                currentHandID = handManager.GetBestHand();
            }
            //Logger.Log("1 hand tracking", this);

            sphereRef.SetActive(true);

            sphereRef.transform.position = handManager.GetHandLocation(currentHandID.Value);

            if (GestureManager.Instance.TapEvent)
            {
                LaunchBall(sphereRef.transform.position);
            }
        }
        else if (handManager.NumberOfTrackedHands == 2)
        {
            //Logger.Log("2 hands tracking", this);
            if (toggle)
            {
                toggle = false;
                SpatialMappingManager.Instance.DrawVisualMeshes ^= SpatialMappingManager.Instance.DrawVisualMeshes;
            }
            
        }
        else
        {
            //Logger.Log("Not tracking", this);
            toggle = true;

        }

        if (timeSinceLastShot < .5f)
        {
            sphereRef.SetActive(false);
        }
    }
#endif

    void LaunchBall(Vector3 hand)
    {
        Vector3 direction = Vector3.Normalize(hand - cameraRef.transform.position);
        direction *= velocity;

        GameObject newBall = (GameObject)Instantiate(ballPrefab, hand, cameraRef.transform.rotation);
        newBall.GetComponent<Rigidbody>().velocity = direction;
        timeSinceLastShot = 0;
    }
}
