using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static CreationManager Creation { get; private set; }
    public static SavingLoadingManager SavingLoading { get; private set; }
    public static SceneController Scene { get; private set; }
    //public static UIController UI { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Scene = GetComponent<SceneController>();
        Creation = GetComponent<CreationManager>();
        SavingLoading = GetComponent<SavingLoadingManager>();
       // UI = GetComponent<UIController>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Creation);
        _startSequence.Add(SavingLoading);
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach (IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }
            if (numReady > lastReady)
            Debug.Log("Progress: " + numReady + "/" + numModules);
            yield return null; 
        }
        Debug.Log("All managers started up");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
