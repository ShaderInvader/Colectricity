using System;
using System.Collections;
using Modules.SceneModule.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.SceneModule
{
    public class SceneModule : MonoBehaviour
    {
        public SceneDatabase sceneDatabase;

        public GameScene CurrentScene { get; private set; } = null;
        // Current section number, from 0 to inf; -1 represents a menu scene
        public int CurrentSection { get; private set; } = -1;
        // Current level number, from 0 to inf; -1 represents a menu scene
        public int CurrentLevel { get; private set; } = -1;
        
        public bool IsLoading { get; private set; } = false;
        public float LoadingProgress { get; private set; } = 0.0f;
        
        public event Action<GameScene, int, int> SceneLoadingStarted;
        public event Action<GameScene, int, int> SceneLoadingFinished;

        public void LoadMainMenu()
        {
            StartCoroutine(LoadSceneCoroutine(sceneDatabase.mainMenu, -1, -1, true));
        }
        
        public void LoadLevel(int section, int level)
        {
            StartCoroutine(LoadSceneCoroutine(sceneDatabase.sections[section].sectionLevels[level], section, level, true));
        }

        public void LoadNextLevel()
        {
            int section = CurrentSection;
            int level = CurrentLevel;
            if (level < sceneDatabase.sections[section].sectionLevels.Length - 1)
            {
                level++;
            }
            else if (section < sceneDatabase.sections.Length - 1)
            {
                section++;
                level = 0;
            }
            else
            {
                LoadEndingScene();
                return;
            }
            
            LoadLevel(section, level);
        }

        public void LoadPreviousLevel()
        {
            int section = CurrentSection;
            int level = CurrentLevel;
            if (level > 0)
            {
                level--;
            }
            else if (section > 0)
            {
                section--;
                level = sceneDatabase.sections[section].sectionLevels.Length - 1;
            }
            else
            {
                LoadMainMenu();
                return;
            }
            
            LoadLevel(section, level);
        }

        public void LoadEndingScene()
        {
            StartCoroutine(LoadSceneCoroutine(sceneDatabase.endingScene, -1, -1, false));
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }

        private IEnumerator LoadSceneCoroutine(GameScene scene, int sectionNumber, int levelNumber, bool showLoadingScreen)
        {
            SceneLoadingStarted?.Invoke(scene, sectionNumber, levelNumber);
            IsLoading = true;
            
            if (showLoadingScreen)
            {
                // Load scene with loading screen synchronously
                SceneManager.LoadScene(sceneDatabase.loadingScene.name);
                CurrentScene = sceneDatabase.loadingScene;
                CurrentSection = -1;
                CurrentLevel = -1;
            }
            
            // Load target scene asynchronously
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(scene.sceneName);
            // Prevent scene from activating as long as the progress is less than 90%
            loadSceneAsync.allowSceneActivation = false;
            
            while (!loadSceneAsync.isDone)
            {
                LoadingProgress = loadSceneAsync.progress;

                if (LoadingProgress >= 0.9f)
                {
                    loadSceneAsync.allowSceneActivation = true;
                }
                
                yield return null;
            }

            CurrentScene = scene;
            CurrentSection = sectionNumber;
            CurrentLevel = levelNumber;
            
            IsLoading = false;
            SceneLoadingFinished?.Invoke(scene, sectionNumber, levelNumber);
        }
    }
}
