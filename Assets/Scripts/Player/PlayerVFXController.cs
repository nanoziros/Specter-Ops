using DG.Tweening;

namespace SpecterOps.Player
{
    using UnityEngine;

    public class PlayerVFXController : MonoBehaviour
    {
        // Sprite management
        private SpriteRenderer playerSpriteRenderer;

        // Invulnerability vfx
        public float InvulnerabilitySpriteAlpha = 0.5f;
        public float InvulnrabilityVfxInterval = 0.2f;
        private Sequence invulnerabilitySequence;

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
            // Kill tween if it was executing before
            if (this.invulnerabilitySequence != null && this.invulnerabilitySequence.IsPlaying())
                this.invulnerabilitySequence.Kill();

            // Restore alpha
            this.playerSpriteRenderer.color = new Color(this.playerSpriteRenderer.color.r,
                this.playerSpriteRenderer.color.g, this.playerSpriteRenderer.color.b,
                1);

            // Execute animation
            if (state)
            {
                // Generate flickering sequence
                this.invulnerabilitySequence = DOTween.Sequence().SetLoops(-1);
                this.invulnerabilitySequence.Append(this.playerSpriteRenderer.DOFade(this.InvulnerabilitySpriteAlpha,
                    this.InvulnrabilityVfxInterval/2.0f));
                this.invulnerabilitySequence.Append(this.playerSpriteRenderer.DOFade(1,
                    this.InvulnrabilityVfxInterval / 2.0f));
            }
        }
    }
}
