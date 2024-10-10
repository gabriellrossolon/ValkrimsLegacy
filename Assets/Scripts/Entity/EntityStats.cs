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
    public float base_attack_damage;
    public float attack_damage;
    public float strong_attack_cooldown;
    public float dash_attack_cooldown;
    public float attack_speed;
    public float knockback_force;

    private float original_enemy_attack_damage;
    private float original_enemy_max_hp;

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

    [Header("Xp Stats")]
    public int level = 1;
    public float xp = 0;
    [HideInInspector] public float xp_to_up;
    public int level_up_points;

    [Header("Gold Stats")]
    public float gold_carry;
    public int min_gold_chance;
    public int max_gold_chance;

    [Header("Player Ambient")]
    public string player_ambient;

    [Header("Identifier (If Necessary)")]
    public string identifier_name;

    private bool isResettingColor = false;

    void Start()
    {
        setInitialAttributes();

        actual_speed = base_speed;
        original_speed = base_speed;
        max_speed = base_speed * 1.5f;

        hp = max_hp;
        can_regen_hp = true;

        stamina = max_stamina;
        can_regen_stamina = true;

        mana = max_mana;
        can_regen_mana = true;

        xp_to_up = level * 10f;

        EnemyGoldRandomizer();

        if (this.gameObject.CompareTag("Enemy"))
        {
            original_enemy_attack_damage = attack_damage;
            original_enemy_max_hp = max_hp;
        }
        EnemyLevelBuff();

    }

    void Update()
    {
        StaminaControl();
        ManaControl();
        HpControl();

        LevelUp();
    }

    void FixedUpdate()
    {
        
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    private void setInitialAttributes()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            if (ClassManager.Instance != null)
            {
                if (ClassManager.Instance.choosedClass == "Warrior")
                {
                    max_hp = 25f;
                    max_stamina = 50f;
                    max_mana = 30f;
                    base_speed = 4000;
                }
                else if (ClassManager.Instance.choosedClass == "Mage")
                {
                    max_hp = 10f;
                    max_stamina = 40f;
                    max_mana = 70f;
                    base_speed = 4000;
                }
                else if (ClassManager.Instance.choosedClass == "Thief")
                {
                    max_hp = 15f;
                    max_stamina = 80f;
                    max_mana = 30f;
                    base_speed = 5000;
                }
            }
        }
    }

    void Death()
    {
        if(hp <= 0)
        {
            if (this.gameObject.CompareTag("Player"))
            {
                StartCoroutine(ShowGameOverHud(0.1f));
                SoundManager.Instance.misc_soundSource.PlayOneShot(SoundManager.Instance.gameover_soundClip);
                SoundManager.Instance.step_soundSource.Stop();

            }
            else if (this.gameObject.CompareTag("Enemy"))
            {
                Animator anim = this.gameObject.GetComponent<Animator>();
                if(anim != null) { anim.SetTrigger("isDie"); }
                
                this.gameObject.GetComponentInChildren<EnemyHpBar>().gameObject.SetActive(false);

                GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>().AddExp(xp);

                GameObject gold_drop = Instantiate(DropManager.Instance.gold_pilon, transform.position, Quaternion.identity);
                gold_drop.gameObject.GetComponent<DropItemBehavior>().gold_to_give = gold_carry;

                if(QuestManager.Instance.mushroom_investigation == true)
                {
                    if(identifier_name == "Mushroom")
                    {
                        QuestManager.Instance.enemy_killed += 1;
                        QuestManager.Instance.VerifyQuestCondition();
                    }
                }

                DisableEnemyComponents(
                  this.gameObject.GetComponent<MeleeEnemyMove>(),
                  this.gameObject.GetComponent<MeleeEnemyAttack>(),
                  this.gameObject.GetComponent<RangedEnemyMove>(),
                  this.gameObject.GetComponent<RangedEnemyAttack>(),
                  this.gameObject.GetComponent<BoxCollider2D>(),
                  this.gameObject.GetComponent<SortingOrder>(),
                  this.gameObject.GetComponent<CircleCollider2D>()
                );

                Destroy(this.gameObject, 2f);
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

    private void ShowDamageInCanvas(float hp_to_remove)
    {
        GameObject new_popup = Instantiate(HudManager.Instance.damage_popup, this.gameObject.transform.position, Quaternion.identity);
        new_popup.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), 5f), ForceMode2D.Impulse);
        new_popup.GetComponentInChildren<Text>().text = hp_to_remove.ToString();
        Destroy(new_popup, 1);
    }

    private Color originalColor;
    private Color[] childOriginalColors;

    private void RedDamageIndicator()
    {
        if (isResettingColor)
        {
            return; // Aguarda a corrotina atual terminar
        }

        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        SpriteRenderer[] childSpriteRenderers = this.gameObject.GetComponentsInChildren<SpriteRenderer>();

        childOriginalColors = new Color[childSpriteRenderers.Length];
        for (int i = 0; i < childSpriteRenderers.Length; i++)
        {
            childOriginalColors[i] = childSpriteRenderers[i].color;
        }

        spriteRenderer.color = Color.red;

        if (this.gameObject.CompareTag("Player"))
        {
            foreach (SpriteRenderer sr in childSpriteRenderers)
            {
                sr.color = Color.red;
            }
        }

        StartCoroutine(ResetColor(spriteRenderer, childSpriteRenderers, 0.1f));
    }

    private IEnumerator ResetColor(SpriteRenderer spriteRenderer, SpriteRenderer[] childSpriteRenderers, float delay)
    {
        isResettingColor = true;
        yield return new WaitForSeconds(delay);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;

            for (int i = 0; i < childSpriteRenderers.Length; i++)
            {
                if (childSpriteRenderers[i] != null)
                {
                    childSpriteRenderers[i].color = childOriginalColors[i];
                }
            }
        }
        isResettingColor = false;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private IEnumerator ShowGameOverHud(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        HudManager.Instance.gameover_hud.SetActive(true);
        SoundManager.Instance.music_soundSource.Stop();
        SoundManager.Instance.ambient_soundSource.Stop();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====--> Health Control <--=====//=====//=====//=====//=====//=====//=====//=====//=====//

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

    public void RemoveHp(float hp_to_remove)
    {
        RedDamageIndicator();
        if (this.gameObject.CompareTag("Enemy"))
        {
            ShowDamageInCanvas(hp_to_remove);
            Animator anim = this.gameObject.GetComponent<Animator>();
            if(anim != null) {anim.SetTrigger("isHurt"); }
        }
        if (this.gameObject.CompareTag("Player"))
        {
            HudManager.Instance.player_bars_go.GetComponent<Animator>().SetTrigger("isDamaged");
        }

        hp -= hp_to_remove;
        Death();
    }

    public void AddHp(float hp_to_add)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            hp += hp_to_add;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====--> Stamina Control <--=====//=====//=====//=====//=====//=====//=====//=====//=====//

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
        stamina -= 1f;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====--> Mana Control <--=====//=====//=====//=====//=====//=====//=====//=====//=====//

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

    //=====//=====//=====//=====//=====//=====//=====//=====--> Experience Control <--=====//=====//=====//=====//=====//=====//=====//=====//=====//

    public void AddExp(float xp_gained)
    {
        xp += xp_gained;
        LevelUp();
    }

    private void LevelUp()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            if (xp >= xp_to_up)
            {
                xp -= xp_to_up;
                level++;
                level_up_points++;

                xp_to_up = level * 10f;
                HudManager.Instance.level_text.text = level.ToString();
                SoundManager.Instance.exp_soundSource.PlayOneShot(SoundManager.Instance.levelUp_soundClip);
                ShowLevelUpInCanvas();
            }
        }
    }

    private void ShowLevelUpInCanvas()
    {
        GameObject new_popup = Instantiate(HudManager.Instance.levelUp_popup, this.gameObject.transform.position, Quaternion.identity);
        new_popup.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
        Destroy(new_popup, 1);
    }

    private void EnemyLevelBuff()
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            attack_damage = original_enemy_attack_damage + (level - 1) * 0.5f;
            max_hp = original_enemy_max_hp + (level - 1) * 1.5f;
            hp = max_hp;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====--> Gold Control <--=====//=====//=====//=====//=====//=====//=====//=====//=====//
    
    public void AddGold(float gold_gained)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.drop_soundSource.PlayOneShot(SoundManager.Instance.goldCollect_soundClip);
            gold_carry += gold_gained;
            HudManager.Instance.gold_text.text = gold_carry.ToString();
        }
    }

    private void EnemyGoldRandomizer()
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            gold_carry = Random.Range(min_gold_chance, max_gold_chance + 1);
        }
    }

}
