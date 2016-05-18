namespace SpecterOps.Player
{
    using UnityEngine;
    using System;
    using System.Collections;

    /// <summary>
    /// This class reports all collisions with gameplay elements
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCollisionController : MonoBehaviour
    {
        // Colission events
        public event Action<EnemyController> EnemyCollided;
        public event Action<CollectableController> CollectableCollided;
        public event Action<EnemyProjectileController> EnemyProjectileCollided;

        /// <summary>
        /// On trigger enter event
        /// </summary>
        void OnTriggerEnter2D(Collider2D col)
        {
            // Ignore collisions if the game isn't running
            if (GamePresenter.Instance.CurrentMatchState != GamePresenter.GameState.Running)
                return;

            // Get gameplay element reference, if it doesn't exist, don't react to this collision
            GameplayElementController element = col.GetComponent<GameplayElementController>();
            if(element == null)
                return;
            
            // Now, depending on the TYPE of element, react accordingly
            // Enemy impact
            EnemyController enemy = element as EnemyController;
            if (enemy != null)
            {
                // Request enemy collision response (raise event)
                Action<EnemyController> handler = this.EnemyCollided;
                if (handler != null) { handler(enemy); }
            }
            else
            {
                // Collectable impact
                CollectableController collectable = element as CollectableController;
                if (collectable != null)
                {
                    // Request collectable collision response (raise event)
                    Action<CollectableController> handler = this.CollectableCollided;
                    if (handler != null) { handler(collectable); }
                }
                else
                {
                    // Projectile impact
                    EnemyProjectileController projectile = element as EnemyProjectileController;
                    if (projectile != null)
                    {
                        // Request collectable collision response (raise event)
                        Action<EnemyProjectileController> handler = this.EnemyProjectileCollided;
                        if (handler != null) { handler(projectile); }
                    }
                }
            }

            // Finally, execute report to the object itself that it has impacted a player
            element.PlayerImpact();
        }
    }
}