namespace SpecterOps.Player
{
    using UnityEngine;

    /// <summary>
    /// Core player manager and hub
    /// </summary>
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerController : MonoBehaviour {

        // Core player components
        private PlayerInputController inputController;
        private PlayerMovementController movementController;

        // Player health & scoring
        public int CurrentHealthPoints { get; private set; }
        public int CurrentCollectables { get; private set; }

        // Player parameters
        public float PlayerSpeed { get { return this.movementController.MovementSpeed; } }

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize () {

            // Set player gameobject name
            this.transform.name = this.transform.name.Replace("(Clone)","");

            // Set initial health and collectables
            // todo:
            this.CurrentHealthPoints = 1;
            this.CurrentCollectables = 0;

            // Get core player component  references
            this.inputController = this.GetComponent<PlayerInputController>();
            this.movementController = this.GetComponent<PlayerMovementController>();

            // Initialize player components
            this.inputController.Initialize();
            this.movementController.Initialize(GamePresenter.Instance.CameraRigPresenter.BoundariesCamera);

            // Mark control flags
            this.isInitialized = true;
        }

        /// <summary>
        /// Update is called once per frame 
        /// </summary>
        public void UpdatePlayer()
        {
            // Update player input given the current input key configuration
            this.inputController.UpdateInput(); 
        }

        /// <summary>
        /// Update is called once per physics frame 
        /// </summary>
        public void FixedPlayerUpdate()
        {
            // Forward player input to required scripts
            this.movementController.FixedUpdateMovement(this.inputController.InputInstance);
        }

        /// <summary>
        /// Called when the player earns a new collectable
        /// </summary>
        public void AddCollectable(int collectableAmount)
        {
            // Add new collectables
            this.CurrentCollectables += collectableAmount;
        }

        /// <summary>
        /// Called when the player loses a set amount of life points
        /// </summary>
        public void RemoveLifePoint(int damage)
        {
            // Do damage but clamp it at zero
            this.CurrentHealthPoints = Mathf.Clamp(this.CurrentHealthPoints - damage, 0, this.CurrentHealthPoints);
        }
    }
}

