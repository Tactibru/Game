using UnityEngine;
using UnityEditor;
using System.IO;

namespace Editor.Util
{
	/// <summary>
	/// Utility class related to implementing custom assets, such as the <see cref="NodeSkeletonSystem.NodeSkeletonStructure" />
	/// </summary>
	public static class CustomAssetUtility
	{
		/// <summary>
		/// Creates a new asset based on the specified type.
		/// 
		/// Author: Jacob Pennock
		/// http://www.jacobpennock.com/Blog/?page_id=715
		/// </summary>
		/// <typeparam name='T'>
		/// Scriptable object type that the asset will represent.
		/// </typeparam>
		public static T CreateAsset<T>() where T : ScriptableObject
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if(path == "")
				path = "Assets";
			else if(Path.GetExtension(path) != "")
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString () + ".asset");

			return CreateAsset<T>(assetPathAndName);
		}

		/// <summary>
		/// Creates a new asset based on the specified type.
		/// 
		/// Author: Jacob Pennock
		/// http://www.jacobpennock.com/Blog/?page_id=715
		/// </summary>
		/// <typeparam name='T'>
		/// Scriptable object type that the asset will represent.
		/// </typeparam>
		/// <param name="assetPathAndName">Path where the asset should be placed.</param>
		public static T CreateAsset<T>(string assetPathAndName) where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();
			
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			AssetDatabase.SaveAssets ();
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;

			return asset;
		}
	}
}