using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Helpers
{
	public enum ABAnimationType {
		None = 0,
		Group,
		Sequence,
		MoveTo,
		MoveBy,
		RotateTo,
		RotateBy,
		ScaleTo,
		ScaleBy
	}

	public class ABAnimationTypeHelper {
		public static string typeDescription(ABAnimationType type) {
			return Enum.GetName (typeof(ABAnimationType), type);
		}

		public static ABAnimationType typeByDescription(string description) {
			description = description.ToLower ();
			string[] types = Enum.GetNames (typeof(ABAnimationType));
			for (int index = 0; index < types.Length; ++index) {
				if (description == types[index].ToLower ()) {
					return (ABAnimationType)index;
				}
			}

			Debug.Log ("No such animation type");
			return ABAnimationType.None;
		}

		public static string[] typeDescriptions() {
			return Enum.GetNames (typeof(ABAnimationType));
		}

		public static int indexOfAnimation(ABAnimation animation) {
			if (animation == null) {
				return (int)ABAnimationType.None;
			}
			return indexOfAnimationType (animation.Type);
		}

		public static int indexOfAnimationType(ABAnimationType type) {
			return (int)type;
		}
	}


	
	public partial class ABAnimation
	{
		public const int RepeatForever = -1;

		#region Type
		[XmlIgnore]
		public ABAnimationType Type;
		[XmlAttribute("type")]
		public string TypeString {
			get {
				return ABAnimationTypeHelper.typeDescription(Type);
			}
			set {
				Type = ABAnimationTypeHelper.typeByDescription(value);
			}
		}
		#endregion

		#region Animations
		[XmlElement("Animation")]
		public List<ABAnimation> Animations = new List<ABAnimation> ();
		#endregion

		#region X, Y, Z Vector
		[XmlIgnore]
		public Vector3 Vector
		{
			get {
				return new Vector3(X, Y, Z);
			}
			set {
				X = value.x;
				Y = value.y;
				Z = value.z;
			}
		}

		[XmlIgnore]
		public float X = 0;
		[XmlAttribute("x")]
		public string XString {
			get {
				if (X == 0) {
					return null;
				}
				return X.ToString ();
			}
			set {
				X = Convert.ToSingle(value);
			}
		}

		[XmlIgnore]
		public float Y = 0;
		[XmlAttribute("y")]
		public string YString {
			get {
				if (Y == 0) {
					return null;
				}
				return Y.ToString ();
			}
			set {
				Y = Convert.ToSingle(value);
			}
		}

		[XmlIgnore]
		public float Z = 0;
		[XmlAttribute("z")]
		public string ZString {
			get {
				if (Z == 0) {
					return null;
				}
				return Z.ToString ();
			}
			set {
				Z = Convert.ToSingle(value);
			}
		}
		#endregion

		#region Duration
		private float duration = 0;
		[XmlIgnore]
		public float Duration {
			get {
				if (Type == ABAnimationType.Group) {
					float maxDuration = 0;
					foreach (var animation in Animations) {
						if (animation.Duration > maxDuration) {
							maxDuration = animation.Duration;
						}
					}
					return maxDuration;
				}

				if (Type == ABAnimationType.Sequence) {
					float sum = 0;
					foreach (var animation in Animations) {
						sum += animation.Duration;
					}
					return sum;
				}

				return duration;
			}
			set {
				duration = value;
			}
		}
		[XmlAttribute("duration")]//, DefaultValue(0)]
		public string DurationString {
			get {
				if (Duration <= 0 || Type == ABAnimationType.Group || Type == ABAnimationType.Sequence) {
					return null;
				}
				return Duration.ToString ();
			}
			set {
				Duration = Convert.ToSingle(value);
			}
		}
		#endregion

		#region Repeat
		[XmlAttribute("repeat")]
		public string repeat {
			get {
				if (RepeatsCount == RepeatForever) {
					return "forever";
				}
				if (RepeatsCount == 1) {
					return null;
				}
				return RepeatsCount.ToString ();
			}
			set {
				if (value.ToLower () == "forever") {
					RepeatsCount = RepeatForever;
				} else if (value.ToLower () == "once") {
					RepeatsCount = 1;
				} else {
					RepeatsCount = Convert.ToInt32(RepeatsCount);
				}
			}
		}
		[XmlIgnore]
		public int RepeatsCount = 1;
		#endregion
	}
}