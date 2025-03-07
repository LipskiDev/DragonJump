using System;
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class Player : RigidBody2D
{

	Vector2 MouseStartPos;
	Vector2 MouseCurrentPos;
	bool showLine = false;
	bool inputHasStarted = false;
	float threshold = 0.001f;


	[Export]
	float minLaunchStrength { get; set; } = 10;
	
	[Export]
	float maxLaunchStrength { get; set; } = 100;
	
	[Export]
	float launchMultiplier { get; set; } = 1;

	Vector2 SlopeDirection = Vector2.Down;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.TimeScale = 1.3;		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(LinearVelocity.Length() < threshold)
		{
			LinearVelocity = Vector2.Zero;
		}
	}

	public void LaunchPlayer(Vector2 startPosition, Vector2 endPosition)
	{
		Vector2 launchDirection = endPosition - startPosition;

		launchDirection = ClipVector(launchDirection, minLaunchStrength, maxLaunchStrength);
		
		ApplyCentralImpulse(-launchDirection * launchMultiplier);
	}

	public void DrawLine(Vector2 startPosition, Vector2 endPosition)
	{
		Vector2 LaunchLineDirection = endPosition - startPosition;
		LaunchLineDirection = ClipVector(LaunchLineDirection, minLaunchStrength, maxLaunchStrength);
		GetNode<Line2D>("Line2D").AddPoint(GlobalPosition - Position);
		GetNode<Line2D>("Line2D").AddPoint(GlobalPosition - Position + LaunchLineDirection);
	}

	

	public Vector2 ClipVector(Vector2 input, float minLenght, float maxLength) 
	{
		float length = input.Length();

		Vector2 normalized = input.Normalized();

		if(length < minLenght) 
		{
			return normalized * minLenght;
		} 
		else if(length > maxLength)
		{
			return normalized * maxLength;
		}

		return input;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if(@event.IsAction("Restart"))
		{
			GetTree().ReloadCurrentScene();
		}
		
		if(LinearVelocity != Vector2.Zero) return;
		if(@event.IsAction("click") && @event is InputEventMouse mouseEvent)
		{
			if(@event.IsPressed())
			{
				inputHasStarted = true;
				showLine = true;
				MouseStartPos = mouseEvent.GlobalPosition;
			} 
			else if(@event.IsReleased())
			{
				if(inputHasStarted)
				{
					showLine = false;
					MouseCurrentPos = mouseEvent.GlobalPosition;

					GetNode<Line2D>("Line2D").ClearPoints();

					LaunchPlayer(MouseStartPos, MouseCurrentPos);
					inputHasStarted = false;
				}
				
			}
		}

		if(@event is InputEventMouseMotion mouseMotionEvent) 
		{
			GetNode<Line2D>("Line2D").ClearPoints();
			MouseCurrentPos = mouseMotionEvent.GlobalPosition;
			if(showLine) 
			{
				DrawLine(MouseStartPos, MouseCurrentPos);
			}
		}
	}
}
