using UnityEngine;
#if UNITY_EDITOR || UNITY_WSA
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
#endif

public class Movable : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_WSA
    private Vector3 lastHandLocation = new Vector3();
    private uint? currentHandID = null;
    private GestureRecognizer gestureRecognizer;

    public GameObject cameraRef;

    void Start()
    {
        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new GestureRecognizer();

        gestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

        gestureRecognizer.ManipulationStartedEvent += OnManipulationStartedEvent;
        gestureRecognizer.ManipulationCompletedEvent += OnManipulationEndedEvent;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();
    }

    void Update()
    {
        if (currentHandID != null)
        {
            if (lastHandLocation != Vector3.zero)
            {
                Vector3 currentHandLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);

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

                //TODO: This doesn't work...
                //lock axis 
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                transform.position = transform.position * Vector3.Project(currentHandLocation, lastHandLocation).magnitude / lastHandLocation.magnitude;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(HandsManager.Instance.GetHandLocation(currentHandID.Value) - cameraRef.transform.position);
            }

            lastHandLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);

        }
        else
        {
            lastHandLocation = Vector3.zero;
        }
    }

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
