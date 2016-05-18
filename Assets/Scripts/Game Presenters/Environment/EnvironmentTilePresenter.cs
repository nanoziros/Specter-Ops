namespace SpecterOps
{
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Manages an environment tile instance, along with its turrets and pickups
    /// </summary>
    public class EnvironmentTilePresenter : MonoBehaviour
    {
        // Spawn positions parents
        public Transform PlayerSpawnTransform;
        public Transform EnemySpawnTransformParent;
        public Transform CollectableSpawnTransformParent;

        // Spawn positions
        private Transform[] enemySpawnTransforms;
        private Transform[] collectableSpawnTransforms;

        // Spawn pools
        private GenericPoolSystem enemyPool;
        private GenericPoolSystem collectablesPool;

        // Basic Components
        public SpriteRenderer FloorBG;

        /// <summary>
        /// Initialize (or reinitialize if this isn't the first time) this tile and give it a new layout
        /// </summary>
        public void Initialize()
        {
            // Get the initial spawn positions for the gameplay elements
            this.ExtractDynamicSpawnPositions();

            // Spawn dynamic elements
            this.SpawnGameplayElements();
        }

        /// <summary>
        /// Spawn enemies and collectables using the rules defined in game presenter
        /// </summary>
        private void SpawnGameplayElements()
        {
            // Initialize pools if they are empty
            if (this.enemyPool == null && this.collectablesPool == null)
            {
                
            }
        }

        /// <summary>
        /// Extract the enemies and collectables spawn positions
        /// </summary>
        private void ExtractDynamicSpawnPositions()
        {
            // Get enemy spawn transforms
            if (this.enemySpawnTransforms == null)
                this.enemySpawnTransforms = this.EnemySpawnTransformParent.GetComponentsInChildren<Transform>();

            // Get collectables spawn transforms
            if (this.collectableSpawnTransforms == null)
                this.collectableSpawnTransforms = this.CollectableSpawnTransformParent.GetComponentsInChildren<Transform>();
        }

        #region Utilities
        /// <summary>
        /// Gets floor world size
        /// </summary>
        public float GetFloorWorldSize()
        {
            return Mathf.Abs( this.FloorBG.bounds.min.y - this.FloorBG.bounds.max.y);
        }
        #endregion
    }
}
