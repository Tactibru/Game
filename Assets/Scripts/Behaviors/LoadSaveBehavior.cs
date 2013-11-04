//using UnityEngine;
//using System.Collections;
//using System.Xml;
//using System.Xml.Serialization;
//using System.IO;
//using System.Text;



//public class LoadSaveBehavior : MonoBehaviour
//{
//    private Rect save;
//    private Rect load;
//    private Rect saveMSG;
//    private Rect loadMSG;
//    private bool shouldSave;
//    private bool shouldLoad;
//    private bool switchSave;
//    private bool switchLoad;
//    private string fileLocation;
//    private string fileName;
//    private string playerName;
//    private string data;
//    private UserData1 myData;
//    private Vector3 VPosition;
//    public GameObject Player;

//    void Start()
//    {
//        save = new Rect(10, 80, 100, 20);
//        load = new Rect(10, 100, 100, 20);
//        saveMSG = new Rect(10, 120, 400, 40);
//        loadMSG = new Rect(10, 140, 400, 40);

//        // Where we want to save and load to and from 
//        fileLocation = Application.dataPath;
//        fileName = "SaveData.xml";

//        // we need something to store the information into 
//        myData = new UserData1();
//    }

//    void Update() { }

//    void OnGUI()
//    {

//        if (GUI.Button(load, "Load"))
//        {

//            GUI.Label(loadMSG, "Loading from: " + fileLocation);
//            // Load our UserData into myData 
//            LoadXML();
//            if (data.ToString() != "")
//            {
//                myData = (UserData1)DeserializeObject(data);
//                // set the players position to the data we loaded 
//                VPosition = new Vector3(myData.iUser.x, myData.iUser.y, myData.iUser.z);
//                Player.transform.position = VPosition;
//                //Debug.Log(myData.iUser.name);
//            }

//        }

//        if (GUI.Button(save, "Save"))
//        {

//            GUI.Label(saveMSG, "Saving to: " + fileLocation);

//            // Time to creat our XML! 
//            data = SerializeObject(myData);
//            // This is the final resulting XML from the serialization process 
//            CreateXML();
//            Debug.Log(data);
//        }
//    }

//    /* The following metods came from the referenced URL */
//    string UTF8ByteArrayToString(byte[] characters)
//    {
//        UTF8Encoding encoding = new UTF8Encoding();
//        string constructedString = encoding.GetString(characters);
//        return (constructedString);
//    }

//    byte[] StringToUTF8ByteArray(string pXmlString)
//    {
//        UTF8Encoding encoding = new UTF8Encoding();
//        byte[] byteArray = encoding.GetBytes(pXmlString);
//        return byteArray;
//    }

//    // Here we serialize our UserData object of myData 
//    string SerializeObject(object pObject)
//    {
//        string XmlizedString = null;
//        MemoryStream memoryStream = new MemoryStream();
//        XmlSerializer xs = new XmlSerializer(typeof(UserData1));
//        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
//        xs.Serialize(xmlTextWriter, pObject);
//        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
//        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
//        return XmlizedString;
//    }

//    // Here we deserialize it back into its original form 
//    object DeserializeObject(string pXmlizedString)
//    {
//        XmlSerializer xs = new XmlSerializer(typeof(UserData1));
//        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
//        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
//        return xs.Deserialize(memoryStream);
//    }

//    // Finally our save and load methods for the file itself 
//    void CreateXML()
//    {
//        StreamWriter writer;
//        FileInfo t = new FileInfo(fileLocation + "\\" + fileName);
//        if (!t.Exists)
//        {
//            writer = t.CreateText();
//        }
//        else
//        {
//            t.Delete();
//            writer = t.CreateText();
//        }
//        writer.Write(data);
//        writer.Close();
//        Debug.Log("File written.");
//    }

//    void LoadXML()
//    {
//        StreamReader r = File.OpenText(fileLocation + "\\" + fileName);
//        string _info = r.ReadToEnd();
//        r.Close();
//        data = _info;
//        Debug.Log("File Read");
//    }

//    void FindAllEntities()
//    {
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
//        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("Player");

//        foreach (GameObject enemy in enemies)
//        {
//            myData.iUser.x = enemy.transform.position.x;
//            myData.iUser.y = enemy.transform.position.y;
//            myData.iUser.z = enemy.transform.position.z;
//            //myData.iUser.health = enemy.GetComponent<"WhereHealthIsLocation">();
//        }

//        foreach (GameObject friend in friendlies)
//        {
//            myData.iUser.x = friend.transform.position.x;
//            myData.iUser.y = friend.transform.position.y;
//            myData.iUser.z = friend.transform.position.z;
//            //myData.iUser.health = friend.GetComponent<"WhereHealthIsLocation">();
//        }
//    }
//}

//public class UserData1
//{
//    // We have to define a default instance of the structure 
//    public UserData iUser;
//    // Default constructor doesn't really do anything at the moment 
//    public UserData1() { }

//    //For now saves player coords and name
//    //need to add in squads etc.
//    public struct UserData
//    {
//        public float x;
//        public float y;
//        public float z;
//        public float health;
//    }
//}
