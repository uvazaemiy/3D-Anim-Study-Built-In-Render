using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Key values")]
    public EnemyData stats;
    public Transform paths;
    public LevelScript level;
    //Finish

    [Header("Stats")]
    [SerializeField] private float damage; 
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float worth;
    private int curPos = 0;
    private Vector2Int target;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = stats.Health;
        speed = stats.Speed;
        worth = stats.Worth;
        target = Vector2Int.RoundToInt(new Vector2(paths.GetChild(curPos).position.x, paths.GetChild(curPos).position.z));
    }

    private void FixedUpdate()
    {
        if (Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z)) == target)
        {
            if (curPos < paths.childCount - 1)
            {
                curPos++;
                target = Vector2Int.RoundToInt(new Vector2(paths.GetChild(curPos).position.x, paths.GetChild(curPos).position.z));
            }
            else
            {
                level.DamageFinish();
                Destroy(gameObject);
            }
        }
        transform.LookAt(new Vector3(target.x, transform.position.y, target.y), Vector3.up);
        rb.AddForce(transform.forward * speed);
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        if (transform.parent.childCount <= 1) level.ChangeLevel();
        level.FillScoreText((int)worth);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Bullet")
        {
            TakeDamage(float.Parse(col.transform.name));
            Destroy(col.gameObject);
        }
    }
}
