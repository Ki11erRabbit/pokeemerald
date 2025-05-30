using Godot;
using System;

namespace PokeEmerald.Characters.Player;

public enum AnimationState
{
	idle_down,
	idle_up,
	idle_left,
	idle_right,
	turn_down,
	turn_up,
	turn_left,
	turn_right,
	walk_down,
	walk_up,
	walk_left,
	walk_right,
	run_down,
	run_up,
	run_left,
	run_right,
	bike_idle_down,
	bike_idle_up,
	bike_idle_left,
	bike_idle_right,
	bike_acro_ride_down,
	bike_acro_ride_up,
	bike_acro_ride_left,
	bike_acro_ride_right,
	bike_mach_ride_down,
	bike_mach_ride_up,
	bike_mach_ride_left,
	bike_mach_ride_right,
	bike_acro_wheelie_idle_down,
	bike_acro_wheelie_idle_up,
	bike_acro_wheelie_idle_left,
	bike_acro_wheelie_idle_right,
	bike_acro_wheelie_ride_down,
	bike_acro_wheelie_ride_up,
	bike_acro_wheelie_ride_left,
	bike_acro_wheelie_ride_right,
	bike_acro_wheelie_start_down,
	bike_acro_wheelie_start_up,
	bike_acro_wheelie_start_left,
	bike_acro_wheelie_start_right,
}

public enum WheelieState
{
	ToWheelie,
	FromWheelie,
	InWheelie,
	NoWheelie,
}

