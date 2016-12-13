using UnityEngine;
using System.Collections;
using XInputDotNetPure;

namespace ControlWrapping
{
    public class GamePadWrapper
    {
        private readonly XInputDotNetPure.PlayerIndex[] playerIndices = 
            { XInputDotNetPure.PlayerIndex.One, XInputDotNetPure.PlayerIndex.Two,
            XInputDotNetPure.PlayerIndex.Three, XInputDotNetPure.PlayerIndex.Four };

        private GamePadState currentState;
        private GamePadState oldState;
        private Button button;
        private Stick stick;
        private Trigger trigger;
        private ButtonPress buttonPress;

        private float triggerSensitivity = .85f;

        private int playerID;

        public delegate void UpdateStateDel(float tick);

        private float timeSinceVibration = 0;
        private float vibrationDuration = 0;

        public GamePadWrapper(int ID)
        {
            playerID = ID;
            button = new Button(this);
            stick = new Stick(this);
            trigger = new Trigger(this);
            buttonPress = new ButtonPress(this);
        }

        public void UpdateState(float tick)
        {
            timeSinceVibration += tick;
            CurrentState = GamePad.GetState(playerIndices[playerID]);
        }

        public void SetVibration(float leftMotor, float rightMotor, float duration = 0)
        {
            vibrationDuration = duration;
            timeSinceVibration = 0;
            GamePad.SetVibration(playerIndices[playerID], leftMotor, rightMotor);
        }

        public void EndVibration()
        {
            if (vibrationDuration != 0 && timeSinceVibration > vibrationDuration)
            {
                GamePad.SetVibration(playerIndices[playerID], 0, 0);
            }
        }

        public GamePadState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                oldState = currentState;
                currentState = value;
            }
        }

        public GamePadState OldState
        {
            get
            {
                return oldState;
            }
        }

        public float TriggerSensitivity
        {
            get
            {
                return triggerSensitivity;
            }
            set
            {
                triggerSensitivity = Mathf.Clamp(value, 0, 1);
            }
        }

        public Button Button
        {
            get
            {
                return button;
            }
        }
        public Stick Stick
        {
            get
            {
                return stick;
            }
        }
        public Trigger Trigger
        {
            get
            {
                return trigger;
            }
        }
        public ButtonPress ButtonPress
        {
            get
            {
                return buttonPress;
            }
        }
    }
}