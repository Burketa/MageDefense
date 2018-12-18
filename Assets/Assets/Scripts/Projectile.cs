using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;

    public Enemy enemy;

    //private ParticleSystem projectileParticleSystem;

    public IEnumerator ToEnemy(Enemy enemy)
    {
        this.enemy = enemy;

        while (enemy)
        {
            var dir = (enemy.transform.position - transform.position).normalized;
            dir.z = 0;

            transform.Translate(transform.right * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            var objHit = Physics2D.OverlapCircleAll(transform.position, 0.1f);

            if (objHit.Length > 0)
            {
                foreach (Collider2D hit in objHit)
                {
                    var enemyHit = hit.GetComponent<Enemy>();
                    if (enemyHit != null)
                    {
                        if (enemyHit.Equals(enemy))
                        {
                            enemy.TakeDamage();
                            AreaDamage(enemy);
                            Destroy(gameObject);
                        }
                    }
                }
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    public void AreaDamage(Enemy mainTarget)
    {
        var radius = FindObjectOfType<Player>().atkRadius;

        var areaHit = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D enemyHit in areaHit)
        {
            var enemy = enemyHit.GetComponent<Enemy>();
            if (enemy != null && enemy != mainTarget)
            {
                enemy.TakeDamage();
            }
        }

        var projectileParticleSystem = transform.parent.GetComponentInChildren<ParticleSystem>();
        projectileParticleSystem.transform.position = transform.position;
        projectileParticleSystem.Play();
    }

    // void OnDrawGizmos()
    // {
    // Draw a yellow sphere at the transform's position
    // Gizmos.color = Color.red;
    //Gizmos.DrawSphere(transform.position, areaRadius);
    //}
}
