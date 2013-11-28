using System;
using System.Runtime.Serialization;

namespace Tactibru.SaveSystem
{
	/// <summary>
	/// Represents save-game data for Tactibru.
	/// </summary>
	[Serializable]
	public class SaveData : ISerializable
	{
		public string testString = "Test String!";

		public SaveData()
		{
		}

		public SaveData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			testString = (string)serializationInfo.GetValue ("testString", typeof(string));
		}

		public void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue ("testString", testString);
		}
	}
}

