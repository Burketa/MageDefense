using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public int baseCost = 10;

    public int cost;

    public int costMultiplier = 1;

    [Header("Attack")]
    public int atk;
    public float singleAtackSpeed = 1f;
    public float multipleAttackSpeed = 3f;
    public float atkRadius = 1f;

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

    public Material shieldMaterial;
    public float asd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        cost = baseCost;
    }

    private void Update()
    {
        shieldMaterial.SetFloat("_MY", asd);

        var shield = transform.GetChild(0);
        float val = (float)currentHealth / (float)maxHealth;
        if (val <= 0)
            Die();

        shieldMaterial.SetFloat("_Distort", Mathf.Clamp(val * 10, 0, 10));

        if (val < 0.5f)
        {
            shieldMaterial.SetFloat("_FresnelWidth", Mathf.Clamp(val * 3, 0.3f, 1));
        }
        else
        {
            shieldMaterial.SetFloat("_FresnelWidth", Mathf.Clamp(1 + val, 1, 2));
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(0);
        print(FindObjectOfType<Spawner>().spawned);
        print(FindObjectOfType<Enemy>().atk);
    }

    public void TakeDamage(int incomingDmg)
    {
        Sound.instance.Play("shield_defense");
        var dmg = incomingDmg - def;
        if (dmg == 0)
            dmg = 1;
        else if (dmg < 0)
            dmg = 0;
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
        singleAtackSpeed *= 0.90f;
        multipleAttackSpeed *= 0.90f;
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
        healSpeed *= 0.90f;
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
        cost = Mathf.CeilToInt((baseCost + costMultiplier) * costMultiplier);
    }
}
