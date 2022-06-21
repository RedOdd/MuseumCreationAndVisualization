using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectInSceneList : MonoBehaviour, IEnumerable<ObjectInScene>
{
    public GameObject SpawnPoint;
    public Mesh LightMesh;
    public List<ObjectInScene> objectsInScene { get; private set; }
    public int lastID { get; set; }
    public ObjectInSceneList(int lastID)
    {
        this.lastID = lastID;
        objectsInScene = new List<ObjectInScene>();

    }

    public GameObject AddLightInScene()
    {
        string name = "Light" + lastID;
        GameObject lightGO = new GameObject(name);
        lightGO.AddComponent<ObjectInScene>();
        lightGO.GetComponent<ObjectInScene>().SetInfo(lastID, name, true);

        lightGO.AddComponent<Light>();
        lightGO.GetComponent<Light>().type = LightType.Point;
        lightGO.GetComponent<Light>().shadows = LightShadows.Soft;
        if (Managers.Creation.ST == CreationManager.ShadowType.Low) 
        {
            lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.Low;
        }
        else if (Managers.Creation.ST == CreationManager.ShadowType.Mid)
        {
            lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.Medium;
        }
        else if (Managers.Creation.ST == CreationManager.ShadowType.Hig)
        {
            lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.High;
        }
        else if (Managers.Creation.ST == CreationManager.ShadowType.VHig)
        {
            lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.VeryHigh;
        }
        //lightGO.GetComponent<Light>().shadowResolution =  LightShadowResolution.VeryHigh;

        lightGO.AddComponent<LightObject>();
        lightGO.GetComponent<LightObject>().SetInfo(lastID, name);

        GameObject tempShpere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lightGO.AddComponent<MeshFilter>();
        lightGO.GetComponent<MeshFilter>().mesh = tempShpere.GetComponent<MeshFilter>().mesh;
        lightGO.AddComponent<MeshRenderer>();
        lightGO.GetComponent<MeshRenderer>().sharedMaterial = tempShpere.GetComponent<MeshRenderer>().sharedMaterial;
        lightGO.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        lightGO.GetComponent<MeshRenderer>().receiveShadows = false;
        lightGO.GetComponent<MeshRenderer>().lightProbeUsage = LightProbeUsage.Off;
        lightGO.GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
        lightGO.transform.localScale = lightGO.transform.localScale / 2f;
        DestroyImmediate(tempShpere);

        lightGO.AddComponent<Outline>();
        lightGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
        lightGO.GetComponent<Outline>().OutlineColor = Color.yellow;
        lightGO.GetComponent<Outline>().OutlineWidth = 5f;
        lightGO.GetComponent<Outline>().enabled = false;
        lightGO.AddComponent<BoxCollider>();

        objectsInScene.Add(lightGO.GetComponent<ObjectInScene>());
        lastID++;
        return lightGO;
    }

    public GameObject AddObjectInScene(GameObject gameObject, string name)
    {
        GameObject newGO = Instantiate(gameObject);
        newGO.AddComponent<ObjectInScene>();
        newGO.GetComponent<ObjectInScene>().SetInfo(lastID,name,false);

        newGO.AddComponent<Outline>();
        newGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
        newGO.GetComponent<Outline>().OutlineColor = Color.yellow;
        newGO.GetComponent<Outline>().OutlineWidth = 5f;
        newGO.GetComponent<Outline>().enabled = false;

        newGO.AddComponent<Exhibit>();
        newGO.GetComponent<Exhibit>().SetInfo(lastID);

        newGO.name = newGO.name.Split('(')[0];
        objectsInScene.Add(newGO.GetComponent<ObjectInScene>());
       // Managers.UI.AddOISSPanel(newGO,lastID,name);
        lastID++;
        return newGO;
    }

    ObjectInScene AddObjectInScene(int oldid, GameObject gameObject, string name)
    {
        GameObject newGO = Instantiate(gameObject);
        newGO.AddComponent<ObjectInScene>();
        newGO.GetComponent<ObjectInScene>().SetInfo(oldid, name,false);
        
        newGO.AddComponent<Outline>();
        newGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
        newGO.GetComponent<Outline>().OutlineColor = Color.yellow;
        newGO.GetComponent<Outline>().OutlineWidth = 5f;
        newGO.GetComponent<Outline>().enabled = false;

        newGO.AddComponent<Exhibit>();
        newGO.GetComponent<Exhibit>().SetInfo(oldid);
        //newGO.GetComponent<Exhibit>().enabled = false;

        objectsInScene.Add(newGO.GetComponent<ObjectInScene>());
        //Managers.UI.AddOISSPanel(newGO, oldid, name);
        return GetObjectInScene(oldid);
    }

    public GameObject AddSpawnPoint(GameObject playerCameraPrefab)
    {
        GameObject newGO = Instantiate(SpawnPoint);
        newGO.AddComponent<ObjectInScene>();
        newGO.GetComponent<ObjectInScene>().SetInfo(lastID, "SpawnPoint", false);

        newGO.AddComponent<Outline>();
        newGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
        newGO.GetComponent<Outline>().OutlineColor = Color.yellow;
        newGO.GetComponent<Outline>().OutlineWidth = 5f;
        newGO.GetComponent<Outline>().enabled = false;

        newGO.AddComponent<Exhibit>();
        newGO.GetComponent<Exhibit>().SetInfo(lastID);

        newGO.GetComponent<SpawnPoint>().SetPlayerCameraPrefab(playerCameraPrefab);
        newGO.name = "SpawnPoint";
        objectsInScene.Add(newGO.GetComponent<ObjectInScene>());
        lastID++;
        return newGO;
    }

    public void UpdateObjectInScene(int id, GameObject newGameObject)
    {
        string name = (GetObjectInScene(id).gameObject.name);
        Vector3 pos = GetObjectInScene(id).gameObject.transform.position;
        Quaternion rot = GetObjectInScene(id).gameObject.transform.rotation;
        Vector3 sca = GetObjectInScene(id).gameObject.transform.localScale;
        bool oldActive = GetObjectInScene(id).gameObject.activeSelf;

        GameObject old = GetObjectInScene(id).gameObject;
        //Managers.UI.DeleteOISSPanel(id);
        DeleteObjectInScene(id);
        Destroy(old.gameObject);
        ObjectInScene ois = AddObjectInScene(id, newGameObject, name);
        ois.gameObject.transform.position = pos;
        ois.gameObject.transform.rotation = rot;
        ois.gameObject.transform.localScale = sca;
        ois.gameObject.name = ois.gameObject.name.Split('(')[0];
        if (oldActive)
        {
            ois.gameObject.SetActive(true);
        }
        else
        {
            ois.gameObject.SetActive(false);
        }
        //Managers.UI.UpdateOISSPanel(ois.gameObject, id, name);
    }

    public void LoadObjectInScene(Save.ObjectInSceneSaveData oisSD)
    {
        
        if (!oisSD.IsLight)
        {
            if (oisSD.ID == 0)
            {
                GetObjectInScene(0).transform.position = new Vector3(oisSD.Pos.x, oisSD.Pos.y, oisSD.Pos.z);
                GetObjectInScene(0).transform.rotation = new Quaternion(oisSD.Rot.x, oisSD.Rot.y, oisSD.Rot.z,oisSD.Rot.w);
                GetObjectInScene(0).transform.localScale = new Vector3(oisSD.Sca.x, oisSD.Sca.y, oisSD.Sca.z);
            }
            else
            {
                GameObject newGO = Instantiate(Managers.Creation.ItemList.TakeItem(oisSD.Name).Prefab);
                newGO.AddComponent<ObjectInScene>();
                newGO.GetComponent<ObjectInScene>().SetInfo(oisSD.ID, oisSD.Name, false);
                newGO.GetComponent<ObjectInScene>().IsLight = false;

                newGO.AddComponent<Outline>();
                newGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
                newGO.GetComponent<Outline>().OutlineColor = Color.yellow;
                newGO.GetComponent<Outline>().OutlineWidth = 5f;
                newGO.GetComponent<Outline>().enabled = false;

                newGO.GetComponent<ObjectInScene>().IsExhibit = oisSD.IsExh;
                newGO.AddComponent<Exhibit>();
                newGO.GetComponent<Exhibit>().SetInfo(oisSD.ID);
                newGO.GetComponent<Exhibit>().Name = oisSD.ExhName;
                newGO.GetComponent<Exhibit>().Discription = oisSD.ExhDiscr;
                newGO.GetComponent<Exhibit>().photos.LastID = oisSD.LastPhotoID;
                for( int i =0; i< oisSD.PhotosID.Length; i++)
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.LoadImage(Managers.SavingLoading.GetPhotoData(oisSD.ID, oisSD.PhotosID[i]));
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    newGO.GetComponent<Exhibit>().LoadPhoto(spr,oisSD.PhotosID[i]);
                }
                newGO.name = newGO.name.Split('(')[0];
                objectsInScene.Add(newGO.GetComponent<ObjectInScene>());

                newGO.transform.position = new Vector3(oisSD.Pos.x, oisSD.Pos.y, oisSD.Pos.z);
                newGO.transform.rotation = new Quaternion(oisSD.Rot.x, oisSD.Rot.y, oisSD.Rot.z, oisSD.Rot.w);
                newGO.transform.localScale = new Vector3(oisSD.Sca.x, oisSD.Sca.y, oisSD.Sca.z);

                if (GameObject.Find("Controller").GetComponent<UIController>() != null)
                {
                    GameObject.Find("Controller").GetComponent<UIController>().AddOISSPanel(newGO, newGO.GetComponent<ObjectInScene>().ID, newGO.GetComponent<ObjectInScene>().Name);
                }
            }
        }
        else
        {
            GameObject lightGO = new GameObject(oisSD.Name);
            lightGO.AddComponent<ObjectInScene>();
            lightGO.GetComponent<ObjectInScene>().SetInfo(oisSD.ID, oisSD.Name, true);

            lightGO.AddComponent<Light>();
            lightGO.GetComponent<Light>().type = LightType.Point;
            lightGO.GetComponent<Light>().shadows = LightShadows.Soft;
            if (Managers.Creation.ST == CreationManager.ShadowType.Low)
            {
                lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.Low;
            }
            else if (Managers.Creation.ST == CreationManager.ShadowType.Mid)
            {
                lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.Medium;
            }
            else if (Managers.Creation.ST == CreationManager.ShadowType.Hig)
            {
                lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.High;
            }
            else if (Managers.Creation.ST == CreationManager.ShadowType.VHig)
            {
                lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.VeryHigh;
            }
            //lightGO.GetComponent<Light>().shadowResolution = LightShadowResolution.VeryHigh;

            lightGO.AddComponent<LightObject>();
            lightGO.GetComponent<LightObject>().SetInfo(oisSD.ID, oisSD.Name);
            lightGO.GetComponent<LightObject>().ChangeColor(new Color(oisSD.Colo.r, oisSD.Colo.g, oisSD.Colo.b, oisSD.Colo.a));
            lightGO.GetComponent<LightObject>().ChangeIntensity(oisSD.Intens);
            lightGO.GetComponent<LightObject>().ChangeRange(oisSD.Range);


            GameObject tempShpere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lightGO.AddComponent<MeshFilter>();
            lightGO.GetComponent<MeshFilter>().mesh = tempShpere.GetComponent<MeshFilter>().mesh;
            lightGO.AddComponent<MeshRenderer>();
            lightGO.GetComponent<MeshRenderer>().sharedMaterial = tempShpere.GetComponent<MeshRenderer>().sharedMaterial;
            lightGO.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            lightGO.GetComponent<MeshRenderer>().receiveShadows = false;
            lightGO.GetComponent<MeshRenderer>().lightProbeUsage = LightProbeUsage.Off;
            lightGO.GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
            DestroyImmediate(tempShpere);

            lightGO.AddComponent<Outline>();
            lightGO.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAndSilhouette;
            lightGO.GetComponent<Outline>().OutlineColor = Color.yellow;
            lightGO.GetComponent<Outline>().OutlineWidth = 5f;
            lightGO.GetComponent<Outline>().enabled = false;
            lightGO.AddComponent<BoxCollider>();

            lightGO.transform.position = new Vector3(oisSD.Pos.x, oisSD.Pos.y, oisSD.Pos.z);
            lightGO.transform.rotation = new Quaternion(oisSD.Rot.x, oisSD.Rot.y, oisSD.Rot.z, oisSD.Rot.w);
            lightGO.transform.localScale = new Vector3(oisSD.Sca.x, oisSD.Sca.y, oisSD.Sca.z);

            objectsInScene.Add(lightGO.GetComponent<ObjectInScene>());
            if (GameObject.Find("Controller").GetComponent<UIController>() != null)
            {
                GameObject.Find("Controller").GetComponent<UIController>().AddOISSPanel(lightGO, lightGO.GetComponent<ObjectInScene>().ID, lightGO.GetComponent<ObjectInScene>().Name);
            }
        }
    }

    public void DisableMeshOnLight()
    {
        foreach (var ois in objectsInScene)
        {
            if (ois.IsLight)
            {
                ois.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void EnalbeMeshOnLight()
    {
        foreach (var ois in objectsInScene)
        {
            if (ois.IsLight)
            {
                ois.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void DeleteObjectInScene(int id)
    {
        Destroy(GetObjectInScene(id).gameObject);
        objectsInScene.Remove(GetObjectInScene(id));

    }

    public ObjectInScene GetObjectInScene(int id)
    {
        return objectsInScene.Find(i => i.ID == id);
    }

    public ObjectInScene GetObjectInScene(string name)
    {
        return objectsInScene.Find(i => i.Name == name);
    }

    public IEnumerator<ObjectInScene> GetEnumerator()
    {
        foreach (ObjectInScene oIS in objectsInScene)
        {
            yield return oIS;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
