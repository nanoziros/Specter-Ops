namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Core audio manager for mainscene
    /// </summary>
    public class MainSceneAudioPresenter : AudioPresenter
    {
        // Music themes
        public AudioClip MainThemeClip;

        // Sfx clips
        public AudioClip MouseHoverClip;
        public AudioClip MousePressClip;

        /// <summary>
        /// Use this for initilization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.PlayMainMusic();
        }

        #region SFX Clips
        public void PlayMouseHoverSfx()
        {
            this.PlaySfxClip(this.MouseHoverClip);

        }
        public void PlayMousePressSfx()
        {
            this.PlaySfxClip(this.MousePressClip);
        }
        public void PlaySfxClip(AudioClip clip)
        {
            if (clip != null)
                this.SfxAudioSource.PlayOneShot(clip);
        }
        #endregion

        /// <summary>
        /// Start playing the main theme
        /// </summary>
        public void PlayMainMusic()
        {
            if (this.MainThemeClip == null)
                return;

            this.PingPongCrossFade(this.MainThemeClip);
        }

    }
}