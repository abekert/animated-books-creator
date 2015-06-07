using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using BookModel;

namespace UI {

	public class TopPanelDelegate : MonoBehaviour
	{
		static public bool ParallaxEnabled = true;
		static public bool AnimationEnabled = false;

		public GameObject StartStopAnimationButton;
//		private string StartStopAnimationLabelText {
//			get {
////				var component = StartStopAnimationLabel.GetComponent<Text> ();
////				return component.Content;
//			}
//			set {
////				var component = StartStopAnimationLabel.GetComponent<Text> ();
////				component.Content = value;
//			}
//		}

		public void ToggleParallax(bool newValue)
		{
			ParallaxEnabled = newValue;
		}

		public void AimationButtonPressed()
		{
			AnimationEnabled = !AnimationEnabled;
			if (AnimationEnabled == true) {
				startPlayingAnimations ();

				var button = StartStopAnimationButton.GetComponent<UnityEngine.UI.Button> ();
				var label = button.GetComponentInChildren<UnityEngine.UI.Text> ();
				label.text = "Stop Animation";
			} else {
				stopPlayingAnimations ();

				var button = StartStopAnimationButton.GetComponent<UnityEngine.UI.Button> ();
				var label = button.GetComponentInChildren<UnityEngine.UI.Text> ();
				label.text = "Start Animation";
			}
		}

		public void ShowBookWindow ()
		{
			showWindow<BookWindow> ();
		}

		public void ShowPagesWindow()
		{
			showWindow<PagesWindow> ();
		}

		public void ShowTextWindow()
		{
			showWindow<TextWindow> ();
		}

		public void ShowPicturesWindow()
		{
			showWindow<PicturesWindow> ();
		}

		public void ShowAnimationsWindow ()
		{
			showWindow<AnimationsWindow> ();
		}

		int multiplier = 1;
		float positionX = 0;
		float xDelta = 2;
		float angle = 0;
		float angleDelta = 30;

		private void PlaySampleAnimation ()
		{
			var pictures = BookComponent.CurrentBook.CurrentPage.Pictures;
			LeanTween.rotate (pictures[0].pictureObject, new Vector3 (0, 0, -3600f), 100f);
			LeanTween.rotate (pictures[1].pictureObject, new Vector3 (0, 0, -3600f), 100f);
			
			positionX = pictures [2].pictureObject.transform.position.x;
			angle = pictures [2].pictureObject.transform.eulerAngles.z;
			movingComplete ();
		}

		private void movingComplete ()
		{
			var pictures = BookComponent.CurrentBook.CurrentPage.Pictures;
			var pictureObject = pictures [2].pictureObject;
			float time = 3;
			multiplier *= -1;

			float newX = positionX + (xDelta * multiplier);
			var d = LeanTween.moveLocalX (pictureObject, newX, time);
			d.setEase (LeanTweenType.easeInOutSine);

			float newAngle = angle + (angleDelta * multiplier);
			d = LeanTween.rotateZ (pictureObject, newAngle, time);
			d.setOnComplete (movingComplete).setEase (LeanTweenType.easeInOutSine);
		}

		private void showWindow<T>() where T : EditorWindow
		{
			#if UNITY_EDITOR
			var window = ScriptableObject.CreateInstance<T>();
			window.ShowAuxWindow();
			#else
			Debug.Log ("You can't use this function outside the Unity Editor");
			#endif
		}

		private void startPlayingAnimations ()
		{
			var pictures = BookComponent.CurrentBook.CurrentPage.Pictures;
			foreach (var picture in pictures) {
				Helpers.ABAnimationSystem.RunAnimation(picture.Animation, picture.pictureObject, null);
			}
		}

		private void stopPlayingAnimations ()
		{
			Helpers.ABAnimationSystem.CancelAllAnimations ();
		}

	}
}

#endif