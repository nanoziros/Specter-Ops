namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Particle vfx life manager
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleVFX : MonoBehaviour
    {
        // Core components
        public ParticleSystem ParticleSystem;


        /// <summary>
        /// Force particle emission on start
        /// </summary>
        private void OnEnable()
        {
            this.ParticleSystem.Play();
            StartCoroutine("AutoDisabler");
        }

        /// <summary>
        /// Disable the particle system when it stops playing
        /// </summary>
        /// <returns></returns>
        IEnumerator AutoDisabler()
        {
            while (this.ParticleSystem.isPlaying)
                yield return null;

            this.gameObject.SetActive(false);
        }

    }
}
