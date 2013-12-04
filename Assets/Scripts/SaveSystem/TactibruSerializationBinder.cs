using System;
using System.Runtime.Serialization;
using System.Reflection;

namespace Tactibru.SaveSystem
{
	public class TactibruSerializationBinder : SerializationBinder
	{
		public override Type BindToType (string assemblyName, string typeName)
		{
			if(!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
			{
				Type deserializedType = null;

				assemblyName = Assembly.GetExecutingAssembly().FullName;

				deserializedType = Type.GetType (string.Format ("{0}, {1}", typeName, assemblyName));

				return deserializedType;
			}

			return null;
		}
	}
}

