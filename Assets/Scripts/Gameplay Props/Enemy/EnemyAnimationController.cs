using System;

namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Manages all enemy animations
    /// </summary>
    public class EnemyAnimationController : MonoBehaviour
    {
        // Animation controllers
        public Animator EnemyHeadAnimator;
        public event Action OnFireProjectileEvent;

        /// <summary>
        /// Use this class for initialziation
        /// </summary>
        public void Initialize()
        {}

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
    }
}