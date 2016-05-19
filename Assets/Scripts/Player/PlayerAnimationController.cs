﻿using System;

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
        private Animator playerAnimator;

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this to initialize this instance
        /// </summary>
        public void Initialize()
        {
            // Get component references
            this.PlayerSpriteRenderer = this.GetComponent<SpriteRenderer>();
            this.playerAnimator = this.GetComponent<Animator>();

            // Subscribe to proper game events
            GamePresenter.Instance.MatchEnded += this.ExecuteEndGameAnimation;

            // Set initialization flag
            this.isInitialized = true;
        }
        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on destroy (to avoid nullreference errors)
        /// </summary>
        public void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe match events
            GamePresenter.Instance.MatchEnded -= this.ExecuteEndGameAnimation;
        }


        /// <summary>
        /// Execute the proper end game animation
        /// </summary>
        public void ExecuteEndGameAnimation(GamePresenter.GameResult gameResult,int score)
        {
            switch (gameResult)
            {
                case GamePresenter.GameResult.Win:
                    this.playerAnimator.SetTrigger("PlayerWins");
                    break;
                case GamePresenter.GameResult.Lose:
                    this.playerAnimator.SetTrigger("PlayerLoses");
                    break;
            }
        }
    }
}
