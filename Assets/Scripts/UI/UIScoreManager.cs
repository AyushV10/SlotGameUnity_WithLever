using System;
using TMPro;
using UnityEngine;

namespace UI
{
    
    /// Ui Manager which Receives bet amount and win amount and displays it on the ui screen to the player
    
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip coinCollectSfx;
        [SerializeField] private TextMeshProUGUI winText;
        [SerializeField] private TextMeshProUGUI paidText;
        [SerializeField] private TextMeshProUGUI creditText;
        [SerializeField] private TextMeshProUGUI betText;
        [SerializeField] private GameObject decBet;
        [SerializeField] private GameObject incBet;
        [SerializeField] private float payoutSpeed = 0.25f;
        private int payoutCounter;
        private bool isPayingOut;
        private float timeStamp;

        private void OnValidate()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            SlotFunctionality.ScoreManager.betScore = 5;
            SlotFunctionality.ScoreManager.maxBet = 80;
            SlotFunctionality.ScoreManager.minBet = 5;
        }

        private void Update()
        {
            if (!isPayingOut)
                return;

            if (timeStamp < Time.time)
            {
                payoutCounter--;

                if (payoutCounter < 0)
                {
                    isPayingOut = false;
                    return;
                }

                timeStamp = Time.time + payoutSpeed;

                paidText.text = (int.Parse(paidText.text) + 1).ToString();
                creditText.text = (int.Parse(creditText.text) + 1).ToString();
                audioSource.PlayOneShot(coinCollectSfx);
            }
        }

        public void SetAllText(int winAmount, int paidAmount, int creditAmount, int betAmount)
        {
            winText.text = winAmount.ToString();
            paidText.text = paidAmount.ToString();
            creditText.text = creditAmount.ToString();
            betText.text = betAmount.ToString();
        }

        public void SetCredits(int creditAmount)
        {
            creditText.text = creditAmount.ToString();
            isPayingOut = false;
            paidText.text = "0";
            winText.text = "0";
        }
        public void increaseBetPressed()
        {
            SlotFunctionality.ScoreManager.betScore = SlotFunctionality.ScoreManager.betScore * SlotFunctionality.ScoreManager.betMultiplier;

            if (SlotFunctionality.ScoreManager.betScore <= SlotFunctionality.ScoreManager.maxBet)
            {
                SetBet(SlotFunctionality.ScoreManager.betScore);
                decBet.SetActive(true);
            }
            else
            {
                incBet.SetActive(false);
            }
        }
        public void decreaseBetPressed()
        {
            SlotFunctionality.ScoreManager.betScore = SlotFunctionality.ScoreManager.betScore / SlotFunctionality.ScoreManager.betMultiplier;

            if (SlotFunctionality.ScoreManager.betScore < SlotFunctionality.ScoreManager.minBet)
            {
                decBet.SetActive(false);
                SlotFunctionality.ScoreManager.betScore = SlotFunctionality.ScoreManager.minBet;
            }
            else
            {
                Debug.Log("----------AAYAA YAH");
                SetBet(SlotFunctionality.ScoreManager.betScore);
                incBet.SetActive(true);
            }
        }
        public void SetBet(int betAmount)
        {
            betText.text = betAmount.ToString();
        }

        public void VictoryPayout(int winAmount, int creditAmount)
        {
            winText.text = winAmount.ToString();
            payoutCounter = winAmount;
            creditText.text = (creditAmount-winAmount).ToString();
            paidText.text = "0";
            timeStamp = Time.time + payoutSpeed;
            isPayingOut = true;
        }
    }
}
