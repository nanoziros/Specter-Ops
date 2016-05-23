namespace SpecterOps.Utilities
{
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;

    public class AutoScaler : MonoBehaviour
    {

        // Animation Parameters
        public float OriginScale = 1.0f;
        public float TargetScale = 0.75f;
        public float AnimationInterval = 0.2f;
        public float DelayBetweenScaling= 0.0f;
        private Sequence scalingSequence;

        // Start scaling on enable
        private void OnEnable()
        {
            // Kill tween if it was executing before
            if (this.scalingSequence != null && this.scalingSequence.IsPlaying())
                this.scalingSequence.Kill();

            // Restore scale
            this.transform.localScale = Vector3.one*this.OriginScale;

            // Generate flickering sequence
            this.scalingSequence = DOTween.Sequence().SetLoops(-1);
            this.scalingSequence.Append(this.transform.DOScale(this.TargetScale,
                this.AnimationInterval / 2.0f));
            this.scalingSequence.Append(this.transform.DOScale(this.OriginScale,
                this.AnimationInterval / 2.0f));
            this.scalingSequence.AppendInterval(this.DelayBetweenScaling);
        }
    }
}