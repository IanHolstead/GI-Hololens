// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine.VR.WSA.Input;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// HandsDetected determines if the hand is currently detected or not.
    /// </summary>
    public partial class HandsManager : Singleton<HandsManager>
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }
        //TODO: New way of doing things, Remove or update
        //private HashSet<uint> trackedHands = new HashSet<uint>();

        public int NumberOfTrackedHands
        {
            get { return trackedHands.Count; }
        }

        private Dictionary<uint, InteractionSourceState> trackedHands = new Dictionary<uint, InteractionSourceState>();

        void Awake()
        {
            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
            InteractionManager.SourceLost += InteractionManager_SourceLost;
        }

        private void InteractionManager_SourceDetected(InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            trackedHands.Add(state.source.id, state);
        }

        private void InteractionManager_SourceUpdated(InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            trackedHands[state.source.id] = state;
        }

        private void InteractionManager_SourceLost(InteractionSourceState state)
        {
            // Check to see that the source is a hand.
            if (state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            //This really shouldn't be required
            if (trackedHands.ContainsKey(state.source.id))
            {
                trackedHands.Remove(state.source.id);
            }
        }

        /// <summary>
        /// Gets hand which is least likely to be lost and returns its ID. 
        /// If no hands are tracked it returns uint.MaxValue
        /// </summary>
        /// <returns></returns>
        public uint GetBestHand()
        {
            InteractionSourceState? state = null;
            foreach (KeyValuePair<uint, InteractionSourceState> hand in trackedHands)
            {
                if (state == null)
                {
                    state = hand.Value;
                }
                else if(hand.Value.properties.sourceLossRisk < state.Value.properties.sourceLossRisk)
                {
                    state = hand.Value;
                }
            }
            if (state == null)
            {
                //this feels a little gross
                return uint.MaxValue;
            }
            return state.Value.source.id;
        }

        public UnityEngine.Vector3 GetHandLocation(uint handID)
        {
            if (!IsHandTracked(handID))
            {
                return new UnityEngine.Vector3();
            }
            InteractionSourceState state = trackedHands[handID];
            UnityEngine.Vector3 location;

            state.properties.location.TryGetPosition(out location);

            return location;
        }

        public bool IsHandTracked(uint handID)
        {
            if (!trackedHands.ContainsKey(handID))
            {
                return false;
            }
            return true;
        }

        public bool IsHandTapping(uint handID)
        {
            if (IsHandTracked(handID))
            {
                InteractionSourceState state = trackedHands[handID];
                return state.pressed;
            }
            return false;
        }

        void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;
        }
    }
}