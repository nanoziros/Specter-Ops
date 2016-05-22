namespace SpecterOps
{
    using UnityEngine;
    using DG.Tweening;

    /// <summary>
    /// Core audio manager for gamescene
    /// </summary>
    public class GameSceneAudioPresenter : AudioPresenter
    {
        // Run SFX audio source (since this will be looping , we are gonna use a separate audio source for it)
        public AudioSource RunSfxAudioSource;
        
        // Music themes
        public AudioClip MainThemeClip;
        public AudioClip VictoryThemeClip;
        public AudioClip DefeatThemeClip;

        // Sfx clips
        public AudioClip RunSfx;
        public AudioClip ChargingShootSfx;
        public AudioClip ShootSfx;
        public AudioClip ShootImpactSfx;
        public AudioClip EnemyCollisionSfx;
        public AudioClip[] CollectablePickupSfxs;

        // Control parameters
        private bool isInitialized = false;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
                        
            // Start the main theme
            this.PlayGameplayMusic();

            // Subscribe to game flow events
            GamePresenter.Instance.MatchEnded += this.EndGameMusic;

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
            GamePresenter.Instance.MatchEnded -= this.EndGameMusic;
        }

        /// <summary>
        /// Play proper music on match end
        /// </summary>
        public void EndGameMusic(GamePresenter.GameResult results, int score)
        {
            switch (results)
            {
                case GamePresenter.GameResult.Win:
                    this.PlayVictoryMusic();
                    break;
                case GamePresenter.GameResult.Lose:
                    this.PlayDefeatMusic();
                    break;
            }
        }


        #region SFX Clips
        public void PlayCollectablePickupSfx()
        {
            this.PlaySfxClip(this.CollectablePickupSfxs.Length > 0
                ? this.CollectablePickupSfxs[Random.Range(0, this.CollectablePickupSfxs.Length)]
                : null);
        }
        public void PlayChargeShootSfx()
        {
            this.PlaySfxClip(this.ChargingShootSfx);
        }
        public void PlayShootSfx()
        {
            this.PlaySfxClip(this.ShootSfx);
        }
        public void PlayShootImpactSfx()
        {
            this.PlaySfxClip(this.ShootImpactSfx);
        }
        public void PlayEnemyCollisionSfx()
        {
            this.PlaySfxClip(this.EnemyCollisionSfx);
        }
        public void PlaySfxClip(AudioClip clip)
        {
            if(clip != null)
                this.SfxAudioSource.PlayOneShot(clip);
        }
        #endregion

        #region Music Themes

        /// <summary>
        /// Start playing the main theme
        /// </summary>
        public void PlayGameplayMusic()
        {
            if (this.MainThemeClip == null)
                return;

            this.PingPongCrossFade(this.MainThemeClip);
        }

        public void PlayVictoryMusic()
        {
            this.PingPongCrossFade(this.VictoryThemeClip);
        }

        public void PlayDefeatMusic()
        {
            this.PingPongCrossFade(this.DefeatThemeClip);
        }

        #endregion
        }
}

