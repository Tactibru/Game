using UnityEngine;
using System.Collections;
using UEd = UnityEditor.Editor;

namespace Tactibru.Editor
{
	/// <summary>
	/// Serves as a base class for all Inspector-based editors, to work around
	/// <see cref="Editor" />'s use of a generic <see cref="UnityEngine.Object">.
	/// 
	/// Based on code from:
	/// http://www.gamasutra.com/view/news/126740/Opinion_Extending_The_Unity3D_Editor.php
	/// </summary>
	/// <example>
	/// [CustomEditor(typeof(TestBehavior))]
	/// public class MyEditor : EditorBase<TestBehavior> {
	/// }
	/// </example>
	public class EditorBase<T> : UEd where T : UnityEngine.Object
	{
		/// <summary>
		/// Provides a strongly-typed reference to the editor target.
		/// </summary>
		protected T Target { get { return (T)target; } }
	}
}
