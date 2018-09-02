using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public bool enableMouse;
    [Header("PlayerConfig")]
    public string PlayerName;
    public int Life;
    public float speed;
    public float RunSpeed;
    public float sensibility;
    [Header("Imports")]
    public Camera cam;
    public GameObject model;
    public Slider lifeSlider;
    public GameObject canvas;
    public GameObject dieCanvas;

    //Privates
    private Rigidbody rb;
    private float realSpeed;
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 camRotation;
    private float rotCam;
    [HideInInspector]
    public NetworkView nv;

	void Start () {
        nv = GetComponent<NetworkView>();
        if (nv.isMine)
        {
            rb = GetComponent<Rigidbody>();
            model.SetActive(false);
        }
        else
        {
            cam.enabled = false;
            canvas.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

	void Update () {
        model.transform.position = Vector3.LerpUnclamped(model.transform.position, transform.position, 10 * Time.deltaTime);
        model.transform.rotation = Quaternion.LerpUnclamped(model.transform.rotation, transform.rotation, 10 * Time.deltaTime);
        if (nv.isMine)
        {
            #region Canvas
            lifeSlider.value = Life;
            #endregion
            #region Moviment
            float _xMov = Input.GetAxisRaw("Horizontal");
            float _yMov = Input.GetAxisRaw("Vertical");

            if (Input.GetButton("Run") == true && _xMov == 0 && _yMov == 1)
            {
                realSpeed = RunSpeed;
            }
            else
            {
                realSpeed = speed;
            }

            Vector3 _MoveHorizontal = transform.right * _xMov;
            Vector3 _MoveVertical = transform.forward * _yMov;

            velocity = (_MoveHorizontal + _MoveVertical).normalized * realSpeed;

            #endregion
            #region Rotation
            float _yMouse = Input.GetAxisRaw("Mouse X");
            rotation = new Vector3(0, _yMouse, 0) * sensibility;

            float _xMouse = Input.GetAxisRaw("Mouse Y");
            camRotation = new Vector3(_xMouse, 0, 0) * sensibility;
            #endregion
            #region enableMouse
            if (enableMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            #endregion
        }
        if(transform.position.y < -20)
        {
            remLife(1);
            nv.RPC("remLifeRPC", RPCMode.All, 1);
        }
    }

    public void remLife(int i)
    {
        Life -= i;
        if (Life <= 0)
            die();
    }

    void die()
    {
        dieCanvas.SetActive(true);
        enableMouse = false;
        nv.RPC("respawn", RPCMode.All, false);
    }

    public void respawn()
    {
        dieCanvas.SetActive(false);
        enableMouse = true;
        Life = 100;
        nv.RPC("respawn", RPCMode.All, true);
        transform.position = new Vector3(0, 2, 0);
    }

    private void FixedUpdate()
    {
        if (nv.isMine)
        {
            if (enableMouse == true)
            {
                Moviment();
                Rotation();
            }
        }
    }

    void Moviment()
    {
        if (velocity != Vector3.zero)
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Rotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if(cam != null)
        {
            rotCam += camRotation.x;
            rotCam = Mathf.Clamp(rotCam, -80, 80);

            cam.transform.localEulerAngles = new Vector3(-rotCam, 0, 0);
        }
    }

    [RPC]
    public void respawn(bool b)
    {
        if (b)
        {
            model.SetActive(true);
            Life = 100;
        }
        else
        {
            model.SetActive(false);
        }
    }

    [RPC]
    public void remLifeRPC(int i)
    {
        remLife(i);
    }
}
