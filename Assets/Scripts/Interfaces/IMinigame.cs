public interface IMinigame
{
    void StartMinigame();
    void EndMinigame();
    bool IsGameRunning { get; }
}
