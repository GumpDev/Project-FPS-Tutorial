using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("BulletConfig")]
    public int range;
    public int damage;
    public float mass;
    public float speed;
    [Header("Imports")]
    public PlayerController player;
    public ParticleSystem impact;

    //Privates
    private Rigidbody rb;
    private Vector3 origin;

	void Start () {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
	}
	
	void Update () {
        //Massa
        rb.mass = mass;
        //Frente
        Vector3 horizontal = transform.right * 0;
        Vector3 vertical = transform.forward * 1;
        Vector3 velocity = (horizontal + vertical).normalized * speed;
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
        //Range
        if(Vector3.Distance(origin,transform.position) > range)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if(collision.gameObject.tag == "Terrain")
        {
            Network.Instantiate(impact, transform.position,Quaternion.EulerAngles(new Vector3(-90, 0, 0)), 1);
        }
    }
}
