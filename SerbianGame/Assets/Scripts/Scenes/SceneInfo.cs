using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Custom Scene Loader/SceneInfo")]
public class SceneInfo : ScriptableObject
{
    public string sceneName = "";
    public SceneType type = SceneType.None;
}

