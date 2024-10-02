using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotateController : MonoBehaviour
{
    public Transform weapon;
    public Transform sword_parent;
    public float weaponDistance = 0.5f;

    public Transform sword_transform;
    public Transform staff_transform;
    public Transform bow_transform;

    [HideInInspector] public bool weaponFacingRight = true;
    public float flipDuration = 0.5f;

    void Update()
    {
        
    }

    void LateUpdate()
    {
        WeaponLookAtPoint();
    }

    void WeaponLookAtPoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

       
        if (weapon.gameObject.name == "Sword")
        {
            WeaponFlipController(mousePos);
            //weapon.rotation = Quaternion.Euler(weapon.rotation.eulerAngles.x, weapon.rotation.eulerAngles.y, 0);
            //sword_parent.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(weaponDistance, 0, 0);

            sword_parent.rotation = Quaternion.Euler(weapon.rotation.eulerAngles.x, weapon.rotation.eulerAngles.y, angle);
            sword_parent.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(weaponDistance, 0, 0);
        } 
        else
        {
            weapon.rotation = Quaternion.Euler(weapon.rotation.eulerAngles.x, weapon.rotation.eulerAngles.y, angle);
            weapon.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(weaponDistance, 0, 0);

            //WeaponFlipController(mousePos); //
        }
    }

    private void WeaponFlipController(Vector3 mousePos)
    {
        if (mousePos.x < transform.position.x && weaponFacingRight)
        {
            //StartCoroutine(WeaponFlipSmooth());
            WeaponFlip();
        }
        else if (mousePos.x > transform.position.x && !weaponFacingRight)
        {
            //StartCoroutine(WeaponFlipSmooth());
            WeaponFlip();
        }
    }

    private void WeaponFlip()
    {
        weaponFacingRight = !weaponFacingRight;
        weapon.localScale = new Vector3(weapon.localScale.x, weapon.localScale.y * -1, weapon.localScale.z); //
    }

    private IEnumerator WeaponFlipSmooth()
    {
        weaponFacingRight = !weaponFacingRight;

        Vector3 initialScale = weapon.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, initialScale.y * -1, initialScale.z);
        float elapsedTime = 0;
        while (elapsedTime < flipDuration)
        {
            weapon.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / flipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        weapon.localScale = targetScale;
    }
}
