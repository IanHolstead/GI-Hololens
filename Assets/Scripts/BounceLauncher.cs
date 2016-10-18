using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;

public class BounceLauncher : MonoBehaviour { 

    public float velocity = 100;
    // Use this for initialization

    public GameObject cameraRef;
    public GameObject sphereRef;
    public GameObject ballPrefab;
     
    public GameObject temp;
    public GameObject temp2;

    private Vector3 lastLocation = new Vector3();
    private bool toggle = true;
    private float timeSinceLastShot = 0f;

    void Start () {

    }

    // Update is called once per frame
    void Update ()
    {
        timeSinceLastShot += Time.deltaTime;
        GI.HandsManager handManager = GI.HandsManager.Instance;
        if (handManager.NumberOfTrackedHands == 1)
        {
            sphereRef.SetActive(true);

            sphereRef.transform.position = handManager.GetHandLocation(handManager.GetBestHand());
            temp.SetActive(true);
            temp2.SetActive(false); 
            if (lastLocation != Vector3.zero)
            {
                //This needs to consider the distance from the camera (further distance means it will have to travel further)
                temp.transform.Translate((handManager.GetHandLocation(handManager.GetBestHand()) - lastLocation) * 2.5f);
            }
            else
            {
                temp.transform.position = cameraRef.transform.position + cameraRef.transform.forward * 1.5f;
            }
            lastLocation = handManager.GetHandLocation(handManager.GetBestHand());
            if (handManager.NumberOfTrackedHands == 3)
            {
                temp2.SetActive(true);
            }

        }
        else
        {
            lastLocation = new Vector3();
            temp.SetActive(false);
            //temp2.SetActive(false);
        }
        if (handManager.NumberOfTrackedHands == 2 && toggle)
        {
            toggle = false;
            GI.SpatialMappingManager.Instance.drawVisualMeshes ^= GI.SpatialMappingManager.Instance.drawVisualMeshes;
        }
        else
        {
            toggle = true;
        }
        if (timeSinceLastShot < .5f)
        {
            sphereRef.SetActive(false);
        }
        
        //System.Diagnostics.Debug.WriteLine(sphereRef.transform);
    }

    void LaunchBall(Vector3 hand)
    {
        Vector3 direction = Vector3.Normalize(hand - cameraRef.transform.position);
        direction *= velocity;

        GameObject newBall = (GameObject)Instantiate(ballPrefab, hand, cameraRef.transform.rotation);
        newBall.GetComponent<Rigidbody>().velocity = direction;

    }
}
