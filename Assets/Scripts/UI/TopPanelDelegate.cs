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

		public void ToggleAnimation (bool newValue)
		{
			AnimationEnabled = newValue;

			if (AnimationEnabled == true) {
				startPlayingAnimations ();
			} else {
				stopPlayingAnimations ();
			}
		}

		public void ToggleParallax (bool newValue)
		{
			ParallaxEnabled = newValue;
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

		public void ShowSoundWindow ()
		{
			showWindow<SoundWindow> ();
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
				ABAnimationSystem.RunAnimation(picture.Animation, picture.GameObject, null);
			}
		}

		private void stopPlayingAnimations ()
		{
			ABAnimationSystem.CancelAllAnimations ();
			var pictures = BookComponent.CurrentBook.CurrentPage.Pictures;
			foreach (var picture in pictures) {
				picture.UpdateGameObject ();
			}
		}

		public void GoToNextPage ()
		{
			BookComponent.GoToNextPage (true);
		}

		public void GoToPreviousPage ()
		{
			BookComponent.GoToPreviousPage (true);
		}
	}
}

#endif