using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SwitchToSceneIndex : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;

        public void SwitchToScene()
        {
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}