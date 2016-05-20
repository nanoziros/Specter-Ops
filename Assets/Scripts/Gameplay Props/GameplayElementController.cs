namespace SpecterOps
{
    using UnityEngine;

    /// <summary>
    /// Base class for all gameplay objects instanced on the environment
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class GameplayElementController : MonoBehaviour
    {
        /// <summary>
        /// Method called on player impact
        /// </summary>
        public virtual void PlayerImpact()
        { }

        /// <summary>
        /// Method called to update object state
        /// </summary>
        public virtual void UpdateObject()
        { }
    }
}