public partial class PlayerAnimation : CharacterAnimation
{
	[ExportCategory("Animation Vars")] 
	[Export]
	public AnimationState AnimationState = AnimationState.idle_down;
	[ExportCategory("Animation")]
	[Export] public AnimationPlayer AnimationPlayer;
	private WheelieState _wheelieState = WheelieState.NoWheelie;
	public override void PlayAnimation(string animationType)
	{
		var previousAnimation = AnimationState;

		if (CharacterMovement.IsMoving() && !GameState.GameState.Instance.DoingWheelie) return;

		if (GameState.GameState.Instance.DoingWheelie && _wheelieState == WheelieState.NoWheelie)
		{
			_wheelieState = WheelieState.ToWheelie;
			SetAnimationState([AnimationState.bike_acro_wheelie_start_up, AnimationState.bike_acro_wheelie_start_left, AnimationState.bike_acro_wheelie_start_right, AnimationState.bike_acro_wheelie_start_down]);
			
			if (previousAnimation != AnimationState)
			{
				AnimatedSprite.Play(AnimationState.ToString());
			}

			return;
		}
		else if (!GameState.GameState.Instance.DoingWheelie && _wheelieState == WheelieState.InWheelie)
		{
			_wheelieState = WheelieState.FromWheelie;
			SetAnimationState([AnimationState.bike_acro_wheelie_start_up, AnimationState.bike_acro_wheelie_start_left, AnimationState.bike_acro_wheelie_start_right, AnimationState.bike_acro_wheelie_start_down]);
			
			if (previousAnimation != AnimationState)
			{
				AnimatedSprite.PlayBackwards(AnimationState.ToString());
			}
			return;
		}
		
		switch (animationType)
			{
				case "walk":
					if (GameState.GameState.Instance.RidingBike)
					{
						if (GameState.GameState.Instance.DoingWheelie)
						{
							SetAnimationState([AnimationState.bike_acro_wheelie_ride_up, AnimationState.bike_acro_wheelie_ride_left, AnimationState.bike_acro_wheelie_ride_right, AnimationState.bike_acro_wheelie_ride_down]);
							if (!GameState.GameState.Instance.DoingWheelieBounce)
							{
								AnimationPlayer.Play("RESET");
								break;
							}
							if (AnimationPlayer.AssignedAnimation != "wheelie_bounce") AnimationPlayer.Play("wheelie_bounce");
							break;
						}

						switch (GameState.GameState.Instance.BikeState)
						{
							case GameState.BikeState.AcroBike:
								SetAnimationState([AnimationState.bike_acro_ride_up, AnimationState.bike_acro_ride_left, AnimationState.bike_acro_ride_right, AnimationState.bike_acro_ride_down]);
								break;
							case GameState.BikeState.MachBike:
								SetAnimationState([AnimationState.bike_mach_ride_up, AnimationState.bike_mach_ride_left, AnimationState.bike_mach_ride_right, AnimationState.bike_mach_ride_down]);
								break;
						}
						break;
					}
					SetAnimationState([AnimationState.walk_up, AnimationState.walk_left, AnimationState.walk_right, AnimationState.walk_down]);
					break;
				case "turn":
					SetAnimationState([AnimationState.turn_up, AnimationState.turn_left, AnimationState.turn_right, AnimationState.turn_down]);
					break;
				case "run":
					if (GameState.GameState.Instance.RidingBike)
					{
						if (GameState.GameState.Instance.DoingWheelie)
						{
							SetAnimationState([AnimationState.bike_acro_wheelie_ride_up, AnimationState.bike_acro_wheelie_ride_left, AnimationState.bike_acro_wheelie_ride_right, AnimationState.bike_acro_wheelie_ride_down]);
							if (AnimationPlayer.AssignedAnimation != "wheelie_bounce") AnimationPlayer.Play("wheelie_bounce");
							break;
						}
						if (!GameState.GameState.Instance.DoingWheelieBounce)
						{
							AnimationPlayer.Play("RESET");
							break;
						}

						switch (GameState.GameState.Instance.BikeState)
						{
							case GameState.BikeState.AcroBike:
								SetAnimationState([AnimationState.bike_acro_ride_up, AnimationState.bike_acro_ride_left, AnimationState.bike_acro_ride_right, AnimationState.bike_acro_ride_down]);
								break;
							case GameState.BikeState.MachBike:
								SetAnimationState([AnimationState.bike_mach_ride_up, AnimationState.bike_mach_ride_left, AnimationState.bike_mach_ride_right, AnimationState.bike_mach_ride_down]);
								break;
						}
						break;
					}
					SetAnimationState([AnimationState.run_up, AnimationState.run_left, AnimationState.run_right, AnimationState.run_down]);
					break;
				case "cycle":
					SetAnimationState([AnimationState.bike_idle_up, AnimationState.bike_idle_left, AnimationState.bike_idle_right, AnimationState.bike_idle_down]);
					break;
				case "idle":
					// TODO: handle idling on bike, swimming/surfing, diving

					if (GameState.GameState.Instance.RidingBike)
					{
						if (GameState.GameState.Instance.DoingWheelie)
						{
							SetAnimationState([AnimationState.bike_acro_wheelie_idle_up, AnimationState.bike_acro_wheelie_idle_left, AnimationState.bike_acro_wheelie_idle_right, AnimationState.bike_acro_wheelie_idle_down]);
							if (AnimationPlayer.AssignedAnimation != "wheelie_bounce") AnimationPlayer.Play("wheelie_bounce");
							break;
						}
						if (!GameState.GameState.Instance.DoingWheelieBounce)
						{
							AnimationPlayer.Play("RESET");
							break;
						}
						SetAnimationState([AnimationState.bike_idle_up, AnimationState.bike_idle_left, AnimationState.bike_idle_right, AnimationState.bike_idle_down]);
						break;
					}
				
					SetAnimationState([AnimationState.idle_up, AnimationState.idle_left, AnimationState.idle_right, AnimationState.idle_down]);
					break;
			}
		
		if (previousAnimation != AnimationState)
		{
			AnimatedSprite.Play(AnimationState.ToString());
		}
	}

	private void SetAnimationState(AnimationState[] possibleStates)
	{
		if (CharacterController.Direction == Vector2.Up)
		{
			AnimationState = possibleStates[0];
		}
		else if (CharacterController.Direction == Vector2.Left)
		{
			AnimationState = possibleStates[1];
		}
		else if (CharacterController.Direction == Vector2.Right)
		{
			AnimationState = possibleStates[2];
		}
		else if (CharacterController.Direction == Vector2.Down)
		{
			AnimationState = possibleStates[3];
		}
	}

	private bool DoingWheelieTransition()
	{
		return _wheelieState switch
		{
			WheelieState.FromWheelie => true,
			WheelieState.ToWheelie => true,
			_ => false,
		};
	}

	public override void AnimationFinished()
	{
		switch (_wheelieState)
		{
			case WheelieState.ToWheelie:
				_wheelieState = WheelieState.InWheelie;
				break;
			case WheelieState.FromWheelie:
				_wheelieState = WheelieState.NoWheelie;
				break;
		}
	}
}
