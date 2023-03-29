using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SlotMachine _slotMachine;
    [SerializeField] private UIController _uiController;
    [SerializeField] private int[] _bets;
    [SerializeField] private Vector2Int _payoutLineMinMax;
    [SerializeField] private int _currentBetIndex;
    [SerializeField] private int _currentPayoutLine;
    [SerializeField] private int _currentBalance;
    [SerializeField] private float _autoStopDelay = 3;

    private Coroutine _autoStopSpinRoutine;
    private void Start()
    {
        Setup();
        AdjustCurrentBet(0);
        AdjustCurrentPayoutLine(0);
    }

    private void Setup()
    {
        _currentPayoutLine = _payoutLineMinMax.x;
        _currentBetIndex = 0;
        _currentBalance = 1000; // for debugging purposes
        _uiController.UpdateSpinText("Spin");
    }

    public void TriggerSpin()
    {
        // check if slot machine is currently spinning
        // start or stop spin respectively and adjust UI
        if (!_slotMachine.IsSpinning)
        {
            if (_currentBalance - _bets[_currentBetIndex] * _currentPayoutLine < 0)
            {
                Debug.Log("<color=#FF0000> NOT ENOUGH BALANCE! </color>");
                return;
            }

            _currentBalance -= _bets[_currentBetIndex] * _currentPayoutLine;

            // update UI
            _uiController.UpdateSpinText("Stop Spin");
            _uiController.UpdateBalanceText(_currentBalance.ToString());
            _uiController.UpdateWinText("0");

            // control slot machine
            _slotMachine.Spin(_bets[_currentBetIndex], _currentPayoutLine, SpinEndCallback);
            _uiController.TriggerSpinClickCooldown(0.5f);


            if (_autoStopSpinRoutine != null)
                StopCoroutine(_autoStopSpinRoutine);

            // setup auto stoppage of spin
            _autoStopSpinRoutine = StartCoroutine(AutoStopSpinRoutine());
        }
        else
        {
            // control slot machine
            // Handle manual stoppage of spin
            _uiController.UpdateSpinText("Spin");
            _uiController.TriggerSpinClickCooldown(1.5f);
            _slotMachine.StopSpin();

            if (_autoStopSpinRoutine != null)
                StopCoroutine(_autoStopSpinRoutine);
        }
    }

    private IEnumerator AutoStopSpinRoutine()
    {
        yield return new WaitForSeconds(_autoStopDelay);
        TriggerSpin();
    }
    
    private void SpinEndCallback(int winnings)
    {
        // update player data
        _currentBalance += winnings;

        // update UI
        _uiController.UpdateWinText(winnings.ToString());
        _uiController.UpdateBalanceText(_currentBalance.ToString());
    }

    public void AdjustCurrentBet(int delta)
    {
        _currentBetIndex = Mathf.Clamp(_currentBetIndex + delta, 0, _bets.Length - 1);
        _uiController.UpdateBetText(_bets[_currentBetIndex].ToString());
    }

    public void AdjustCurrentPayoutLine(int delta)
    {
        _currentPayoutLine = Mathf.Clamp(_currentPayoutLine + delta, _payoutLineMinMax.x, _payoutLineMinMax.y);
        _uiController.UpdatePayoutlineText(_currentPayoutLine.ToString());
    }
}
