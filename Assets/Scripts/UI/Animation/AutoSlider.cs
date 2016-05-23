namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;
    using UnityEngine.UI;


    public class AutoSlider : MonoBehaviour
    {
        // UI Components
        public Image Image;

        // Animation Parameters
        public float OriginValue = 1.0f;
        public float EndValue = 0.75f;
        public float AnimationInterval = 0.2f;
        public float DelayBetween = 0.0f;
        private Sequence animationSequence;

        // Start animation on enable
        private void OnEnable()
        {
            // Kill tween if it was executing before
            if (this.animationSequence != null && this.animationSequence.IsPlaying())
                this.animationSequence.Kill();

            // Restore scale
            this.Image.fillAmount = this.OriginValue;

            // Generate flickering sequence
            this.animationSequence = DOTween.Sequence().SetLoops(-1);
            this.animationSequence.Append(this.Image.DOFillAmount(this.EndValue,
                this.AnimationInterval / 2.0f));
            this.animationSequence.Append(this.Image.DOFillAmount(this.OriginValue,
                this.AnimationInterval / 2.0f));
            this.animationSequence.AppendInterval(this.DelayBetween);
        }
    }
}