using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 1f;
    public float rotationSpeed = 1f;
    public Enemy enemy;

    public IEnumerator ToEnemy(Enemy enemy)
    {
        this.enemy = enemy;

        while (true)
        {
            //Se o inimigo original ainda estiver vivo
            if (enemy)
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
                var dir = (enemy.transform.position - transform.position).normalized;
                dir.z = 0;

                transform.Translate(transform.right * speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

                if (hits.Length > 0)
                {
                    foreach (Collider2D hit in hits)
                    {
                        var enemyHit = hit.GetComponent<Enemy>();
                        if (enemyHit != null)
                        {
                            if (enemyHit.Equals(enemy))
                            {
                                enemy.TakeDamage();
                                Destroy(gameObject);
                            }
                        }
                    }
                }

            }
            else
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
