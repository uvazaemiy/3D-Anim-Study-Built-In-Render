using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform[] pages;
    [SerializeField] private Transform hearts;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Stats")]
    public int health;
    public int level;
    public int score;
    private int countOfEnemy = 10;

    [Header("Path")]
    public Vector3 spawn;
    public Transform paths;

    [Header("Prefabs")]
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private TurretData[] turrets;

    [Header("Turret")]
    [SerializeField] private BuildingPlacer placer;

    private void Start()
    {
        FillTurretIcons();
        FillScoreText(0);
        Invoke("GenerateEnemy", 5f);
    }

    private void FillTurretIcons()
    {
        int index = 0;
        for(int i = 0; i < pages.Length; i++)
        {
            for(int j = 0; j < pages[i].childCount; j++)
            {
                if (index < turrets.Length && turrets[index].Sprite)
                {
                    pages[i].GetChild(j).GetChild(0).GetComponent<Image>().sprite = turrets[index].Sprite;
                    pages[i].GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().text = turrets[index].Price.ToString();
                    pages[i].GetChild(j).GetChild(2).GetComponent<TextMeshProUGUI>().text = turrets[index].TurretName;
                }
                index++;
            }
        }
    }

    public void ChangeLevel()
    {
        level++;
        levelText.text = level.ToString();
        Invoke("GenerateEnemy", 5f);
    }

    public void FillScoreText(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void GenerateEnemy()
    {
        EnemyScript curEnemy = Instantiate(enemies[level / 5].Model, spawn, Quaternion.identity, transform.GetChild(0)).GetComponent<EnemyScript>();
        curEnemy.paths = paths;
        curEnemy.level = this;
        countOfEnemy--;
        if (countOfEnemy > 0) Invoke("GenerateEnemy", 2f);
        else countOfEnemy = 10 + level;
    }

    public void DamageFinish()
    {
        health--;
        if(hearts.childCount > 0) Destroy(hearts.GetChild(0).gameObject);
        if (health <= 0)
        {
            Invoke("EndGame", 3f);
        }
    }

    private void EndGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void BuyTurret(int indexOfTurret)
    {
        if (indexOfTurret < turrets.Length && score >= turrets[indexOfTurret].Price && turrets[indexOfTurret].Model)
        {
            placer.buildingPrefab = turrets[indexOfTurret].Model;
            placer.price = -turrets[indexOfTurret].Price;
            placer.needToBuild = true;
        }
    }    
}
