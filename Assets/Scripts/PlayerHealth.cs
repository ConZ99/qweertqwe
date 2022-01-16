using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] TextMeshProUGUI healthText;

    public healthbar hp_bar;

    private void Start()
    {
        hp_bar.SetMaxHealth(hitPoints);
    }

    void Update()
    {
        DisplayHealth();
    }

    private void DisplayHealth()
    {
        healthText.text = hitPoints.ToString();
    }

    public void TakeDamage(float damage)
    {
        
        hitPoints -= damage;
        hp_bar.SetHealth(hitPoints);
        if (hitPoints <= 0)
        {
            UnityEngine.Debug.Log("DEAD");
            GetComponent<DeathHandler>().HandleDeath();
            //respawn menu
        }
    }
}
