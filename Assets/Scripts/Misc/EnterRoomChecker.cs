using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomChecker : MonoBehaviour
{
    private GameObject transition_go;

    private void Start()
    {
        transition_go = GameObject.FindWithTag("Transitioner");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name == "EnterCamping")
            {
                StartCoroutine(EnterController(collision, new Vector3(21.5f, -40f, 0)));
                transition_go.gameObject.GetComponent<Animator>().SetTrigger("isTransition");
            } 
            else if (this.gameObject.name == "LeaveCamping")
            {
                StartCoroutine(EnterController(collision, new Vector3(-4.5f, 0.5f, 0)));
                transition_go.gameObject.GetComponent<Animator>().SetTrigger("isTransition");
            }
        }
    }

    private IEnumerator EnterController(Collider2D collision, Vector3 destiny_location)
    {
        yield return new WaitForSeconds(1f);
        collision.transform.position = destiny_location;

    }
}
