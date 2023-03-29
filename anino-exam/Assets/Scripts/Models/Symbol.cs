using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    [SerializeField] private SymbolData _data;
    [SerializeField] private ReelRow _fromReelRow;
    [SerializeField] private ReelRow _toReelRow;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public SymbolData Data => _data;

    public void SetupSymbol(SymbolData data, ReelRow initialReelRow)
    {
        _data = data;
        _spriteRenderer.sprite = data.icon;
        transform.position = initialReelRow.transform.position;
        _fromReelRow = initialReelRow;
        _toReelRow = initialReelRow.NextReelRow;

        transform.parent = initialReelRow.transform;
    }

    public void UpdateSymbolReelRow()
    {
        var teleport = _toReelRow.IsLastReelRow;

        // update parent from and to target so that the symbol will lerp unto the next given reel row
        _fromReelRow = _fromReelRow.NextReelRow;
        _toReelRow = _fromReelRow.NextReelRow;
        transform.parent = _fromReelRow.transform;

        // update current reel row's data
        _fromReelRow.UpdateCurrentSymbol(_data);

        // check if on edge already
        // set from and to reelrow as the same so it will teleport and lerp on the to's position
        if (teleport)
            _fromReelRow = _toReelRow;
    }

    public void Move(float lerpValue)
    {
        transform.position = Vector3.Lerp(_fromReelRow.transform.position, _toReelRow.transform.position, lerpValue);

        if (lerpValue >= 1)
        {
            // update symbol's from and to reelrow
            UpdateSymbolReelRow();
        }
    }
}
