namespace SpecterOps
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Manages all enemy animations
    /// </summary>
    public class EnemyAnimationController : MonoBehaviour
    {
        // Animation controllers
        public Animator EnemyBaseAnimator;
        public Animator EnemyHeadAnimator;
        public event Action OnFireProjectileEvent;

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this class for initialziation
        /// </summary>
        public void Initialize()
        {
            // Subscribe to proper game events
            GamePresenter.Instance.GamePaused += this.PauseAnimation;
            GamePresenter.Instance.GameResumed += this.ResumeAnimation;

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
        /// Request enemy fire projectile animation
        /// NOTE: This animation clip must at some point have an event calling the OnFireProjectileFrame() method on this class 
        /// </summary>
        public void RequestFireProjectileAnimation()
        {
            // Set animation trigger
            this.EnemyHeadAnimator.SetTrigger("AttackTrigger");
        }


        /// <summary>
        /// This method is called by the animation clip itself, and it should forward this event to the enemy controller so it can fire
        /// </summary>
        public void OnFireProjectileFrame()
        {
            // Request any event related to the enemy being on the fire projectile frame
            Action handler = this.OnFireProjectileEvent;
            if (handler != null) { handler(); }
        }

        /// <summary>
        /// Pause animator
        /// </summary>
        public void PauseAnimation()
        {
            this.EnemyBaseAnimator.enabled = false;
            this.EnemyHeadAnimator.enabled = false;
        }
        /// <summary>
        /// Resume animator
        /// </summary>
        public void ResumeAnimation()
        {
            this.EnemyBaseAnimator.enabled = true;
            this.EnemyHeadAnimator.enabled = true;
        }
    }
}