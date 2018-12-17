using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public Text atk;
    public Text singleSpeed;
    public Text multipleSpeed;

    public Text def;

    public Text heal;
    public Text healSpeed;

    public Button atkButton;
    public Text atkCost;

    public Button speedButton;
    public Text speedCost;

    public Button defButton;
    public Text defCost;

    public Button healButton;
    public Text healCost;

    private Player player;

    private Button[] allButtons;
    private Text[] allCosts;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        allButtons = new Button[] { atkButton, speedButton, defButton, healButton};

        allCosts = new Text[] { atkCost, speedCost, defCost, healCost };
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

        foreach(Text text in allCosts)
        {
            text.text = player.cost.ToString();
        }
    }

    public void UpdateAvailability(bool state)
    {
        foreach (Button button in allButtons)
        {
              button.interactable = state;
        }
    }
}
