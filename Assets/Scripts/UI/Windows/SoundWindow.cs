using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
using BookModel;

namespace UI
{
	public class SoundWindow : EditorWindow
	{
		static WWW www;
		static AudioSource audioSource;
		void OnGUI ()
		{
			GUILayout.Label ("Sound", EditorStyles.boldLabel);

			if (GUILayout.Button ("Play")) {
				soundFileChooser ();
			}

		}

		private void soundFileChooser () {
			Debug.Log ("OpenExistingBookButtonPressed");
			var path = EditorUtility.OpenFilePanel(
				"Select a sound file",
				"",
				"");
			if (path.Length != 0) {
				Debug.Log("Open sound at path: " + path);

				var uri = new System.Uri (path);
				var absoluteUri = uri.AbsoluteUri;

//				www = new WWW("file://" + path);
//				Debug.Log("loading " + path);

				www = new WWW (absoluteUri);
				audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
				audioSource.clip = www.GetAudioClip( false );

//				
//				AudioClip clip = www.GetAudioClip (false);
//				clip.name = System.IO.Path.GetFileName(path);
//				audioSource = Camera.main.gameObject.AddComponent <AudioSource> ();
//				var songWWW = new WWW (absoluteUri);
//				audioSource.clip = songWWW.GetAudioClip (false, true);
//				audioSource.Play ();
//
//
//				// Add picture to page of the book
//				var name = Path.GetFileNameWithoutExtension(path);
//				var picture = new Picture(path, name);
//				var page = BookComponent.CurrentBook.CurrentPage;
//				if (page.Pictures == null) {
//					page.Pictures = new List<Picture>();
//				}
//				page.Pictures.Add(picture);
//				picture.AddToTheScene ();
//				
//				// Combo box
//				picturesNames.Add(picture.Name);
//				isRenamingCurrentPicture = true;
//				CurrentPictureIndex = picturesNames.Count - 1;
			}
		}

		void Update()
		{
			Debug.Log( www.progress );
			
			if( !audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded)
			{
				Debug.Log( "Ready!" );
				audioSource.Play();
			}
		}

	}
}

#endif
