using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace BookModel
{
	public class ABAnimationSystem
	{
		public static void RunAnimation (ABAnimation animation, GameObject obj, Action completion = null)
		{
			if (animation == null || obj == null) {
				return;
			}

			switch (animation.Type) {
			case ABAnimationType.RotateBy:
				RunRotateByAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.RotateTo:
				RunRotateToAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.MoveBy:
//				var pos = new ABPosition (animation.Vector);
//				var vector = pos.ToVector ();
				RunMoveByAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.MoveTo:
//				pos = new ABPosition (animation.Vector);
//				vector = pos.ToVector ();
				RunMoveToAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.Sequence:
				RunAnimationSequence (obj, animation.Animations, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.Group:
				RunAnimationGroup (obj, animation.Animations, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.ScaleBy:
				RunScaleByAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.ScaleTo:
				RunScaleToAnimation (obj, animation.Vector, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.FadeAlphaBy:
				RunFadeAlphaByAnimation (obj, animation.Value, animation.Duration, animation.Timing, completion, animation.RepeatsCount);
				break;
			case ABAnimationType.FadeAlphaTo:
				RunFadeAlphaToAnimation (obj, animation.Value, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.FadeIn:
				RunFadeInAnimation (obj, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.FadeOut:
				RunFadeOutAnimation (obj, animation.Duration, animation.Timing, completion);
				break;
			case ABAnimationType.Wait:
				RunWaitForDurationAnimation (animation.Duration, completion);
				break;
			default:
				break;
			}
		}

		public static void PreviewAnimation (ABAnimation animation, GameObject obj)
		{
			var position = obj.transform.position;
			var rotation = obj.transform.rotation;
			var scale = obj.transform.localScale;
			var renderer = obj.GetComponent<SpriteRenderer> ();
			var color = renderer.color;
			
			RunAnimation (animation, obj, () => {
				obj.transform.position = position;
				obj.transform.rotation = rotation;
				obj.transform.localScale = scale;
				renderer.color = color;
			});
		}

		public static void CancelAllAnimations ()
		{
			LeanTween.cancelAll (false);
		}

		public static void ChangeCameraColor (Color toColor, float duration)
		{
			Color initialColor = Camera.main.backgroundColor;
			var d = LeanTween.value (Camera.main.gameObject, initialColor, toColor, duration);
			d.setOnUpdate ((Color newColor) => { 
				Camera.main.backgroundColor = newColor;
			});
		}

		private static LeanTweenType leanTweenEaseType (ABAnimationTiming timing)
		{
			switch (timing) {
			case ABAnimationTiming.EaseIn:
				return LeanTweenType.easeInSine;
			case ABAnimationTiming.EaseOut:
				return LeanTweenType.easeOutSine;
			case ABAnimationTiming.EaseInEaseOut:
				return LeanTweenType.easeInOutSine;
			default:
				return LeanTweenType.linear;
			}
		}

		private static void RunRotateByAnimation (GameObject obj, Vector3 delta, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null, int loopsCount = 1)
		{
			var d = LeanTween.rotateAround (obj, delta.normalized, delta.magnitude, duration);
			d.setEase (leanTweenEaseType (timing));

			if (loopsCount > 1) {
				d.setOnComplete (() => RunRotateByAnimation (obj, delta, duration, timing, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				d.setOnComplete (() => RunRotateByAnimation (obj, delta, duration, timing, onComplete, loopsCount));
			} else if (loopsCount == 1) {
				d.setOnComplete (onComplete);
			}
		}
	
		private static void RunRotateToAnimation (GameObject obj, Vector3 newAngles, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			var d = LeanTween.rotate (obj, newAngles, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunMoveToAnimation (GameObject obj, Vector3 newPosition, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			var d = LeanTween.move (obj, newPosition, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunMoveByAnimation (GameObject obj, Vector3 delta, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null, int loopsCount = 1)
		{
			var newPosition = obj.transform.position + delta;

			if (loopsCount > 1) {
				RunMoveToAnimation (obj, newPosition, duration, timing, () => RunMoveByAnimation(obj, delta, duration, timing, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				RunMoveToAnimation (obj, newPosition, duration, timing, () => RunMoveByAnimation(obj, delta, duration, timing, onComplete, loopsCount));
            } else if (loopsCount == 1) {
				RunMoveToAnimation (obj, newPosition, duration, timing, onComplete);
			}
		}

		private static void RunScaleToAnimation (GameObject obj, Vector3 newScale, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			var d = LeanTween.scale (obj, newScale, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunScaleByAnimation (GameObject obj, Vector3 delta, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null, int loopsCount = 1)
		{
			var newScale = obj.transform.localScale + delta;

			if (loopsCount > 1) {
				RunScaleToAnimation (obj, newScale, duration, timing, () => RunScaleByAnimation(obj, delta, duration, timing, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				RunScaleToAnimation (obj, newScale, duration, timing, () => RunScaleByAnimation(obj, delta, duration, timing, onComplete, loopsCount));
			} else if (loopsCount == 1) {
				RunScaleToAnimation (obj, newScale, duration, timing, onComplete);
			}
		}

		private static void RunAnimationGroup (GameObject obj, List<ABAnimation> animations, Action onComplete = null, int loopsCount = 1)
		{
			float maxDuration = 0;
			int maxDurationIndex = 0;

			for (int i = 0; i < animations.Count; i++) {
				var animation = animations [i];
				if (animation.Duration >= maxDuration) {
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
			if (loopsCount == 1) {
				_RunAnimationSequenceOnce (obj, animations, onComplete);
			} else if (loopsCount == ABAnimation.RepeatForever) {
				_RunAnimationSequenceOnce (obj, animations, () => RunAnimationSequence (obj, animations, onComplete, loopsCount));
			} else if (loopsCount > 1) {
				_RunAnimationSequenceOnce (obj, animations, () => RunAnimationSequence (obj, animations, onComplete, loopsCount - 1));
			}
		}

		private static void _RunAnimationSequenceOnce (GameObject obj, List<ABAnimation> animations, Action onComplete)
		{
			var currentAnimation = animations[0];

			if (animations.Count == 1) {
				RunAnimation (currentAnimation, obj, onComplete);
			} else {
				var otherAnimations = new List<ABAnimation> (animations);
				otherAnimations.RemoveAt (0);
				RunAnimation (currentAnimation, obj, () => {
					_RunAnimationSequenceOnce (obj, otherAnimations, onComplete);
				});
			}
		}

		private static void RunFadeAlphaByAnimation (GameObject obj, float delta, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null, int loopsCount = 1)
		{
			var renderer = obj.GetComponent<Renderer> ();
			if (renderer == null) {
				Debug.LogError ("Game Object has no renderer to fade");
				return;
			}

			var currentAlpha = renderer.material.color.a;
			var newAlpha = Mathf.Max (Mathf.Min (currentAlpha + delta, 1), 0);

			var d = LeanTween.alpha (obj, newAlpha, duration);
			d.setEase (leanTweenEaseType (timing));
			
			if (loopsCount > 1) {
				d.setOnComplete (() => RunFadeAlphaByAnimation (obj, delta, duration, timing, onComplete, loopsCount - 1));
			} else if (loopsCount == ABAnimation.RepeatForever) {
				d.setOnComplete (() => RunFadeAlphaByAnimation (obj, delta, duration, timing, onComplete, loopsCount));
			} else if (loopsCount == 1) {
				d.setOnComplete (onComplete);
			}
		}

		private static void RunFadeAlphaToAnimation (GameObject obj, float newAlpha, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			newAlpha = Mathf.Max (Mathf.Min (newAlpha, 1), 0);

			var d = LeanTween.alpha (obj, newAlpha, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunFadeInAnimation (GameObject obj, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			var d = LeanTween.alpha (obj, 1, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunFadeOutAnimation (GameObject obj, float duration, ABAnimationTiming timing = ABAnimationTiming.Linear, Action onComplete = null)
		{
			var d = LeanTween.alpha (obj, 0, duration);
			d.setEase (leanTweenEaseType (timing));
			d.setOnComplete (onComplete);
		}

		private static void RunWaitForDurationAnimation (float duration, Action onComplete = null)
		{
			LeanTween.delayedCall (duration, onComplete);
		}
	}
}
