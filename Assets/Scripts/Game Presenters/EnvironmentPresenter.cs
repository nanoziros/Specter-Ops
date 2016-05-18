namespace SpecterOps
{
    using UnityEngine;
    using Utilities;

    using System.Collections.Generic;
    using System.Linq;

    using DG.Tweening;

    /// <summary>
    /// Manages the endless runner environment, activating and deactivating rooms as required
    /// </summary>
    public class EnvironmentPresenter : MonoBehaviour
    {
        // Core prefabs
        public GameObject InitialEnvironmentTilePrefab;
        public GameObject EnvironmentTilePrefab;

        // Active environment tiles
        private List<EnvironmentTilePresenter> currentEnvironmentTiles; 

        // Tiles presentation parameters
        [Range(0,1)]
        public float PlayerSpeedMultiplier = 0.2f;
        private float distanceBetweenTiles = 6.0f;

        // Pool parameters
        public int PooledTiles = 3;
        private GenericPoolSystem tilePool;

        /// <summary>
        /// Get the initial spawn point for the player
        /// </summary>
        public void Initialize()
        {
            // Generate initial environment tiles
            this.GenerateEnvironmentTiles();
        }

        /// <summary>
        /// Update environment tiles and cycle them when they are out of the screen
        /// </summary>
        public void StartEnvironmentAnimation()
        {
            // Move each tile downwards
            for (int index = 0; index < this.currentEnvironmentTiles.Count; index++)
            {
                // Calculate final downwards position (this was made through trial and error so please place special attention here while debugging)
                var tile = this.currentEnvironmentTiles[index];
                float destination = -this.distanceBetweenTiles*(this.currentEnvironmentTiles.Count - index - 1);

                // Execute downwards animation, when it finish, restart it and re initialize the tile so new collectables and enemies can spawn
                tile.transform.DOMoveY(destination,
                    GamePresenter.Instance.PlayerPresenter.Player.PlayerSpeed*
                    this.PlayerSpeedMultiplier).SetLoops(-1, LoopType.Restart).SetSpeedBased().SetEase(Ease.Linear).OnStepComplete(tile.Initialize);
            }
        }

        /// <summary>
        /// Stop and kill current environment animation
        /// </summary>
        public void StopEnvironmentAnimation()
        {
            foreach (var tile in this.currentEnvironmentTiles)
                tile.transform.DOKill();
        }

        /// <summary>
        /// Generate initial enviroment tiles and fill the currentEnvironmentTiles list
        /// </summary>
        private void GenerateEnvironmentTiles()
        {
            // Instantite initial environment tile 
            GameObject nObj = Instantiate(EnvironmentTilePrefab) as GameObject;

            // Set  gameobject name
            nObj.name = nObj.name.Replace("(Clone)", "(Non-Pooled)");

            // Set object parent
            nObj.transform.parent = this.transform;

            // Fill pool of actual gameplay tiles
            this.tilePool = new GenericPoolSystem(this.EnvironmentTilePrefab.gameObject, this.PooledTiles, this.transform);

            // Add instanced environment tiles to current environment stack
            this.currentEnvironmentTiles = new List<EnvironmentTilePresenter>();

            // Add initial tile but DON'T initialize it (we don't want to spawn anything on it)
            this.currentEnvironmentTiles.Add(nObj.GetComponent<EnvironmentTilePresenter>()); 

            // Add remaining tiles and initialize them
            for (int i = 0; i < this.PooledTiles; i++)
            {
                EnvironmentTilePresenter tilePresenter =
                    this.tilePool.GetObject().GetComponent<EnvironmentTilePresenter>();
                this.currentEnvironmentTiles.Add(tilePresenter);

                // Spawn initial enemies and collectables
                tilePresenter.Initialize();
            }

            // Now we need to get the world space size of the floor sprites
            this.distanceBetweenTiles = this.currentEnvironmentTiles[0].GetFloorWorldSize();

            // Finally, based on their positions in the list, set their initial positions
            for (int index = 0; index < this.currentEnvironmentTiles.Count; index++)
            {
                var tile = this.currentEnvironmentTiles[index];
                tile.transform.position = new Vector3(this.transform.position.x, (index - 1) * this.distanceBetweenTiles, 0);
            }
        }

        #region Utilities
        /// <summary>
        /// Get the initial spawn point for the player
        /// </summary>
        public Vector3 GetInitialPosition()
        {
            //todo: request this position from the bottom fo currentEnvironmentTiles
            Debug.Log("Returning placeholder position");
            return Vector3.zero;
        }
        #endregion
    }
}