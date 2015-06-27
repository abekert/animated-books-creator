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

		private Vector2 scrollPos = new Vector2 ();
		
		void OnGUI () {
			GUILayout.Label ("Animations", EditorStyles.boldLabel);

			if (CurrentPicture == null) {
				return;
			}

			scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

			var index = ABAnimationTypeHelper.IndexOfAnimation(CurrentPicture.Animation);
			var newIndex = EditorGUILayout.Popup (index, ABAnimationTypeHelper.TypeDescriptions());
			if (newIndex != index) {
				var type = (ABAnimationType)newIndex;
				Debug.Log(newIndex + " is " + ABAnimationTypeHelper.TypeDescription(type));
				CurrentPicture.Animation = new ABAnimation(type);
			}

			if (CurrentPicture.Animation != null) {
				foldIndex = 0;
				EditorGUI.indentLevel = 0;

				showPreviewAnimationButtonIfExists (CurrentPicture.Animation);
				showAnimationControls (CurrentPicture.Animation);
			}

			EditorGUILayout.EndScrollView ();
		}

		void showPreviewAnimationButtonIfExists (ABAnimation animation, string label = "Preview Animation")
		{
			if (animation == null || animation.Type == ABAnimationType.None || animation.Type == ABAnimationType.Wait) {
				return;
			}

			if (GUILayout.Button(label)) {
				ABAnimationSystem.PreviewAnimation(animation, CurrentPicture.GameObject);
			}
		}

		void showChooseAnimationControls(ABAnimation animation)
		{
			EditorGUILayout.BeginHorizontal ();

			var index = ABAnimationTypeHelper.IndexOfAnimation(animation);
			var newIndex = EditorGUILayout.Popup (index, ABAnimationTypeHelper.TypeDescriptions());
			if (newIndex != index) {
				var type = (ABAnimationType)newIndex;
				Debug.Log(newIndex + " is " + ABAnimationTypeHelper.TypeDescription(type));
				animation.Type = type;
			}

			showPreviewAnimationButtonIfExists (animation, "Preview");

			EditorGUILayout.EndHorizontal ();
		}

		void showAnimationControls (ABAnimation animation)
		{
			switch (animation.Type) {
			case ABAnimationType.MoveBy:
//				GUILayout.Label ("Move By", EditorStyles.boldLabel);
				showVector3DurationControls (animation, "Delta vector");
				break;
			case ABAnimationType.MoveTo:
//				GUILayout.Label ("Move To", EditorStyles.boldLabel);
				showVector3DurationControls (animation, "Final position", false);
				break;
			case ABAnimationType.RotateBy:
//				GUILayout.Label ("Rotate By", EditorStyles.boldLabel);
				showVector3DurationControls (animation, "Delta angle");
				break;
			case ABAnimationType.RotateTo:
//				GUILayout.Label ("Rotate To", EditorStyles.boldLabel);
				showVector3DurationControls (animation, "Final angle", false);
				break;
			case ABAnimationType.ScaleBy:
//				GUILayout.Label ("Scale By", EditorStyles.boldLabel);
				showVector2DurationControls (animation, "Delta values");
				break;
			case ABAnimationType.ScaleTo:
//				GUILayout.Label ("Scale To", EditorStyles.boldLabel);
				showVector2DurationControls (animation, "Final values", false);
				break;
			case ABAnimationType.Group:
				showGroupControls (animation, "Animation Group");
				break;
			case ABAnimationType.Sequence:
				showGroupControls (animation, "Animation Sequence");
				break;
			case ABAnimationType.FadeAlphaBy:
				showValueDurationControls (animation, "Delta Alpha", -1, 1);
				break;
			case ABAnimationType.FadeAlphaTo:
				showValueDurationControls (animation, "Final Alpha", 0, 1, false);
				break;
			case ABAnimationType.FadeIn:
			case ABAnimationType.FadeOut:
				showDurationTimingControls (animation);
				break;
			case ABAnimationType.Wait:
				animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
				break;
			default:
				break;
			}
		}

		void showVector3DurationControls (ABAnimation animation, string vectorLabel = "Vector", bool showRepeats = true)
		{
			animation.Vector = EditorGUILayout.Vector3Field (vectorLabel, animation.Vector);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.Timing = timingModeChooser (animation.Timing);
			if (showRepeats) {
				animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
			}
		}

		void showVector2DurationControls (ABAnimation animation, string vectorLabel = "Vector", bool showRepeats = true)
		{
			animation.Vector = EditorGUILayout.Vector2Field (vectorLabel, animation.Vector);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.Timing = timingModeChooser (animation.Timing);
			if (showRepeats) {
				animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
			}
		}


		private int repeatsCountChooser (int currentRepeatsCount)
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

		private ABAnimationTiming timingModeChooser (ABAnimationTiming currentTiming)
		{
			int index = (int)currentTiming;
			index = EditorGUILayout.Popup ("Timing Mode", index, ABAnimationTimingHelper.TimingDescriptions ());
			return (ABAnimationTiming)index;
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
				foldsEnablers.Add (true);
			}

			EditorGUILayout.BeginHorizontal ();

			foldsEnablers [foldIndex] = EditorGUILayout.Foldout (foldsEnablers[foldIndex], foldLabel);

			// Show Add & Remove buttons
			var count = animation.Animations.Count;
			if (foldsEnablers [foldIndex]) {
				if (GUILayout.Button ("Add")) {
					animation.Animations.Add (new ABAnimation (ABAnimationType.None));
					count++;
				}
				GUI.enabled = count > 1;
				if (GUILayout.Button ("Remove")) {
					animation.Animations.RemoveAt (count - 1);
					count--;
				}
				GUI.enabled = true;
			}

			EditorGUILayout.EndHorizontal ();

			// Show Animation controls
			if (foldsEnablers [foldIndex]) {
				foldIndex++;
				EditorGUI.indentLevel++;

				animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);

				for (int index = 0; index < count; ++index) {
					EditorGUILayout.LabelField ("Animation " + (index + 1));
					var item = animation.Animations [index];
					showChooseAnimationControls (item);
					showAnimationControls (item);
					EditorGUILayout.Separator ();
				}

				EditorGUI.indentLevel--;
				EditorGUILayout.Separator ();
			} else {
				foldIndex += 1 + foldableAnimationsCount (animation);
			}
		}

		private int foldableAnimationsCount (ABAnimation animation)
		{
			int count = 0;
			foreach (var item in animation.Animations) {
				if (item.Type == ABAnimationType.Group ||
				    item.Type == ABAnimationType.Sequence) {
					count += 1 + foldableAnimationsCount (item);
				}
			}
			return count;
		}

		private void showValueDurationControls (ABAnimation animation, string vectorLabel = "Value", float low = 0, float high = 1, bool showRepeats = true)
		{
			animation.Value = EditorGUILayout.Slider (vectorLabel, animation.Value, low, high);
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.Timing = timingModeChooser (animation.Timing);
			if (showRepeats) {
				animation.RepeatsCount = repeatsCountChooser (animation.RepeatsCount);
			}
		}

		private void showDurationTimingControls (ABAnimation animation) 
		{
			animation.Duration = EditorGUILayout.FloatField ("Duration", animation.Duration);
			animation.Timing = timingModeChooser (animation.Timing);
		}

	}
}

#endif

