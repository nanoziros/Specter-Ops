namespace SpecterOps.Utilities
{
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;

    public class AutoTextFlickering : MonoBehaviour
    {
        // Text source
        public Text TextComponent;

        // AnimationParameters
        public float TargetAlpha = 0.5f;
        public float AnimationInterval = 0.2f;
        public float DelayBetweenFlickering = 1.0f;
        private Sequence flickeringSequence;

        // Start flickering on enable
        private void OnEnable()
        {
            // Kill tween if it was executing before
            if (this.flickeringSequence != null && this.flickeringSequence.IsPlaying())
                this.flickeringSequence.Kill();

            // Restore alpha
            this.TextComponent.color = new Color(this.TextComponent.color.r,
                this.TextComponent.color.g, this.TextComponent.color.b,
                1);

            // Generate flickering sequence
            this.flickeringSequence = DOTween.Sequence().SetLoops(-1);
            this.flickeringSequence.Append(this.TextComponent.DOFade(this.TargetAlpha,
                this.AnimationInterval / 2.0f));
            this.flickeringSequence.Append(this.TextComponent.DOFade(1,
                this.AnimationInterval / 2.0f));
            this.flickeringSequence.AppendInterval(this.DelayBetweenFlickering);
        }
    }
}