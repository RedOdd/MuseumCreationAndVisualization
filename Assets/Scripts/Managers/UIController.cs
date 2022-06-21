using RuntimeGizmos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject SpawnPoint;
    public GameObject SpawnPointPanel;
    public GameObject PlayerCameraPrefab;

    public GameObject MainCamera;
    public GameObject TopCamera;
    public GameObject ItemCamera;
    public GameObject DirLight;
    
    public GameObject itemButton;
    public GameObject oisPanel; 

    public GameObject posXInput;
    public GameObject posYInput;
    public GameObject posZInput;
    public GameObject rotXInput;
    public GameObject rotYInput;
    public GameObject rotZInput;
    public GameObject scaXInput;
    public GameObject scaYInput;
    public GameObject scaZInput;

    public GameObject can;
    public GameObject ps;
    public GameObject oiss;
    public GameObject selectedOISSButton;

    public GameObject ToggleAndExhPanel;

    public GameObject ExhButton;
    public GameObject ExhPannel;
    public GameObject NameExhInput;
    public GameObject PhotoButton;
    public GameObject DiscrExhInput;
    public GameObject PhotoScrollContent;

    public GameObject LightPannel;
    public FlexibleColorPicker fcp;
    public GameObject RangeInput;
    public GameObject IntensityInput;

    public GameObject wireButton;
    public bool isWireframe = false;

    List<GameObject> itemButtonList;
    List<GameObject> oisPanelList;

    public Sprite eyeClose;
    public Sprite eyeUp;

    GameObject GONowSelect;
    private void Awake()
    {
        oisPanelList = new List<GameObject>();
        
    }

    private void Start()
    {
        SpawnPoint = Managers.Creation.ObjectInSceneLsit.AddSpawnPoint(PlayerCameraPrefab);
        SpawnPointPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().SetInfo(SpawnPoint, 0, "SpawnPoint");
        SpawnPointPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OISHideButtonClicked(SpawnPointPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().oisGO, SpawnPointPanel));
        LoadInnerItems();
        Managers.SavingLoading.LoadProject();
    }
    public void LoadNewCreationScene()
    {
        Managers.Creation.LoadProject();
        Managers.Scene.LoadScene("Creation");
    }

    public void AddIternalItems()
    {
        itemButtonList = new List<GameObject>();
        Managers.SavingLoading.AddIternalItems();
        //SetCanvasObject();
        ClearPS();
        foreach(Item item in Managers.Creation.ItemList)
        {
            GameObject itemGO = Instantiate(item.Prefab, new Vector3(0, -100, 0), Quaternion.identity);
            float maxSize=0;
            float yCentre=0;
            foreach (BoxCollider bc in itemGO.GetComponents<BoxCollider>())
            {
                Vector3 bcSize = bc.size;
                if (bcSize.x > maxSize)
                {
                    maxSize = bcSize.x;
                    yCentre = bc.center.y;
                }
                else if (bcSize.y > maxSize)
                {
                    maxSize = bcSize.y;
                    yCentre = bc.center.y;
                }
                else if (bcSize.z > maxSize)
                {
                    maxSize = bcSize.z;
                    yCentre = bc.center.y;
                }
            }
            /*foreach (Transform child in itemGO.transform)
            {
                child.gameObject.AddComponent<BoxCollider>();
                

                Vector3 childSize = child.gameObject.GetComponent<BoxCollider>().size;
                if (childSize.x > maxSize)
                {
                    maxSize = childSize.x;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                } else if ( childSize.y > maxSize )
                {
                    maxSize = childSize.y;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                } else if (childSize.z > maxSize)
                {
                    maxSize = childSize.z;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                }

                //child.gameObject.de
            }*/

            DirLight.SetActive(true);
            ItemCamera.transform.SetPositionAndRotation(new Vector3(ItemCamera.transform.position.x, -100 + yCentre, -10 - maxSize),Quaternion.identity);
            ItemCamera.GetComponent<Camera>().orthographicSize= maxSize;
            Texture2D tex = RTImage(ItemCamera.GetComponent<Camera>());
            DestroyImmediate(itemGO);
            Sprite spr = Sprite.Create(tex,
                                        new Rect(0,0, ItemCamera.GetComponent<Camera>().activeTexture.width, ItemCamera.GetComponent<Camera>().activeTexture.height),
                                        new Vector2(0.5f,0.5f));
            CreateItemButton(item,spr);
            DirLight.SetActive(false);
        }

        foreach (GameObject but in itemButtonList)
        {
            but.GetComponent<Button>().onClick.AddListener(()=>itemButtonClicked(but.GetComponent<ItemButton>().Item));
        }

        for (int i =1; i< Managers.Creation.ObjectInSceneLsit.objectsInScene.Count; i++)
        {
            Managers.Creation.ObjectInSceneLsit.UpdateObjectInScene(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].ID,
                                                                    Managers.Creation.ItemList.TakeItem(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].Name).Prefab);
            UpdateOISSPanel(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].gameObject,
                            Managers.Creation.ObjectInSceneLsit.objectsInScene[i].ID,
                            Managers.Creation.ObjectInSceneLsit.objectsInScene[i].Name);
            //DeleteOISSPanel(id);
        }
    }

    public void LoadInnerItems()
    {
        itemButtonList = new List<GameObject>();
        ClearPS();
        foreach (Item item in Managers.Creation.ItemList)
        {
            GameObject itemGO = Instantiate(item.Prefab, new Vector3(0, -100, 0), Quaternion.identity);
            float maxSize = 0;
            float yCentre = 0;
            foreach (BoxCollider bc in itemGO.GetComponents<BoxCollider>())
            {
                Vector3 bcSize = bc.size;
                if (bcSize.x > maxSize)
                {
                    maxSize = bcSize.x;
                    yCentre = bc.center.y;
                }
                else if (bcSize.y > maxSize)
                {
                    maxSize = bcSize.y;
                    yCentre = bc.center.y;
                }
                else if (bcSize.z > maxSize)
                {
                    maxSize = bcSize.z;
                    yCentre = bc.center.y;
                }
            }
            /*foreach (Transform child in itemGO.transform)
            {
                child.gameObject.AddComponent<BoxCollider>();
                

                Vector3 childSize = child.gameObject.GetComponent<BoxCollider>().size;
                if (childSize.x > maxSize)
                {
                    maxSize = childSize.x;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                } else if ( childSize.y > maxSize )
                {
                    maxSize = childSize.y;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                } else if (childSize.z > maxSize)
                {
                    maxSize = childSize.z;
                    yCentre = child.gameObject.GetComponent<BoxCollider>().center.y;
                }

                //child.gameObject.de
            }*/

            DirLight.SetActive(true);
            ItemCamera.transform.SetPositionAndRotation(new Vector3(ItemCamera.transform.position.x, -100 + yCentre, -10 - maxSize), Quaternion.identity);
            ItemCamera.GetComponent<Camera>().orthographicSize = maxSize;
            Texture2D tex = RTImage(ItemCamera.GetComponent<Camera>());
            DestroyImmediate(itemGO);
            Sprite spr = Sprite.Create(tex,
                                        new Rect(0, 0, ItemCamera.GetComponent<Camera>().activeTexture.width, ItemCamera.GetComponent<Camera>().activeTexture.height),
                                        new Vector2(0.5f, 0.5f));
            CreateItemButton(item, spr);
            DirLight.SetActive(false);
        }

        foreach (GameObject but in itemButtonList)
        {
            but.GetComponent<Button>().onClick.AddListener(() => itemButtonClicked(but.GetComponent<ItemButton>().Item));
        }

        for (int i = 1; i < Managers.Creation.ObjectInSceneLsit.objectsInScene.Count; i++)
        {
            Managers.Creation.ObjectInSceneLsit.UpdateObjectInScene(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].ID,
                                                                    Managers.Creation.ItemList.TakeItem(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].Name).Prefab);
            UpdateOISSPanel(Managers.Creation.ObjectInSceneLsit.objectsInScene[i].gameObject,
                            Managers.Creation.ObjectInSceneLsit.objectsInScene[i].ID,
                            Managers.Creation.ObjectInSceneLsit.objectsInScene[i].Name);
            //DeleteOISSPanel(id);
        }
    }

    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
    public void PhotoButtonClicked()
    {
        GameObject oisGo = selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO;

        GameObject photoButGo = Instantiate(PhotoButton, PhotoScrollContent.transform);
        photoButGo.GetComponent<PhotoButton>().SetInfo(oisGo.GetComponent<Exhibit>().photos.LastID, oisGo.GetComponent<ObjectInScene>().ID);

        int photoID = oisGo.GetComponent<Exhibit>().photos.LastID;

        FileManager.CopyPhotoToFolder(FileManager.ChoosePhotoFile(), oisGo.GetComponent<ObjectInScene>().ID, photoID, Managers.Creation.ProjectDirectory);
        Texture2D tex = new Texture2D(1,1);
        tex.LoadImage(Managers.SavingLoading.GetPhotoData(oisGo.GetComponent<ObjectInScene>().ID, photoID));
        Sprite spr = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
        
        oisGo.GetComponent<Exhibit>().AddPhoto(spr);
        photoButGo.GetComponent<Image>().sprite = oisGo.GetComponent<Exhibit>().photos.GetPhoto(photoID).Sprite;
        photoButGo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DeletePhotoButton(oisGo, photoID); } );
    }

    public void DeletePhotoButton(GameObject oisGO, int photoID) 
    {
        Debug.Log(0);
        oisGO.GetComponent<Exhibit>().DeletePhoto(photoID);
        FileManager.DeletePhoto(oisGO.GetComponent<ObjectInScene>().ID, photoID);
        /*foreach(Transform child in PhotoScrollContent.transform)
        {
            if (child.GetComponent<PhotoButton>() != null) {
                if (child.GetComponent<PhotoButton>().photoID == photoID)
                {
                    Destroy(child);
                }
            }
        }*/
    }

    public void ClearPhotoScroll()
    {
        foreach (Transform child in PhotoScrollContent.transform)
        {
            if (child.name != "AddPhotoButton")
                GameObject.Destroy(child.gameObject);
        }
    }

    public void UpdatePhotoScroll()
    {
        //if ((selectedOISSButton != null) & (selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO !=null) & (selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO.GetComponent<Exhibit>().photos !=null))
        {
            GameObject oisGo = selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO;
           // Debug.Log(3);
            foreach (Photo ph in oisGo.GetComponent<Exhibit>().photos)
            {
                GameObject photoButGo = Instantiate(PhotoButton, PhotoScrollContent.transform);
                photoButGo.GetComponent<PhotoButton>().SetInfo(ph.ID, oisGo.GetComponent<ObjectInScene>().ID);
                photoButGo.GetComponent<Image>().sprite = ph.Sprite;
                photoButGo.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DeletePhotoButton(oisGo, ph.ID); });
            }
        }
    }
    
    public void ClearPS() 
    {
       foreach( Transform child in ps.transform)
        {
            if (child.name != "AddItemsButton")
            GameObject.Destroy(child.gameObject);
        }
    }
    
    public void CreateItemButton(Item item,Sprite sprite)
    {  
        GameObject newItemButton = Instantiate(itemButton, ps.transform);
        newItemButton.GetComponent<ItemButton>().SetItem(item);
        newItemButton.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        itemButtonList.Add(newItemButton);
    }

    void itemButtonClicked(Item item)
    {
        GameObject tempGO = Managers.Creation.ObjectInSceneLsit.AddObjectInScene(item.Prefab, item.Name);
        AddOISSPanel(tempGO,tempGO.GetComponent<ObjectInScene>().ID,tempGO.GetComponent<ObjectInScene>().Name);
    }

    public void LightButtonClicked()
    {
        GameObject lightGO = Managers.Creation.ObjectInSceneLsit.AddLightInScene();
        AddOISSPanel(lightGO, lightGO.GetComponent<ObjectInScene>().ID, lightGO.GetComponent<ObjectInScene>().Name);
    }

    public void OISButtonClicked(GameObject oisGO,GameObject oissButton)
    {
        selectedOISSButton = oissButton;
        foreach (Transform child in oiss.transform)
        {
            child.GetChild(1).GetChild(0).GetComponent<Text>().color = new Color32(190, 190, 190, 255);
        }
        oissButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(240, 186, 49, 255);
        /*
        posXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        posYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        posZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();

        Vector3 pos = oisGO.transform.position;
        Quaternion rot = oisGO.transform.rotation;
        Vector3 sca = oisGO.transform.localScale;
        posXInput.GetComponent<InputField>().text = oisGO.transform.position.x.ToString();
        posYInput.GetComponent<InputField>().text = oisGO.transform.position.y.ToString();
        posZInput.GetComponent<InputField>().text = oisGO.transform.position.z.ToString();
        
        rotXInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.x.ToString();
        rotYInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.y.ToString();
        rotZInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.z.ToString();
        if (oisGO.GetComponent<ObjectInScene>().scaleWasChange == false)
        {
            scaXInput.GetComponent<InputField>().text = (-1 * oisGO.transform.localScale.x).ToString();
        }
        else
        {
            scaXInput.GetComponent<InputField>().text = (1 * oisGO.transform.localScale.x).ToString();
        }
        scaYInput.GetComponent<InputField>().text = oisGO.transform.localScale.y.ToString();
        scaZInput.GetComponent<InputField>().text = oisGO.transform.localScale.z.ToString();
        oisGO.transform.SetPositionAndRotation(pos, rot);
        oisGO.transform.localScale = sca;

        posXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); }) ;
        posYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        posZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        scaXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeXScaInput(); });
        scaYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        scaZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        */
        foreach (ObjectInScene obj in Managers.Creation.ObjectInSceneLsit.objectsInScene)
        {
            obj.DisableOutline();
        }

        oisGO.GetComponent<ObjectInScene>().EnableOutline();

        if (oisGO.GetComponent<LightObject>() == null)
        {
            ToggleAndExhPanel.SetActive(true);
            LightPannel.SetActive(false);
        }
        else
        {
            ToggleAndExhPanel.SetActive(false);
            LightPannel.SetActive(true);
            UpdateColorInput(oisGO);
        }
        MainCamera.GetComponent<TransformGizmo>().ClearTargets();
        MainCamera.GetComponent<TransformGizmo>().AddTarget(oisGO.transform);
    }

    public void OISButtonClicked(GameObject oisGO)
    {
        selectedOISSButton = null;
        foreach (Transform child in oiss.transform)
        {
            if (oisGO.GetComponent<ObjectInScene>().ID == child.GetChild(1).GetComponent<ObjectInSceneButton>().oisButtonID )
                selectedOISSButton = child.GetChild(1).gameObject;
        }
        
        foreach (Transform child in oiss.transform)
        {
            child.GetChild(1).GetChild(0).GetComponent<Text>().color = new Color32(190, 190, 190, 255);
        }
        selectedOISSButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(240, 186, 49, 255);
        foreach (ObjectInScene obj in Managers.Creation.ObjectInSceneLsit.objectsInScene)
        {
            obj.DisableOutline();
        }

        oisGO.GetComponent<ObjectInScene>().EnableOutline();

        if (oisGO.GetComponent<LightObject>() == null)
        {
            ToggleAndExhPanel.SetActive(true);
            LightPannel.SetActive(false);
        }
        else
        {
            ToggleAndExhPanel.SetActive(false);
            LightPannel.SetActive(true);
            UpdateColorInput(oisGO);
        }
    }

    public void FixedUpdate()
    {
        if (selectedOISSButton !=null)
        {
            UpdatePRSPanel(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO);
            UpdateExhButton(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO);

            if (selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO.GetComponent<Exhibit>() != null)
            UpdateExhPanel(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO);

            if (selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO.GetComponent<LightObject>() != null)
            {
                UpdateLightPannel(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisGO);
                //ChangeColorInput();
            }
            //ClearPhotoScroll();
            //UpdatePhotoScroll();
        }
    }

    public void ObjectInSceneSelected(GameObject oisGO)
    {

        GameObject oissButton = null;
        foreach (Transform child in oiss.transform)
        {
            child.GetChild(1).GetChild(0).GetComponent<Text>().color = new Color32(190, 190, 190, 255);
            if (oisGO.GetComponent<ObjectInScene>() != null)
            {
                if (oisGO.GetComponent<ObjectInScene>().ID == child.GetChild(1).GetComponent<ObjectInSceneButton>().oisButtonID)
                {
                    oissButton = child.GetChild(1).gameObject;
                }
            }
        }
        oissButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(240, 186, 49, 255);
        selectedOISSButton = oissButton;

        //

        foreach (ObjectInScene obj in Managers.Creation.ObjectInSceneLsit.objectsInScene)
        {
            obj.DisableOutline();
        }

        oisGO.GetComponent<ObjectInScene>().EnableOutline();
    }


    public void UpdatePRSPanel(GameObject oisGO)
    {
        posXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        posYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        posZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        rotZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaXInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaYInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        scaZInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();

        Vector3 pos = oisGO.transform.position;
        Quaternion rot = oisGO.transform.rotation;
        Vector3 sca = oisGO.transform.localScale;
        posXInput.GetComponent<InputField>().text = oisGO.transform.position.x.ToString();
        posYInput.GetComponent<InputField>().text = oisGO.transform.position.y.ToString();
        posZInput.GetComponent<InputField>().text = oisGO.transform.position.z.ToString();

        rotXInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.x.ToString();
        rotYInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.y.ToString();
        rotZInput.GetComponent<InputField>().text = oisGO.transform.rotation.eulerAngles.z.ToString();
        if (oisGO.GetComponent<ObjectInScene>().scaleWasChange == false)
        {
            scaXInput.GetComponent<InputField>().text = (-1 * oisGO.transform.localScale.x).ToString();
        }
        else
        {
            scaXInput.GetComponent<InputField>().text = (1 * oisGO.transform.localScale.x).ToString();
        }
        scaYInput.GetComponent<InputField>().text = oisGO.transform.localScale.y.ToString();
        scaZInput.GetComponent<InputField>().text = oisGO.transform.localScale.z.ToString();
        oisGO.transform.SetPositionAndRotation(pos, rot);
        oisGO.transform.localScale = sca;

        posXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        posYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        posZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        rotZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        scaXInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeXScaInput(); });
        scaYInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
        scaZInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeInput(); });
    }

    public void ChangeInput()
    {
        GameObject oisGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;

        if ((selectedOISSButton !=null)&&
            (float.TryParse(posXInput.GetComponent<InputField>().text,out var xpos)) &&
            (float.TryParse(posYInput.GetComponent<InputField>().text, out var ypos)) &&
            (float.TryParse(posZInput.GetComponent<InputField>().text, out var zpos)) &&
            (float.TryParse(rotXInput.GetComponent<InputField>().text, out var xrot)) &&
            (float.TryParse(rotYInput.GetComponent<InputField>().text, out var yrot)) &&
            (float.TryParse(rotZInput.GetComponent<InputField>().text, out var zrot)) &&
            (float.TryParse(scaXInput.GetComponent<InputField>().text, out var xsca)) &&
            (float.TryParse(scaYInput.GetComponent<InputField>().text, out var ysca)) &&
            (float.TryParse(scaZInput.GetComponent<InputField>().text, out var zsca)))
        {
               oisGO.transform.SetPositionAndRotation(
                new Vector3(xpos, ypos,zpos),Quaternion.Euler(xrot,yrot,zrot));
            oisGO.transform.localScale = new Vector3(oisGO.transform.localScale.x, ysca, zsca);
        }
    }

    public void ChangeXScaInput()
    {
        GameObject oisGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;
        if ((selectedOISSButton != null) &&
            (float.TryParse(scaXInput.GetComponent<InputField>().text, out var xsca)))
        {
            oisGO.transform.localScale = new Vector3(xsca, oisGO.transform.localScale.y, oisGO.transform.localScale.z);
            oisGO.GetComponent<ObjectInScene>().scaleWasChange = true;
        }
    }

    public void UpdateLightPannel(GameObject lightGO)
    {
        RangeInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        IntensityInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();

        //fcp.color = lightGO.GetComponent<LightObject>().Color;
        RangeInput.GetComponent<InputField>().text = lightGO.GetComponent<LightObject>().Range.ToString();
        IntensityInput.GetComponent<InputField>().text = lightGO.GetComponent<LightObject>().Intensity.ToString();

        RangeInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeRangeInput(); });
        IntensityInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeIntensityInput(); });
    }

    public void UpdateColorInput(GameObject lightGO)
    {
        fcp.color = lightGO.GetComponent<LightObject>().Color;
    }

    public void ChangeColorInput()
    {
        GameObject lightGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;
        if (selectedOISSButton != null)
        {
            lightGO.GetComponent<LightObject>().ChangeColor(fcp.color);
        }
    }

    public void ChangeRangeInput()
    {
        GameObject lightGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;
        if ((selectedOISSButton != null) &&
            (float.TryParse(RangeInput.GetComponent<InputField>().text, out var rng)))
        {
            lightGO.GetComponent<LightObject>().ChangeRange(rng);
        }
    }

    public void ChangeIntensityInput()
    {
        GameObject lightGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;
        if ((selectedOISSButton != null) &&
            (float.TryParse(IntensityInput.GetComponent<InputField>().text, out var intens)))
        {
            lightGO.GetComponent<LightObject>().ChangeIntensity(intens);
        }
    }

    public void UpdateExhButton(GameObject oisGO)
    {
        ExhButton.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();

        ExhButton.GetComponent<Toggle>().isOn = oisGO.GetComponent<ObjectInScene>().IsExhibit;

        ExhButton.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ChangeExhButton(); });

        if (ExhButton.GetComponent<Toggle>().isOn)
        {
            ExhPannel.SetActive(true);
        }
        else
        {
            ExhPannel.SetActive(false);
        }
    }

    public void ChangeExhButton()
    {
        if (selectedOISSButton != null)
        {
            GameObject oisGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;

            oisGO.GetComponent<ObjectInScene>().IsExhibit = ExhButton.GetComponent<Toggle>().isOn;
            
            if (ExhButton.GetComponent<Toggle>().isOn)
            {
                ExhPannel.SetActive(true);
            }
            else
            {
                ExhPannel.SetActive(false);
            }

            Debug.Log("ExhButtonChangerd: " + oisGO.GetComponent<ObjectInScene>().name + " "+ oisGO.GetComponent<ObjectInScene>().IsExhibit);
        } 
    }

    public void UpdateExhPanel(GameObject oisGO)
    {
        NameExhInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        DiscrExhInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();

        NameExhInput.GetComponent<InputField>().text = oisGO.GetComponent<Exhibit>().Name;
        DiscrExhInput.GetComponent<InputField>().text = oisGO.GetComponent<Exhibit>().Discription;

        NameExhInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeExhPanel(); });
        DiscrExhInput.GetComponent<InputField>().onValueChanged.AddListener(delegate { ChangeExhPanel(); });

        ClearPhotoScroll();
        UpdatePhotoScroll();
    }

    public void ChangeExhPanel()
    {
        if (selectedOISSButton != null)
        {
            GameObject oisGO = Managers.Creation.ObjectInSceneLsit.GetObjectInScene(selectedOISSButton.GetComponent<ObjectInSceneButton>().oisButtonID).gameObject;

            oisGO.GetComponent<Exhibit>().ChangeName(NameExhInput.GetComponent<InputField>().text);
            oisGO.GetComponent<Exhibit>().ChangeDiscription(DiscrExhInput.GetComponent<InputField>().text);

            //Debug.Log("ExhPanelChangerd: " + oisGO.GetComponent<Exhibit>().Name + " " + oisGO.GetComponent<Exhibit>().Discription);
        }
    }

    void OISHideButtonClicked(GameObject go,GameObject newOISSPanel)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
            newOISSPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = eyeClose;
        }
        else
        {
            go.SetActive(true);
            newOISSPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = eyeUp;
        }
    }

    void OISButtonDeleted(int oisID)
    {
        Managers.Creation.ObjectInSceneLsit.DeleteObjectInScene(oisID);
        DeleteOISSPanel(oisID);
    }

    public void AddOISSPanel(GameObject oisGO, int oisID, string name)
    {
        //SetCanvasObject();
        GameObject newOISSPanel = Instantiate(oisPanel, oiss.transform);
        newOISSPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=>OISHideButtonClicked(newOISSPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().oisGO, newOISSPanel));
        newOISSPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().SetInfo(oisGO, oisID, name);
        newOISSPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>OISButtonClicked(newOISSPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().oisGO, newOISSPanel.transform.GetChild(1).gameObject));
        newOISSPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => OISButtonDeleted(newOISSPanel.transform.GetChild(1).GetComponent<ObjectInSceneButton>().oisButtonID));

        oisPanelList.Add(newOISSPanel);
    }

    public void UpdateOISSPanel(GameObject oisGO, int oisID, string name)
    {
        //SetCanvasObject();
        foreach (Transform child in oiss.transform)
        {
            if (child.transform.GetChild(1).gameObject.GetComponent<ObjectInSceneButton>().oisGO.GetComponent<ObjectInScene>().ID == oisID)
            {
                child.transform.GetChild(1).gameObject.GetComponent<ObjectInSceneButton>().SetInfo(oisGO, oisID, name);
            }
        }
    }

    public void DeleteOISSPanel(int oisID) 
    {
        //SetCanvasObject();
        foreach (Transform child in oiss.transform)
        {
            if (child.transform.GetChild(1).gameObject.GetComponent<ObjectInSceneButton>().oisGO.GetComponent<ObjectInScene>().ID == oisID)
            {
                oisPanelList.Remove(child.gameObject);
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void WireframeButtonClicked()
    {
        if (!isWireframe)
        {
            foreach (ObjectInScene ois in Managers.Creation.ObjectInSceneLsit.objectsInScene) {
                ois.ChangeShaderToWireframe();
            }
        }
        else
        {
            foreach (ObjectInScene ois in Managers.Creation.ObjectInSceneLsit.objectsInScene)
            {
                ois.ChangeShaderToNormal();
            }
        }
        //поменять иконку
        isWireframe = !isWireframe;
    }

    public void MainCameraButtonClicked()
    {
        MainCamera.SetActive(true);
        TopCamera.SetActive(false);
        SpawnPoint.GetComponent<SpawnPoint>().DeletePlayerCamrea();
        Managers.Creation.ObjectInSceneLsit.EnalbeMeshOnLight();
    }
    public void TopCameraButtonClicked()
    {
        MainCamera.SetActive(false);
        TopCamera.SetActive(true);
        SpawnPoint.GetComponent<SpawnPoint>().DeletePlayerCamrea();
        Managers.Creation.ObjectInSceneLsit.EnalbeMeshOnLight();
    }

    public void PlayerCameraButtonClicked()
    {
        MainCamera.SetActive(false);
        TopCamera.SetActive(false);
        SpawnPoint.GetComponent<SpawnPoint>().DeletePlayerCamrea();
        SpawnPoint.GetComponent<SpawnPoint>().CreatePlayerCamera();
        Managers.Creation.ObjectInSceneLsit.DisableMeshOnLight();
    }

    public void SaveButtonClicked()
    {
        Managers.SavingLoading.SaveProject();
    }
}
