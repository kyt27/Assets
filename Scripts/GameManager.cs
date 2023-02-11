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

    public Animator showInstall;

    public void OnLevelLoad(Scene s, LoadSceneMode mode) {
        player.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
    }

    public void Respawn() {
        Reset();
        SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public void SaveState() {
        string s = "";

        s += player.hitpoint[0].ToString() + "|";
        s += player.hitpoint[1].ToString() + "|";
        s += player.hitpoint[2].ToString() + "|";
        s += player.hitpoint[3].ToString() + "|";
        s += player.getCharacterID().ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= LoadState;

        //First load of the game
        if (!PlayerPrefs.HasKey("SaveState")) return;

        //0-3 hitpoints, 4 character b
        string[] data = PlayerPrefs.GetString("SaveState").Split("|");

        player.hitpoint[0] = int.Parse(data[0]);
        player.hitpoint[1] = int.Parse(data[1]);
        player.hitpoint[2] = int.Parse(data[2]);
        player.hitpoint[3] = int.Parse(data[3]);
        player.SwapSprite(int.Parse(data[4]));
    }

    private void Reset() {
        PlayerPrefs.DeleteAll();
    }
}
