using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnterRoomChecker : MonoBehaviour
{
    private GameObject transition_go;
    private CinemachineVirtualCamera virtual_camera;

    private void Start()
    {
        transition_go = GameObject.FindWithTag("Transitioner");
        GameObject camObject = GameObject.FindGameObjectWithTag("CMCam");
        virtual_camera = camObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name == "EnterCamping")
            {
                StartCoroutine(EnterController(collision, new Vector3(24f, -92f, 0), 4));
                
                transition_go.gameObject.GetComponent<Animator>().SetTrigger("isTransition");
            } 
            else if (this.gameObject.name == "LeaveCamping")
            {
                StartCoroutine(EnterController(collision, new Vector3(-4.5f, 0.5f, 0), 6));
                transition_go.gameObject.GetComponent<Animator>().SetTrigger("isTransition");
            }
        }
    }

    private IEnumerator EnterController(Collider2D collision, Vector3 destiny_location, int camSize)
    {
        yield return new WaitForSeconds(1f);
        ChangeCamSize(camSize);
        collision.transform.position = destiny_location;
    }

    private void ChangeCamSize(int size)
    {
        virtual_camera.m_Lens.OrthographicSize = size;
    }
}
