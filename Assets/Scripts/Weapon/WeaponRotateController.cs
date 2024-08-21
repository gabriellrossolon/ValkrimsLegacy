using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotateController : MonoBehaviour
{
    public Transform weapon;
    public float weaponDistance = 0.5f;

    public Transform sword_transform;
    public Transform staff_transform;
    public Transform bow_transform;

    [HideInInspector] public bool weaponFacingRight = true;

    void Update()
    {
        WeaponLookAtPoint();
    }

    void WeaponLookAtPoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weapon.rotation = Quaternion.Euler(weapon.rotation.eulerAngles.x, weapon.rotation.eulerAngles.y, angle);

        
        weapon.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(weaponDistance, 0, 0);

        if (weapon.gameObject.name == "Sword")
        {
            WeaponFlipController(mousePos);
        } 
        else if (weapon.gameObject.name == "Staff")
        {

        }
        
    }

    private void WeaponFlipController(Vector3 mousePos)
    {
        if (mousePos.x < weapon.position.x && weaponFacingRight)
        {
            WeaponFlip();
        }
        else if (mousePos.x > weapon.position.x && !weaponFacingRight)
        {
            WeaponFlip();
        }
    }

    private void WeaponFlip()
    {
        weaponFacingRight = !weaponFacingRight;
        weapon.localScale = new Vector3(weapon.localScale.x * -1, weapon.localScale.y, weapon.localScale.z);
    }
}
