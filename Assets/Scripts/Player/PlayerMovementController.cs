namespace SpecterOps.Player
{
    using UnityEngine;

    public class PlayerMovementController : MonoBehaviour {
        // Boundaries variables
        public Boundaries MovementBoundaries;
        private Boundaries worldSpaceBoundaries;
        private Camera coreCamera;
        /// <summary>
        /// Use this for initialization
        /// </summary>
        public void Initialize(Camera boundariesCamera)
        {
            // Set boundaries camera
            this.coreCamera = boundariesCamera;

            // Get movement borders
            this.GetWorldSpaceBoundaries();
        }

        /// <summary>
        /// Use this to update the player movement every frame
        /// </summary>
        /// <param name="inputInstance">This frame input instance</param>
        public void FixedUpdateMovement(InputInstance inputInstance)
        {
            // Get movement input
            Vector3 movementInput = new Vector3(inputInstance.StrafeRight, inputInstance.MoveForward, 0);

            // Execute movemet based on player input
            this.transform.position += movementInput* GamePresenter.Instance.GamePrefs.PlayerSpeed * Time.fixedDeltaTime;

            // Constrain player movement to screen space boundaries
            this.transform.position = new Vector2(Mathf.Clamp(this.transform.position.x, worldSpaceBoundaries.xMin, worldSpaceBoundaries.xMax),
                                                  Mathf.Clamp(this.transform.position.y, worldSpaceBoundaries.yMin, worldSpaceBoundaries.yMax));
        }

        /// <summary>
        /// Calculate the movement world space boundaries
        /// </summary>
        void GetWorldSpaceBoundaries()
        {
            // Initialize boundaries
            this.worldSpaceBoundaries = new Boundaries();

            // Get screen to world corner positions using the current screen dimensions
            Vector3 worldLeftUpCorner =
                this.coreCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, this.coreCamera.transform.position.z));
            Vector3 worldRightDownCorner = 
                this.coreCamera.ScreenToWorldPoint(new Vector3(0, 0, this.coreCamera.transform.position.z));

            // Set horizontal boundaries
            worldSpaceBoundaries.xMax = Mathf.Lerp(worldRightDownCorner.x, worldLeftUpCorner.x,MovementBoundaries.xMax);
            worldSpaceBoundaries.xMin = Mathf.Lerp(worldRightDownCorner.x, worldLeftUpCorner.x, MovementBoundaries.xMin);

            // Set vertical boundaries
            worldSpaceBoundaries.yMax = Mathf.Lerp(worldRightDownCorner.y, worldLeftUpCorner.y, MovementBoundaries.yMax);
            worldSpaceBoundaries.yMin = Mathf.Lerp(worldRightDownCorner.y, worldLeftUpCorner.y, MovementBoundaries.yMin);
        }

        /// <summary>
        /// Helper class used to define screen movement boundarias
        /// </summary>
        [System.Serializable]
        public class Boundaries
        {
            // Normalized boundaries
            [Range(0,1)]
            public float xMin, xMax, yMin, yMax;
        }
    }
}
