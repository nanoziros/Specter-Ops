namespace SpecterOps
{
    /// <summary>
    /// Manages all collectable elements in the game
    /// </summary>
    public class CollectableController : GameplayElementController
    {
        // Score awarded to the player on collection
        public int CollectableValue { get; private set; }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(int collectableValue)
        {
            // Set initial parameters
            this.CollectableValue = collectableValue;
        }

        /// <summary>
        /// Method called on player impact
        /// </summary>
        public override void PlayerImpact()
        {
            base.PlayerImpact();

            // Play collection sfx
            GamePresenter.Instance.AudioPresenter.PlayCollectablePickupSfx();

            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }
}
