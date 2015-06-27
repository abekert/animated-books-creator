using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BookModel;

namespace Helpers
{

	public class PageTransitions
	{
		public enum TransitionDirection {
			Backward,
			Forward
		}

		private const float animationDuration = 1;

		public static string FontName;

		public static void PushOut (GameObject obj, float duration, TransitionDirection direction, Action completion = null)
		{
			var sign = direction == TransitionDirection.Backward ? -1 : 1;

			var screenLocation = Camera.main.WorldToScreenPoint(obj.transform.position);
			var x = Screen.width / 2 + (sign * Screen.width * 1.25f);
//			Debug.Log ("X = " + x);
			var destinationScreen = new Vector3 (x, screenLocation.y * 0.8f, screenLocation.z * 0.8f);
			var destinationWorld = Camera.main.ScreenToWorldPoint (destinationScreen);

			var moveAnimation = ABAnimation.MoveTo (destinationWorld, duration);
			moveAnimation.Timing = ABAnimationTiming.EaseIn;

			var deltaAngle = sign * 90;
			var rotateAnimation = ABAnimation.RotateBy (new	Vector3 (0, deltaAngle, 0), duration);
			rotateAnimation.Timing = ABAnimationTiming.EaseInEaseOut;

			var list = new List<ABAnimation> (2);
			list.Add (moveAnimation);
			list.Add (rotateAnimation);
			var group = ABAnimation.Group (list);

			ABAnimationSystem.RunAnimation (group, obj, completion);
		}

		public static void PushIn (GameObject obj, float duration, TransitionDirection direction, Action completion = null)
		{
			var sign = direction == TransitionDirection.Backward ? 1 : -1;

			var screenLocation = Camera.main.WorldToScreenPoint (obj.transform.position);
			var x = Screen.width / 2 + (sign * Screen.width);
			Debug.Log ("X = " + x);
			var sourceScreen = new Vector3 (x, screenLocation.y * 0.8f, screenLocation.z * 0.8f);
			var sourceWorld = Camera.main.ScreenToWorldPoint (sourceScreen);
			var destinationWorld = obj.transform.position;
			obj.transform.position = sourceWorld;
			
			var moveAnimation = ABAnimation.MoveTo (destinationWorld, duration);
			moveAnimation.Timing = ABAnimationTiming.EaseOut;

			var destinationAngles = obj.transform.eulerAngles;
			var rotateAnimation = ABAnimation.RotateTo (destinationAngles, duration);
			rotateAnimation.Timing = ABAnimationTiming.EaseInEaseOut;
			obj.transform.eulerAngles = new Vector3 (destinationAngles.x, destinationAngles.y + (sign * 90), destinationAngles.z);

			
			var list = new List<ABAnimation> (2);
			list.Add (moveAnimation);
			list.Add (rotateAnimation);
			var group = ABAnimation.Group (list);
			
			ABAnimationSystem.RunAnimation (group, obj, completion);
		}


		public static void ShowNextPage (BookPage currentPage, BookPage nextPage, bool isPlayerMode = false, Action completion = null)
		{
			switchPages (currentPage, nextPage, TransitionDirection.Backward, isPlayerMode, completion);
		}

		public static void ShowPreviousPage (BookPage currentPage, BookPage previousPage, bool isPlayerMode = false, Action completion = null)
		{
			switchPages (currentPage, previousPage, TransitionDirection.Forward, isPlayerMode, completion);
		}

		private static void switchPages (BookPage currentPage, BookPage newPage, TransitionDirection direction, bool isPlayerMode = false, Action completion = null)
		{
			ABAnimationSystem.CancelAllAnimations ();

			if (newPage.Pictures != null) {
				foreach (var picture in newPage.Pictures) {
					picture.AddToScene (!isPlayerMode);
				}
			}

			if (currentPage.Pictures != null) {
				foreach (var picture in currentPage.Pictures) {
					var obj = picture.GameObject;
					PushOut (obj, animationDuration, direction, () => {
						GameObject.Destroy (obj);
					});
				}
			}

			PushOut (currentPage.Text.GameObject, animationDuration, direction, () => {
				GameObject.Destroy (currentPage.Text.GameObject);
			});

			if (newPage.Pictures != null) {
				foreach (var picture in newPage.Pictures) {
					var obj = picture.GameObject;
					PushIn (obj, animationDuration, direction);
				}
			}

			newPage.Text.AddToTheScene (!isPlayerMode, FontName);
			PushIn (newPage.Text.GameObject, animationDuration, direction, completion);

			ABAnimationSystem.ChangeCameraColor (newPage.Color.ToColor (), animationDuration);
		}

	}
}

