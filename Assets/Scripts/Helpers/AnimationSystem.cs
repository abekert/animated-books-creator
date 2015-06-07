using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Helpers
{
	public class ABAnimationSystem
	{
		public static void RunAnimation (ABAnimation animation, GameObject obj, Action completion = null)
		{
			switch (animation.Type) {
			case ABAnimationType.RotateBy:
				RunRotateByAnimation (obj, animation.Vector, animation.Duration, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.RotateTo:
				RunRotateToAnimation (obj, animation.Vector, animation.Duration, completion);
				break;
			case ABAnimationType.MoveBy:
				var pos = new ABPosition (animation.Vector);
				var vector = pos.ToVector ();
				RunMoveByAnimation (obj, vector, animation.Duration, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.MoveTo:
				pos = new ABPosition (animation.Vector);
				vector = pos.ToVector ();
				RunMoveToAnimation (obj, vector, animation.Duration, completion);
				break;
			case ABAnimationType.Sequence:
				RunAnimationSequence (obj, animation.Animations, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.Group:
				RunAnimationGroup (obj, animation.Animations, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.ScaleBy:
				RunScaleByAnimation (obj, animation.Vector, animation.Duration, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.ScaleTo:
				RunScaleToAnimation (obj, animation.Vector, animation.Duration, completion);
				break;

			default:
				break;
			}
		}

		public static void CancelAllAnimations ()
		{
			LeanTween.cancelAll (false);
		}

		public static void PreviewAnimation (ABAnimation animation, GameObject obj)
		{
			RunAnimation (animation, obj, () => Debug.Log ("Say hey!"));
		}

		private static void RunRotateByAnimation (GameObject obj, Vector3 delta, float duration, Action onComplete = null, int loopsCount = 1)
		{
			var d = LeanTween.rotateAround (obj, delta.normalized, delta.magnitude, duration);

			if (loopsCount > 1) {
				d.setOnComplete (() => RunRotateByAnimation (obj, delta, duration, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				d.setOnComplete (() => RunRotateByAnimation (obj, delta, duration, onComplete, loopsCount));
			} else if (loopsCount == 1) {
				d.setOnComplete (onComplete);
			}
		}
	
		private static void RunRotateToAnimation (GameObject obj, Vector3 newAngles, float duration, Action onComplete = null)
		{
			var d = LeanTween.rotate (obj, newAngles, duration);
		}

		private static void RunMoveToAnimation (GameObject obj, Vector3 newPosition, float duration, Action onComplete = null)
		{
			LeanTween.moveLocal (obj, newPosition, duration).setOnComplete (onComplete);
		}

		private static void RunMoveByAnimation (GameObject obj, Vector3 delta, float duration, Action onComplete = null, int loopsCount = 1)
		{
			var newPosition = obj.transform.position + delta;

			if (loopsCount > 1) {
				RunMoveToAnimation (obj, newPosition, duration, () => RunMoveByAnimation(obj, delta, duration, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				RunMoveToAnimation (obj, newPosition, duration, () => RunMoveByAnimation(obj, delta, duration, onComplete, loopsCount));
            } else if (loopsCount == 1) {
				RunMoveToAnimation (obj, newPosition, duration, onComplete);
			}
		}

		private static void RunScaleToAnimation (GameObject obj, Vector3 newScale, float duration, Action onComplete = null, int loopsCount = 1)
		{
			LeanTween.scale (obj, newScale, duration).setOnComplete (onComplete);
		}

		private static void RunScaleByAnimation (GameObject obj, Vector3 delta, float duration, Action onComplete = null, int loopsCount = 1)
		{
			var newScale = obj.transform.localScale + delta;
			RunScaleToAnimation (obj, newScale, duration, onComplete);
		}

		private static void RunAnimationGroup (GameObject obj, List<ABAnimation> animations, Action onComplete = null, int loopsCount = 1)
		{
			float maxDuration = 0;
			int maxDurationIndex = 0;

			for (int i = 0; i < animations.Count; i++) {
				var animation = animations [i];
				if (animation.Duration > maxDuration) {
					maxDuration = animation.Duration;
					maxDurationIndex = i;
				}
			}

			for (int i = 0; i < animations.Count; i++) {
				var animation = animations [i];
				if (i == maxDurationIndex) {
					if (loopsCount > 1) {
						RunAnimation (animation, obj, () => RunAnimationGroup (obj, animations, onComplete, loopsCount - 1));
					} else if (loopsCount == ABAnimation.RepeatForever) {
						RunAnimation (animation, obj, () => RunAnimationGroup (obj, animations, onComplete, loopsCount));
					} else if (loopsCount == 1) {
						RunAnimation (animation, obj, onComplete);
					}
				} else {
					RunAnimation (animation, obj, null);
				}
			}
		}

		private static void RunAnimationSequence (GameObject obj, List<ABAnimation> animations, Action onComplete = null, int loopsCount = 1)
		{
			if (animations.Count == 1) {
				RunAnimation (animations [0], obj);
			} else {
				RunAnimation (animations [0], obj, () => {
					var a = new List<ABAnimation> (animations);
					a.RemoveAt (0);
					RunAnimationSequence (obj, a);
				});
			}
		}

		private static void _RunAnimationSequence (GameObject obj, List<ABAnimation> animations, Action onComplete)
		{
			if (animations.Count == 1) {
				RunAnimation (animations [0], obj);
			} else {
				RunAnimation (animations [0], obj, () => {
					var a = new List<ABAnimation> (animations);
					a.RemoveAt (0);
					RunAnimationSequence (obj, a);
				});
			}
		}


	}
}
