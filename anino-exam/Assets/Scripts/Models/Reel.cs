using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    [SerializeField] private ReelRow[] _rows;
    [SerializeField] private Transform _symbolsParent;
    [SerializeField] private List<SymbolDataContainer> _symbolPool = new List<SymbolDataContainer>();
    [SerializeField] private List<Symbol> _currentSymbols = new List<Symbol>();
    [SerializeField] private List<SymbolData> _currentResult = new List<SymbolData>();
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _generateUniqueSymbols;
    
    private SlotMachine _slotMachineManager;
    private float _currentLerpValue;
    private int _visibleRowCount;
    private bool _isSpinning;
    public int VisibleRowCount => _visibleRowCount;
    public List<SymbolData> CurrentResult => _currentResult;

    public void SetupReel(SlotMachine slotMachineManager, bool isSpinning = true)
    {
        _slotMachineManager = slotMachineManager;
        _currentLerpValue = 0;
        CountVisibleRows();

        if (!isSpinning)
            SetupSymbols();
        else
            StartCoroutine(ToggleSpin(true));
    }

    private void SetupSymbols()
    {
        // destroy all existing symbols if any.
        foreach (Symbol symbol in _currentSymbols)
            Destroy(symbol.gameObject);

        _currentSymbols.Clear();

        // create temporary data to help avoid duplicate symbols per reels
        List<SymbolDataContainer> tempSymbolDataContainer = new List<SymbolDataContainer>();

        foreach (ReelRow reelRow in _rows)
        {
            if (reelRow.IsLastReelRow)
                return;

            // instantiate a symbol and assign its position to reel row's position
            // also handle duplicate symbols per reel
            SymbolDataContainer newSymbolData;

            while (true)
            {
                newSymbolData = GetRandomSymbol();

                if (_generateUniqueSymbols)
                {
                    if (!tempSymbolDataContainer.Contains(newSymbolData))
                    {
                        tempSymbolDataContainer.Add(newSymbolData);
                        break;
                    }
                }
                else
                    break;
                
            }

            GameObject newSymbol = Instantiate(_slotMachineManager.GetSymbolPrefab(), _symbolsParent, false);
            var symbolScript = newSymbol.GetComponent<Symbol>();
            symbolScript.SetupSymbol(newSymbolData.Data, reelRow);
            _currentSymbols.Add(symbolScript);
        }
    }

    private void CountVisibleRows()
    {
        // reset row count as safety flag
        _visibleRowCount = 0;

        // get current number of visible rows
        for (int i = 0; i < _rows.Length; i++)
        {
            if (_rows[i]._isVisibleRow)
                _visibleRowCount++;
        }
    }

    private IEnumerator ToggleSpin(bool isSpinning)
    {
        if (isSpinning)
        {
            // reset all reelrows animation
            foreach (ReelRow reelRow in _rows)
                reelRow.TriggerAnimation("idle");

            _animator.SetTrigger("preSpin");
            yield return new WaitForSeconds(0.35f);
            _isSpinning = isSpinning;
        }
        else
        {
            _isSpinning = false;
            _animator.SetTrigger("postSpin");
        }
    }

    public void Tick(float deltaTime = 0)
    {
        if (!_isSpinning)
            return;

        _currentLerpValue = Mathf.Clamp01( _currentLerpValue + deltaTime);

        foreach (Symbol symbol in _currentSymbols)
            symbol.Move(_currentLerpValue);

        if (_currentLerpValue >= 1)
            _currentLerpValue = 0;
    }

    public void StopReel()
    {
        // set current lerp value to zero and tick to make symbols lock in with their respective current reel row
        _currentLerpValue = 0;
        Tick();

        StartCoroutine(ToggleSpin(false));
        SetCurrentResult();
    }

    private void SetCurrentResult()
    {
        _currentResult.Clear();

        // populate currentResultHolder array with data of visible rows
        for (int i = 0; i < _rows.Length; i++)
        {
            if (!_rows[i]._isVisibleRow)
                continue;

            _currentResult.Add(_rows[i].CurrentSymbol);
        }
    }

    private SymbolDataContainer GetRandomSymbol()
    {
        return _symbolPool[Random.Range(0, _symbolPool.Count)];
    }

    public ReelRow GetVisibleReelRow(int index)
    {
        int indexChecker = 0;
        for (int i = 0; i < _rows.Length; i++)
        {
            if (_rows[i]._isVisibleRow)
            {
                if (indexChecker >= index)
                    return _rows[i];
                else
                    indexChecker++;
            }
        }

        return null;
    }
}
