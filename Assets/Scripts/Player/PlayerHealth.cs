using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerHealth : MonoBehaviour
{
    [Header("Infos Joueur")]
    // vie max du joueur. (idéalement 100)
    public int health;
    [Header("UI des dégats")]
    public Image darkenScreenUI;
    public TextMeshProUGUI buttonRespawn;
    public Sprite imageDeDegats1;
    public Sprite imageDeDegats2;
    public Sprite imageDeDegats3;
    private Color color;
    public TextMeshProUGUI lifeText;
    // Ui de fermeture des yeux en fonction des dégats.
    
    private static int _tempHealth;
    private static int _halfLife;
    private static int _quarterLife;
    private static bool _isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isDead = false;
        darkenScreenUI.enabled = true;
        _tempHealth = health;
        lifeText.text = _tempHealth.ToString();
        _halfLife = _tempHealth / 2;
        _quarterLife = _tempHealth / 4;
        Console.WriteLine("Vie du joueur: " + health);
        Debug.Log("Start");
    }
    void Update()
    {
        if (_tempHealth <= 0 && _isDead == false)
        { 
            PlayerDeath();
        }
        Debug.Log("Update");
        lifeText.text = _tempHealth.ToString();
        UpdateTransparency();
    }

    public void PlayerHit(int damage = 1)
    {
        {
            _tempHealth -= damage;
            Debug.Log("joueur touché! nouvelle vie: " + _tempHealth);
        }
    }

    private void UpdateTransparency()
    {
        Debug.Log("Transparence");
        var colorUI = darkenScreenUI.GetComponent<Image>().color;
        if (_tempHealth > _halfLife)
        {
            colorUI = new Color(255, 0, 0, 0f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats1;
            buttonRespawn.enabled = false;
            print("le joueur a toute de sa vie");
        }
        else if (_tempHealth <= _halfLife && _tempHealth >= _quarterLife)
        {
            colorUI = new Color(255, 0, 0, 0.2f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats1;
            buttonRespawn.enabled = false;
            print("le joueur a la moitié de sa vie");
        }
        else if (_tempHealth < _quarterLife && _tempHealth > 0)
        {
            colorUI = new Color(255, 0, 0, 0.5f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats2;
            buttonRespawn.enabled = false;
            print("le joueur a un quart de sa vie");
        }
        else if (_tempHealth <= 0)
        {
            colorUI = new Color(255, 0, 0, 1);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats3;
            print("la couleur de la mort");
        }
    }
    private void PlayerDeath()
    {
        _isDead = true;
        buttonRespawn.enabled = true;
        Debug.Log("Mort");
    }

    public void Relaunch()
    {
        Start();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonPressed()
    {
        buttonRespawn.GetComponent<Button>().onClick.Invoke();
        Debug.Log("ButtonPressed");
    }
}