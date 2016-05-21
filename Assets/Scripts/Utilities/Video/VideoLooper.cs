namespace SpecterOps.Utilities
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(RawImage))]
    public class VideoLooper : MonoBehaviour
    {
        // Raw image with the video file
        public RawImage Image;
        // Use this for initialization
        void OnEnable()
        {
            // Run video
            var video = (MovieTexture) this.Image.mainTexture;
            video.loop = true;
            video.Play();
        }
    }
}
