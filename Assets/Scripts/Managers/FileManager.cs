using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileManager
{
    public static string ChoseProjectFolder()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        while (string.IsNullOrEmpty(directory))
        {
            directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        }
        var folderModels = Directory.CreateDirectory(directory+@"/Models");
        return directory;
    }

    public static bool ProjectInfoFileIsExist(string projectDirectory)
    {
       return File.Exists(projectDirectory + @"/projectInfo");
    }

    public static void CreateProjectInfoFile(string projectDirectory)
    {
        File.Create(projectDirectory + @"/projectInfo");
    }

    public static string LoadOrCreateProjectInfoFile(string projectDirectory)
    {
        //FileStream infoFile = new FileStream(projectDirectory + @"/projectInfo.txt", FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
        //var file = File.OpenText(infoFile.Name);
        return File.ReadAllText(projectDirectory + @"/projectInfo");
        //return "1";
    }

    public static string ChooseIternalModelsFolder()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        return directory;
    }

    public static void CopyIternalModelsToFolder(string iternalModelsDirectory, string projectDirectory)
    {
        string[] dirs = Directory.GetDirectories(iternalModelsDirectory);
        for (int i = 0; i < dirs.Length; i++)
        {
            var dir= Directory.CreateDirectory(projectDirectory + @"/Models/" + dirs[i].Split('\\')[1]);
            
            string[] files = Directory.GetFiles(dirs[i]);
            for (int j =0; j < files.Length; j++)
            {
               string desFile1 =files[j];
               string desFile2 = dir.FullName + @"\" +files[j].Split('\\').Last();
               File.Copy(desFile1, desFile2, true);
            }
        }
    }

    public static string[] GetModelsDirectories(string projectDirectory)
    {
        return Directory.GetDirectories(projectDirectory + @"/Models");
    }

    public static string[] GetModelsFilesPath(string modelDirectory)
    {
        return Directory.GetFiles(modelDirectory);
    }

    public static byte[] GetDataFromFile(string filePath)
    {

        //FileStream fs = File.Open(filePath,FileMode.OpenOrCreate);
        return File.ReadAllBytes(filePath);
    }

    public static string ChoosePhotoFile()
    {
        return EditorUtility.OpenFilePanel("Выберите Фото", "", "");
    }

    public static void CopyPhotoToFolder(string photoPath, int oisID, int photoID, string projectDirectory)
    {
        var folderPhoto = Directory.CreateDirectory(projectDirectory + @"/Photos");
        var folderIDPhoto = Directory.CreateDirectory(folderPhoto.FullName + @"/" + oisID);
        File.Copy(photoPath, folderIDPhoto.FullName+@"/"+ photoID + ".png", true);
    }

    public static void DeletePhoto(int oisID, int photoID)
    {
        File.Delete(Managers.Creation.ProjectDirectory + @"/Photos" + @"/" + oisID + @"/" + photoID + ".png");
    }
}
