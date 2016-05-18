namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Core enemy manager and hub
    /// </summary>
    public class EnemyController : GameplayElementController
    {
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

        // Attack control variables
        public float ColdownDuration = 1.0f;
        private bool inColdown = false;
        private float coldownTimer = 0;

        // Damage this enemy deals on collision
        public int DamageOnCollision { get; private set; }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(int damageOnCollision, int projectilesDamage,float projectilesSpeed)
        {
            // Set initial parameters
            this.DamageOnCollision = damageOnCollision;
            this.ProjectileSpeed = projectilesSpeed;
            this.ProjectilesDamage = projectilesDamage;

            // Reset firing coldown
            this.inColdown = false;
            this.coldownTimer = 0;
        }


        /// <summary>
        /// Call this method to update the enemy turret
        /// </summary>
        public void UpdateEnemy()
        {
            // Only update the turret if it's visible by any camera
            if (!this.BaseRenderer.isVisible)
                return;

            // Update attack coldown
            if (this.inColdown)
            {
                // Update timer
                this.coldownTimer += Time.deltaTime;
                // Check if coldown is over
                if (this.coldownTimer >= this.ColdownDuration)
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
                this.FireAtTarget(GamePresenter.Instance.PlayerPresenter.Player.transform.position);

                // Start firing coldown
                this.inColdown = true;
            }
        }

        /// <summary>
        /// Fire projectile at target position
        /// </summary>
        private void FireAtTarget(Vector3 targetPosition)
        {
            // Get projectile to launch
            EnemyProjectileController projectile =
                GamePresenter.Instance.ProjectilePresenter.ProjectilesPool.GetObject().GetComponent<EnemyProjectileController>();

            // Move projectile to origin position (deactivate before moving to avoid trail/particle artifacts)
            projectile.gameObject.SetActive(false);
            projectile.transform.position = this.ProjectileOrigin.position;
            projectile.gameObject.SetActive(true);

            // Initialize projectile and launch it ( the projectile presenter will handle its movement)
            projectile.Initialize(this.ProjectilesDamage,this.ProjectileSpeed,this.ProjectileLife,targetPosition);
        }
    }
}