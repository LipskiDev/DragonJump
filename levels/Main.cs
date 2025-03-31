using Godot;
using System;

public partial class Main : Node2D
{
    double start;
    double end;

    int jumpCount = 0;

    public override void _Ready()
    {
        StartGame();
        SignalBus.Instance.JumpSignal += IncreaseJumpCount;
    }

    public void StartGame()
    {
        start = Time.GetUnixTimeFromSystem();
    }
    public void EndGame()
    {
        end = Time.GetUnixTimeFromSystem();

        GD.Print($"Total time: {end - start}[s]");
        GD.Print($"Amount of Jumps: {jumpCount}");

    }

    public void OnEndAreaEntered(Node2D body)
    {
        EndGame();
    }

    public void IncreaseJumpCount()
    {
        ++jumpCount;
    }


    
}
