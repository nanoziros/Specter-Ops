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

        IEnumerator AutoDisabler()
        {
            yield return new WaitForSeconds(this.ParticleSystem.duration);

            this.gameObject.SetActive(false);
        }

    }
}
