namespace SpecterOps.Player
{
    using UnityEngine;

    /// <summary>
    /// Listens to the player input
    /// </summary>
    public class PlayerInputController : MonoBehaviour
    {
        // Player configuration
        public InputInstance InputInstance { get; private set; }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize()
        {
            // Initialize input class
            this.InputInstance = new InputInstance();
        }

        /// <summary>
        /// Use this to update the input instance
        /// </summary>
        public void UpdateInput()
        {
            // Update input
            this.InputInstance.Update(GamePresenter.Instance.DataPresenter.GamePrefs.inputConfig);
        }
    }

    /// <summary>
    /// This class represents the player input at a given moment
    /// </summary>
    public class InputInstance
    {
        // Current input
        public bool PauseGame;
        public float MoveForward;
        public float StrafeRight;

        /// <summary>
        /// Update this input instance
        /// </summary>
        /// <param name="config">Input configuration for this frame update</param>
        public void Update(InputConfiguration config)
        {
            // Update game flow input
            this.PauseGame = Input.GetKeyDown(config.PauseRequest);

            // Update vertical movement input (1 = forward, -1 = backwards)
            this.MoveForward = Input.GetKey(config.MoveForward) || Input.GetKey(config.AltMoveForward) ? 1 : 0;
            this.MoveForward = Input.GetKey(config.MoveBack) || Input.GetKey(config.AltMoveBack) ? -1 : this.MoveForward;

            // Update horizontal movement input (1 = right, -1 = left)
            this.StrafeRight = Input.GetKey(config.StrafeRight) || Input.GetKey(config.AltStrafeRight) ? 1 : 0;
            this.StrafeRight = Input.GetKey(config.StrafeLeft) || Input.GetKey(config.AltStrafeLeft) ? -1 : this.StrafeRight;
        }

        /// <summary>
        /// Input key configuration setup
        /// </summary>
        [System.Serializable]
        public class InputConfiguration
        {
            [Header("Game Flow")]
            public KeyCode PauseRequest = KeyCode.Escape;

            [Header("Directional Movement")]
            public KeyCode MoveForward = KeyCode.W;
            public KeyCode MoveBack = KeyCode.S;
            public KeyCode StrafeLeft = KeyCode.A;
            public KeyCode StrafeRight = KeyCode.D;

            [Header("Alternate Directional Movement")]
            public KeyCode AltMoveForward = KeyCode.UpArrow;
            public KeyCode AltMoveBack = KeyCode.DownArrow;
            public KeyCode AltStrafeLeft = KeyCode.LeftArrow;
            public KeyCode AltStrafeRight = KeyCode.RightArrow;
        }
    }
}