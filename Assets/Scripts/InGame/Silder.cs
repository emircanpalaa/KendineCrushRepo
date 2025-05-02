using UnityEngine;
using UnityEngine.UI;

public class Silder : MonoBehaviour
{
    private Image image;
    [SerializeField] private GameObject roundManagerObj;
    private RoundManager roundManager;
    private float roundTime;
    private float startTime;
    void Start()
    {
        image = GetComponent<Image>();
        roundManager = roundManagerObj.GetComponent<RoundManager>();
        startTime = roundManager.GetRoundTime();
    }

    // Update is called once per frame
    void Update()
    {
        roundTime = roundManager.GetRoundTime();
        image.fillAmount = roundTime / startTime;
    }
}
