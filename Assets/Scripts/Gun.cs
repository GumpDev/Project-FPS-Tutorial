using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour {

    [Header("GunConfig")]
    public int Damage;
    public int Range;
    public float reloadTime;
    public float speed;
    public float BulletMass;
    public float Firerate;
    public enum type
    {
        Rifle,
        Pistol,
        Sniper,
        Shotgun,
        Rocket
    }
    public type tipo;
    [Header("Ammo")]
    public int Ammo;
    public int maxAmmoInMag;
    public int AmmoInMag;
    [Header("Imports")]
    public GameObject bullet;
    public PlayerController player;
    public GameObject pointGun;
    public ParticleSystem effectFire;
    public ParticleSystem effectEject;
    public Text ammoTxt;

    //Privates
    private bool firerateBool = true;

    private void Start()
    {
        if (!player.nv.isMine)
        {
            gameObject.SetActive(false);
        }
    }

    void Update () {
        if (player.nv.isMine)
        {
            ammoTxt.text = AmmoInMag + "/" + Ammo;
            if (tipo.ToString() == "Rifle")
            {
                if (Input.GetMouseButton(0))
                {
                    if (AmmoInMag > 0 && firerateBool == true)
                    {
                        Shoot();
                    }
                    StopCoroutine(reloadCoolDown());
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (AmmoInMag > 0 && firerateBool == true)
                    {
                        Shoot();
                    }
                    StopCoroutine(reloadCoolDown());
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (AmmoInMag != maxAmmoInMag && Ammo != 0)
                    StartCoroutine(reloadCoolDown());
            }
        }
	}

    void Shoot()
    {
        GameObject go = (GameObject)Network.Instantiate(bullet, pointGun.transform.position, transform.rotation,2);
        Bullet bll = go.GetComponent<Bullet>();
        bll.mass = BulletMass;
        bll.damage = Damage;
        bll.range = Range;
        bll.speed = speed;
        effectEject.Play();
        effectFire.Play();
        AmmoInMag--;
        StartCoroutine(fireRateLoop());
    }

    void reload()
    {
        for(int i = 0; i < maxAmmoInMag; i++)
        {
            if(Ammo > 0 && AmmoInMag < maxAmmoInMag)
            {
                AmmoInMag++;
                Ammo--;
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator reloadCoolDown()
    {
        yield return new WaitForSeconds(reloadTime);
        reload();
    }

    IEnumerator fireRateLoop()
    {
        firerateBool = false;
        yield return new WaitForSeconds(Firerate);
        firerateBool = true;
    }
}
