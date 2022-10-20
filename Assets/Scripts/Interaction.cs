using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class Interaction : MonoBehaviour
{
    bool commandLight=true;
    bool commandDoor=false;
    bool commandWall=false;
    bool hitLight=false;
    bool hitDoor=false;
    bool hitWall=false;
    [SerializeField]
    GameObject main_camera;
    RaycastHit hit;
    [SerializeField]

    Animator anim;
    [SerializeField]
    GameObject light1,light2;
    TcpListener listener;
    TcpClient client;
    string dataReceived="0";
    [SerializeField]bool interact = false;
    [SerializeField]
    GameObject[] walls;

    bool isHit=false;

    // Start is called before the first frame update
    void Start()
    {
        IPAddress localAdd = IPAddress.Parse("127.0.0.1");
        listener = new TcpListener(IPAddress.Any, 13000);
        listener.Start();

        var thread = new System.Threading.Thread(getSocketInput);
        thread.Start();

        StartCoroutine(setState());


        //Debug.Log(SerialPort.GetPortNames()[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < -60) {
            gameObject.transform.position = new Vector3(2.552f,1.129f,1.639f);
        }

        if(Physics.Raycast(main_camera.transform.position,main_camera.transform.forward,
            out hit, Mathf.Infinity) ){
            
            if(hit.collider.tag=="light"){
                hitLight=true;
                if(!commandLight || interact){
                    light1.SetActive(false);
                    light2.SetActive(false);
                }
                else{
                    light1.SetActive(true);
                    light2.SetActive(true);
                }
                
            }
            else if(hit.collider.tag == "door"){
                hitDoor = true;
                if(commandDoor || interact){
                    anim.SetBool("doorOpen",true);
                }
                else{
                    anim.SetBool("doorOpen",false);
                }
            }
            else if(hit.collider.tag == "wall"){
                hitWall = true;
                if(commandWall || interact){
                    for(int i=0;i<3;i++) walls[i].SetActive(false);
                }
            }
            else{
                hitLight=false;
                hitDoor=false;
                hitWall=false;
            }
            Debug.Log("Data:" + dataReceived);
        }
        //Debug.DrawRay(transform.position, transform.TransformDirection(main_camera.transform.forward), Color.yellow);
        if(Input.GetKeyDown(KeyCode.X)){
            listener.Stop();
            client.Close();
            Application.Quit();
        }
    }

    IEnumerator setState(){
        while(true){
            string oldData = dataReceived;
            yield return new WaitForSeconds(2f);
            if(Int16.Parse(dataReceived)>Int16.Parse(oldData)+2){
                if(hitLight) commandLight=!commandLight;
                if(hitDoor) commandDoor=!commandDoor;
                if(hitWall) commandWall=!commandWall;
            }
        }
    }

    void getSocketInput(){
        client = listener.AcceptTcpClient();
        while(true){


            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];


            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);


            dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Blink Received");

        }
        
    }

}
