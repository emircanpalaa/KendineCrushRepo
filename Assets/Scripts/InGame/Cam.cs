
using UnityEngine;
public class Cam : MonoBehaviour
{
    public Transform _cam;
    
    void Start()
    {
        _cam.transform.position=new Vector3((float)GetComponent<Board>().width*-.35f,(float)GetComponent<Board>().height/2-.7f,-5f);
    }


    void Update()
    {
        
    }
}
