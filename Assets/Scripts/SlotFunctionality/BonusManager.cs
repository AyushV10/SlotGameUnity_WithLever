using System;
using Events;
using UI;
using UnityEngine;

namespace SlotFunctionality
{
    //Class to manage the bonus game 
    public class BonusManager : MonoBehaviour
    {
        [SerializeField] private SliderBar healthBar;
        [SerializeField] private SpriteRenderer bonusSprite;
        [SerializeField] private GameObject bonusBattleTitle;
        [SerializeField] private GameObject bonusBattleTitle2;
        [SerializeField] private GameObject overlayWhenBonus;
        [SerializeField] private int maxHealth = 30;
        [SerializeField]private int turnsToKill = 3;
        [SerializeField] private int killReward = 100;

        private int currentHealth;
        private int turnsLeft;
        private bool isDead;

        public bool IsDead => isDead;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventIdentifiers.BonusSummon, OnBonusSummon);
        }

        private void OnValidate()
        {
            if (bonusSprite == null)
                bonusSprite = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            ChangeActive(false);
        }

        private void SetupBonus()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);
            turnsLeft = turnsToKill;
            isDead = false;
        }

        private void ChangeActive(bool isActive)
        {
            overlayWhenBonus.SetActive(isActive);
            bonusSprite.enabled=isActive;
            bonusBattleTitle.SetActive(isActive);
            healthBar.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// When the player scores a match, reduce the boss's health by the score
        /// </summary>
        private void OnTakeDamage(EventData eventData)
        {
            if (!eventData.IsEventOfType(out SendScore bossDamage))
                return;

            currentHealth -= bossDamage.Score;
            healthBar.SetCurrentValue(currentHealth);

            if (currentHealth > 0 || isDead)
                return;

            //give player a big score reward for killing the boss
            EventManager.currentManager.AddEvent(new SendScore(killReward));

            ChangeActive(false);
            bonusBattleTitle2.SetActive(true);
            EventManager.currentManager.Unsubscribe(EventIdentifiers.SendScore, OnTakeDamage);
            isDead = true;
        }

        private void OnBonusSummon(EventData eventData)
        {
            if (!eventData.IsEventOfType(out BonusSummon _) && !isDead)
                return;

            EventManager.currentManager.Subscribe(EventIdentifiers.SendScore, OnTakeDamage);
            SetupBonus();
            ChangeActive(true);
        }

        /// <summary>
        /// Reduce the amount of turns left to kill the boss
        /// </summary>
        public void OnNewSpin(EventData eventData)
        {
            turnsLeft--;
            if (turnsLeft == 0)
            {
                ChangeActive(false);
                EventManager.currentManager.Unsubscribe(EventIdentifiers.SendScore, OnTakeDamage);
            }
        }
    }
}
