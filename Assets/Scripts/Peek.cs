using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peek : MonoBehaviour {

    public Camera cam;
    public float time;
    private int peek = 0;
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (peek >= 0)
                peek = -1;
            else
                peek = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (peek <= 0)
                peek = 1;
            else
                peek = 0;
        }

        if(peek == -1)
        {
            cam.transform.eulerAngles = Vector3.LerpUnclamped(cam.transform.eulerAngles, new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 25), time * Time.deltaTime);
            cam.transform.localPosition = Vector3.LerpUnclamped(cam.transform.localPosition, new Vector3(-0.5f, cam.transform.localPosition.y, cam.transform.localPosition.z), time * Time.deltaTime);
        }else if (peek == 1)
        {
            cam.transform.eulerAngles = Vector3.LerpUnclamped(cam.transform.eulerAngles, new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, -25), time * Time.deltaTime);
            cam.transform.localPosition = Vector3.LerpUnclamped(cam.transform.localPosition, new Vector3(0.5f, cam.transform.localPosition.y, cam.transform.localPosition.z), time * Time.deltaTime);
        }
        else
        {
            cam.transform.eulerAngles = Vector3.LerpUnclamped(cam.transform.eulerAngles, new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0), time * Time.deltaTime);
            cam.transform.localPosition = Vector3.LerpUnclamped(cam.transform.localPosition, new Vector3(0, cam.transform.localPosition.y, cam.transform.localPosition.z), time * Time.deltaTime);
        }
    }
}
