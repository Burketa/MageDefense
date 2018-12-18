using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attack")]
    public int atk;
    public float atakSpeed = 1.0f;

    [Header("Defense")]
    public int def;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Misc")]
    public float baseSpeed;
    public float currentSpeed;
    public bool isPriority = false;

    private ParticleSystem enemyParticleSystem;

    public bool incomingDamageIsEnoughToKill = false;

    private Plane[] planes;

    private Collider2D _collider;

    private void Awake()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        _collider = GetComponent<Collider2D>();
        enemyParticleSystem = transform.parent.GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * currentSpeed);
        if (currentHealth <= 0)
        {
            enemyParticleSystem.transform.position = transform.position;
            enemyParticleSystem.Play();
            FindObjectOfType<Player>().AddSouls(maxHealth * 2);
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var colliderTag = collision.collider.tag;
        if (colliderTag.Equals("AreaAttack"))
            TakeDamage();
        if (colliderTag.Equals("Shield"))
        {
            currentSpeed = 0;
            isPriority = true;
            StartCoroutine(DoDamage());
        }
    }

    //TODO: metodo para dar dano na barreira, na barreira !.
    public IEnumerator DoDamage()
    {
        var player = FindObjectOfType<Player>();
        while (currentHealth > 0)
        {
            player.TakeDamage(atk);
            if (GetComponent<Animation>() != null && this != null)
                GetComponent<Animation>().Play("attack");
            yield return new WaitForSeconds(0.4f);
            EZCameraShake.CameraShaker.Instance.Shake(EZCameraShake.CameraShakePresets.Bump);
            yield return new WaitForSeconds(atakSpeed);
            yield return null;
        }
    }

    public bool isVisible()
    {
        //return GetComponent<Renderer>().isVisible;
        if (GeometryUtility.TestPlanesAABB(planes, _collider.bounds))
            return true;
        else
            return false;
    }

    public void TakeDamage()
    {
        var dmg = FindObjectOfType<Player>().atk;
        var incomingDmg = dmg - def;
        if (incomingDmg > 0)
            currentHealth -= incomingDmg;
        else
            currentHealth--;

        if (GetComponent<Animation>() != null && this != null)
            GetComponent<Animation>().PlayQueued("takeDamage");
        if (!isPriority)
            transform.position -= (Vector3)Vector2.left * currentSpeed * Time.deltaTime * 2;

        if (currentSpeed != 0)
            currentSpeed = Mathf.Clamp(currentSpeed * (currentHealth / maxHealth), baseSpeed / 2, baseSpeed);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
    }
}
