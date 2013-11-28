using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tactibru.SaveSystem
{
	/// <summary>
	/// This singleton (static) class implements a save-game system using serialization.
	/// </summary>
	public static class SaveEngine
	{
		/// <summary>
		/// Save folder (within User's directory) used for the game.
		/// </summary>
		public const string SAVE_FOLDER_NAME = ".tactibru";

		/// <summary>
		/// Retrieves the absolute path of the save folder based on the operating system.
		/// </summary>
		public static string SAVE_FOLDER
		{
			get
			{
				if(Environment.OSVersion.Platform == PlatformID.Unix ||
				   Environment.OSVersion.Platform == PlatformID.MacOSX)
				{
					return Environment.GetEnvironmentVariable("HOME") +
						"/" + SAVE_FOLDER_NAME;
				}
				else
				{
					return Environment.ExpandEnvironmentVariables("%APPDATA%") +
						@"\" + SAVE_FOLDER_NAME;
				}
			}
		}

		/// <summary>
		/// Serializes the passed SaveData object and saves it.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="saveData">Save data.</param>
		public static void Save(string fileName, SaveData saveData)
		{
			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentException("fileName");

			if(saveData == null)
				throw new ArgumentNullException("saveData");

			if(!Directory.Exists(SAVE_FOLDER))
				Directory.CreateDirectory(SAVE_FOLDER);

			Stream fileStream = File.Open (string.Format ("{0}/{1}.saviburu", SAVE_FOLDER, fileName), FileMode.Create);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new TactibruSerializationBinder();

			binaryFormatter.Serialize(fileStream, saveData);
			fileStream.Close ();
		}

		/// <summary>
		/// Loads the specified file and deserializes it into a SaveData object.
		/// </summary>
		/// <param name="fileName">File name.</param>
		public static SaveData Load(string fileName)
		{
			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentException("fileName");

			Stream fileStream = File.Open (string.Format ("{0}/{1}.saviburu", SAVE_FOLDER, fileName), FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new TactibruSerializationBinder();

			SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();

			return saveData;
		}
	}
}