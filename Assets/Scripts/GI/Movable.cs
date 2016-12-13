using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;

namespace GI {
    public class Movable : MonoBehaviour {

        private Vector3 lastHandLocation = new Vector3();
        private uint? currentHandID = null;
        private GestureRecognizer gestureRecognizer;
        //private float time;

        public GameObject cameraRef;

        void Start()
        {
            // Create a new GestureRecognizer. Sign up for tapped events.
            gestureRecognizer = new GestureRecognizer();
            //gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
            gestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
            //gestureRecognizer.TappedEvent += OnTappedEvent;
            //gestureRecognizer.HoldStartedEvent += OnHoldStartedEvent;
            gestureRecognizer.ManipulationStartedEvent += OnManipulationStartedEvent;
            //gestureRecognizer.RecognitionStartedEvent += OnRecognitionStartedEvent;
            //gestureRecognizer.ManipulationUpdatedEvent += OnManipulationUpdatedEvent;
            gestureRecognizer.ManipulationCompletedEvent += OnManipulationEndedEvent;

            // Start looking for gestures.
            gestureRecognizer.StartCapturingGestures();
        }

        void Update()
        {
            //time += Time.deltaTime;

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
                    //transform.RotateAround(cameraRef.transform.position, Vector3.up, angle);
                    //lock axis
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                    transform.position = transform.position * Vector3.Project(currentHandLocation, lastHandLocation).magnitude / lastHandLocation.magnitude;
                    //transform.position = transform.position * currentHandLocation.magnitude / lastHandLocation.magnitude;

                    Debug.Log(currentHandLocation.magnitude / lastHandLocation.magnitude);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(HandsManager.Instance.GetHandLocation(currentHandID.Value) - cameraRef.transform.position);
                }

                lastHandLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);// - cameraRef.transform.position;

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

        //void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        //{
        //    Logger.Log("OnTap + " + tapCount);
        //}

        void OnManipulationStartedEvent(InteractionSourceKind source, Vector3 translation, Ray headRay)
        {
            Logger.Log("Manipulation Started");
            if (GazeManager.Instance.HitInfo.collider.gameObject.GetComponent<Movable>() == this)
            {
                StartMoving();
            }
        }

        //void OnManipulationUpdatedEvent(InteractionSourceKind source, Vector3 translation, Ray headRay)
        //{
        //    //Logger.Log("OnManipulation " + translation);
        //}

        void OnManipulationEndedEvent(InteractionSourceKind source, Vector3 translation, Ray headRay)
        {
            Logger.Log("Manipulation Ended");
            FinishMoving();
        }

        //void OnHoldStartedEvent(InteractionSourceKind source, Ray headRay)
        //{
        //    //Logger.Log("OnHold " + time);
        //}

        //void OnRecognitionStartedEvent(InteractionSourceKind source, Ray headRay)
        //{
        //    //Logger.Log("OnRecognition");
        //    time = 0f;
        //}
    }
}