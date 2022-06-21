using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSceneController : MonoBehaviour
{
    public GameObject SpawnPoint;
    public GameObject PlayerCameraPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = Managers.Creation.ObjectInSceneLsit.AddSpawnPoint(PlayerCameraPrefab);
        Managers.SavingLoading.LoadProject();
        SpawnPoint.GetComponent<SpawnPoint>().DeletePlayerCamrea();
        SpawnPoint.GetComponent<SpawnPoint>().CreatePlayerCamera();
        Managers.Creation.ObjectInSceneLsit.DisableMeshOnLight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
