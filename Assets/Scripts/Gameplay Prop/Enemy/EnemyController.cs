namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Core enemy manager and hub
    /// </summary>
    public class EnemyController : GameplayElementController
    {
        // Damage this enemy deals on collision
        public int damageOnCollision = 0;
    }
}