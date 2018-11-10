using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("MoveDatabase")]
public class MoveContainer {

    [XmlArray("MoveLists")]
    [XmlArrayItem("MoveList")]
    [XmlArrayItem("Duration")]
    public List<Move> moves = new List<Move>();

    public static MoveContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(MoveContainer));

        StringReader reader = new StringReader(_xml.text);

        MoveContainer moves = serializer.Deserialize(reader) as MoveContainer;

        reader.Close();

        return moves;
    }

}
