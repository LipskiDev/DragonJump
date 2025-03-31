using System;
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class Player : RigidBody2D
{
	Vector2 MouseStartPos;
	Vector2 MouseCurrentPos;
	
	bool showLine = false;
	bool inputHasStarted = false;
	float threshold = 0.01f;

	[Export]
	float gameSpeed { get; set; } = 1.3f; 

	[Export]
	float maximumSpeed { get; set; } = 10f;


	[Export]
	float minLaunchStrength { get; set; } = 10;
	
	[Export]
	float maxLaunchStrength { get; set; } = 100;
	
	[Export]
	float launchMultiplier { get; set; } = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Engine.TimeScale = gameSpeed;		
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




		Line2D line2D = GetNode<Line2D>("Line2D");
		line2D.Rotation = -Rotation;
		
	}

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

		if(LinearVelocity.Y > maximumSpeed)
		{
			LinearVelocity = LinearVelocity with {Y = maximumSpeed};
		}
    }

    public void LaunchPlayer(Vector2 startPosition, Vector2 endPosition)
	{
		Vector2 launchDirection = endPosition - startPosition;

		launchDirection = ClipVector(launchDirection, minLaunchStrength, maxLaunchStrength);
		
		ApplyCentralImpulse(-launchDirection * launchMultiplier);
		GD.Print((-launchDirection * launchMultiplier).Length());

		SignalBus.Instance.EmitSignal(nameof(SignalBus.JumpSignal));
	}

	public void DrawLine(Vector2 startPosition, Vector2 endPosition)
	{
		Vector2 LaunchLineDirection = endPosition - startPosition;
		LaunchLineDirection = ClipVector(LaunchLineDirection, minLaunchStrength, maxLaunchStrength);
		GetNode<Line2D>("Line2D").AddPoint(GlobalPosition - Position);
		GetNode<Line2D>("Line2D").AddPoint(GlobalPosition - Position + LaunchLineDirection);
	}

	

	public Vector2 ClipVector(Vector2 input, float minLength, float maxLength) 
	{
		float length = input.Length();

		Vector2 normalized = input.Normalized();

		if(length < minLength) 
		{
			return normalized * minLength;
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
		
		if(LinearVelocity.Length() >= threshold) return;
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
