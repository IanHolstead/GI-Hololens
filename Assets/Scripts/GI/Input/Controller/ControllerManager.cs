using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlWrapping;

namespace GI {
    //TODO: this should probably implement an interface or something.
    public class ControllerManager : Singleton<ControllerManager> {
        private const int maxNumberOfGamePads = 4;

        GamePadWrapper[] gamePads = new GamePadWrapper[4];
        GamePadWrapper.UpdateStateDel[] gamePadStateUpdaters = new GamePadWrapper.UpdateStateDel[4];

        private int numberOfGamepadsInUse = 0;

        // Use this for initialization
        void Awake() {
            
        }

        // Update is called once per frame
        void Update() {
            foreach (GamePadWrapper.UpdateStateDel updater in gamePadStateUpdaters)
            {
                if (updater != null)
                {
                    updater(Time.deltaTime);
                }
            }
        }

        public GamePadWrapper RequestGamepad()
        {
            if (numberOfGamepadsInUse >= maxNumberOfGamePads)
            {
                return null;
            }

            int id;

            for (id = 0; id < maxNumberOfGamePads; id++)
            {
                if (gamePads[id] == null)
                {
                    break;
                }
            }

            numberOfGamepadsInUse++;

            gamePads[id] = new GamePadWrapper(id);
            gamePadStateUpdaters[id] = gamePads[id].UpdateState;
            gamePadStateUpdaters[id](0f);

            return gamePads[id];
        }

        public void ReturnGamePad(int gamePadID)
        {
            if (gamePads[gamePadID] != null)
            {
                numberOfGamepadsInUse--;
                gamePads[gamePadID] = null;
                gamePadStateUpdaters[gamePadID] = null;
            }
        }
    }
}