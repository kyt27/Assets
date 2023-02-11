using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private readonly List<string> scenes = new() { "TitleScreen", "Cutscene1", "Dungeon1", "Cutscene2", "Dungeon2", "Cutscene3", "Dungeon3", "Cutscene4" };
    private int currentScene = 2;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(eventSystem);
            return;
        }

        Reset();

        instance = this;

        //Call function when a scene is loaded, needs to have a Scene and LoadSceneLoad
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    //Resources
    public List<Sprite> playerSprites;

    //References
    public Player player;


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
        SceneManager.LoadScene(scenes[currentScene]);
        player.Respawn();
    }

    public void LoadNextScene() {
        Reset();
        currentScene += 1;
        Debug.Log("WTF");
        SceneManager.LoadScene(scenes[currentScene]);
    }


    private void Reset() {
        player.hitpoint = new int[] { 10, 10, 10, 10 };
    }
}
