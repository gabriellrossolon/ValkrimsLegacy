using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TeleporterPad : MonoBehaviour
{
    private GameObject transition_go;
    private CinemachineVirtualCamera virtual_camera;

    public string roomIdentifier;
    public Vector3 coords_to_go;
    public int room_cam_size;

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
            StartCoroutine(TeleportController(collision, coords_to_go, room_cam_size));
            transition_go.gameObject.GetComponent<Animator>().SetTrigger("isTransition");
        }
    }

    private IEnumerator TeleportController(Collider2D collision, Vector3 destiny_location, int camSize)
    {
        yield return new WaitForSeconds(1f);
        //ChangeCamSize(camSize);
        collision.transform.position = destiny_location;
    }

    private void ChangeCamSize(int size)
    {
        virtual_camera.m_Lens.OrthographicSize = size;
    }
}
