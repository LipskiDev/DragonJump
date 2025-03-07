using Godot;
using System;

public partial class RoomController : Area2D
{
	// Called when the node enters the scene tree for the first time.
	Camera2D MyCamera;
	public override void _Ready()
	{
		MyCamera = GetNode<Camera2D>("Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void _OnBodyEntered(Node2D body) 
	{
		if(body.HasMeta("IsPlayer"))
		{
			MyCamera.Enabled = true;
		}
	}

	public void _OnBodyExited(Node2D body)
	{
		if(body.HasMeta("IsPlayer"))
		{
			GD.Print("Exited");
			MyCamera.Enabled = false;
		}
	}



	
}
