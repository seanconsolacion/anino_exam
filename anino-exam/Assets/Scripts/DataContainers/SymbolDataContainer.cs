using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Symbol Data", menuName = "SlotMachineData/SymbolData")]
public class SymbolDataContainer : ScriptableObject
{
    [SerializeField] private SymbolData data;

    public SymbolData Data => data;
}

[System.Serializable]
public struct SymbolData
{
    public string symbolID;
    public Sprite icon;
    public int[] payoutScheme;
}
