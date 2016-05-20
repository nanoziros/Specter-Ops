namespace SpecterOps
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Core main menu hub, it handle all main menu transitions and interactions
    /// </summary>
    public class MainMenuPresenter : MonoBehaviour
    {
        // Game screens
        public GameObject MainMenuScreen;
        public GameObject InstructionsScreen;
        public GameObject CreditsScreen;

        // Main multiple screens pop up data structure
        private Stack<GameObject> screenOrder = new Stack<GameObject>();

        /// <summary>
        /// Use this for initialization (this is the only class that can use this method in MainMenu with the exception of video loopers)
        /// </summary>
        void Start()
        {
            // Go to the main menu screen
            this.GoToMainMenu();
        }

        #region Specific GoTo Methods
        public void GoToMainMenu()
        {
            this.GoTo(this.MainMenuScreen);
        }
        public void GoToInstructions()
        {
            this.GoTo(this.InstructionsScreen);
        }
        public void GoToCredits()
        {
            this.GoTo(this.CreditsScreen);
        }
        #endregion

        #region Generic GoTos
        /// <summary>
        /// Go to the selected window
        /// </summary>
        /// <param name="panel"></param>
        private void GoTo(GameObject panel)
        {
            panel.SetActive(true);
            this.screenOrder.Push(panel);
        }
        /// <summary>
        /// Go to the previous active window
        /// </summary>
        public void GoBack()
        {
            if (this.screenOrder.Count > 0)
            {
                // Deactivate and remove current window from stack
                this.screenOrder.Peek().SetActive(false);
                this.screenOrder.Pop();

                // Go to previous window
                if (this.screenOrder.Peek() != null)
                    this.screenOrder.Peek().SetActive(true);
            }
        }
        #endregion
    }
}
