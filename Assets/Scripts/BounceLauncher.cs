using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using GI;

public class BounceLauncher : MonoBehaviour { 

    public float velocity = 100;
    
    public GameObject sphereRef;
    public GameObject ballPrefab;
    public GameObject cameraRef;
    
    private bool toggle = true; 
    private float timeSinceLastShot = 0f;
    private uint? currentHandID = null;

    void Start () {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Logger.Log("Press");
        }
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        timeSinceLastShot += Time.deltaTime;
        GI.HandsManager handManager = GI.HandsManager.Instance;
        if (handManager.NumberOfTrackedHands == 1)
        {
            if (currentHandID == null || !handManager.IsHandTracked(currentHandID.Value))
            {
                currentHandID = handManager.GetBestHand();
            }
            //Logger.Log("1 hand tracking", this);

            sphereRef.SetActive(true);

            sphereRef.transform.position = handManager.GetHandLocation(currentHandID.Value);

            if (GI.GestureManager.Instance.TapEvent)
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

    void LaunchBall(Vector3 hand)
    {
        Vector3 direction = Vector3.Normalize(hand - cameraRef.transform.position);
        direction *= velocity;

        GameObject newBall = (GameObject)Instantiate(ballPrefab, hand, cameraRef.transform.rotation);
        newBall.GetComponent<Rigidbody>().velocity = direction;
        timeSinceLastShot = 0;
    }
}
