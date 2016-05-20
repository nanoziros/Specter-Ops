using DG.Tweening;

namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    using  DG;

    /// <summary>
    /// This class executes a pop up animation on a text element when its value changes (thanks to UGUI's input field component, its OnValueChange can
    /// call the method on this class)
    /// </summary>
    public class AutoScalerOnValueChange : MonoBehaviour
    {
        // Animation parameters
        public float InitialScale = 1.5f;
        public float FinalScale = 1.0f;
        public float AnimationDuration = 0.5f;
        public Ease AnimationEase = Ease.InBounce;
        private Tween animationTween;

        /// <summary>
        /// Execute de pop up animation using dotween
        /// </summary>
        public void ExecutePopUpAnimation()
        {
            // Kill tween if it was executing before
            if (this.animationTween != null && this.animationTween.IsPlaying())
                this.animationTween.Kill();

            // Play tween
            this.transform.localScale = Vector3.one*this.InitialScale;
            this.animationTween =
                this.transform.DOScale(this.FinalScale, this.AnimationDuration).SetEase(this.AnimationEase);
        }
    }
}