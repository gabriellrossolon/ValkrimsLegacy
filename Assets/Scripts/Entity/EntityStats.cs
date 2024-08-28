using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStats : MonoBehaviour
{
    [Header("Movement Stats")]
    public float base_speed;
    public float actual_speed;
    public float max_speed;
    [HideInInspector] public float original_speed;

    [Header("Attack Stats")]
    public float attack_damage;
    public float strong_attack_cooldown;
    public float attack_speed;
    public float knockback_force;

    [Header("Health Stats")]
    public float max_hp;
    public float hp;
    public bool can_regen_hp;
    public float hp_regen_rate;
    private Coroutine hp_regen_coroutine;

    [Header("Stamina Stats")]
    public float max_stamina;
    public float stamina;
    public bool can_regen_stamina;
    public float stamina_regen_rate;
    private Coroutine stamina_regen_coroutine;

    [Header("Mana Stats")]
    public float max_mana;
    public float mana;
    public bool can_regen_mana;
    public float mana_regen_rate;
    private Coroutine mana_regen_coroutine;


    void Start()
    {
        actual_speed = base_speed;
        original_speed = base_speed;
        max_speed = base_speed * 1.5f;

        hp = max_hp;
        can_regen_hp = true;

        stamina = max_stamina;
        can_regen_stamina = true;

        mana = max_mana;
        can_regen_mana = true;
    }

    void Update()
    {
        StaminaControl();
        ManaControl();
        HpControl();
    }

    void FixedUpdate()
    {
        
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

    private void HpControl()
    {
        if (hp > max_hp)
        {
            hp = max_hp;
        }

        if (hp < 0)
        {
            hp = 0;
        }

        if (hp < max_hp && can_regen_hp && hp_regen_coroutine == null)
        {
            hp_regen_coroutine = StartCoroutine(HpRegen());
        }

        if ((hp >= max_hp || !can_regen_hp) && hp_regen_coroutine != null)
        {
            StopCoroutine(hp_regen_coroutine);
            hp_regen_coroutine = null;
        }
    }

    private IEnumerator HpRegen()
    {
        while (can_regen_hp && hp < max_hp)
        {
            hp += hp_regen_rate * Time.deltaTime;
            yield return null;
        }
        hp_regen_coroutine = null;

    }

    //REMOVE HP ESTA SEPARADO, A CIMA

    public void AddHp(float hp_to_add)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            hp += hp_to_add;
        }
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

        if (stamina < max_stamina && can_regen_stamina && stamina_regen_coroutine == null)
        {
            stamina_regen_coroutine = StartCoroutine(StaminaRegen());
        }

        if ((stamina >= max_stamina || !can_regen_stamina) && stamina_regen_coroutine != null)
        {
            StopCoroutine(stamina_regen_coroutine);
            stamina_regen_coroutine = null;
        }
    }

    private IEnumerator StaminaRegen()
    {
        while (can_regen_stamina && stamina < max_stamina)
        {
            stamina += stamina_regen_rate * Time.deltaTime;
            yield return null;
        }
        stamina_regen_coroutine = null;

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

    public void DrainStamina()
    {
        stamina -= 1.5f;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void ManaControl()
    {
        if (mana > max_mana)
        {
            mana = max_mana;
        }

        if (mana < 0)
        {
            mana = 0;
        }

        if (mana < max_mana && can_regen_mana && mana_regen_coroutine == null)
        {
            mana_regen_coroutine = StartCoroutine(ManaRegen());
        }

        if ((mana >= max_mana || !can_regen_mana) && mana_regen_coroutine != null)
        {
            StopCoroutine(mana_regen_coroutine);
            mana_regen_coroutine = null;
        }
    }

    private IEnumerator ManaRegen()
    {
        while (can_regen_mana && mana < max_mana)
        {
            mana += mana_regen_rate * Time.deltaTime;
            yield return null;
        }
        mana_regen_coroutine = null;

    }

    public void RemoveMana(float mana_to_remove)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            mana -= mana_to_remove;
        }
    }

    public void AddMana(float mana_to_add)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            mana += mana_to_add;
        }
    }
}
