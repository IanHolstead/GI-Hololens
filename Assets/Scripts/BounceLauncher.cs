using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using GI;

public class BounceLauncher : MonoBehaviour { 

    public float velocity = 100;
    
    public GameObject sphereRef;
    public GameObject ballPrefab;
     
    public GameObject temp;
    
    private bool toggle = true; 
    private float timeSinceLastShot = 0f;
    private uint? currentHandID = null;
    private Movable movable;

    void Start () {
        movable = GetComponent<Movable>();
    }

    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LaunchBall(new Vector3(0, 0, 1));
        }
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        timeSinceLastShot += Time.deltaTime;
        GI.HandsManager handManager = GI.HandsManager.Instance;
        if (handManager.NumberOfTrackedHands == 1)
        {
            movable.StartMoving();

            sphereRef.SetActive(true);

            sphereRef.transform.position = handManager.GetHandLocation(currentHandID.Value);

            if (GI.GestureManager.Instance.TapEvent)
            {
                LaunchBall(sphereRef.transform.position);
            }
            temp.SetActive(true);

            //if (lastLocation != Vector3.zero)
            //{
            //    //This needs to consider the distance from the camera (further distance means it will have to travel further)
            //    temp.transform.Translate((handManager.GetHandLocation(handManager.GetBestHand()) - lastLocation) * 2.5f);
            //}
            //else
            //{
            //    temp.transform.position = cameraRef.transform.position + cameraRef.transform.forward * 1.5f;
            //}
            //lastLocation = handManager.GetHandLocation(handManager.GetBestHand());
        }
        else
        {
            temp.SetActive(false);
            //lastLocation = new Vector3();
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
        Vector3 direction = Vector3.Normalize(hand - movable.cameraRef.transform.position);
        direction *= velocity;

        GameObject newBall = (GameObject)Instantiate(ballPrefab, hand, movable.cameraRef.transform.rotation);
        newBall.GetComponent<Rigidbody>().velocity = direction;
        timeSinceLastShot = 0;
    }
}
