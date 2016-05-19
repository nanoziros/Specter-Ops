namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    [RequireComponent(typeof(RawImage))]
    public class VideoLooper : MonoBehaviour
    {
        // Raw image with the video file
        private RawImage image;

        // Use this for initialization
        void Start()
        {
            this.image = this.GetComponent<RawImage>();

            var video = (MovieTexture) this.image.mainTexture;
            video.loop = true;
            video.Play();
        }
    }
}
