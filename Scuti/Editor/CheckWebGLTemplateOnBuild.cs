using UnityEditor;
using UnityEngine;

public class CheckWebGLTemplateOnBuild
{
    [InitializeOnLoadMethod]
    private static void RegisterBuildHandler()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(CheckWebGLTemplatesBeforeBuild);
    }

    private static void CheckWebGLTemplatesBeforeBuild(BuildPlayerOptions buildPlayerOptions)
    {
        if (buildPlayerOptions.target == BuildTarget.WebGL)
        {
            string templateFolderPath = Application.dataPath + "/WebGLTemplates";
            bool folderExists = System.IO.Directory.Exists(templateFolderPath);

            if (!folderExists)
            {
                Debug.LogError("WebGLTemplates folder does not exist. Create the folder using the Scuti menu before building for WebGL.");
                return;
            }
        }

        // Continue with the build process
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
    }
}
