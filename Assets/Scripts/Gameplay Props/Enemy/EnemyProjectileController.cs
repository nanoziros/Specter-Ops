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

        /// <summary>
        /// Use this for initialization
        /// </summary>
        /// <param name="damage"></param>
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
            GameObject impactVfx= GamePresenter.Instance.ProjectilePresenter.ProjectilesImpactPool.GetObject();
            impactVfx.transform.position = this.transform.position;

            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }
}
