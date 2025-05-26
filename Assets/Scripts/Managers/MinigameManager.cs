using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{
    private Dictionary<string, IMinigame> minigames = new Dictionary<string, IMinigame>();
    private IMinigame currentMinigame;
    public bool minigameActive = false;

    public Key key;
    public PoliceRoomKey policeRoomKey;
    public Crowbar crowbar;

    public void RegisterMinigame(string name, IMinigame minigame)
    {
        if (!minigames.ContainsKey(name))
        {
            minigames.Add(name, minigame);
        }
    }

    void Start()
    {
        RegisterMinigame("OpenDoor", GetComponent<OpenDoorMG>());
        RegisterMinigame("BrakeDoor", GetComponent<BrakeDoorMG>());
    }
    void Update()
    {
        if (minigameActive && Input.GetKeyDown(KeyCode.Escape))
        {
            StopCurrentMinigame();
        }
    }

    public void StartMinigame(string name)
    {
        if (minigames.ContainsKey(name) && !minigameActive)
        {
            minigameActive = true;
            currentMinigame?.EndMinigame();
            currentMinigame = minigames[name];
            currentMinigame.StartMinigame();
        }
    }

    public void StopCurrentMinigame()
    {
        currentMinigame?.EndMinigame();
        currentMinigame = null;
        minigameActive = false;

    }
}
