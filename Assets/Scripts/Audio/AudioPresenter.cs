namespace SpecterOps
{
    using UnityEngine;
    using DG.Tweening;

    public class AudioPresenter : MonoBehaviour
    {
        // Music cross fade parameters
        [Range(1, 20)]
        public float CrossFadeDuration = 1.0f;

        // Audio sources (we use two to enable cross fading between two songs)
        public AudioSource MainAudioSource;
        public AudioSource SecondaryAudioSource;

        // SFX audio source (used for individual & small audio clips)
        public AudioSource SfxAudioSource;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        public virtual void Initialize()
        {
            // Set loop mode for both music sources
            this.MainAudioSource.loop = true;
            this.SecondaryAudioSource.loop = true;
        }

        /// <summary>
        /// Cross Fade target audio clip in a ping pong fashion between our two audio sources
        /// </summary>
        protected void PingPongCrossFade(AudioClip clip)
        {
            if (!this.MainAudioSource.isPlaying)
                this.CrossFadeAudioSources(this.MainAudioSource, this.SecondaryAudioSource, clip);
            else
                this.CrossFadeAudioSources(this.SecondaryAudioSource, this.MainAudioSource, clip);
        }


        /// <summary>
        /// Cross fade any two audio sources with the newClip
        /// </summary>
        private void CrossFadeAudioSources(AudioSource sourceA, AudioSource sourceB, AudioClip newClip)
        {
            sourceA.volume = 0;
            sourceA.clip = newClip;
            sourceA.Play();

            sourceA.DOFade(1, this.CrossFadeDuration);
            sourceB.DOFade(0, this.CrossFadeDuration).OnComplete(sourceB.Stop);
        }
    }
}
