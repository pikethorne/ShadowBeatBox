using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

[XmlRoot("MoveDatabase")]
public class ParseToList : MonoBehaviour {

    [XmlArray("MoveList")]
    [XmlArrayItem("Moves")]

    public List<string> moveItems = new List<string>();
    
}