using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelRow : MonoBehaviour
{
    [SerializeField] private SymbolData _currentSymbolData;
    [SerializeField] private ReelRow _nextReelRow;
    [SerializeField] private Animator _reelRowAnimator;
    public bool _isLastReelRow;
    public bool _isVisibleRow;

    public SymbolData CurrentSymbol => _currentSymbolData;
    public ReelRow NextReelRow => _nextReelRow;
    public bool IsLastReelRow => _isLastReelRow;


    public void UpdateCurrentSymbol(SymbolData newSymbolData)
    {
        _currentSymbolData = newSymbolData;
    }

    public void TriggerAnimation(string triggerName)
    {
        _reelRowAnimator.SetTrigger(triggerName);

        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
            return;
        
        spriteRenderer.sortingOrder = triggerName == "spin" ? 10 : 0;
    }
}
