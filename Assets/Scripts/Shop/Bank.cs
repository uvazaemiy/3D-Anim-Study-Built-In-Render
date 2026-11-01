using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public static Action<int> OnPlusGold;
    public static Action<int> OnMinGold;

    private int gold;

    private void Awake()
    {
        gold = 0;

        OnPlusGold += AddGold;
        OnMinGold += MinGold;
    }

    private void AddGold(int value)
    {
        gold += value;
        
        GoldInterface.OnGoldChange.Invoke(gold);
    }

    private void MinGold(int value)
    {
        gold -= value;
        
        GoldInterface.OnGoldChange.Invoke(gold);
    }

    private void OnDestroy()
    {
        OnPlusGold -= AddGold;
        OnMinGold -= MinGold;
    }
}
