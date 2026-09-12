using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgardeScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] statsTexts;
    private GameObject range;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "Turret")
                {
                    FillPanel(hit);
                }
            }
        }
    }

    private void FillPanel(RaycastHit hit)
    {
        transform.GetChild(0).GetComponent<PointerScript>().target = hit.transform;
        transform.GetChild(0).gameObject.SetActive(true);
        range = hit.transform.GetComponent<TurretScript>().range;
        range.SetActive(true);
        nameText.text = hit.transform.name;
        TurretScript turret = hit.transform.GetComponent<TurretScript>();
        statsTexts[0].text = turret.data.Damage.ToString();
        statsTexts[1].text = turret.data.FireRate.ToString();
        statsTexts[2].text = turret.data.SpeedOfBullet.ToString();
    }

    public void ClosePanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if (range) range.SetActive(false);
    }
}
