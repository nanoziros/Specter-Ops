namespace SpecterOps
{
    using UnityEngine;

    /// <summary>
    /// Manages all collectable elements in the game
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CollectableController : GameplayElementController
    {
        // Score awarded to the player on collection
        public int CollectableValue { get; private set; }

        // Animation controllers
        private Animator collectableAnimator;

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(int collectableValue)
        {
            // Get component references
            this.collectableAnimator = this.GetComponent<Animator>();

            // Subscribe to proper game events
            GamePresenter.Instance.GamePaused += this.PauseAnimation;
            GamePresenter.Instance.GameResumed += this.ResumeAnimation;

            // Set initial parameters
            this.CollectableValue = collectableValue;

            // Set initialized flag
            this.isInitialized = true;
        }

        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on disable and on destroy
        /// </summary>
        private void OnDisable()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Remove initialization flag
            this.isInitialized = false;

            // Unsubscribe gameflow events
            GamePresenter.Instance.GamePaused -= this.PauseAnimation;
            GamePresenter.Instance.GameResumed -= this.ResumeAnimation;
        }
        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on disable and on destroy
        /// </summary>
        private void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe gameflow events
            GamePresenter.Instance.GamePaused -= this.PauseAnimation;
            GamePresenter.Instance.GameResumed -= this.ResumeAnimation;
        }

        /// <summary>
        /// Method called on player impact
        /// </summary>
        public override void PlayerImpact()
        {
            base.PlayerImpact();

            // Play collection sfx
            GamePresenter.Instance.AudioPresenter.PlayCollectablePickupSfx();

            // Play impact vfx
            GameObject impactVfx = GamePresenter.Instance.VfxPresenter.RewardImpactPool.GetObject();
            impactVfx.transform.position = this.transform.position;

            // Deactivate gameobject
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Pause animator
        /// </summary>
        public void PauseAnimation()
        {
            this.collectableAnimator.enabled = false;
        }
        /// <summary>
        /// Resume animator
        /// </summary>
        public void ResumeAnimation()
        {
            this.collectableAnimator.enabled = true;
        }
    }
}
