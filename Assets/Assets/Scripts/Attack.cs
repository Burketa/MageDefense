using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform projectilesParent;
    public GameObject projectile;
    public bool canDamageSingle, canDamageAll;
    private Player player;
    private float currentSingleAtackTime;
    public float currentAllAtackTime;
    private Enemy currentTarget = null;
    public GameObject buttons;
    public Animator _animator;
    public Transform castPoint;

    void Start()
    {
        player = Player.instance;
        currentSingleAtackTime = player.singleAtackSpeed;
        currentAllAtackTime = 0;
    }

    void Update()
    {
        if (canDamageSingle)
            DamageVisibleEnemy();
        //if (canDamageAll)
        //DamageAllVisibleEnemies();
        if (canDamageAll)
        {
            if (currentAllAtackTime >= player.multipleAttackSpeed)
            {
                EnableAreaAttackButton();
                currentAllAtackTime = 0;
            }
            else
            {
                currentAllAtackTime += Time.deltaTime;
            }
        }
    }

    public void OnMouseDown()
    {
        var other = buttons.transform.parent.GetChild(1).gameObject;
        var otherState = other.activeSelf;
        if (otherState)
            other.SetActive(false);
        var state = buttons.activeSelf;
        buttons.SetActive(!state);
    }

    public void DamageVisibleEnemy()
    {
        (var enemies, var enemyPresent) = AreEnemiesVisible();
        if (currentSingleAtackTime >= player.singleAtackSpeed && enemyPresent)
        {
            currentTarget = ChooseTarget(enemies);
            //Atire !
            if (currentTarget != null)
            {
                //Shot(currentTarget);
                _animator.SetTrigger("attack");
            }
            currentSingleAtackTime = 0;
        }
        else
        {
            currentSingleAtackTime += Time.deltaTime;
        }
    }

    public (List<Enemy>, bool) AreEnemiesVisible()
    {
        var enemies = FindObjectsOfType<Enemy>();
        var visibleEnemies = new List<Enemy>();
        var enemyIsVisible = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.isVisible())
                visibleEnemies.Add(enemy);
        }
        if (visibleEnemies.Count > 0)
            enemyIsVisible = true;
        else
            enemyIsVisible = false;

        //só pra deixar o current como o primeiro a entrar na tela depois de não ter nenhum target.
        if (currentTarget == null && enemyIsVisible)
            currentTarget = visibleEnemies[0];

        return (visibleEnemies, enemyIsVisible);
    }

    //Escolhe o target baseado na prioridade (Atacando o shield)
    public Enemy ChooseTarget(List<Enemy> enemies)
    {
        //Quanto dano o player da ?
        var dmg = player.atk;

        //Se esse for o primeiro tiro...
        if (projectilesParent.childCount == 1)
        {
            //Checa todos os inimigos e ve se algum é prioridade, se for, retorne-o
            foreach (Enemy enemy in enemies)
            {
                if (enemy.isPriority)
                    return enemy;
            }
            //Senão, retorna o primeiro visto
            return currentTarget;
        }

        //Senão...
        else
        {
            //Quanto vai receber de dano bruto ?
            var incomingDamage = 0;

            //Soma os danos dos projeteis que sairam para atacar o monstro que esta sendo mirado
            foreach (Transform projectile in projectilesParent)
            {
                var projectileProjectile = projectile.GetComponent<Projectile>();
                if (projectileProjectile)
                {
                    if (projectileProjectile.enemy == currentTarget)
                        incomingDamage += currentTarget.def - dmg;
                }
            }
            //Se o dano total a ser desferido no inimigo for negativo ou nulo, de o dano minimo
            if (incomingDamage >= 0)
                incomingDamage = 1;

            //Se não for suficiente pra matar, continue atirando nele
            if (currentTarget.currentHealth - Mathf.Abs(incomingDamage) > 0)
                return currentTarget;

            //Se for suficiente para matar...
            else
            {
                //Marca o inimigo como ja tendo dano suficiente pra ser morto.
                currentTarget.incomingDamageIsEnoughToKill = true;

                //TODO: PQ ISSO NÃO FUNCIONA ?
                //Checa todos os inimigos e ve se algum é prioridade, se for, retorne-o
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.isPriority && enemy.incomingDamageIsEnoughToKill == false)
                        return enemy;
                }

                //Se não foi achada uma prioridade, ir para o proximo inimigo da lista
                foreach (Enemy enemy in enemies)
                {
                    if (enemy != currentTarget && enemy.incomingDamageIsEnoughToKill == false)
                        return enemy;
                }
            }

            //Caso não tenha mais inimigos para serem targetados, returna nulo.
            return null;
        }
    }

    public void Shot()//(Enemy enemy)
    {
        var shot = Instantiate(projectile, castPoint.position, transform.rotation, projectilesParent).GetComponent<Projectile>();
        shot.StartCoroutine(shot.ToEnemy(currentTarget));
    }

    public void DamageAllVisibleEnemies()
    {
        if (currentAllAtackTime >= player.multipleAttackSpeed)
        {
            GameObject.Find("fall").GetComponent<Animation>().Play("fall");
            currentAllAtackTime = 0;
        }
        else
        {
            currentAllAtackTime += Time.deltaTime;
        }

    }
    public void DamageAllVisibleEnemiesButton()
    {
        GameObject.Find("fall").GetComponent<Animation>().Play("fall");
        currentAllAtackTime = 0;
    }

    public void EnableAreaAttackButton()
    {
        UI.instance.EnableAreaAttack();
    }
}
