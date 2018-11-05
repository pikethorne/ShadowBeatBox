using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;

public class ParseToArray : MonoBehaviour {

    public TextAsset xmlRawFile;
    public Text uiText;

	// Use this for initialization
	void Start ()
    {
        string data = xmlRawFile.text;
        ParseXmlFile(data);
	}

    public string[] ParseXmlFile(string xmlData)
    {
        int i = 0;
        string[] moveArray = new string[8];


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string xmlPathPattern = "//MoveList";
        XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);

        foreach(XmlNode node in myNodeList)
        {
            XmlNode move = node.FirstChild;
            moveArray[i] = move.InnerXml;

            i++;
        }

        return moveArray;
    }

    public void PrintArray()
    {

    }
	
}
