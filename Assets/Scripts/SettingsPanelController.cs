using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    
    private void Update()
    {
        if (settingsPanel != null && Input.GetKeyDown(KeyCode.Escape))
            settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }
}
