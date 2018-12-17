using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseCost = 10;
    
    public int cost;

    public int costMultiplier = 1;

    [Header("Attack")]
    public int atk;
    public float singleAtackSpeed = 1f;
    public float multipleAttackSpeed = 3f;

    [Header("Defense")]
    public int def;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    [Space]
    public float healSpeed = 1f;
    public int healAmmount = 1;
    
    public int souls = 0;
    public GameObject soulsText;


    public int enemiesKilled = 0;
    public GameObject enemiesText;

    private void Start()
    {
        currentHealth = maxHealth;
        cost = baseCost;
    }

    private void Update()
    {
        var shield = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        float val = (float)currentHealth / (float)maxHealth;
        if (val <= 0)
            SceneManager.LoadScene(0);
        var colorAux = shield.startColor.color;
        colorAux.a = val;
        shield.startColor = colorAux;
    }

    public void TakeDamage(int incomingDmg)
    {
        var dmg = incomingDmg - def;
        if (dmg <= 0)
            dmg = 1;
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void AttackUpgrade()
    {
        atk++;
        AddSouls(-cost);
        AdjustCost();
    }

    public void SpeedUpgrade()
    {
        singleAtackSpeed *= 0.95f;
        multipleAttackSpeed *= 0.95f;
        AddSouls(-cost);
        AdjustCost();
    }

    public void DefenseUpgrade()
    {
        def++;
        AddSouls(-cost);
        AdjustCost();
    }

    public void HealthUpgrade()
    {
        var max = maxHealth;
        var current = currentHealth;
        var val = current / max;
        maxHealth++;
        currentHealth = Mathf.CeilToInt(currentHealth * val);
        //
        healAmmount++;
        healSpeed *= 0.95f;
        AddSouls(-cost);
        AdjustCost();
    }

    public void AddSouls(int val)
    {
        souls += val;
        enemiesKilled++;
        soulsText.GetComponent<TMPro.TextMeshProUGUI>().text = souls.ToString();
        enemiesText.GetComponent<TMPro.TextMeshProUGUI>().text = enemiesKilled.ToString();
    }
    
    public void AdjustCost()
    {
        costMultiplier++;
        cost = Mathf.CeilToInt((baseCost + costMultiplier * 1.23f) * costMultiplier);
    }
}
