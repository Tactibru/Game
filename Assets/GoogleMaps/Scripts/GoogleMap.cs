using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

public class GoogleMap : MonoBehaviour
{
    public enum Direction
    {
        right,
        left,
        straight,
        NUMBER_OF_DIRECTIONS
    }
    public bool loadOnStart = true;
    public GoogleMapLocation startLocation;
    public GoogleMapLocation endLocation;
    List<DirectionSteps> theRoute = new List<DirectionSteps>();
    public List<DirectionAndDistance> theMapForGame = new List<DirectionAndDistance>();

    void Start()
    {
        if (loadOnStart) Refresh();
    }

    public void Refresh()
    {
        
        StartCoroutine(_Refresh());
    }

    IEnumerator _Refresh()
    {
        var url = "http://maps.googleapis.com/maps/api/directions/xml";
        var qs = "";
       
        if (startLocation.address != "")
            qs += "origin=" + HTTP.URL.Encode(startLocation.address);
        else
        {
            qs += "origin=" + HTTP.URL.Encode(string.Format("{0},{1}", startLocation.latitude, startLocation.longitude));
        }

        if (endLocation.address != "")
            qs += "&destination=" + HTTP.URL.Encode(endLocation.address);
        else
        {
            qs += "&destination=" + HTTP.URL.Encode(string.Format("{0},{1}", endLocation.latitude, startLocation.longitude));
        }

        qs += "&sensor=false";

        var req = new HTTP.Request("GET", url + "?" + qs, true);
        req.Send();
        while (!req.isDone)
            yield return null;
        if (req.exception == null)
        {
            string outputOfGoogleMap = req.response.Text;
            theRoute = parsing(outputOfGoogleMap);
            theMapForGame = makingTheMap();
            //Debug.Log(theRoute[0].Steps[0].Distance.ToString());
            Debug.Log(theMapForGame.Count);

            //Debug.Log(outputOfGoogleMap);
            //var tex = new Texture2D(size, size);
            //tex.LoadImage(req.response.Bytes);
            //renderer.material.mainTexture = tex;
        }
    }
    private List<DirectionAndDistance> makingTheMap()
    {
        List<DirectionAndDistance> results = new List<DirectionAndDistance>();

        for (int index = 0; index < theRoute[0].Steps.Count; index++)
        {
            DirectionAndDistance directionAndDistance = new DirectionAndDistance();
            if (theRoute[0].Steps[index].Description.Contains("right"))
                directionAndDistance.direction = Direction.right;
            else if (theRoute[0].Steps[index].Description.Contains("left"))
                directionAndDistance.direction = Direction.left;
            else
                directionAndDistance.direction = Direction.straight;

            if (theRoute[0].Steps[index].Distance.Contains("mi"))
            {
                string tempString = theRoute[0].Steps[index].Distance;
                tempString = tempString.Remove(tempString.Length - 2, 2);
                float theDistance = float.Parse(tempString);
                //Debug.Log(theDistance.ToString());
                theDistance = theDistance * 5280.0f;
                directionAndDistance.distanceTillTurn = (int)theDistance / 1000;
                //Debug.Log(directionAndDistance.distanceTillTurn.ToString());
            }
            else
            {
                string tempString = theRoute[0].Steps[index].Distance;
                tempString = tempString.Remove(tempString.Length - 2, 2);
                float theDistance = float.Parse(tempString);
                directionAndDistance.distanceTillTurn = ((int)theDistance / 1000) + 1;
                //Debug.Log(directionAndDistance.distanceTillTurn.ToString());

            }
            Debug.Log(directionAndDistance.distanceTillTurn.ToString());
            Debug.Log(directionAndDistance.direction.ToString());
            results.Add(directionAndDistance);
        }

        return results;
    }

    private List<DirectionSteps> parsing(string origin)
    {
        List<DirectionSteps> results = new List<DirectionSteps>();
        XmlDocument xmlDoc = new XmlDocument { InnerXml = origin };

        if (xmlDoc.HasChildNodes)
        {
            XmlNode directionResponseNode = xmlDoc.SelectSingleNode("DirectionsResponse");
            if (directionResponseNode != null)
            {
                XmlNode statusNode = directionResponseNode.SelectSingleNode("status");
                if (statusNode != null && statusNode.InnerText.Equals("OK"))
                {
                    XmlNodeList legs = directionResponseNode.SelectNodes("route/leg");
                    foreach (XmlNode leg in legs)
                    {
                        int stepCount = 1;
                        XmlNodeList stepNodes = leg.SelectNodes("step");
                        List<DirectionStep> steps = new List<DirectionStep>();
                        foreach (XmlNode stepNode in stepNodes)
                        {
                            DirectionStep directionStep = new DirectionStep();
                            directionStep.Index = stepCount++;
                            directionStep.Distance = stepNode.SelectSingleNode("distance/text").InnerText;
                            directionStep.Duration = stepNode.SelectSingleNode("duration/text").InnerText;

                            directionStep.Description = Regex.Replace(stepNode.SelectSingleNode("html_instructions").InnerText, "<[^<]+?>", "");

                            steps.Add(directionStep);
                        }

                        DirectionSteps directionSteps = new DirectionSteps();
                        directionSteps.OriginAddress = leg.SelectSingleNode("start_address").InnerText;
                        directionSteps.DestinationAddress = leg.SelectSingleNode("end_address").InnerText;
                        directionSteps.TotalDistance = leg.SelectSingleNode("distance/text").InnerText;
                        directionSteps.TotalDuration = leg.SelectSingleNode("duration/text").InnerText;
                        directionSteps.Steps = steps;

                        results.Add(directionSteps);
                    }
                }
            }
        }
        return results;
    }
}

[System.Serializable]
public class GoogleMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}

public class DirectionStep
{
    public int Index;
    public string Description;
    public string Distance;
    public string Duration;
}

public class DirectionSteps
{
    public string TotalDuration;
    public string TotalDistance;
    public string OriginAddress;
    public string DestinationAddress;
    public List<DirectionStep> Steps;
}

public class DirectionAndDistance
{
    public GoogleMap.Direction direction;
    public int distanceTillTurn;
}


//[System.Serializable]
//public class GoogleMapMarker
//{
//    public enum GoogleMapMarkerSize
//    {
//        Tiny,
//        Small,
//        Mid
//    }
//    public GoogleMapMarkerSize size;
//    public GoogleMapColor color;
//    public string label;
//    public GoogleMapLocation[] locations;

//}

//[System.Serializable]
//public class GoogleMapPath
//{
//    public int weight = 5;
//    public GoogleMapColor color;
//    public bool fill = false;
//    public GoogleMapColor fillColor;
//    public GoogleMapLocation[] locations;
//}

//[System.Serializable]
//public class route
//{
//    public google_jsonmap routes;
//}
//[System.Serializable]
//public class google_jsonmap
//{
//    public leg[] leg;
//}

//[System.Serializable]
//public class leg
//{
//    public string end_address;
//    public string start_address;
//    public start_location[] start_location;
//    public end_location[] end_location;
//}

//[System.Serializable]
//public class start_location
//{
//    public double lng;
//    public double lat;
//}

//[System.Serializable]
//public class end_location
//{
//    public double lng;
//    public double lat;
//}

//public enum GoogleMapColor
//{
//    black,
//    brown,
//    green,
//    purple,
//    yellow,
//    blue,
//    gray,
//    orange,
//    red,
//    white
//}