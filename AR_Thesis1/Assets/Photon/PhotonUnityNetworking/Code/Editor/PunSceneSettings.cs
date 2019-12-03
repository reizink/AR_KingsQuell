// ----------------------------------------------------------------------------
// <copyright file="PunSceneSettings.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//	Optional lowest-viewID setting per-scene. So PhotonViews don't get the same ID.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;

namespace Photon.Pun
{
    [Serializable]
    public class SceneSetting
    {
        public SceneAsset sceneAsset;
        public string sceneName;
        public int minViewId;
    }

    public class PunSceneSettings : ScriptableObject
    {

#if UNITY_EDITOR
        // Suppressing compiler warning "this variable is never used". Only used in the CustomEditor, only in Editor
#pragma warning disable 0414
        [SerializeField]
        bool SceneSettingsListFoldoutOpen = true;
#pragma warning restore 0414
#endif
        
        [SerializeField]
        public List<SceneSetting> MinViewIdPerScene = new List<SceneSetting>();

      
        private const string SceneSettingsFileName = "PunSceneSettingsFile.asset";

        // we use the path to PunSceneSettings.cs as path to create a scene settings file
        private static string punSceneSettingsCsPath;

        public static string PunSceneSettingsCsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(punSceneSettingsCsPath))
                {
                    return punSceneSettingsCsPath;
                }

                // Unity 4.3.4 does not yet have AssetDatabase.FindAssets(). Would be easier.
                var result = Directory.GetFiles(Application.dataPath, "PunSceneSettings.cs", SearchOption.AllDirectories);
                if (result.Length >= 1)
                {
                    punSceneSettingsCsPath = Path.GetDirectoryName(result[0]);
                    punSceneSettingsCsPath = punSceneSettingsCsPath.Replace('\\', '/');
                    punSceneSettingsCsPath = punSceneSettingsCsPath.Replace(Application.dataPath, "Assets");

                    // AssetDatabase paths have to use '/' and are relative to the project's folder. Always.
                    punSceneSettingsCsPath = punSceneSettingsCsPath + "/" + SceneSettingsFileName;
                }

                return punSceneSettingsCsPath;
            }
        }


        private static PunSceneSettings instanceField;

        public static PunSceneSettings Instance
        {
            get
            {
                if (instanceField != null)
                {
                    return instanceField;
                }

                instanceField = (PunSceneSettings) AssetDatabase.LoadAssetAtPath(PunSceneSettingsCsPath, typeof(PunSceneSettings));
                if (instanceField == null)
                {
                    instanceField = ScriptableObject.CreateInstance<PunSceneSettings>();
                    AssetDatabase.CreateAsset(instanceField, PunSceneSettingsCsPath);
                }

                return instanceField;
            }
        }


        public static int MinViewIdForScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return 1;
            }

            PunSceneSettings pss = Instance;
            if (pss == null)
            {
                Debug.LogError("pss cant be null");
                return 1;
            }

            foreach (SceneSetting setting in pss.MinViewIdPerScene)
            {
                if (setting.sceneName.Equals(sceneName))
                {
                    return setting.minViewId;
                }
            }
            return 1;
        }

        public static void SanitizeSettings()
        {
            if (Instance == null)
            {
                return;
            }
            
            #if UNITY_EDITOR
            foreach (SceneSetting sceneSetting in Instance.MinViewIdPerScene)
            {
                if (sceneSetting.sceneAsset == null && !string.IsNullOrEmpty(sceneSetting.sceneName))
                {
                    
                    string[] guids = AssetDatabase.FindAssets(sceneSetting.sceneName + " t:SceneAsset");

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (Path.GetFileNameWithoutExtension(path) == sceneSetting.sceneName)
                        {
                            sceneSetting.sceneAsset =
                                AssetDatabase.LoadAssetAtPath<SceneAsset>(
                                    AssetDatabase.GUIDToAssetPath(guid));
                            
                        //    Debug.Log("SceneSettings : ''"+sceneSetting.sceneName+"'' scene is missing: Issue corrected",Instance);
                            break;
                        }
                    }
                    
                    //Debug.Log("SceneSettings : ''"+sceneSetting.sceneName+"'' scene is missing",Instance);
                    
                    continue;
                }
                
                if (sceneSetting.sceneAsset != null && sceneSetting.sceneName!= sceneSetting.sceneAsset.name )
                {
                 //   Debug.Log("SceneSettings : '"+sceneSetting.sceneName+"' mismatch with sceneAsset: '"+sceneSetting.sceneAsset.name+"' : Issue corrected",Instance);
                    sceneSetting.sceneName = sceneSetting.sceneAsset.name;
                    continue;
                }
            }
            #endif
        }
    }
}