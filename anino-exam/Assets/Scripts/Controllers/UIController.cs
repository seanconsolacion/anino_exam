using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _betText;
    [SerializeField] private TextMeshProUGUI _payoutLineText;
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _winningsText;
    [SerializeField] private TextMeshProUGUI _spinText;
    [SerializeField] private Button _spinButton;

    public void UpdateBetText(string newBet)
    {
        _betText.text = $"Bet: {newBet}";
    }

    public void UpdateBalanceText(string newBalance)
    {
        _balanceText.text = $"Balance: {newBalance}";
    }

    public void UpdatePayoutlineText(string newPayoutLine)
    {
        _payoutLineText.text = $"Lines: {newPayoutLine}";
    }

    public void UpdateWinText(string winnings)
    {
        _winningsText.text = $"Winnings: {winnings}";
    }

    public void UpdateSpinText(string newText)
    {
        _spinText.text = newText;
    }

    public void TriggerSpinClickCooldown(float delay)
    {
        StartCoroutine(SpinClickCooldownRoutine());

        IEnumerator SpinClickCooldownRoutine()
        {
            _spinButton.interactable = false;
            yield return new WaitForSeconds(delay);
            _spinButton.interactable = true;

        }
    }
}
