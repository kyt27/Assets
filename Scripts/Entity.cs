using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health = 12;
    public int max_health = 12;
    public string display_name = "";
    public List<string> skills = new List<string>();
    public IDictionary<string, int> statuses = new Dictionary<string, int>();
    public bool is_enemy = false;
    public HealthBar bar;
    public SpriteBox box;
    public Color baseBackground = new Color(0.25F, 0.25F, 0.25F, 1);
    public Color hurtBackground = new Color(1, 0.25F, 0.25F, 1);
    public int flashTime = 50;
    private SFXEngine sfx;
    public string hurtSound = "Hurt";

    // Start is called before the first frame update
    void Start()
    {
        sfx = GameObject.Find("SFX Engine").GetComponent<SFXEngine>();
        box.SetBackgroundColor(baseBackground);
        bar.SetValAndMaxVal(this.health, this.max_health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int value, int max_value)
    {
        this.health = value;
        this.max_health = max_value;
        bar.SetValAndMaxVal(this.health, this.max_health);
    }

    public void ChangeHealth(int value)
    {
        this.health -= value;
        if (this.health < 0){
            this.health = 0;
        }
        if (this.health > this.max_health){
            this.health = this.max_health;
        }
        bar.SetValAndMaxVal(this.health, this.max_health);
        box.FlashBackgroundColor(hurtBackground, flashTime);
        sfx.PlayClip(hurtSound);
    }

    public void ModStatus(string name, int duration){
        if (this.statuses.ContainsKey(name)){
            this.statuses[name] += duration+1;
        }else{
            this.statuses.Add(name, duration);
        }
        UpdateStatuses();
    }

    public void UpdateStatuses(){
        foreach(string key in new List<string>(this.statuses.Keys)){
            int d = this.statuses[key]-1;
            string n = (string)key;
            if (d <= 0){
                this.statuses.Remove(n);
            }else{
                this.statuses[n] = d;
            }
        }
    }

    public bool IsDead(){
        if (this.health <= 0){
            return true;
        }
        return false;
    }
}
