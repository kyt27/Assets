using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private readonly List<string> scenes = new() { "TitleScreen", "Cutscene1", "Dungeon1", "Cutscene2", "Dungeon2", "Cutscene3", "Dungeon3", "Cutscene4" };
    private readonly List<bool> dialogue = new() {         false,        true,      false,        true, false, true, false, true };
    private readonly int[] musicC1 = {  2,  2, 0,  2, 0,  3, 0,  3 };
    private readonly int[] musicC2 = { -1, -1, 1, -1, 1, -1, 1, -1 };
    private int currentScene = 0;
    private Vector3 respawnPosition;
    private string currentLevel;
    private bool fromBattle = false;

    private void Awake() {
        currentScene = scenes.IndexOf(SceneManager.GetActiveScene().name);
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        if (GameObject.FindWithTag("Tracker") != null)
        {
            tracker = GameObject.FindWithTag("Tracker").GetComponent<EnemyTracker>();
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
    public EnemyTracker tracker;

    public RectTransform healthBar;
    public TextMeshPro healthText;
    public GameObject hud;
    public GameObject menu;

    bool respawning = true;

    public Animator showInstall;
    private List<string> battleEnemyTypes;
    private List<int> battleEnemyHitpoints;
    private List<int> battleEnemyMaxHitpoints;
    private int battleId;
    private List<bool> battleStatuses;

    public void OnLevelLoad(Scene s, LoadSceneMode mode)
    {
        if (dialogue[currentScene])
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            GameObject.FindWithTag("Dialogue").GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        if (GameObject.FindWithTag("Tracker") != null)
        {
            tracker = GameObject.FindWithTag("Tracker").GetComponent<EnemyTracker>();
        }
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
        if (!respawning)
        {
            if (tracker != null)
            {
                tracker.KillDeadEnemies(battleStatuses);
            }
        }
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            CombatControl combatHandler = GameObject.Find("CombatHandler").GetComponent<CombatControl>();
            combatHandler.gameManager = this;
            combatHandler.enemyLevel = ((int) (currentScene + 1) / 2);

            combatHandler.inputEnemyTypes = battleEnemyTypes;
            combatHandler.inputEnemyHitpoints = battleEnemyHitpoints;
            combatHandler.inputEnemyMaxHitpoints = battleEnemyMaxHitpoints;

            MusicEngine.instance.FadeToChannel2(0.5F);
        }

        if (!fromBattle) {
            SetupMusic();
        } 
        else
        {
            fromBattle = false;
        }

    }

    private void SetupMusic()
    {
        IEnumerator WaitUntilReady()
        {
            while(MusicEngine.instance == null)
            {
                yield return null;
            }
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
        StartCoroutine(WaitUntilReady());
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
        battleStatuses[battleId] = false;
        SceneManager.LoadScene(scenes[currentScene]);
        respawning = false;
        MusicEngine.instance.FadeToChannel1(0.5F);
        fromBattle = true;
    }

    public void Respawn() {
        Reset();
        SceneManager.LoadScene(scenes[currentScene]);
        player.Respawn();
    }

    public void LoadBattle(Enemy enemy)
    {
        battleId = enemy.id;
        respawnPosition = player.transform.position;
        currentLevel = SceneManager.GetActiveScene().name;
        battleEnemyTypes = enemy.types;
        battleEnemyHitpoints = enemy.hitpoints;
        battleEnemyMaxHitpoints = enemy.maxHitpoints;
        battleStatuses = new List<bool> (tracker.enemyStatuses.Count);
        for (int i = 0; i < tracker.enemyStatuses.Count; i++)
        {
            Debug.Log(i);
            battleStatuses.Add(tracker.enemyStatuses[i]);
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
