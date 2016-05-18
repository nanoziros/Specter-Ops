namespace SpecterOps
{
    using UnityEngine;
    using DG.Tweening;

    public class CameraRigPresenter : MonoBehaviour
    {
        // Camera Objects
        public Transform MainCameraParent;
        [Tooltip("Preferably this camera should be the last object in its hierarchy since it will shake")]
        public Camera MainCamera;
        public Camera BoundariesCamera;

        // Movement variables
        [Range(0,1)]
        public float MainCameraMovementLimiter = 0.1f;
        public float MainCameraSmoothTime = 0.3F;
        private Vector3 mainCameraVelocity = Vector3.zero;

        // Shake variables
        public float ShakeDuration = 0.5f;
        public float ShakeMagnitude = 1.0f;
        private Tween shakeTween;

        /// <summary>
        /// Update main camera position by moving its parent (Cannot move the camera itself because it can shake)
        /// </summary>
        public void UpdateMainCamera()
        {
            // Get target position
            Vector3 playerPosition = GamePresenter.Instance.PlayerPresenter.Player.transform.position;

            // Limit camera movement
            Vector3 targetPosition = new Vector3(playerPosition.x*this.MainCameraMovementLimiter,
                playerPosition.y*this.MainCameraMovementLimiter, this.MainCameraParent.transform.position.z);

            // Smoothly go towards constricted target position
            this.MainCameraParent.position = Vector3.SmoothDamp(this.MainCameraParent.position, targetPosition,
                ref this.mainCameraVelocity, this.MainCameraSmoothTime);
        }

        /// <summary>
        /// Shakes the main camera by a set strenght and duration
        /// </summary>
        public void ShakeMainCamera()
        {
            // If we are currently shaking, stop it and kill the tween so we can reuse it
            if (this.shakeTween != null || this.shakeTween.IsPlaying())
                this.shakeTween.Kill();

            // Execute shake tween
            this.shakeTween = this.MainCamera.DOShakePosition(this.ShakeDuration, this.ShakeMagnitude);
        }
    }
}
