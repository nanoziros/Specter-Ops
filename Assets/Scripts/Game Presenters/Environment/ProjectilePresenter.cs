namespace SpecterOps
{
    using UnityEngine;
    using Utilities;
    using System.Collections.Generic;

    /// <summary>
    /// Projectile updater: this was made to centralize projectile updates so they can be easily frozen in plaze when the game is paused
    /// This acts as a pool manager
    /// </summary>
    public class ProjectilePresenter : MonoBehaviour
    {
        // Core projectile pool 
        public GenericPoolSystem ProjectilesPool { get; private set; }
        public EnemyProjectileController ProjectilePrefab;
        public int PreloadedProjectiles = 30;

        // Control parameters
        private List<GameplayElementController> gameplayProjectilesPool = new List<GameplayElementController>();

        /// <summary>
        /// Use this to initialize the instance
        /// </summary>
        public void Initialize()
        {
            // Preload initial projectiles
            this.ProjectilesPool = new GenericPoolSystem(this.ProjectilePrefab.gameObject, this.PreloadedProjectiles,
                this.transform);
        }

        /// <summary>
        /// Get GameplayElementController refences from the pooled gameobject projectile list
        /// </summary>
        private void RefreshGameplayProjectileList()
        {
            // Get all pooled objects
            List<GameObject> pooledObjects = this.ProjectilesPool.GetAllPooledObjects();

            // Only refresh if the gameplay list is outdated
            if (this.gameplayProjectilesPool.Count != pooledObjects.Count)
            {
                // Reset gameplay list
                this.gameplayProjectilesPool = new List<GameplayElementController>();

                // Get a gameplay reference of those preloaded projectiles
                foreach (var projectile in pooledObjects)
                    this.gameplayProjectilesPool.Add(projectile.GetComponent<GameplayElementController>());
            }
        }

        /// <summary>
        /// Update all active projectiles
        /// </summary>
        public void UpdateProjectiles()
        {
            // Refresh current gameplay reference of projectiles
            this.RefreshGameplayProjectileList();

            // Update active projectiles exclusively
            foreach (var projectile in this.gameplayProjectilesPool)
            {
                // Only update active gameobjects
                if (projectile.gameObject.activeSelf)
                    // Update projectile lifetime and movement
                    projectile.UpdateObject();
                
            }
        }
    }
}