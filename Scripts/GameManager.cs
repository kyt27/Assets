using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private readonly List<string> scenes = new() { "TitleScreen", "Cutscene1", "Dungeon1", "Cutscene2", "Dungeon2", "Cutscene3", "Dungeon3", "Cutscene4" };
    private readonly int[] musicC1 = {  2,  1, 0,  1, 0,  2, 0,  0 };
    private readonly int[] musicC2 = { -1, -1, 1, -1, 1, -1, 1, -1 };
    private int currentScene = 2;
    private Vector3 respawnPosition;
    private string currentLevel;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        DontDestroyOnLoad(gameObject);

        instance = this;

        //Call function when a scene is loaded, needs to have a Scene and LoadSceneLoad
        SceneManager.sceneLoaded += OnLevelLoad;
        SetupMusic();
    }

    //Resources
    public List<Sprite> playerSprites;

    //References
    public Player player;

    public RectTransform healthBar;
    public TextMeshPro healthText;
    public GameObject hud;
    public GameObject menu;

    bool respawning = true;

    public Animator showInstall;
    private List<string> battleEnemyTypes;
    private List<int> battleEnemyHitpoints;
    private List<int> battleEnemyMaxHitpoints;

    public void OnLevelLoad(Scene s, LoadSceneMode mode)
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        if (GameObject.FindWithTag("SpawnPoint") != null)
        {
            if (respawning)
            {
                player.gameObject.SetActive(true);
                player.transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
            } else
            {
                player.gameObject.SetActive(true);
                player.transform.position = respawnPosition;
            }
        }
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            CombatControl combatHandler = GameObject.Find("CombatHandler").GetComponent<CombatControl>();
            combatHandler.gameManager = this;

            combatHandler.inputEnemyTypes = battleEnemyTypes;
            combatHandler.inputEnemyHitpoints = battleEnemyHitpoints;
            combatHandler.inputEnemyMaxHitpoints = battleEnemyMaxHitpoints;

            MusicEngine.instance.FadeToChannel2(0.5F);
        }

    }

    private void SetupMusic()
    {
        int sceneIndex = scenes.IndexOf(SceneManager.GetActiveScene().name);
        if (sceneIndex >= 0)
        {
            int track1 = musicC1[sceneIndex];
            int track2 = musicC2[sceneIndex];
            if (track2 == -1)
            {
                MusicEngine.instance.PlayOnChannel1(track1, true);
                MusicEngine.instance.StopChannel(2);
            }
            else
            {
                MusicEngine.instance.PlayOnBoth(track1, track2, true, true);
                MusicEngine.instance.MuteChannel(2);
            }
        }
    }

    public void Lose()
    {
        Debug.Log("Lose");
        SceneManager.LoadScene(scenes[currentScene]);
        respawning = true;
        Reset();
        player.Respawn();
    }

    public void Win()
    {
        Debug.Log("Win");
        SceneManager.LoadScene(scenes[currentScene]);
        respawning = false;
        MusicEngine.instance.FadeToChannel1(0.5F);
    }

    public void Respawn() {
        Reset();
        SceneManager.LoadScene(scenes[currentScene]);
        player.Respawn();
    }

    public void LoadBattle(Enemy enemy)
    {
        respawnPosition = player.transform.position;
        currentLevel = SceneManager.GetActiveScene().name;
        List<Enemy> battleEnemies = new() { enemy };
        battleEnemyTypes = new List<string>();
        battleEnemyHitpoints = new List<int>();
        battleEnemyMaxHitpoints = new List<int>();
        foreach (Enemy e in battleEnemies)
        {
            battleEnemyTypes.Add("bat");
            battleEnemyHitpoints.Add(enemy.hitpoint);
            battleEnemyMaxHitpoints.Add(enemy.maxHitpoint);
        }
        SceneManager.LoadScene("Battle");
    }

    public void LoadNextScene() {
        Reset();
        currentScene += 1;
        Debug.Log("WTF");
        SceneManager.LoadScene(scenes[currentScene]);
    }


    private void Reset() {
        PlayerData.instance.Respawn();
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        MusicEngine.instance.Setup();
    }
}
