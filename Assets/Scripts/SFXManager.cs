using UnityEngine;
using UnityEngine.UI;


public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public AudioSource gemSound,explodeSound,stoneSound,roundOverSound;

    public void PlayGemBreak()
    {
        gemSound.Stop();

        gemSound.pitch = Random.Range(.8f,1.2f);

        gemSound.Play();
    }
    public void PlayExplode()
    {
        explodeSound.Stop();

        explodeSound.pitch = Random.Range(.8f,1.2f);

        explodeSound.Play();
    }
    public void PlayStone()
    {
        stoneSound.Stop();

        stoneSound.pitch = Random.Range(.8f,1.2f);

        stoneSound.Play();
    }
    public void PlayRoundOver()
    {
        roundOverSound.Play();
    }

}
