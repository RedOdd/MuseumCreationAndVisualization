using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour
{
    public GameObject ShowCam;
    public GameObject ShowLight;
    public bool isActive;

    public GameObject ShowPanel;
    public GameObject NameText;
    public GameObject Image3D;
    public GameObject ImagePhoto;
    public GameObject DiscrText;

    public GameObject _oisGO;
    public List<Photo> photos;
    public int photoIndex = 0;
    public GameObject showCube;

    Vector3 newScale;

    Texture2D tex;

    public void ActivateShowPanel()
    {
        ShowPanel.SetActive(true);
        isActive = true;
        showCube.GetComponent<ShowCubeUI>().ResetCube();
    }

    public void DisactivateShowPanel()
    {
        ShowPanel.SetActive(false);
        isActive = false;
        showCube.GetComponent<ShowCubeUI>().ResetCube();
        DestroyImmediate(_oisGO);
    }

    public void SetGameObject(GameObject oisGO)
    {
        _oisGO = oisGO;

        photos = _oisGO.GetComponent<Exhibit>().photos.Photos;
         photoIndex = 0;
         SetPhoto(photoIndex);

        ActivateShowPanel();
        NameText.GetComponent<Text>().text = _oisGO.GetComponent<Exhibit>().Name;
        Image3D.GetComponent<Image>().sprite = TakeRender(_oisGO);
        DiscrText.GetComponent<Text>().text = _oisGO.GetComponent<Exhibit>().Discription;
    }

    public void SetPhoto(int pIndex)
    {
        if ( photos.Count>0) {
            //ImagePhoto.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(photos[pIndex].Sprite.texture.width, photos[pIndex].Sprite.texture.height);
            Texture2D tex = photos[pIndex].Sprite.texture;
            float x = 1;
            float y=1;
            if (tex.width>=tex.height)
            {
                y = (float) tex.height/ (float)tex.width ;
            }
            else
            {
                x = (float)tex.width / (float)tex.height;
            }
            ImagePhoto.GetComponent<Image>().rectTransform.localScale = new Vector3(x, y);
            ImagePhoto.GetComponent<Image>().sprite = photos[pIndex].Sprite;
        } else {
        }
    }

    public void NextButtonClicked()
    {
        if (photoIndex + 1 > photos.Count - 1)
        {
            photoIndex = 0;
        } else
        {
            photoIndex++;
        }
        SetPhoto(photoIndex);
    }

    public void PervButtonClicked()
    {
        if (photoIndex - 1 < 0)
        {
            photoIndex = photos.Count - 1;
        }
        else
        {
            photoIndex--;
        }
        SetPhoto(photoIndex);
    }

    public void PositionOISGO(float top, float down, float right, float left)
    {
        _oisGO.transform.position += new Vector3( right - left, top - down, 0) * Time.deltaTime;

        Image3D.GetComponent<Image>().sprite = TakeRender(_oisGO);
    }

    public void RotateOISGO(float rotX, float rotY)
    {
        _oisGO.transform.RotateAround(Vector3.up, -rotX);
        _oisGO.transform.RotateAround(Vector3.right, rotY);

        Image3D.GetComponent<Image>().sprite = TakeRender(_oisGO);
    }

    public void ScaleOISGO(float MouseScroll)
    {
        if (_oisGO.transform.localScale.x > -0.1 || _oisGO.transform.localScale.y < -0.1 ||_oisGO.transform.localScale.z < 0.1)
        {
            _oisGO.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
        }else {
            float scrollSpeed = 50f;
            //_oisGO.transform.localScale += scrollSpeed * new Vector3(-Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime;
            _oisGO.transform.localScale += new Vector3(-Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"));
            Image3D.GetComponent<Image>().sprite = TakeRender(_oisGO);
        } 
    }

    public Sprite TakeRender(GameObject oisGO)
    {
        float maxSize = 0;
        float yCentre = 0;
        foreach (BoxCollider bc in oisGO.GetComponents<BoxCollider>())
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

        ShowLight.SetActive(true);
        ShowCam.transform.SetPositionAndRotation(new Vector3(ShowCam.transform.position.x, 100 + yCentre, -10 - maxSize), Quaternion.identity);
        ShowCam.GetComponent<Camera>().orthographicSize = maxSize;
        tex = RTImage(ShowCam.GetComponent<Camera>());
        Sprite spr = Sprite.Create(tex,
                                    new Rect(0, 0, ShowCam.GetComponent<Camera>().activeTexture.width, ShowCam.GetComponent<Camera>().activeTexture.height),
                                    new Vector2(0.5f, 0.5f));
        ShowLight.SetActive(false);
        return spr;
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
}
