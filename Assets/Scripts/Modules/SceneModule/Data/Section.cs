using UnityEngine;

namespace Modules.SceneModule.Data
{
    [CreateAssetMenu(fileName = "NewSection", menuName = "Scene Data/Section")]
    public class Section : ScriptableObject
    {
        public string sectionName;
        public Level[] sectionLevels;
    }
}
