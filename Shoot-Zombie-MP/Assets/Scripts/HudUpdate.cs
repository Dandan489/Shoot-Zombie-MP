using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudUpdate : MonoBehaviour
{
    public Shooting weapon;
    public GameObject weapon1;
    public GameObject weapon2;
    public Player player;

    //ref
    public RectTransform hitpointBar;
    public TMP_Text hitpointText;
    public TMP_Text scoreText;
    public TMP_Text coinText;
    public Animator ammoani;
    public TMP_Text ammoText;
    public RectTransform ammoBar;
    public GameObject upgradePanel;
    private IEnumerator coroutine;

    //logic
    public int score;
    public int coins;
    public int playercnt = 0;
    public bool isreloading = false;

    private void Start()
    {
        ammoText.color = Color.black;
    }

    public void OnHitpointChange()
    {
        float ratio = (float)player.health / (float)player.max_health;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
        hitpointText.text = player.health.ToString();
    }

    public void ChangeScore(int add)
    {
        score += add;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ChangeCoin(int add)
    {
        coins += add;
        coinText.text = "Coins: " + coins.ToString();
    }

    public void ChangeAmmo(int add)
    {
        if (add > 0)
        {
            ammoani.SetTrigger("reload");
            coroutine = Reloading(add);
            StartCoroutine(coroutine);
            return;
        }
        weapon.ammoCount[weapon.currentWeapon] += add;
        ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        weapon.reloading = false;
        if(weapon.ammoCount[weapon.currentWeapon] == 0)
        {
            ammoText.color = Color.red;
        }
    }

    IEnumerator Reloading(int add)
    {
        isreloading = true;
        yield return new WaitForSeconds(0.6f);
        ammoText.color = Color.black;
        weapon.ammoCount[weapon.currentWeapon] += add;
        ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        weapon.reloading = false;
        isreloading = false;
    }

    public void UpgradeWeapon()
    {
        if (coins >= 100 && weapon.currentWeapon < weapon.maxWeapon)
        {
            coins -= 100;
            weapon.Upgrade();
            upgradePanel.SetActive(false);
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        }
    }
}
