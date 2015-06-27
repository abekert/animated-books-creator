using System;
using System.Collections.Generic;
using UnityEngine;

namespace BookModel
{
	public partial class ABAnimation
	{
		public static ABAnimation Group(List <ABAnimation> animations)
		{
			var animation = new ABAnimation(ABAnimationType.Group);
			animation.Animations = animations;
			return animation;
		}
		
		public static ABAnimation Sequence(List <ABAnimation> animations)
		{
			var animation = new ABAnimation(ABAnimationType.Sequence);
			animation.Animations = animations;
			return animation;
		}
		
		public static ABAnimation MoveTo(Vector3 position, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.MoveTo);
			animation.Vector = position;
			animation.Duration = duration;
			return animation;
		}
		
		public static ABAnimation MoveBy(Vector3 deltaPosition, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.MoveBy);
			animation.Vector = deltaPosition;
			animation.Duration = duration;
			return animation;
		}
		
		public static ABAnimation RotateTo(Vector3 rotation, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.RotateTo);
			animation.Vector = rotation;
			animation.Duration = duration;
			return animation;
		}
		
		public static ABAnimation RotateBy(Vector3 deltaRotation, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.RotateBy);
			animation.Vector = deltaRotation;
			animation.Duration = duration;
			return animation;
		}
		
		public static ABAnimation ScaleTo(Vector3 scale, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.ScaleTo);
			animation.Vector = scale;
			animation.Duration = duration;
			return animation;
		}
		
		public static ABAnimation ScaleBy(Vector3 deltaScale, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.ScaleBy);
			animation.Vector = deltaScale;
			animation.Duration = duration;
			return animation;
		}

		public static ABAnimation WaitForDuration (float duration)
		{
			var animation = new ABAnimation(ABAnimationType.Wait);
			animation.Duration = duration;
			return animation;
		}

		public static ABAnimation FadeAlphaTo (float alpha, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.FadeAlphaTo);
			animation.Value = alpha;
			animation.Duration = duration;
			return animation;
		}

		public static ABAnimation FadeAlphaBy (float deltaAlpha, float duration)
		{
			var animation = new ABAnimation(ABAnimationType.FadeAlphaBy);
			animation.Value = deltaAlpha;
			animation.Duration = duration;
			return animation;
		}

		public static ABAnimation FadeIn (float duration)
		{
			var animation = new ABAnimation(ABAnimationType.FadeIn);
			animation.Duration = duration;
			return animation;
		}

		public static ABAnimation FadeOut (float duration)
		{
			var animation = new ABAnimation(ABAnimationType.FadeOut);
			animation.Duration = duration;
			return animation;
		}

		public ABAnimation (ABAnimationType type)
		{
			this.Type = type;
		}

		public ABAnimation(ABAnimation animation)
		{
			Type = animation.Type;
			Animations = new List<ABAnimation> (animation.Animations);
			Vector = animation.Vector;
			Duration = animation.duration;
			RepeatsCount = animation.RepeatsCount;
			Value = animation.Value;
		}

		public ABAnimation()
		{
			
		}
	}
}