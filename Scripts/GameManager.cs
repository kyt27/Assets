using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(textBoxManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(eventSystem);
            return;
        }

        Reset();

        instance = this;

        //Call function when a scene is loaded, needs to have a Scene and LoadSceneLoad
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    //Resources
    public List<Sprite> playerSprites;

    //References
    public Player player;
    public TextBoxManager textBoxManager;

    public RectTransform healthBar;
    public TextMeshPro healthText;
    public GameObject hud;
    public GameObject menu;
    public GameObject eventSystem;

    public Animator deathMenuAnim;

    public void OnLevelLoad(Scene s, LoadSceneMode mode) {
        player.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
    }

    public void Respawn() {
        deathMenuAnim.SetTrigger("Hide");
        Reset();
        SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public void SaveState() {
        string s = "";

        s += player.getSkinID().ToString() + "|";
        s += player.hitpoint.ToString() + "|";

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= LoadState;

        //First load of the game
        if (!PlayerPrefs.HasKey("SaveState")) return;

        //0 - skin, 1 - hitpoint
        string[] data = PlayerPrefs.GetString("SaveState").Split("|");

        player.SwapSprite(int.Parse(data[0]));
        player.hitpoint = int.Parse(data[1]);
    }

    private void Reset() {
        PlayerPrefs.DeleteAll();
    }
}
