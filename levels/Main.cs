using Godot;
using System;
using System.Diagnostics;

public partial class Main : Node2D
{
    double start;
    double end;

    int jumpCount = 0;

    Camera2D camera;
    Player player;

    [Export] Vector2I ScreenSize = new Vector2I(320, 1800);
    [Export] int Offset = 0;
    [Export] int OtherOffset = 0;
    public override void _Ready()
    {
        StartGame();
        SignalBus.Instance.JumpSignal += IncreaseJumpCount;
        SignalBus.Instance.ChangeCameraPositionSignal += ChangeCameraPosition;
        camera = GetNode<Camera2D>("Camera");
        player = GetNode<Player>("Player");
    }

    public override void _Process(double _delta)
    {
        UpdateCamera();
    }


    public void StartGame()
    {
        start = Time.GetUnixTimeFromSystem();
    }

    public void UpdateCamera()
    {
        Vector2 playerPos = player.GlobalPosition;
        int screenY = Mathf.FloorToInt((playerPos.Y + Offset - OtherOffset) / ScreenSize.Y);

        // Snap the camera to that screen
        camera.GlobalPosition = new Vector2(
            0,
            screenY * ScreenSize.Y + ScreenSize.Y / 2 + OtherOffset
        );
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

    public void ChangeCameraPosition(Vector2 NewPosition)
    {
        camera.GlobalPosition = NewPosition;
    }


    
}
