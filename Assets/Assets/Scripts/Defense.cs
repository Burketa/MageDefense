using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    public float currentHealSpeed;
    private Player player;

    public GameObject buttons;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<Player>();
        currentHealSpeed = player.healSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player.currentHealth < player.maxHealth)
        {
            if (currentHealSpeed >= player.healSpeed)
            {
                player.currentHealth = Mathf.Clamp(player.currentHealth + player.healAmmount, 0, player.maxHealth);
                currentHealSpeed = 0;
            }
            else
            {
                currentHealSpeed += Time.deltaTime;
            }
        }
	}

    public void OnMouseDown()
    {
        var other = buttons.transform.parent.GetChild(0).gameObject;
        var otherState = other.activeSelf;
        if (otherState)
            other.SetActive(false);
        var state = buttons.activeSelf;
        buttons.SetActive(!state);
    }
}
