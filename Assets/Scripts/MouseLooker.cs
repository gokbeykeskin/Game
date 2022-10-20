using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MouseLooker : MonoBehaviour
{   
    //[SerializeField] TextMeshProUGUI buttonText;
    public float sensitivity = 3f;
    public Transform playerbody;
    float xRotation = 0f;
    public static bool stopRotation = false;
    
    float horizontalRotation;
    float verticalRotation;

    void Awake()
    {
        //Cursor.visible = false;
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
{       float mouseY;
        horizontalRotation = Input.GetAxis("Mouse X"); //mouse da kullanılabilir
        mouseY = 2 * Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        float mouseX = horizontalRotation * sensitivity * Time.deltaTime;
        

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        if(!stopRotation){
            transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
            playerbody.Rotate(Vector3.up * mouseX);
        }
    }
}
