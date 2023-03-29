using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Line Data", menuName = "SlotMachineData/LineData" )]
public class PayoutLineDataContainer : ScriptableObject
{
    public string payoutLineName; // used for debugging purposes and easy tracking of logs
    public int[] payoutLinePattern;
}
