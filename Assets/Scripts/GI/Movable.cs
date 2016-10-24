using UnityEngine;
using System.Collections;

namespace GI {
    public class Movable : MonoBehaviour {

        private Vector3 lastLocation = new Vector3();
        private uint? currentHandID = null; 

        public GameObject cameraRef;
        GameObject objectToMove;

        void Update()
        {
            if (currentHandID != null)
            {
                if (lastLocation != Vector3.zero)
                {
                    Vector3 currentLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);

                    //normalize about camera
                    currentLocation -= cameraRef.transform.position;
                    lastLocation -= cameraRef.transform.position;

                    float angle = Vector3.Angle(lastLocation, currentLocation);

                    

                    
                    transform.Translate((HandsManager.Instance.GetHandLocation(currentHandID.Value) - lastLocation) * 2.5f);
                }

                lastLocation = HandsManager.Instance.GetHandLocation(currentHandID.Value);

            }
            else
            {
                lastLocation = Vector3.zero;
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
    }
}