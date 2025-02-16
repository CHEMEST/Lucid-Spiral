using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocityTemp = Velocity;

		Vector2 direction = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");
		if (direction != Vector2.Zero)
		{
			velocityTemp = direction * Speed;
		}
		else
		{
			velocityTemp.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocityTemp.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

		Velocity = velocityTemp;
		MoveAndSlide();
	}
}
