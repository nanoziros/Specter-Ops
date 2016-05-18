namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Game UI Hub. Manages the Player Status View, Match Status View and th Victory/Defeat/Pause Views
    /// </summary>
    public class GameUIPresenter : MonoBehaviour
    {
        // Match view data providers
        // Formatted current match duration
        public string CurrentMatchDuration
        {
            get { return ""; }
        }

        /// <summary>
        /// Use this method to initialize the UI
        /// </summary>
        public void Initialize()
        {

        }
    }
}