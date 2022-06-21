using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Managers.Creation.LoadProject();
        SceneManager.LoadScene(sceneName);
        if(FileManager.ProjectInfoFileIsExist(Managers.Creation.ProjectDirectory))
        {
            Managers.Creation.CreateItemAndOISList(0);
            Managers.SavingLoading.LoadInnerItems();
            //Managers.SavingLoading.LoadProject();
        }
        else
        {
            FileManager.CreateProjectInfoFile(Managers.Creation.ProjectDirectory);
            Managers.Creation.CreateItemAndOISList(0);
        }
    }

    public void LoadShowScene()
    {
        Managers.Creation.LoadProject();
        if (FileManager.ProjectInfoFileIsExist(Managers.Creation.ProjectDirectory))
        {
            SceneManager.LoadScene("Show");
            Managers.Creation.CreateItemAndOISList(0);
            Managers.SavingLoading.LoadInnerItems();
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.F2))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
