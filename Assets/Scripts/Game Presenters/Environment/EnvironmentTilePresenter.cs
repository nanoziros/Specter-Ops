using System.Linq;

namespace SpecterOps
{
    using UnityEngine;
    using System.Collections.Generic;
    using Utilities;

    /// <summary>
    /// Manages an environment tile instance, along with its turrets and pickups
    /// </summary>
    public class EnvironmentTilePresenter : MonoBehaviour
    {
        // Active gameplay elements
        private List<EnemyController> activeEnemies = new List<EnemyController>();
        private List<CollectableController> activeCollectables = new List<CollectableController>();

        // Gameplay elements parents
        public Transform enemiesParent;
        public Transform collectablesParent;

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
            // Reset current gameplay elements (if any)
            this.ResetGameplayElements();

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
                // Generate enemies
                this.enemyPool = new GenericPoolSystem(GamePresenter.Instance.EnemyPrefab.gameObject,
                    GamePresenter.Instance.MaxEnemyPerTile, this.enemiesParent);

                // Generate collectables
                this.collectablesPool = new GenericPoolSystem(GamePresenter.Instance.CollectablePrefab.gameObject,
                    GamePresenter.Instance.MaxCollectablePerTile, this.collectablesParent);
            }

            // Spawn enemies
            this.SpawnGameplayElement(GamePresenter.Instance.MinEnemyPerTile, GamePresenter.Instance.MaxEnemyPerTile,
                this.enemySpawnTransforms, this.enemyPool, ref this.activeEnemies);

            // Initialize enemies
            foreach (var enemy in this.activeEnemies)
            {
                // Set gameplay value (damage)
                enemy.damageOnCollision = GamePresenter.Instance.DamagePerCollision;

                // Set bullet's gameplay value (damage)
                // todo:
            }

            // Spawn collectables
            this.SpawnGameplayElement(GamePresenter.Instance.MinCollectablePerTile, GamePresenter.Instance.MaxCollectablePerTile,
                this.collectableSpawnTransforms, this.collectablesPool, ref this.activeCollectables);

            // Initialize collectables
            foreach (var collectable in this.activeCollectables)
            {
                // Set gameplay value (reward)
                collectable.CollectableValue = GamePresenter.Instance.RewardPerCollectable;
            }
        }


        /// <summary>
        /// Spawn random amount of gameplay elements on the environment
        /// </summary>
        private void SpawnGameplayElement<T>(int minElementPerTile, int maxElementPerTile, Transform[] spawnLocations,
            GenericPoolSystem elementPool, ref List<T> activeElements)
        {
            // Spawn random amount of elements
            int elementsToSpawn = Random.Range(minElementPerTile, maxElementPerTile);

            // Generate stack of random locations to spawn the elements
            List<Transform> elementSpawnLocations = spawnLocations.ToList();
            elementSpawnLocations.Shuffle();

            // Spawn elements
            for (int i = 0; i < elementsToSpawn; i++)
            {
                // Don't spawn element if we've exceeded the possible spawn locations
                if (elementSpawnLocations.Count <= 0)
                    break;

                // Get element instance from the predefined pool
                GameObject spawnedElement = elementPool.GetObject();

                // Get and use element spawn position
                spawnedElement.transform.parent = elementSpawnLocations[elementSpawnLocations.Count - 1];
                spawnedElement.transform.localPosition = Vector3.zero;

                // Remove possible spawn location
                elementSpawnLocations.RemoveAt(elementSpawnLocations.Count - 1);

                // Register active element
                activeElements.Add(spawnedElement.GetComponent<T>());
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

        /// <summary>
        /// Called to clear gameplay elements from this tile
        /// </summary>
        private void ResetGameplayElements()
        {
            // Deactivate and clear enemies
            foreach (var enemy in this.activeEnemies)
            {
                enemy.transform.parent = this.enemiesParent;
                enemy.gameObject.SetActive(false);
            }
            this.activeEnemies.Clear();

            // Deactivate and clear collectables
            foreach (var collectable in this.activeCollectables)
            {
                collectable.transform.parent = this.collectablesParent;
                collectable.gameObject.SetActive(false);
            }
            this.activeCollectables.Clear();
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
