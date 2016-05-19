namespace SpecterOps.Player
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Manages all player animations
    /// </summary>
    [RequireComponent(typeof (SpriteRenderer))]
    public class PlayerAnimationController : MonoBehaviour
    {
        // Core animation components
        public SpriteRenderer PlayerSpriteRenderer { get; private set; }

        /// <summary>
        /// Use this to initialize this instance
        /// </summary>
        public void Initialize()
        {
            // Get component references
            this.PlayerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

    }
}
