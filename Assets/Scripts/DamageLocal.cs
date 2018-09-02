using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLocal : MonoBehaviour {

    public float multiplyDamage;
    public PlayerController pc;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.GetComponent<Bullet>() != null)
        {
            float damage = collision.collider.gameObject.GetComponent<Bullet>().damage;
            pc.remLife((int)(multiplyDamage * damage));
            pc.nv.RPC("remLifeRPC", RPCMode.All, (int)(multiplyDamage * damage));
        }
    }
}
