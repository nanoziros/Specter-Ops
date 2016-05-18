namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

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
    }
}
