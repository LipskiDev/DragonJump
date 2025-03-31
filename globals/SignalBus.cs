using Godot;
using System;



public partial class SignalBus : Node
{

	[Signal]
	public delegate void JumpSignalEventHandler();

	private static SignalBus _instance;

	public static SignalBus Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new SignalBus();
			}
			return _instance;
		}
	}

	public override void _Ready()
	{
		if(_instance == null)
		{
			_instance = this;
		}
		else
		{
			QueueFree();
		}
	}



	
}
