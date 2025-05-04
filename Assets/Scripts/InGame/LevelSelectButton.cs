using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public GameObject star1, star2, star3;

    void Start()
    {
        int lastScene = SManager.Instance.previousSceneIndex; 

        Debug.Log("Last Scene Index: " + lastScene);
    }

    public void LoadLevel()
    {
        SManager.Instance.ChangeScene(1);
    }

}
