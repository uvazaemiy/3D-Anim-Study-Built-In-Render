using System;
using UnityEngine;
using UnityEngine.UI;

public class GoldInterface : MonoBehaviour
{
    public static Action<int> OnGoldChange;
    
    [SerializeField] private Text goldText;

    private void Awake()
    {
        OnGoldChange += ChangeGold;
    }

    private void ChangeGold(int value)
    {
        goldText.text = value.ToString();
    }

    private void OnDestroy()
    {
        OnGoldChange -= ChangeGold;
    }
}