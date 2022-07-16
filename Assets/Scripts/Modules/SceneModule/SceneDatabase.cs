using Modules.SceneModule.Data;
using UnityEngine;

namespace Modules.SceneModule
{
    [CreateAssetMenu(fileName = "SceneDatabase", menuName = "Scene Data/Scene Database")]
    public class SceneDatabase : ScriptableObject
    {
        public Menu mainMenu;
        public Menu loadingScene;
        public Menu endingScene;
        public Section[] sections;
    }
}
