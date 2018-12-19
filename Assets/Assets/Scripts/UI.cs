using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("Attack")]
    public Text atk;
    public Text atkCost, speedCost, singleSpeed, multipleSpeed;
    public Button atkButton, speedButton;

    [Header("Defense")]
    public Text def;
    public Text defCost;
    public Button defButton;

    [Header("Heal")]
    public Text heal;
    public Text healSpeed, healCost;
    public Button healButton;

    [Header("Healthbar")]
    public Slider playerHealthbar;
    public GameObject enemyHealthbarPrefab;
    public Transform enemyHealthbarParent;
    public Vector2 enemyHealthbarOffset;

    [Header("Misc")]
    public Button areaAttackButton;

    //Private Variables
    private Player player;
    private Button[] allButtons;
    private Text[] allCosts;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        allButtons = new Button[] { atkButton, speedButton, defButton, healButton };

        allCosts = new Text[] { atkCost, speedCost, defCost, healCost };

        instance = this;
    }

    void Update()
    {
        UpdateUI();
        if (player.souls >= player.cost)
            UpdateAvailability(true);
        else
            UpdateAvailability(false);
    }

    public void UpdateUI()
    {
        var player = FindObjectOfType<Player>();

        atk.text = player.atk.ToString();
        singleSpeed.text = player.singleAtackSpeed.ToString("F2");
        multipleSpeed.text = player.multipleAttackSpeed.ToString("F2");

        def.text = player.def.ToString();

        heal.text = player.healAmmount.ToString();
        healSpeed.text = player.healSpeed.ToString("F2");

        foreach (Text text in allCosts)
        {
            text.text = player.cost.ToString();
        }

        CheckHealthbar();
    }

    public void UpdateAvailability(bool state)
    {
        foreach (Button button in allButtons)
        {
            button.interactable = state;
        }
    }

    void CheckHealthbar()
    {
        //Atualiza a barra de vida
        playerHealthbar.maxValue = player.maxHealth;
        playerHealthbar.value = player.currentHealth;

        //Muda a cor da barra de vida, **melhorar ??
        if (player.currentHealth <= 0.95f * player.maxHealth)
            playerHealthbar.fillRect.GetComponentInChildren<Image>().color = Color.green;
        if (player.currentHealth <= 0.66f * player.maxHealth)
            playerHealthbar.fillRect.GetComponentInChildren<Image>().color = Color.yellow;
        if (player.currentHealth <= 0.33f * player.maxHealth)
            playerHealthbar.fillRect.GetComponentInChildren<Image>().color = Color.red;
    }

    public void AddHealthbar(Enemy enemy)
    {
        var healthbar = Instantiate(enemyHealthbarPrefab, Vector2.one, Quaternion.identity, enemyHealthbarParent);
        StartCoroutine(FollowEnemy(enemy, healthbar));
    }

    public IEnumerator FollowEnemy(Enemy enemy, GameObject healthbar)
    {
        var position = Camera.main.WorldToScreenPoint(enemy.transform.position);

        while (enemy)
        {
            position = Camera.main.WorldToScreenPoint(enemy.transform.position + (Vector3)enemyHealthbarOffset);
            healthbar.transform.position = position;

            var healthbarSlider = healthbar.GetComponent<Slider>();
            healthbarSlider.maxValue = enemy.maxHealth;
            healthbarSlider.value = enemy.currentHealth;

            if (enemy.currentHealth <= 0.95f * enemy.maxHealth)
                healthbarSlider.fillRect.GetComponentInChildren<Image>().color = Color.green;
            if (enemy.currentHealth <= 0.66f * enemy.maxHealth)
                healthbarSlider.fillRect.GetComponentInChildren<Image>().color = Color.yellow;
            if (enemy.currentHealth <= 0.33f * enemy.maxHealth)
                healthbarSlider.fillRect.GetComponentInChildren<Image>().color = Color.red;
            yield return null;
        }
        Destroy(healthbar);
        yield return null;
    }

    public void EnableAreaAttack()
    {
        areaAttackButton.interactable = true;
    }
}
