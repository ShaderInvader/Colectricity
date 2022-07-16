using System;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Modules.SceneModule.Data
{
    public class GameScene : ScriptableObject
    {
        [Tooltip("Must match exactly the name of scene file.")]
        public string sceneName;
    }
}
