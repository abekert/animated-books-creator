using Helpers;

namespace BookModel
{
	interface IPositionable
	{
		ABPosition Position { get; set; }

		void RepositionBegan ();
	}
}