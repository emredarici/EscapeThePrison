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
    public AudioSource opendoorAudioSource;
    public AudioSource brakedoorAudioSource;

    public void RegisterMinigame(string name, IMinigame minigame)
    {
        if (!minigames.ContainsKey(name))
        {
            minigames.Add(name, minigame);
            Debug.Log($"Minigame '{name}' registered successfully.");
        }
    }

    void Start()
    {
    }
    void Update()
    {
        if (minigameActive && Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlayAudio(opendoorAudioSource, AudioManager.Instance.minigameSource, 2);
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
        minigameActive = false;

    }
}
