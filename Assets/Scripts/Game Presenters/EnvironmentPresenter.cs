namespace SpecterOps
{
    using UnityEngine;
    using Utilities;

    using System.Collections.Generic;
    using System.Collections;

    /// <summary>
    /// Manages the endless runner environment, activating and deactivating rooms as required
    /// </summary>
    public class EnvironmentPresenter : MonoBehaviour
    {
        // Core prefabs
        public GameObject EnvironmentTilePrefab;

        // Active environment tiles
        private List<EnvironmentTilePresenter> currentEnvironmentTiles; 

        // Tiles presentation parameters
        [Range(0,1)]
        public float PlayerSpeedMultiplier = 0.2f;
        private float distanceBetweenTiles = 6.0f;

        // Pool parameters
        [Range(1,5)]
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
            foreach (var tile in this.currentEnvironmentTiles)
                StartCoroutine("TileAnimation", tile);
        }

        /// <summary>
        /// Move the tile downwards and reset it when it gets bellow a set position
        /// </summary>
        IEnumerator TileAnimation(EnvironmentTilePresenter tile)
        {
            // Calculate destination
            float destination = -this.distanceBetweenTiles * 2;

            // Execute animation as long as the match hasn't ended
            while (GamePresenter.Instance.CurrentMatchState != GamePresenter.GameState.Ended)
            {
                // Only update tile if the game is running
                if (GamePresenter.Instance.CurrentMatchState == GamePresenter.GameState.Running)
                {
                    // Move tile downwards
                    tile.transform.position += -Vector3.up*GamePresenter.Instance.PlayerPresenter.Player.PlayerSpeed* this.PlayerSpeedMultiplier *
                                               Time.deltaTime;

                    // If tile is bellow visual threshold, reset it
                    if(tile.transform.position.y <= destination)
                        this.ResetTile(tile);
                }
                yield return null;
            }

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

            // Add first initial tile but DON'T initialize it (we don't want to spawn anything on it)
            this.currentEnvironmentTiles.Add(nObj.GetComponent<EnvironmentTilePresenter>()); 

            // Add remaining tiles and initialize them
            for (int i = 0; i < this.PooledTiles; i++)
            {
                EnvironmentTilePresenter tilePresenter =
                    this.tilePool.GetObject().GetComponent<EnvironmentTilePresenter>();
                this.currentEnvironmentTiles.Add(tilePresenter);

                // Spawn initial enemies and collectables
                // NOTE: We DON'T want to initialize the second tile either (this is where the player will start the game)
                if(i>1)
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
        /// Send tile back to the top of possible positions and reinitialize it
        /// </summary>
        private void ResetTile(EnvironmentTilePresenter tile)
        {
            // Set new tile position
            tile.transform.position = new Vector3(this.transform.position.x, (this.currentEnvironmentTiles.Count - 2) * this.distanceBetweenTiles, 0);

            // Reinitialize tile
            tile.Initialize();
        }

        /// <summary>
        /// Get the initial spawn point for the player
        /// </summary>
        public Vector3 GetInitialPosition()
        {
            // Report errorif the second environment tile isn't available
            if (this.currentEnvironmentTiles == null || this.currentEnvironmentTiles.Count<2)
            {
                Debug.LogError("currentEnvironmentTiles is empty");
                return Vector3.zero;
            }
            // Otherwise send the prope spawn position ( the second environment tile's player spawn position)
            else
            {
                return this.currentEnvironmentTiles[1].PlayerSpawnTransform.position;
            }
        }
        #endregion
    }
}