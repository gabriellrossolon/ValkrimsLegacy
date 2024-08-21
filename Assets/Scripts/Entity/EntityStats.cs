using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStats : MonoBehaviour
{
    public float base_speed;
    public float actual_speed;
    public float max_speed;
    [HideInInspector] public float original_speed;

    public float attack_damage;
    public float strong_attack_cooldown;
    public float attack_speed;
    public float knockback_force;

    public float max_hp;
    public float hp;

    public float max_stamina;
    public float stamina;
    public bool can_regen_stamina;
    public float stamina_regen_rate;

    private Coroutine regen_coroutine;

    void Start()
    {
        actual_speed = base_speed;
        original_speed = base_speed;
        max_speed = base_speed * 1.5f;

        hp = max_hp;
        stamina = max_stamina;
        can_regen_stamina = true;
    }


    void FixedUpdate()
    {
        StaminaControl();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    void Death()
    {
        if(hp <= 0)
        {
            if (this.gameObject.CompareTag("Player"))
            {
                StartCoroutine(ShowGameOverHud(0.5f));
                
            } 
            else if (this.gameObject.CompareTag("Enemy"))
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("isDie");
                this.gameObject.GetComponentInChildren<EnemyHpBar>().gameObject.SetActive(false);

                DisableEnemyComponents(
                  this.gameObject.GetComponent<MeleeEnemyMove>(),
                  this.gameObject.GetComponent<MeleeEnemyAttack>(),
                  this.gameObject.GetComponent<BoxCollider2D>(),
                  this.gameObject.GetComponent<SortingOrder>(),
                  this.gameObject.GetComponent<CircleCollider2D>()
                );

                Destroy(this.gameObject, 4f);
            }
        }
    }

    void DisableEnemyComponents(params Behaviour[] components)
    {
        foreach (var component in components)
        {
            if (component != null)
            {
                component.enabled = false;
            }
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    public void RemoveHp(float hp_to_remove)
    {
        RedDamageIndicator();
        if (this.gameObject.CompareTag("Enemy"))
        {
            ShowDamageInCanvas(hp_to_remove);
            this.gameObject.GetComponent<Animator>().SetTrigger("isHurt");
        }

        if (this.gameObject.CompareTag("Player"))
        {

        }

        hp -= hp_to_remove;
        Death(); 
    }

    private void ShowDamageInCanvas(float hp_to_remove)
    {
        GameObject new_popup = Instantiate(HudManager.Instance.damage_popup, this.gameObject.transform.position, Quaternion.identity);
        new_popup.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), 5f), ForceMode2D.Impulse);
        new_popup.GetComponentInChildren<Text>().text = hp_to_remove.ToString();
        Destroy(new_popup, 1);
    }

    private void RedDamageIndicator()
    {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor(spriteRenderer, 0.1f));
    }

    private IEnumerator ResetColor(SpriteRenderer spriteRenderer, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private IEnumerator ShowGameOverHud(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        HudManager.Instance.gameover_hud.SetActive(true);

    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//


    private void StaminaControl()
    {
        if (stamina > max_stamina)
        {
            stamina = max_stamina;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }

        if (stamina < max_stamina && can_regen_stamina && regen_coroutine == null)
        {
            regen_coroutine = StartCoroutine(StaminaRegen());
        }

        if ((stamina >= max_stamina || !can_regen_stamina) && regen_coroutine != null)
        {
            StopCoroutine(regen_coroutine);
            regen_coroutine = null;
        }
    }

    private IEnumerator StaminaRegen()
    {
        while (can_regen_stamina && stamina < max_stamina)
        {
            stamina += stamina_regen_rate * Time.deltaTime;
            yield return null;
        }
        regen_coroutine = null;

    }

    public void RemoveStamina(float stamina_to_remove)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            stamina -= stamina_to_remove;
        }
    }

    public void AddStamina(float stamina_to_add)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            stamina += stamina_to_add;
        }
    }
}
