using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Move
{
    [XmlAttribute("listNum")]
    public string listNum;

    [XmlElement("Moves")]
    public string moves;

    [XmlElement("Duration")]
    public string duration;
}
