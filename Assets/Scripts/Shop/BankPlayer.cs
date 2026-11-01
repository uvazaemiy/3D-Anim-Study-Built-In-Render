using UnityEngine;

public class BankPlayer : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Bank.OnPlusGold.Invoke(100);
        
        if (Input.GetKeyDown(KeyCode.D))
            Bank.OnMinGold.Invoke(100);
    }
}
