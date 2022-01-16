using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] int enemyType = 0;
    [SerializeField] GameObject player;
    bool isDead = false;
    public float force = 5;
    public healthbar hp_bar;

    private void Start()
    {
        hp_bar.SetMaxHealth(hitPoints);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnHitProvoke");
        ////////////////////////////KNOCKBACK///////////////////////////
        Vector3 playerPosition = player.transform.position;
        Vector3 dir = gameObject.transform.position - playerPosition;
        dir = dir.normalized;
        gameObject.transform.Translate(dir);

        hitPoints -= damage;
        hp_bar.SetHealth(hitPoints);
        if (hitPoints <= 0)
        {
            UnityEngine.Debug.Log("DEAD");
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        if (enemyType == 0)
        {
            //Collider colider;
            //colider = GameComponent<Collider>();
            gameObject.GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetTrigger("Die");
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
