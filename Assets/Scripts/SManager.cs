using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SManager : MonoBehaviour
{
    public static SManager Instance;

    public int previousSceneIndex = -1;
    public int currentSceneIndex;
    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
