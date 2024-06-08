using Events;
using UI;

namespace SlotFunctionality
{
    public class ScoreManager
    {
        private int winScore;
        private int paidScore;
        private int creditsScore=300;
        public static int betScore=5;
        public static int maxBet = 80;
        public static int minBet = 5;
        public static int betMultiplier = 2;
        private readonly UIScoreManager uiScoreManager;

        public ScoreManager(UIScoreManager uiScoreManager)
        {
            this.uiScoreManager = uiScoreManager;

            this.uiScoreManager.SetAllText(winScore,paidScore,creditsScore,betScore);

            EventManager.currentManager.Subscribe(EventIdentifiers.SendScore, OnWinCredits);
        }

        private void OnWinCredits(EventData eventData)
        {
            if (!eventData.IsEventOfType(out SendScore winCredits))
                return;

            creditsScore += winCredits.Score;
            uiScoreManager.VictoryPayout(winCredits.Score,creditsScore);
        }

        public void ChangeBet (int amount)
        {
            betScore = amount;
            uiScoreManager.SetBet(betScore);
        }

        /// <summary>
        /// Checks if the player can pay for a spin, if not reject spin attempt
        /// </summary>
        /// <returns>Returns whether the player can pay for a spin</returns>
        public bool TryPayForSpin()
        {
            if (creditsScore < betScore)
                return false;

            creditsScore -= betScore;
            uiScoreManager.SetCredits(creditsScore);
            return true;

        }
    }
}
