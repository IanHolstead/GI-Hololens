using UnityEngine;
#if UNITY_EDITOR || UNITY_WSA
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
#endif

public class Movable : MonoBehaviour
{
    private bool useManualMove = false;
    private Vector3 manualLocation;

    private Vector3 lastHandLocation = new Vector3();
    private uint? currentHandID = null;
#if UNITY_EDITOR || UNITY_WSA
    private GestureRecognizer gestureRecognizer;
#endif
    public GameObject cameraRef;

    void Start()
    {
#if UNITY_EDITOR || UNITY_WSA
        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new GestureRecognizer();

        gestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

        gestureRecognizer.ManipulationStartedEvent += OnManipulationStartedEvent;
        gestureRecognizer.ManipulationCompletedEvent += OnManipulationEndedEvent;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();
#endif
    }

    void Update()
    {
        if (currentHandID != null || useManualMove)
        {
            if (lastHandLocation != Vector3.zero)
            {

                Vector3 currentHandLocation;
                if (!useManualMove)
                {
#if UNITY_EDITOR || UNITY_WSA
                    currentHandLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);
#else
                    Debug.LogError("Tried to move object with hand outside hololens");
                    return;
#endif
                }
                else
                {
                    currentHandLocation = manualLocation;
                }

                //normalize about camera
                currentHandLocation -= cameraRef.transform.position;
                lastHandLocation -= cameraRef.transform.position;

                if (currentHandLocation.magnitude / lastHandLocation.magnitude < .9 || currentHandLocation.magnitude / lastHandLocation.magnitude > 1.1)
                {
                    //this means the hand isn't tracking or something is up
                    return;
                }

                float angle = Vector3.Angle(lastHandLocation, currentHandLocation);

                transform.RotateAround(cameraRef.transform.position, Vector3.Cross(lastHandLocation, currentHandLocation), angle);

                //TODO: I don't think this works...
                //lock axis 
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                transform.position = transform.position * Vector3.Project(currentHandLocation, lastHandLocation).magnitude / lastHandLocation.magnitude;
            }
            else
            {
#if UNITY_EDITOR || UNITY_WSA
                transform.rotation = Quaternion.LookRotation(HandsManager.Instance.GetHandLocation(currentHandID.Value) - cameraRef.transform.position);
#endif
            }
#if UNITY_EDITOR || UNITY_WSA
            lastHandLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);
#else
            lastHandLocation = manualLocation;
#endif
        }
        else
        {
            lastHandLocation = Vector3.zero;
        }
    }

    public void StopManualMove()
    {
        manualLocation = Vector3.zero;
        useManualMove = false;
    }

    public void ManualMovement(Vector3 position)
    {
        manualLocation = position;
        useManualMove = true;
    }
#if UNITY_EDITOR || UNITY_WSA
    public void StartMoving(uint? handID = null)
    {
        if (handID == null || !HandsManager.Instance.IsHandTracked(handID.Value))
        {
            currentHandID = HandsManager.Instance.GetBestHand();
        }
    }

    public void FinishMoving()
    {
        currentHandID = null;
    }

    public bool UpdateTrackedHand(uint handID)
    {
        if (HandsManager.Instance.IsHandTracked(handID))
        {
            currentHandID = handID;
            return true;
        }
        return false;
    }

    void OnManipulationStartedEvent(InteractionSourceKind source, Vector3 translation, Ray headRay)
    {
        Logger.Log("Manipulation Started");
        if (GazeManager.Instance.HitInfo.collider.gameObject.GetComponent<Movable>() == this)
        {
            StartMoving();
        }
    }

    void OnManipulationEndedEvent(InteractionSourceKind source, Vector3 translation, Ray headRay)
    {
        Logger.Log("Manipulation Ended");
        FinishMoving();
    }
#endif
}
