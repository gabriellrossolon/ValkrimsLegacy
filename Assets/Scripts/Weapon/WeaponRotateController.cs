using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotateController : MonoBehaviour
{
    public Transform weapon;
    public float weaponDistance = 0.5f;

    [HideInInspector] public bool weaponFacingRight = true;

    void Update()
    {
        WeaponLookAtPoint();
    }

    void WeaponLookAtPoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weapon.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        
        weapon.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(weaponDistance, 0, 0);

        WeaponFlipController(mousePos);
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
