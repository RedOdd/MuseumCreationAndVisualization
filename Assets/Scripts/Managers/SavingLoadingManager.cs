using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingLoadingManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        Debug.Log("SaveLoadManager starting");
        status = ManagerStatus.Started;
    }

    public void LoadCreation(string projectDirectory)
    {
        FileManager.LoadOrCreateProjectInfoFile(projectDirectory);
    }

    public void AddIternalItems()
    {
        FileManager.CopyIternalModelsToFolder(FileManager.ChooseIternalModelsFolder(), Managers.Creation.ProjectDirectory);
        string[] dirs = FileManager.GetModelsDirectories(Managers.Creation.ProjectDirectory);
        for (int i = 0; i < dirs.Length; i++)
        {
            Managers.Creation.AddItem(dirs[i], dirs[i].Split('\\').Last());
            string[] filesPath = FileManager.GetModelsFilesPath(dirs[i]);
            for (int j = 0; j < filesPath.Length; j++)
            {
                //Debug.Log(filesPath[j]);
            }
        }
    }

    public void LoadInnerItems()
    {
        string[] dirs = FileManager.GetModelsDirectories(Managers.Creation.ProjectDirectory);
        for (int i = 0; i < dirs.Length; i++)
        {
            Managers.Creation.AddItem(dirs[i], dirs[i].Split('\\').Last());
            string[] filesPath = FileManager.GetModelsFilesPath(dirs[i]);
            for (int j = 0; j < filesPath.Length; j++)
            {
                //Debug.Log(filesPath[j]);
            }
        }
    }

    public byte[] GetTextureData(string modelName, string materialName, string textureName)
    {
        //Debug.Log(Managers.Creation.ProjectDirectory + @"/Models/" + modelName + "_" + materialName + "_" + textureName + ".png");
        return FileManager.GetDataFromFile(Managers.Creation.ProjectDirectory + @"/Models/" + modelName + @"/"+modelName + "_" + materialName + "_" + textureName + ".png");
    }

    public byte[] GetPhotoData(int oisID,int photoID)
    {
        return FileManager.GetDataFromFile(Managers.Creation.ProjectDirectory + @"/Photos" + @"/" + oisID + @"/" + photoID + ".png");
    }

    public void SaveProject()
    {
        string filepath = Managers.Creation.ProjectDirectory+ @"/projectInfo";
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);

        Save save = new Save();

        save.SaveObjectsInScene(Managers.Creation.ObjectInSceneLsit);

        bf.Serialize(fs, save);

        fs.Close();
    }

    public void LoadProject()
    {
        string filepath = Managers.Creation.ProjectDirectory + @"/projectInfo";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filepath, FileMode.Open);
        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        Managers.Creation.ObjectInSceneLsit.lastID = save.oisLastID;

        foreach (var oisSD in save.oisSD)
        {
            Managers.Creation.ObjectInSceneLsit.LoadObjectInScene(oisSD);
        }
    }


}

[System.Serializable]
public class Save
{

    public int oisLastID;

    public Save()
    {
        oisLastID = Managers.Creation.ObjectInSceneLsit.lastID;
    }

    [System.Serializable]
    public struct Vec3
    {
        public float x, y, z;
        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [System.Serializable]
    public struct Quant
    {
        public float x, y, z, w;
        public Quant(float x,float y, float z,float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    [System.Serializable]
    public struct Col
    {
        public float r, g, b, a;
        public Col(float r,float g,float b,float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    public List<ObjectInSceneSaveData> oisSD = new List<ObjectInSceneSaveData>();
    [System.Serializable]
    public struct ObjectInSceneSaveData
    {

        public Vec3 Pos;
        public Quant Rot;
        public Vec3 Sca;
        public bool ScaleWasChanged;

        public int ID;
        public string Name;

        public bool IsExh;
        public string ExhName;
        public string ExhDiscr;
        public int[] PhotosID;
        public int LastPhotoID;


        public bool IsLight;
        public Col Colo;
        public float Intens;
        public float Range;


        public ObjectInSceneSaveData(GameObject oisGO)
        {
            Pos = new Vec3(oisGO.transform.position.x, oisGO.transform.position.y, oisGO.transform.position.z);
            Rot = new Quant(oisGO.transform.rotation.x, oisGO.transform.rotation.y, oisGO.transform.rotation.z, oisGO.transform.rotation.w);
            Sca = new Vec3(oisGO.transform.localScale.x, oisGO.transform.localScale.y, oisGO.transform.localScale.z);
            ScaleWasChanged = oisGO.GetComponent<ObjectInScene>().scaleWasChange;

            ID = oisGO.GetComponent<ObjectInScene>().ID;
            Name = oisGO.GetComponent<ObjectInScene>().Name;

            IsLight = oisGO.GetComponent<ObjectInScene>().IsLight;
            if (IsLight)
            {
                Colo = new Col(oisGO.GetComponent<Light>().color.r, oisGO.GetComponent<Light>().color.g, oisGO.GetComponent<Light>().color.b, oisGO.GetComponent<Light>().color.a);
                Intens = oisGO.GetComponent<Light>().intensity;
                Range = oisGO.GetComponent<Light>().range;
            }
            else
            {
                Colo = new Col(1, 1, 1, 1);
                Intens = 1;
                Range = 10;
            }

            IsExh = oisGO.GetComponent<ObjectInScene>().IsExhibit;
            if (oisGO.GetComponent<Exhibit>() != null) 
            { 
                ExhName = oisGO.GetComponent<Exhibit>().Name;
                ExhDiscr = oisGO.GetComponent<Exhibit>().Discription;
                PhotosID = new int[oisGO.GetComponent<Exhibit>().photos.Photos.Count];
                int i = 0;
                foreach(Photo ph in oisGO.GetComponent<Exhibit>().photos)
                {
                    PhotosID[i] = ph.ID;
                    i++;
                }
                LastPhotoID = oisGO.GetComponent<Exhibit>().photos.LastID;
            } 
            else
            {
                ExhName = "";
                ExhDiscr = "";
                PhotosID = new int[0];
                int i = 0;
                LastPhotoID = 0;
            }
        }
    }

    public void SaveObjectsInScene(ObjectInSceneList oisList)
    {
        foreach (ObjectInScene ois in oisList)
        {
            oisSD.Add(new ObjectInSceneSaveData(ois.gameObject));
        }
    }


}
