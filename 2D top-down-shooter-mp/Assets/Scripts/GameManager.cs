using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        NextWave();
    }

    private void Update()
    {
        if(ongoingwaveupdate == false && enemycnt == 0)
        {
            Debug.Log("Wave " + wave + " ended");
            NextWave();
        }
    }

    //ref
    public Player player;
    public RectTransform hitpointBar;
    public Shooting weapon;
    public TitleText tt;
    public TMP_Text hitpointText;
    public TMP_Text scoreText;
    public TMP_Text coinText;
   
    public Animator ammoani;
    public TMP_Text ammoText;
    public RectTransform ammoBar;
    public GameObject upgradePanel;
    private IEnumerator coroutine;
    public GameObject weapon1;
    public GameObject weapon2;
    private IEnumerator coroutine2;

    //logic
    public int score;
    public int coins;
    public int wave = 0;
    public int enemycnt = 0;
    public bool ongoingwave = false;
    public bool ongoingwaveupdate = false;
    public bool isreloading = false;

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
        if (add > 0){
            ammoani.SetTrigger("reload");
            coroutine = reloading(add);
            StartCoroutine(coroutine);
            return;
        }
        weapon.ammoCount[weapon.currentWeapon] += add;
        ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        weapon.reloading = false;
    }

    IEnumerator reloading(int add)
    {
        isreloading = true;
        yield return new WaitForSeconds(0.6f);
        weapon.ammoCount[weapon.currentWeapon] += add;
        ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        weapon.reloading = false;
        isreloading = false;
    }

    public void UpgradeWeapon()
    {
        if(coins >= 100 && weapon.currentWeapon < weapon.maxWeapon)
        {
            coins -= 100;
            weapon.Upgrade();
            upgradePanel.SetActive(false);
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            ammoText.text = weapon.ammoCount[weapon.currentWeapon].ToString() + " / " + weapon.maxAmmo[weapon.currentWeapon].ToString();
        }
    }

    public void NextWave()
    {
        ongoingwaveupdate = true;
        wave++;
        if(wave == 11)
        {
            Application.Quit();
        }
        coroutine2 = BetweenWave();
        StartCoroutine(coroutine2);
    }

    IEnumerator BetweenWave()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Wave " + wave + " start");
        tt.Show("Wave " + wave);
        ongoingwave = true;
        ongoingwaveupdate = false;
    }
}
