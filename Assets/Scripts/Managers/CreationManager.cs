using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreationManager : MonoBehaviour, IGameManager
{
    public GameObject SpawnPoint;
    public Mesh LightMesh;

    public ManagerStatus status { get; private set; }

    public enum ShadowType
    {
        Low = 0,
        Mid = 1,
        Hig = 2,
        VHig = 3
    }

    public ShadowType ST= ShadowType.VHig;

    public string ProjectDirectory { get; private set; }

    public Shader Opaque;
    public Shader Transparent;
    public Shader Wireframe;

    public Material OpaqueMaterial;
    public Material TransparentMaterial;

    public ItemList ItemList { get; private set; }
    public ObjectInSceneList ObjectInSceneLsit { get; private set; }

    public void Awake()
    {
        
    }

    public void Startup()
    {
        Debug.Log("CreationManager starting");
        status = ManagerStatus.Started;
    }

    public void LoadProject()
    {
        ProjectDirectory = FileManager.ChoseProjectFolder();
        Debug.Log(" ProjectDirectory is changed: " + ProjectDirectory);
    }

    public void CreateItemAndOISList(int oisListLastID)
    {
        ItemList = new ItemList(); //Исправить!
        ObjectInSceneLsit = new ObjectInSceneList(oisListLastID);
        ObjectInSceneLsit.SpawnPoint = SpawnPoint;
        ObjectInSceneLsit.LightMesh = LightMesh;
    }

    public void AddItem(string modelPath, string modelName)
    {
        string filePath = modelPath+@"\"+modelName+".obj";
        var loadedObj = new OBJLoader().Load(filePath);

        string localPath = "Assets/Prefabs/" + loadedObj.name + ".prefab";

        GameObject newPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(loadedObj, localPath, InteractionMode.UserAction);

        for (int i = 0; i < loadedObj.transform.childCount; i++)
        {
            GameObject child = loadedObj.transform.GetChild(i).gameObject; //ссылку на сзагруженый объект
            GameObject childPrefab = newPrefab.transform.GetChild(i).gameObject; //ссылка на префаб
            List<Material> mat = new List<Material>(); //материалы
            for (int j = 0; j < child.GetComponentInChildren<Renderer>().sharedMaterials.Length; j++) 
            {
                child.GetComponentInChildren<Renderer>().GetSharedMaterials(mat);
                if (mat[j].name == "Glass")
                {
                    mat[j].shader = Transparent;
                }
                else
                {
                    mat[j].shader = Opaque;
                }
            }

            Mesh meshToSave = Object.Instantiate(child.GetComponentInChildren<MeshFilter>().mesh) as Mesh;
            if (!AssetDatabase.IsValidFolder("Assets/Meshes" + modelName))
            {
               AssetDatabase.CreateFolder("Assets/Meshes", modelName);
            }
            AssetDatabase.CreateAsset(meshToSave, "Assets/Meshes/" + modelName + @"/"+child.name +".asset");
            //AssetDatabase.SaveAssets();

            childPrefab.GetComponent<MeshFilter>().mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Meshes/" + modelName + @"/" + child.name + ".asset");
            //AssetDatabase.SaveAssets();

            if (child.GetComponentInChildren<Renderer>().sharedMaterial.name == "Glass")
            {
                //childPrefab.GetComponentInChildren<Renderer>().sharedMaterial = TransparentMaterial;
                childPrefab.GetComponentInChildren<Renderer>().sharedMaterial = new Material(TransparentMaterial);
            }
            else
            {
                //childPrefab.GetComponentInChildren<Renderer>().sharedMaterial = OpaqueMaterial;
                childPrefab.GetComponentInChildren<Renderer>().sharedMaterial = new Material(OpaqueMaterial);
            }
            //AssetDatabase.SaveAssets();

            Texture2D mainTex = new Texture2D(2048, 2048);
            mainTex.LoadImage(Managers.SavingLoading.GetTextureData(modelName, child.GetComponentInChildren<Renderer>().sharedMaterial.name, "BaseColor"));
            childPrefab.GetComponentInChildren<Renderer>().sharedMaterial.SetTexture("_MainTex", mainTex);
            mainTex = null;

            Texture2D rmtTex = new Texture2D(2048, 2048);
            rmtTex.LoadImage(Managers.SavingLoading.GetTextureData(modelName, child.GetComponentInChildren<Renderer>().sharedMaterial.name, "RMT"));
            childPrefab.GetComponentInChildren<Renderer>().sharedMaterial.SetTexture("_RMT", rmtTex);
            rmtTex = null;

            Texture2D emiTex = new Texture2D(2048, 2048);
            emiTex.LoadImage(Managers.SavingLoading.GetTextureData(modelName, child.GetComponentInChildren<Renderer>().sharedMaterial.name, "Emissive"));
            childPrefab.GetComponentInChildren<Renderer>().sharedMaterial.SetTexture("_Emission", emiTex);
            
            Texture2D normalTex = new Texture2D(2048, 2048);
            normalTex.LoadImage(Managers.SavingLoading.GetTextureData(modelName, child.GetComponentInChildren<Renderer>().sharedMaterial.name, "Normal"));
            childPrefab.GetComponentInChildren<Renderer>().sharedMaterial.SetTexture("_BumpMap", normalTex);

            childPrefab.AddComponent<BoxCollider>();
            newPrefab.AddComponent<BoxCollider>();
            newPrefab.GetComponents<BoxCollider>().Last().size = childPrefab.gameObject.GetComponent<BoxCollider>().size;
            newPrefab.GetComponents<BoxCollider>().Last().center = childPrefab.gameObject.GetComponent<BoxCollider>().center;
            newPrefab.GetComponents<BoxCollider>().Last().enabled = false;
            DestroyImmediate(childPrefab.GetComponent<BoxCollider>(),true);

            childPrefab.AddComponent<MeshCollider>();
            newPrefab.AddComponent<MeshCollider>();
            newPrefab.GetComponents<MeshCollider>().Last().sharedMesh = childPrefab.gameObject.GetComponent<MeshCollider>().sharedMesh;
            DestroyImmediate(childPrefab.GetComponent<MeshCollider>(), true);

            Destroy(loadedObj);

            //AssetDatabase.SaveAssets();

            /*List<Material> matPrefab = new List<Material>();
             for (int j = 0; j < childPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials.Length; j++)
             {
                 childPrefab.GetComponentInChildren<MeshRenderer>().GetSharedMaterials(matPrefab);
                 if (mat[j].name == "Glass")
                 {
                     matPrefab[j] = TransparentMaterial;
                 }
                 else
                 {
                     matPrefab[j] = OpaqueMaterial;                    
                 }

                // childPrefab.GetComponent<MeshRenderer>().sharedMaterial = OpaqueMaterial;
               //  AssetDatabase.SaveAssets();
             } */
        }

        ItemList.AddItem(newPrefab, modelName);
        
       /* foreach(Item item in ItemList)
        {
            Managers.UI.CreateItemButton(item);
        }*/
    }

    public void DeleteAssets()
    {
        string[] unusedFolder = { "Assets/Meshes" };
        foreach (var asset in AssetDatabase.FindAssets("", unusedFolder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
    }

    public void ChangeShadowType(int st)
    {
        if (st == (int) ShadowType.Low)
        ST = ShadowType.Low;

        if (st == (int)ShadowType.Mid)
            ST = ShadowType.Mid;

        if (st == (int)ShadowType.Hig)
            ST = ShadowType.Hig;

        if (st == (int)ShadowType.VHig)
            ST = ShadowType.VHig;

        Debug.Log("Shadow TYPE "+(int)ST);
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
