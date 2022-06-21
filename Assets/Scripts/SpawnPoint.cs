using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject PlayerCameraPrefab { get; set; }
    public GameObject PlayerCamera { get; set; }

    public void SetPlayerCameraPrefab(GameObject playerCameraPrefab)
    {
        PlayerCameraPrefab = playerCameraPrefab;
    }

    public void CreatePlayerCamera()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Managers.Creation.ObjectInSceneLsit.DisableMeshOnLight();
        PlayerCamera = Instantiate(PlayerCameraPrefab,transform.position,Quaternion.identity);
    }

    public void DeletePlayerCamrea()
    {
        if (PlayerCamera != null)
        {
            PlayerCamera.GetComponent<PCM>().OffUI();
            Destroy(PlayerCamera);
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}
