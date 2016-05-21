namespace SpecterOps
{
    using UnityEngine;

    /// <summary>
    /// Core enemy manager and hub
    /// </summary>
    public class EnemyController : GameplayElementController
    {
        // Enemy components
        private EnemyAnimationController animationController;

        // Render parameters
        [Header("Presentation")]
        public SpriteRenderer BaseRenderer;
       
        // Proyectile parameters
        [Header("Projectiles")]
        public EnemyProjectileController ProjectilePrefab;
        public float FireRange = 5.0f;
        public float ProjectileLife = 3.0f;
        public int ProjectilesDamage { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public Transform ProjectileOrigin;

        // Local projectile emission vfx
        public GameObject FireVfx;

        // Attack control variables
        private float coldownDuration;
        private bool inColdown = false;
        private bool chargingFire = false;
        private float coldownTimer = 0;

        // Damage this enemy deals on collision
        public int DamageOnCollision { get; private set; }

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(int damageOnCollision, int projectilesDamage,float projectilesSpeed, float firingColdown)
        {
            // Get class components
            this.animationController = this.GetComponentInChildren<EnemyAnimationController>();

            // Initialize components
            this.animationController.Initialize();

            // Set initial parameters
            this.DamageOnCollision = damageOnCollision;
            this.ProjectileSpeed = projectilesSpeed;
            this.ProjectilesDamage = projectilesDamage;
            this.coldownDuration = firingColdown;

            // Reset firing coldown
            this.inColdown = false;
            this.chargingFire = false;
            this.coldownTimer = 0;

            // Subscribe to proper animation events
            this.animationController.OnFireProjectileEvent += this.FireAtTarget;

            // Mark control flags
            this.isInitialized = true;
        }

        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on disable and on destroy (to avoid nullreference errors)
        /// </summary>
        private void OnDisable()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Remove initialization flag
            this.isInitialized = false;

            // Unsubscribe gameflow events
            this.animationController.OnFireProjectileEvent -= this.FireAtTarget;
        }
        public void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe collision events
            this.animationController.OnFireProjectileEvent -= this.FireAtTarget;
        }

        /// <summary>
        /// Method called on player impact
        /// </summary>
        public override void PlayerImpact()
        {
            base.PlayerImpact();

            // Play collision sfx
            GamePresenter.Instance.AudioPresenter.PlayEnemyCollisionSfx();
        }

        /// <summary>
        /// Call this method to update the enemy turret
        /// </summary>
        public void UpdateEnemy()
        {
            // Update head direction based on player position
            Vector3 relativePlayerPosition =
                this.transform.InverseTransformPoint(GamePresenter.Instance.PlayerPresenter.Player.transform.position);
            this.animationController.transform.localScale = new Vector3(relativePlayerPosition.x > 0 ? -1 : 1,
                this.animationController.transform.localScale.y, this.animationController.transform.localScale.z);

            // Only update the turret if it's visible by any camera
            // if the turrets are above the player position (otherwise the game is too hard!!)
            //  and if we aren't already charging a shot
            if (this.chargingFire || !this.BaseRenderer.isVisible || this.transform.position.y < GamePresenter.Instance.PlayerPresenter.Player.transform.position.y)
                return;

            // Update attack coldown
            if (this.inColdown)
            {
                // Update timer
                this.coldownTimer += Time.deltaTime;
                // Check if coldown is over
                if (this.coldownTimer >= this.coldownDuration)
                {
                    // Reset coldown variables
                    this.coldownTimer = 0.0f;
                    this.inColdown = false;
                }
            }
            
            //  Check attack coldown and firing range 
            if (!this.inColdown &&
                Vector2.Distance(transform.position, GamePresenter.Instance.PlayerPresenter.Player.transform.position) <= this.FireRange)
            {
                // Fire at target position
                this.RequestFireAtTarget();
            }
        }

        /// <summary>
        /// Request Fire projectile at target position
        /// </summary>
        private void RequestFireAtTarget()
        {
            // Request fire projectile animation
            this.animationController.RequestFireProjectileAnimation();

            // Play generic charging shoot sfx
            GamePresenter.Instance.AudioPresenter.PlayChargeShootSfx();

            // Set control flag so we can't shoot again while waiting for this shoot
            this.chargingFire = true;
        }

        /// <summary>
        /// Fire at saved target position, this method is called only when te enemy is in the fire projectile frame (Requested by its EnemyAnimationController)
        /// This guarantees a sync between animation and gameplay!
        /// </summary>
        private void FireAtTarget()
        {
            EnemyProjectileController projectile =
                GamePresenter.Instance.ProjectilePresenter.ProjectilesPool.GetObject()
                    .GetComponent<EnemyProjectileController>();

            // Move projectile to origin position (deactivate before moving to avoid trail/particle artifacts)
            projectile.gameObject.SetActive(false);
            projectile.transform.position = this.ProjectileOrigin.position;
            projectile.gameObject.SetActive(true);

            // Play generic shoot sfx
            GamePresenter.Instance.AudioPresenter.PlayShootSfx();

            // Play fire vfx
            if(this.FireVfx != null)
                this.FireVfx.SetActive(true);

            // Initialize projectile and launch it ( the projectile presenter will handle its movement)
            projectile.Initialize(this.ProjectilesDamage, this.ProjectileSpeed, this.ProjectileLife,
                GamePresenter.Instance.PlayerPresenter.Player.transform.position);

            // Set control flag
            this.chargingFire = false;

            // Start firing coldown
            this.inColdown = true;
        }
    }
}