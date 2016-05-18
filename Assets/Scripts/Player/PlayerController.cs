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
        private PlayerCollisionController collisionController;

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
            // todo: get initial health points from scriptable object
            this.CurrentHealthPoints = 1;
            this.CurrentCollectables = 0;

            // Get core player component  references
            this.inputController = this.GetComponent<PlayerInputController>();
            this.movementController = this.GetComponent<PlayerMovementController>();
            this.collisionController = this.GetComponent<PlayerCollisionController>();

            // Initialize player components
            this.inputController.Initialize();
            this.movementController.Initialize(GamePresenter.Instance.CameraRigPresenter.BoundariesCamera);

            // Subscribe to collision events
            this.collisionController.CollectableCollided     += this.CollectableCollided;
            this.collisionController.EnemyCollided           += this.EnemyCollided;
            this.collisionController.EnemyProjectileCollided += this.EnemyProjectileCollided;

            // Mark control flags
            this.isInitialized = true;
        }

        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on destroy (to avoid nullreference errors)
        /// </summary>
        public void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe to collision events
            this.collisionController.CollectableCollided -= this.CollectableCollided;
            this.collisionController.EnemyCollided -= this.EnemyCollided;
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
        private void AddCollectable(int collectableAmount)
        {
            // Add new collectables
            this.CurrentCollectables += collectableAmount;
        }

        /// <summary>
        /// Called when the player loses a set amount of life points
        /// </summary>
        private void RemoveLifePoint(int damage)
        {
            // Do damage but clamp it at zero
            this.CurrentHealthPoints = Mathf.Clamp(this.CurrentHealthPoints - damage, 0, this.CurrentHealthPoints);
        }

        #region Core Player Event Callbacks
        /// <summary>
        /// Execute vfx and apply collectable reward
        /// </summary>
        private void CollectableCollided(CollectableController collectable)
        {
            // Apply collectable reward
            this.AddCollectable(collectable.CollectableValue);
        }

        /// <summary>
        /// Execute vfx and apply damage
        /// </summary>
        private void EnemyCollided(EnemyController enemy)
        {
            // Request camera shake
            GamePresenter.Instance.CameraRigPresenter.ShakeMainCamera();

            // Apply damage
            this.RemoveLifePoint(enemy.DamageOnCollision);
        }

        /// <summary>
        /// Execute vfx and apply damage
        /// </summary>
        private void EnemyProjectileCollided(EnemyProjectileController projectile)
        {
            // Request camera shake
            GamePresenter.Instance.CameraRigPresenter.ShakeMainCamera();

            // Apply damage
            this.RemoveLifePoint(projectile.DamageOnCollision);
        }
        #endregion
    }
}

