using Godot;
using System;

public partial class Adventurer : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float Acceleration = 1500.0f;
	public const float Friction = 1200.0f;
	
	AnimatedSprite2D charanim;
	// Remember last facing direction
	private Vector2 lastDirection = Vector2.Down; 
	
	private string GetIdleAnimation(Vector2 direction)
	{
		//normalise for clean angle
		direction = direction.Normalized();
		
		// Calculate angle in radians, then convert to degrees
		float angleRad = Mathf.Atan2(direction.Y, direction.X);
		float angle = angleRad * 180f / Mathf.Pi;  
		
		// make sure its in 0-360 range
		if (angle < 0) {
			angle += 360;
		}
		// 6 directions 
		if (angle >= 30f && angle < 90f)
			return "Idle-right_down";
		else if (angle >= 90f && angle < 150f)
			return "Idle-down";
		else if (angle >= 150f && angle < 210f)
			return "Idle-left_down";
		else if (angle >= 210f && angle < 270f)
			return "Idle-left_up";
		else if (angle >= 270f && angle < 330f)
			return "Idle-up";
		else // 330-360 and 0-30
			return "Idle-right_up";
	}
	
	private string GetWalkAnimation(Vector2 direction)
	{
		// normalise for clean angle
		direction = direction.Normalized();
		
		// Calculate angle in radians, then convert to degrees
		float angleRad = Mathf.Atan2(direction.Y, direction.X);
		float angle = angleRad * 180f / Mathf.Pi;
		
		// make sure its in 0-360 range
		if (angle < 0) {
			angle += 360;
		}
		// 6 directions 
		if (angle >= 30f && angle < 90f)
			return "Walk-right_down";
		else if (angle >= 90f && angle < 150f)
			return "Walk-down";
		else if (angle >= 150f && angle < 210f)
			return "Walk-left_down";
		else if (angle >= 210f && angle < 270f)
			return "Walk-left_up";
		else if (angle >= 270f && angle < 330f)
			return "Walk-up";
		else // 330-360 and 0-30
			return "Walk-right_up";
	}
	
	public override void _Ready()
	{
		charanim = GetNode<AnimatedSprite2D>("Laura");
		charanim.Play("Idle-down");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Get the input direction (8-directional movement)
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (direction != Vector2.Zero)
		{
			// Accelerate in the input direction
			velocity = velocity.MoveToward(direction * Speed, Acceleration * (float)delta);
			
			// Update last direction for idle animations
			lastDirection = direction;
			
			// Determine which animation to play based on direction
			string animationName = GetWalkAnimation(direction);
			charanim.Play(animationName);
			
		}
		else
		{
			// Apply friction when no input
			velocity = velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
			
			// Play idle animation facing the last direction
			string idleAnimation = GetIdleAnimation(lastDirection);
			charanim.Play(idleAnimation);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
