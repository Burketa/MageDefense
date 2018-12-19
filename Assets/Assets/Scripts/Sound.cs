using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound instance;
    public AudioSource playerSource, enemySource, shield, musicSource;

    public AudioClip player_attack, shield_defense, enemy_damaged, enemy_attack, enemy_hit;

    public void Awake()
    {
        instance = this;
    }

    public void Play(string name)
    {
        switch (name)
        {
            case "player_attack":
                if (player_attack != null)
                    playerSource.PlayOneShot(player_attack);
                break;

            case "shield_defense":
                if (shield_defense != null)
                    shield.PlayOneShot(shield_defense);
                break;

            case "enemy_damaged":
                if (enemy_damaged != null)
                    enemySource.PlayOneShot(enemy_damaged);
                break;

            case "enemy_attack":
                if (enemy_attack != null)
                    enemySource.PlayOneShot(enemy_attack);
                break;

            default:
                break;
        }
    }
}
