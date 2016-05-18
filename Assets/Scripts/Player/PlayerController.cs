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
        private PlayerAnimationController animationController;
        private PlayerVFXController vfxController;

        // Player health & scoring
        public int CurrentHealthPoints { get; private set; }
        public int CurrentCollectables { get; private set; }

        // Invulnerability state
        [Range(0,4)]
        public float PostDamageInvulnerability = 1.0f;
        private float invulnerabilityTimer = 0.0f;
        private bool isInvulnerable = false;

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
            this.CurrentHealthPoints = 2;
            this.CurrentCollectables = 0;

            // Get core player component  references
            this.inputController = this.GetComponent<PlayerInputController>();
            this.movementController = this.GetComponent<PlayerMovementController>();
            this.collisionController = this.GetComponent<PlayerCollisionController>();
            this.animationController = this.GetComponentInChildren<PlayerAnimationController>();
            this.vfxController = this.GetComponentInChildren<PlayerVFXController>();

            // Initialize player components
            this.animationController.Initialize();
            this.vfxController.Initialize(this.animationController.PlayerSpriteRenderer);
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

            // Unsubscribe collision events
            this.collisionController.CollectableCollided -= this.CollectableCollided;
            this.collisionController.EnemyCollided -= this.EnemyCollided;
        }

        /// <summary>
        /// Update is called once per frame by gamepresenter
        /// </summary>
        public void UpdatePlayer()
        {
            // Update player input given the current input key configuration
            this.inputController.UpdateInput(); 

            // Update invulnerability state
            this.UpdatePostImpactInvulnerability();
        }
        /// <summary>
        /// FixedUpdate is called once per physics frame 
        /// </summary>
        public void FixedPlayerUpdate()
        {
            // Forward player input to required scripts
            this.movementController.FixedUpdateMovement(this.inputController.InputInstance);
        }

        #region Special Statuses
        /// <summary>
        /// Update post impact invulnerability timer
        /// </summary>
        private void UpdatePostImpactInvulnerability()
        {
            if (this.isInvulnerable)
            {
                // Update timer
                this.invulnerabilityTimer += Time.deltaTime;
                // Check if we must disable invulnerabiliy now
                if (this.invulnerabilityTimer > this.PostDamageInvulnerability)
                {
                    // Reset timer and set flag
                    this.invulnerabilityTimer = 0;
                    this.isInvulnerable = false;

                    // Turn off vfx
                    this.vfxController.ManageInvulnerabilityVFX(false);
                }
            }
        }

        /// <summary>
        /// Sets the invulnerability flag and request vfx
        /// </summary>
        private void StartInvulnerability()
        {
            // Ignore if we were already invulnerable
            if (this.isInvulnerable)
                return;

            // Request vfx
            this.vfxController.ManageInvulnerabilityVFX(true);

            // Set flag
            this.isInvulnerable = true;
        }

        #endregion

        #region Score & Status Modifiers
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

        #endregion
        #region Core Player Event Callbacks
        /// <summary>
        /// Execute vfx and apply collectable reward
        /// </summary>
        private void CollectableCollided(CollectableController collectable)
        {
            // Apply collectable reward
            this.AddCollectable(collectable.CollectableValue);

            // Finally, execute report to the object itself that it has impacted a player
            collectable.PlayerImpact();
        }

        /// <summary>
        /// Execute vfx and apply damage
        /// </summary>
        private void EnemyCollided(EnemyController enemy)
        {
            // Ignore this call if the player is invulnerable
            if (this.isInvulnerable) return;

            // Request camera shake
            GamePresenter.Instance.CameraRigPresenter.ShakeMainCamera();

            // Apply damage
            this.RemoveLifePoint(enemy.DamageOnCollision);

            // Request post damage invulnerability if we didn't die
            if(this.CurrentHealthPoints >0)
                this.StartInvulnerability();

            // Finally, execute report to the object itself that it has impacted a player
            enemy.PlayerImpact();
        }

        /// <summary>
        /// Execute vfx and apply damage
        /// </summary>
        private void EnemyProjectileCollided(EnemyProjectileController projectile)
        {
            // Ignore this call if the player is invulnerable
            if (this.isInvulnerable) return;

            // Request camera shake
            GamePresenter.Instance.CameraRigPresenter.ShakeMainCamera();

            // Apply damage
            this.RemoveLifePoint(projectile.DamageOnCollision);

            // Request post damage invulnerability if we didn't die
            if (this.CurrentHealthPoints > 0)
                this.StartInvulnerability();

            // Finally, execute report to the object itself that it has impacted a player
            projectile.PlayerImpact();
        }
        #endregion
    }
}

