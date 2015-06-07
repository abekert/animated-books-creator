using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BookModel;
using Helpers;

#if UNITY_EDITOR
using UnityEditor;

namespace UI
{
	public class AnimationsWindow : EditorWindow
	{
		private Book book {
			get {
				return BookComponent.CurrentBook;
			}
			set {
				BookComponent.CurrentBook = value;
			}
		}

		static private Picture CurrentPicture {
			get {
				return PicturesWindow.CurrentPicture;
			}
		}

		
		void OnGUI () {
			GUILayout.Label ("Animations", EditorStyles.boldLabel);

			if (CurrentPicture == null) {
				return;
			}

			var index = ABAnimationTypeHelper.indexOfAnimation(CurrentPicture.Animation);
			var newIndex = EditorGUILayout.Popup (index, ABAnimationTypeHelper.typeDescriptions());
			if (newIndex != index) {
				var type = (ABAnimationType)newIndex;
				Debug.Log(newIndex + " is " + ABAnimationTypeHelper.typeDescription(type));
				CurrentPicture.Animation = new ABAnimation(type);
			}

			if (CurrentPicture.Animation != null) {
				foldIndex = 0;
				EditorGUI.indentLevel = 0;
				showAnimationControls (CurrentPicture.Animation);
			}
		}

		void showChooseAnimationControls(ABAnimation animation)
		{
			var index = ABAnimationTypeHelper.indexOfAnimation(animation);
			var newIndex = EditorGUILayout.Popup (index, ABAnimationTypeHelper.typeDescriptions());
			if (newIndex != index) {
				var type = (ABAnimationType)newIndex;
				Debug.Log(newIndex + " is " + ABAnimationTypeHelper.typeDescription(type));
				animation.Type = type;
			}
		}

		void showAnimationControls (ABAnimation animation)
		{
			if (GUILayout.Button("Preview animation")) {
				ABAnimationSystem.PreviewAnimation(animation, CurrentPicture.pictureObject);
			}

			switch (animation.Type) {
			case ABAnimationType.MoveBy:
//				GUILayout.Label ("Move By", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Delta vector");
				break;
			case ABAnimationType.MoveTo:
//				GUILayout.Label ("Move To", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Final position");
				break;
			case ABAnimationType.RotateBy:
//				GUILayout.Label ("Rotate By", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Delta angle");
				break;
			case ABAnimationType.RotateTo:
//				GUILayout.Label ("Rotate To", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Final angle");
				break;
			case ABAnimationType.ScaleBy:
//				GUILayout.Label ("Scale By", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Delta values");
				break;
			case ABAnimationType.ScaleTo:
//				GUILayout.Label ("Scale To", EditorStyles.boldLabel);
				showVectorDurationControls (animation, "Final values");
				break;
			case ABAnimationType.Group:
				showGroupControls (animation, "Animation Group");
				break;
			case ABAnimationType.Sequence:
				showGroupControls (animation, "Animation Sequence");
				break;
			default:
				break;
			}
		}

		void showVectorDurationControls (ABAnimation animation, string vectorLabel = "Vector", string durationLabel = "Duration")
		{
			animation.Vector = EditorGUILayout.Vector3Field ("Position", animation.Vector);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
		}

		void showRotateControls (ABAnimation animation)
		{
			animation.Vector = EditorGUILayout.Vector3Field ("Angles", animation.Vector);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
		}

		void showScaleControls (ABAnimation animation)
		{
			animation.Vector = EditorGUILayout.Vector3Field ("Scale", animation.Vector);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
		}

		private int repeatsCountChooser(int currentRepeatsCount)
		{
			string[] repeatDescription = {"Once", "Forever", "Count"};
			int index = 0;
			if (currentRepeatsCount == ABAnimation.RepeatForever) {
				index = 1;
			} else if (currentRepeatsCount > 1) {
				index = 2;
			}

			index = EditorGUILayout.Popup ("Repeat", index, repeatDescription);
			if (index == 0) {
				return 1;
			} else if (index == 1) {
				return ABAnimation.RepeatForever;
			} else {
				currentRepeatsCount = Mathf.Max (2, currentRepeatsCount);
				currentRepeatsCount = EditorGUILayout.IntField ("Repeats Count", currentRepeatsCount);
				return currentRepeatsCount;
			}
		}

		private List<bool> foldsEnablers = new List<bool> ();
		private int foldIndex = 0;
		void showGroupControls (ABAnimation animation, string foldLabel = "Animations")
		{
			if (animation.Animations == null) {
				animation.Animations = new List<ABAnimation> ();
				animation.Animations.Add (new ABAnimation(ABAnimationType.None));
			}

			if (foldIndex == foldsEnablers.Count) {
				foldsEnablers.Add(true);
			}

			foldsEnablers[foldIndex] = EditorGUILayout.Foldout (foldsEnablers[foldIndex], foldLabel);
			if (foldsEnablers[foldIndex]) {
				EditorGUI.indentLevel++;

				var count = animation.Animations.Count;
				EditorGUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Add")) {
					animation.Animations.Add (new ABAnimation(ABAnimationType.None));
					count++;
				}
				GUI.enabled = count > 1;
				if (GUILayout.Button ("Remove")) {
					animation.Animations.RemoveAt(count - 1);
					count--;
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal ();

				animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);

				foldIndex++;
				for (int index = 0; index < count; ++index) {
					EditorGUILayout.LabelField ("Animation " + (index + 1));
					var item = animation.Animations[index];
					showChooseAnimationControls (item);
					showAnimationControls (item);
				}

				EditorGUI.indentLevel--;
			}
		}

		
	}
}

#endif

