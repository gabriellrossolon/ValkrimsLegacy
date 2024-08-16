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

    public float max_hp;
    public float hp;

    public float attack_speed;

    public float knockback_force;

    void Start()
    {
        actual_speed = base_speed;
        original_speed = base_speed;
        max_speed = base_speed * 1.5f;
        hp = max_hp;
    }


    void Update()
    {
        
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    void Death()
    {
        if(hp <= 0)
        {
            if (this.gameObject.CompareTag("Player"))
            {
                Time.timeScale = 0;
            } 
            else if (this.gameObject.CompareTag("Enemy"))
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("isDie");
                this.gameObject.GetComponent<MeleeEnemyMove>().enabled = false;
                this.gameObject.GetComponent<MeleeEnemyAttack>().enabled = false;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                this.gameObject.GetComponent<SortingOrder>().enabled = false;
                StartCoroutine(DestroyObject(4f));
            }
        }
    }

    public void RemoveHp(float hp_to_remove)
    {
        GameObject new_popup = Instantiate(HudManager.Instance.damage_popup, this.gameObject.transform.position, Quaternion.identity);
        new_popup.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), 5f), ForceMode2D.Impulse);
        new_popup.GetComponentInChildren<Text>().text = hp_to_remove.ToString();
        Destroy(new_popup, 1);

        hp -= hp_to_remove;

        Death();
    }

    private IEnumerator DestroyObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
}
