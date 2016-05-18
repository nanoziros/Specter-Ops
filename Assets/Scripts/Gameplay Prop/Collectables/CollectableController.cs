namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Manages all collectable elements in the game
    /// </summary>
    public class CollectableController : GameplayElementController
    {
        // Score awarded to the player on collection
        public int CollectableValue = 1;

        /// <summary>
        /// Method called on player impact
        /// </summary>
        public override void PlayerImpact()
        {
            base.PlayerImpact();

            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }
    }
}
