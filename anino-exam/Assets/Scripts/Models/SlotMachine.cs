using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [Header("Slot Configuration")]
    [SerializeField] private float _spinSpeed = 1;
    [SerializeField] private GameObject _symbolPrefab;
    [SerializeField] private List<PayoutLineDataContainer> _payoutLines = new List<PayoutLineDataContainer>();
    [SerializeField] private List<Reel> _reels = new List<Reel>();
    [SerializeField] private float _reelStoppageDelay = 0.1f;

    [Header("Spin Configuration")]
    [SerializeField] private int _currentBet;
    [SerializeField] private int _currentPayoutLine;


    private bool _isSpinning;
    private WaitForSeconds _waitForSec;
    private Action<int> _onSpinEndCallback;
    public bool IsSpinning => _isSpinning;

    private void Start()
    {
        SetupReels(false);
    }

    private void Update()
    {
        if (!_isSpinning)
            return;
        
        Tick();
    }

    public void Spin(int bet, int payoutLine, Action<int> onSpinEndCallback)
    {

        _currentBet = bet;
        _currentPayoutLine = payoutLine;
        SetupReels();
        _isSpinning = true;
        _onSpinEndCallback = onSpinEndCallback;
    }

    public void StopSpin()
    {
        StartCoroutine(StopSpinRoutine());

        IEnumerator StopSpinRoutine()
        {
            _waitForSec = new WaitForSeconds(_reelStoppageDelay);
            
            // set can flag to check if the slot machine is finished stopping
            // will be unable to spin again if not yet finished.

            // stop all reel from left to right with a delay for better visuals
            foreach (Reel reel in _reels)
            {
                reel.StopReel();
                yield return _waitForSec;
            }

            _isSpinning = false;
            GetCurrentResult();
        }
    }

    private void SetupReels(bool spinReels = true)
    {
        foreach (Reel reel in _reels)
            reel.SetupReel(this, spinReels);
    }

    private void Tick()
    {
        foreach (Reel reel in _reels)
            reel.Tick(Time.deltaTime * _spinSpeed);
    }

    public void GetCurrentResult()
    {
        SymbolData[,] results = new SymbolData[_reels[0].VisibleRowCount, _reels.Count];
        
        for (int i = 0; i < _reels[0].VisibleRowCount; i++)
        {
            for (int i2 = 0; i2 < _reels.Count; i2++)
            {
                results[i, i2] = _reels[i2].CurrentResult[i];
            }
        }


        Debug.Log(results[0,0].symbolID + " | " + results[0, 1].symbolID + " | " + results[0, 2].symbolID + " | " + results[0, 3].symbolID + " | " + results[0, 4].symbolID); 
        Debug.Log(results[1,0].symbolID + " | " + results[1, 1].symbolID + " | " + results[1, 2].symbolID + " | " + results[1, 3].symbolID + " | " + results[1, 4].symbolID); 
        Debug.Log(results[2,0].symbolID + " | " + results[2, 1].symbolID + " | " + results[2, 2].symbolID + " | " + results[2, 3].symbolID + " | " + results[2, 4].symbolID);

        int hits = 0;
        int winnings = 0;
        SymbolData tempHeadSymbol = new SymbolData();
        List<ReelRow> tempReelRows = new List<ReelRow>();

        for (int i = 0; i < _currentPayoutLine; i++)
        {
            tempReelRows.Clear();
            for (int i2 = 0; i2 < _payoutLines[i].payoutLinePattern.Length; i2++)
            {
                if (i2 == 0)
                {
                    // head of payline, set head symbol of possible sequence
                    tempHeadSymbol = results[_payoutLines[i].payoutLinePattern[i2], i2];
                    hits = 1;

                    tempReelRows.Add(_reels[i2].GetVisibleReelRow(_payoutLines[i].payoutLinePattern[i2]));
                }
                else if (tempHeadSymbol.symbolID == results[_payoutLines[i].payoutLinePattern[i2], i2].symbolID)
                {
                    // update hits if same symbol as head symbol
                    hits++;
                    tempReelRows.Add(_reels[i2].GetVisibleReelRow(_payoutLines[i].payoutLinePattern[i2]));
                }
                else
                {
                    // if not head of payline and is not same symbol as head symbol, end of counting of payline
                    break;
                }
            }
            
            Debug.Log(_payoutLines[i].name + " " + hits + "hits. Win: " + tempHeadSymbol.payoutScheme[hits - 1]);
            winnings += tempHeadSymbol.payoutScheme[hits - 1] * _currentBet;

            // animate all symbols if win
            if (tempHeadSymbol.payoutScheme[hits - 1] > 0)
            {
                foreach (ReelRow reelRow in tempReelRows)
                    reelRow.TriggerAnimation("spin");
            }
        }

        Debug.Log("Total winnings: " + winnings);
        _onSpinEndCallback.Invoke(winnings);
    }


    public GameObject GetSymbolPrefab()
    {
        return _symbolPrefab;
    }
}
