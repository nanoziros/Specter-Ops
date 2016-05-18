namespace SpecterOps.Player
{
    using UnityEngine;
    using System.Collections;

    public class PlayerVFXController : MonoBehaviour
    {
        // Sprite management
        private SpriteRenderer playerSpriteRenderer;

        // Invulnerability vfx
        public float InvulnerabilitySpriteAlpha = 0.5f;

        /// <summary>
        /// Use this to initialize this instance
        /// </summary>
        public void Initialize(SpriteRenderer sprite)
        {
            // Store compoents
            this.playerSpriteRenderer = sprite;
        }

        /// <summary>
        /// Starts and ends the invulnerability vfx
        /// </summary>
        public void ManageInvulnerabilityVFX(bool state)
        {
            // Set alpha based on invulnerability state
            this.playerSpriteRenderer.color = new Color(this.playerSpriteRenderer.color.r,
                this.playerSpriteRenderer.color.g, this.playerSpriteRenderer.color.b,
                state ? this.InvulnerabilitySpriteAlpha : 1);
        }
    }
}
