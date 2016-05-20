namespace SpecterOps
{
    using UnityEngine;

    public class EnemyProjectileController : GameplayElementController
    {
        // Damage this enemy deals on collision
        public int DamageOnCollision { get; private set; }
        public float ProjectileSpeed { get; private set; }

        // Base projectile information
        private Vector3 directionVector;
        private float lifeTime;

        // Projectile vfx
        public ParticleSystem ParticleSystem;

        // Control variables
        private bool isInitialized = false;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(int damage,float speed,float life, Vector3 destination)
        {
            // Set initial parameters
            this.DamageOnCollision = damage;
            this.ProjectileSpeed = speed;
            this.lifeTime = life;
            
            // Calculate directional vector
            this.directionVector = destination - this.transform.position;

            // Firing vector debug
            // todo: remove
            Debug.DrawLine(this.transform.position, this.transform.position + this.directionVector, Color.red,2.0f);

            // Subscribe to game pause events (in order to pause particle system)
            GamePresenter.Instance.GamePaused += this.PauseProjectile;
            GamePresenter.Instance.GameResumed += this.UnpauseProjectle;

            // Set control variable
            this.isInitialized = true;
        }

        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on disable and on destroy
        /// </summary>
        private void OnDisable()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Remove initialization flag
            this.isInitialized = false;

            // Unsubscribe gameflow events
            GamePresenter.Instance.GamePaused -= this.PauseProjectile;
            GamePresenter.Instance.GameResumed -= this.UnpauseProjectle;
        }
        private void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe gameflow events
            GamePresenter.Instance.GamePaused -= this.PauseProjectile;
            GamePresenter.Instance.GameResumed -= this.UnpauseProjectle;
        }

        /// <summary>
        /// Update projectile lifetime and movement
        /// </summary>
        public override void UpdateObject()
        {
            base.UpdateObject();

            // Update movement
            this.transform.position += (this.directionVector.normalized*this.ProjectileSpeed) *
                                        Time.deltaTime;

            // Update lifetime
            this.lifeTime -= Time.deltaTime;

            // Deactivate projectile if its lifetime is done
            if (this.lifeTime <=0)
                this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Executed on player impact
        /// </summary>
        public override void PlayerImpact()
        {
            base.PlayerImpact();

            // Play impact sfx
            GamePresenter.Instance.AudioPresenter.PlayShootImpactSfx();

            // Play impact vfx
            GameObject impactVfx= GamePresenter.Instance.VfxPresenter.ProjectilesImpactPool.GetObject();
            impactVfx.transform.position = this.transform.position;

            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }


        /// <summary>
        /// Pause projectile's vfx
        /// </summary>
        public void PauseProjectile()
        {
            this.ParticleSystem.Pause();
        }
        /// <summary>
        /// Resume projectile's vfx
        /// </summary>
        public void UnpauseProjectle()
        {
            this.ParticleSystem.Play();
        }
    }
}
