namespace SpecterOps
{
    using UnityEngine;
    using Player;

    /// <summary>
    /// This class handles player creation and provides the data for proper UI representation
    /// </summary>
    public class PlayerPresenter : MonoBehaviour
    {
        // Core component references
        private EnvironmentPresenter environmentPresenter;

        // Player prefab
        [Header("Player Setup")]
        public PlayerController PlayerPrefab;

        // Player reference
        public PlayerController Player { get; private set; }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(EnvironmentPresenter environmentPresenter)
        {
            // Store environment presenters
            this.environmentPresenter = environmentPresenter;

            // Instantiate player
            this.CreatePlayer();
        }

        /// <summary>
        /// Creates the initial player instance
        /// </summary>
        void CreatePlayer()
        {
            // Check we haven't instanced the player already
            if (this.Player != null)
            {
                Debug.LogError("Player already instanced, cannot do again");
                return;
            }

            // Instance new player
            this.Player = Instantiate(this.PlayerPrefab);
            this.Player.transform.SetParent(this.transform,true);

            // Set new player initial transform parameters
            this.Player.transform.position = this.environmentPresenter.GetInitialPosition();

            // Initialize player
            this.Player.Initialize();
        }

    }
}