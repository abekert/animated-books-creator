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

		public void ToggleParallax(bool newValue)
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
			Debug.Log ("Animations button pressed");
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

	}
}

#endif