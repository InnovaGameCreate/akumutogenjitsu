using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerControl : MonoBehaviour 
{
    [SerializeField] private float SpeedX;
    private int i = 0;


    void Start()
    {

    }
    void Update()
    {
      
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3 (SpeedX, 0, 0) * Time.deltaTime);
            i++;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-SpeedX, 0, 0) * Time.deltaTime);
            i++;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, SpeedX, 0) * Time.deltaTime);
            i++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -SpeedX, 0) * Time.deltaTime);
            i++;
        }



    }



}
