using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Infos Joueur")]
    // vie max du joueur. (idéalement 100)
    public int health;
    // Ui de fermeture des yeux en fonction des dégats.
    public Image darkenScreenUI;
    // Les 3 états de la vie du joueur (je les utilise que dans UpdateTransparency pour l'ui pour l'instant)
    private bool _fullLife;
    private bool _midLife;
    private bool _quarterLife;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Console.WriteLine("Vie du joueur: " + health);
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayerDeath();
            
        }
        UpdateTransparency();
    }
    
    public void PlayerHit(int damage = 1)
    {
        {
            health -= damage;
            Console.WriteLine("joueur touché! nouvelle vie: " + health);
            
        }
    }

    private void UpdateTransparency()
    {
        if (health > 50 && _fullLife == false)
        {
            _fullLife = true;
            _midLife = false;
            _quarterLife = false;
            darkenScreenUI.GetComponent<Image>().color.a = 50;
            Console.WriteLine("le joueur a toute de sa vie");
        }
        else if (50 >= health && health >= 25 && _midLife == false)
        {
            if (_midLife == false){
            _fullLife = false;
            _midLife = true;
            _quarterLife = false;
            darkenScreenUI.GetComponent<Image>().color.a = 150;
            Console.WriteLine("le joueur a la moitié de sa vie");}
        }
        else if (health < 25 && _quarterLife == false)
        {
            _fullLife = false;
            _midLife = false;
            _quarterLife = true;
            darkenScreenUI.GetComponent<Image>().color.a = 255;
            Console.WriteLine("le joueur a un quart de sa vie");
        }
    }
    private void PlayerDeath()
    {
        {
            Console.WriteLine("joueur est mort");
        }
    }
    // Update is called once per frame
}