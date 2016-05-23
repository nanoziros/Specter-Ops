namespace SpecterOps
{
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Manages particular vfx instantiation
    /// </summary>
    public class VFXPresenter : MonoBehaviour
    {
        // Projectile impact pool
        public GenericPoolSystem ProjectilesImpactPool { get; private set; }
        public GameObject ProjectileImpactPrefab;
        public int PreloadedProjectilesImpact = 10;

        // Reward impact pool
        public GenericPoolSystem RewardImpactPool { get; private set; }
        public GameObject RewardImpactPrefab;
        public int PreloadedRewardImpact = 10;

        /// <summary>
        /// Use this to initialize the instance
        /// </summary>
        public void Initialize()
        {
            // Preload vfx pools
            this.ProjectilesImpactPool = new GenericPoolSystem(this.ProjectileImpactPrefab, this.PreloadedProjectilesImpact,
                this.transform);
            this.RewardImpactPool = new GenericPoolSystem(this.RewardImpactPrefab, this.PreloadedRewardImpact,
                this.transform);
        }
    }
}
