using System;

namespace Units
{
	/// <summary>
	/// Stores the position of a unit in the squad.
	/// </summary>
	/// <remarks>
	/// This is laid out assuming two rows of five columns each.
	/// </remarks>
	[Serializable]
	public class UnitPosition
	{
		/// <summary>
		/// Column the unit sits in.
		/// </summary>
		/// <remarks>
		/// If the unit occupies 2 columns, this will be the leftmost column.
		/// </remarks>
		public int Column = 0;

		/// <summary>
		/// Row the unit sits in.
		/// </summary>
		/// <remarks>
		/// If the unit occupies 2 rows, this will be ignored.
		/// </remarks>
		public int Row = 0;
	}
}
