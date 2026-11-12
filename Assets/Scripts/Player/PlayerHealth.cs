using UnityEngine;
using System;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Infos Joueur")]
    // vie max du joueur. (idéalement 100)
    public int health;
    // des images d'UI qu'on affiche en fonction de notre vie.
    [Header("UI des dégats")]
    public Image darkenScreenUI;
    public Sprite imageDeDegats1;
    public Sprite imageDeDegats2;
    public Sprite imageDeDegats3;
    private Color _color;
    public TextMeshProUGUI lifeText;
    // Variables qui permettent de définir la moitié de la vie, son quart etc.
    private static int _tempHealth;
    private static int _halfLife;
    private static int _quarterLife;
    private static bool _isDead;
    private static string _currentState = "default";
    
    // On récupère ici le profil de global volume qu'on veut au démarrage
    [Header("Global Volume par défault")]
    public VolumeProfile defaultGlobalVolumeProfile;
    
    // Le texte qui s'affiche quand on peut respawn (pour l'instant j'essaie de faire en sorte
    // qu'il s'affiche quand on appuie sur O.
    [Header("Bouton de Respawn")]
    public TextMeshProUGUI buttonRespawn;

    [Header("Canvas d'indice")]
    public GridLayoutGroup uiIndice;
    public Canvas indiceE;
    public Canvas indiceX;
    public Canvas indiceI;
    public Canvas indiceT;
    // Variables pour être sûr que l'input est toggle ou non etc.
    private bool _boutonIndiceOn;
    private bool _toucheIndicePressed;
    
    //Tout ça je l'ai implémenté au cas où c'est nécessaire pour les inputs.
    [Header("Input System (New Input System)")]
    [Tooltip("Le composant PlayerInput situé sur votre Joueur.")]
    private PlayerInput _playerInputComponent;
    private IInteractable _currentInteractable;
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private InputAction _respawn;
    private InputAction _clue;
    
    //-
     // Dans le Start je définit les fonctions par défaut et j'applique le global volume et l'état par défaut.
    void Start()
    {
        uiIndice.enabled = false;
        buttonRespawn.GetComponent<TextMeshProUGUI>().enabled = false;
        _toucheIndicePressed = false;
        if (_playerInputComponent == null)
            _playerInputComponent = FindFirstObjectByType<PlayerInput>();

        // on bind les touches aux variables des actions.
        if (_playerInputComponent != null)
        {
            _respawn = _playerInputComponent.actions.FindAction("Respawn");
            if (_respawn == null)
                Debug.Log("l'action respawn n'existe pas");
            _clue = _playerInputComponent.actions.FindAction("Clue");
            if (_clue == null)
                Debug.Log("l'action clue n'existe pas");
        }
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _isDead = false;
        darkenScreenUI.enabled = true;
        _tempHealth = health;
        lifeText.text = _tempHealth.ToString();
        _halfLife = _tempHealth / 2;
        _quarterLife = _tempHealth / 4;
        ApplyGlobalVolume(defaultGlobalVolumeProfile);
        ApplyState("default");
        Console.WriteLine("Vie du joueur: " + health);
        Debug.Log("Start");
    }
    
    //-
    // Dans l'update je vérifie si le joueur est mort et je met à jour l'affichage de sa vie.
    // J'ai aussi essayé de mettre une ligne pour vérifier si la touche de respawn était pressée.
    void Update()
    {
        if (_tempHealth <= 0 && _isDead == false)
        {
            
            Debug.Log("UI mort: lancement");
            PlayerDeath();
        }
        else if (_tempHealth <= 0 && _isDead)
        {
            if (_respawn.IsPressed())
            {
                Relaunch();
            }
        }
        Debug.Log("Update");
        lifeText.text = _tempHealth.ToString();

        if (_clue.IsPressed())
        {
                ToggleClue();
                _toucheIndicePressed = true;
        }
        else
        {
            _toucheIndicePressed = false;
        }
    }

    //C'est une fonction pour faire des dégats au joueur puis vérifier/mettre à jour son état.
    public void PlayerHit(int damage = 1)
    {
        {
            _tempHealth -= damage;
            Debug.Log("joueur touché! nouvelle vie: " + _tempHealth);
            CheckPlayerHealth();
        }
    }

    // La fonction qui vérifie l'état de vie du joueur.
    // Elle compare son état actuel avec l'état du dernier changement.
    // Elle demande un changement d'UI si l'état à changé.
    private void CheckPlayerHealth()
    {
        if (_tempHealth <= _halfLife && _tempHealth > _quarterLife && _currentState != "default")
        {
            ApplyState("default");
        }
        else if (_tempHealth <= _halfLife && _tempHealth > _quarterLife && _currentState != "peur1")
        {
            ApplyState("peur1");
        }
        else if (_tempHealth <= _quarterLife && _tempHealth > 0 && _currentState != "peur2")
        {
            ApplyState("peur2");
        }
        else if (health <= 0 && _currentState != "mort")
        {
            ApplyState("mort");
        }
    }

    // Là on applique l'UI correspondante au nouvel état du joueur.
    public void ApplyState(string state)
    {
        Debug.Log("Transparence");
        var colorUI = darkenScreenUI.GetComponent<Image>().color;
        if (state == "default")
        {
            colorUI = new Color(255, 0, 0, 0f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats1;
            print("le joueur a toute de sa vie");
        }
        else if (state == "peur3")
        {
            colorUI = new Color(255, 0, 0, 0.1f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats1;
            print("le joueur a la moitié de sa vie");
        }
        else if (state == "peur2")
        {
            colorUI = new Color(255, 0, 0, 0.2f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats2;
            print("le joueur a un quart de sa vie");
        }
        else if (state == "peur3")
        {
            colorUI = new Color(255, 0, 0, 0.3f);
            darkenScreenUI.color =  colorUI;
            darkenScreenUI.GetComponent<Image>().sprite = imageDeDegats3;
            print("la couleur de la mort");
        }
    }
    
    // On relance le jeu après la mort du joueur.
    // la ligne -> buttonRespawn.enabled = true; <- était là pour quand je retirais -> Relaunch() <- pour vérifier si le bouton s'affichait bien.
    public void PlayerDeath()
    {
        _isDead = true;
        buttonRespawn.GetComponent<TextMeshProUGUI>().enabled = true;
        Debug.Log("Mort");
    }

    // La fonction qui redémarre la scène après la mort.
    public void Relaunch()
    {
        Start();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // La fonction qui applique le global volume souhaité (indépendamment de l'état du joueur)
    // ça sera surement un deuxième script à terme.
    public void ApplyGlobalVolume(VolumeProfile profile)
    {
        var volume = FindAnyObjectByType<Volume>();
        volume.GetComponent<Volume>().profile = profile;
        Debug.Log(volume.gameObject.GetComponent<Volume>().profile.name);
    }
    
    // La fonction que j'essaie de faire fonctionner en appuyant sur le "o" de mon clavier.
    // Fait apparaitre le texte "Respawn" à l'écran.
    public void ToggleClue()
    {
        if (_boutonIndiceOn && _toucheIndicePressed == false)
        {
            uiIndice.enabled = true;
            _boutonIndiceOn = false;
            Debug.Log("indice pas visible");
        }
        else if (_boutonIndiceOn == false && _toucheIndicePressed == false)
        {
            
            uiIndice.enabled = false;
            _boutonIndiceOn = true;
            Debug.Log("indice visible");
        }
    }

    public void CollectClue(string clue)
    {
        if (clue == "E")
        {
            indiceE.enabled = true;
        }
        else if (clue == "E")
        {
            indiceX.enabled = true;
        }
        else if (clue == "E")
        {
            indiceI.enabled = true;
        }
        else if (clue == "E")
        {
            indiceT.enabled = true;
        }
        else
        {
            Debug.Log(clue + " n'est pas un indice valide!");
        }
    }
}