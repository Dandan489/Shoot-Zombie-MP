using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Transform player;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;

    public float[] bulletForce = {1f, 2f};
    private float nextshoot = 0.0f;
    private float lastshoot = 0.0f;
    public float[] shootcooldown = {0.5f, 0.2f};

    public int[] maxAmmo = {11, 25};
    public int[] ammoCount = {11, 25};
    public bool reloading = false;

    public Transform[] firePoint = {};
    public GameObject[] weaponlist;
    public int currentWeapon = 0;
    public int maxWeapon = 1;

    private void Start(){
        player = GameObject.Find("Player").transform;
    }
    
    private void Update(){
        if (muzzleFlash.activeSelf && Time.time - lastshoot >= 0.15f)
        {
            muzzleFlash.SetActive(false);
        }
        if (Input.GetMouseButton(0) && Time.time > nextshoot && ammoCount[currentWeapon] !=0 && !reloading)
        {
            nextshoot = Time.time + shootcooldown[currentWeapon];
            lastshoot = Time.time;
            muzzleFlash.SetActive(true);
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && GameManager.instance.isreloading == false)
        {
            reloading = true;
            Reload();
        }
    }

    private void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint[currentWeapon].position, firePoint[currentWeapon].rotation);
        Rigidbody2D rigi = bullet.GetComponent<Rigidbody2D>();

        rigi.AddForce(firePoint[currentWeapon].up * bulletForce[currentWeapon], ForceMode2D.Impulse);

        GameManager.instance.ChangeAmmo(-1);
    }

    private void Reload()
    {
        GameManager.instance.ChangeAmmo(maxAmmo[currentWeapon] -ammoCount[currentWeapon]);
    }

    public void Upgrade()
    {
        weaponlist[currentWeapon].SetActive(false);
        currentWeapon++;
        weaponlist[currentWeapon].SetActive(true);
    }
}
