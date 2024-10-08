using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shooting : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public HudUpdate HU;

    public float[] bulletForce = {5f, 8f};
    private float nextshoot = 0.0f;
    private float lastshoot = 0.0f;
    public float[] shootcooldown = {0.5f, 0.2f};

    public int[] maxAmmo = {11, 26};
    public int[] ammoCount = {11, 26};
    public bool reloading = false;

    public Transform[] firePoint = {};
    public GameObject[] weaponlist;
    public int currentWeapon = 0;
    public int maxWeapon = 1;

    private void Update(){
        if (!isLocalPlayer) return;

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
        if (Input.GetKeyDown(KeyCode.R) && HU.isreloading == false)
        {
            reloading = true;
            Reload();
        }
    }

    private void Shoot()
    {
        HU.ChangeAmmo(-1);
        CmdServerShoot(gameObject);
    }

    [Command]
    private void CmdServerShoot(GameObject player)
    {
        Shooting player_shoot = player.GetComponent<Shooting>();
        GameObject bullet = Instantiate(bulletPrefab, player_shoot.firePoint[player_shoot.currentWeapon].position, player_shoot.firePoint[player_shoot.currentWeapon].rotation);
        Rigidbody2D rigi = bullet.GetComponent<Rigidbody2D>();
        rigi.AddForce(player_shoot.firePoint[player_shoot.currentWeapon].up * player_shoot.bulletForce[player_shoot.currentWeapon], ForceMode2D.Impulse);
        RpcPlayerShoot(gameObject);
    }

    [ClientRpc]
    private void RpcPlayerShoot(GameObject player) 
    {
        Shooting player_shoot = player.GetComponent<Shooting>();
        GameObject bullet = Instantiate(bulletPrefab, player_shoot.firePoint[player_shoot.currentWeapon].position, player_shoot.firePoint[player_shoot.currentWeapon].rotation);
        Rigidbody2D rigi = bullet.GetComponent<Rigidbody2D>();
        rigi.AddForce(player_shoot.firePoint[player_shoot.currentWeapon].up * player_shoot.bulletForce[player_shoot.currentWeapon], ForceMode2D.Impulse);
    }

    public void Upgrade()
    {
        CmdUpgrade(gameObject);
    }

    private void Reload()
    {
        HU.ChangeAmmo(maxAmmo[currentWeapon] -ammoCount[currentWeapon]);
    }

    [Command]
    private void CmdUpgrade(GameObject player)
    {
        RpcUpgrade(player);
    }

    [ClientRpc]
    private void RpcUpgrade(GameObject player)
    {
        Shooting player_shoot = player.GetComponent<Shooting>();
        player_shoot.weaponlist[player_shoot.currentWeapon].SetActive(false);
        player_shoot.currentWeapon++;
        player_shoot.weaponlist[player_shoot.currentWeapon].SetActive(true);
    }
}