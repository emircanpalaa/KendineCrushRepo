using UnityEngine;

public class LevelSelectScreen : MonoBehaviour
{

    public void GoToMainMenu()
    {
        SManager.Instance.ChangeScene(0);
    }

    public void Level1()
    {
        SManager.Instance.ChangeScene(2);
    }

    public void Level2()
    {
        SManager.Instance.ChangeScene(3);
    }

    public void LevelSelect()
    {
        SManager.Instance.ChangeScene(1);
    }

    public void Level3()
    {
        SManager.Instance.ChangeScene(4);
    }

    public void Level4()
    {
        SManager.Instance.ChangeScene(5);
    }


}
