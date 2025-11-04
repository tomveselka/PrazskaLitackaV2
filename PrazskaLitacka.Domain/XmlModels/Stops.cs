using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PrazskaLitacka.Domain.PidStops.Models
{
    [XmlRoot("stops")]
    public class Stops
    {
        [XmlElement("group")]
        public List<Group> Groups { get; set; } = new();

        [XmlAttribute("generatedAt")]
        public DateTime GeneratedAt { get; set; }

        [XmlAttribute("dataFormatVersion")]
        public string DataFormatVersion { get; set; } = string.Empty;
    }

    public class Group
    {
        [XmlElement("stop")]
        public List<Stop> Stops { get; set; } = new();

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("districtCode")]
        public string DistrictCode { get; set; } = string.Empty;

        [XmlAttribute("isTrain")]
        public bool IsTrain { get; set; }

        [XmlIgnore]
        public bool IsTrainSpecified { get; set; } // so optional bool works

        [XmlAttribute("idosCategory")]
        public int IdosCategory { get; set; }

        [XmlAttribute("idosName")]
        public string IdosName { get; set; } = string.Empty;

        [XmlAttribute("fullName")]
        public string FullName { get; set; } = string.Empty;

        [XmlAttribute("uniqueName")]
        public string UniqueName { get; set; } = string.Empty;

        [XmlAttribute("node")]
        public int Node { get; set; }

        [XmlAttribute("cis")]
        public int Cis { get; set; }

        [XmlIgnore]
        public bool CisSpecified { get; set; } // optional int

        [XmlAttribute("avgLat")]
        public float AvgLat { get; set; }

        [XmlAttribute("avgLon")]
        public float AvgLon { get; set; }

        [XmlAttribute("avgJtskX")]
        public float AvgJtskX { get; set; }

        [XmlAttribute("avgJtskY")]
        public float AvgJtskY { get; set; }

        [XmlAttribute("municipality")]
        public string? Municipality { get; set; }
    }

    public class Stop
    {
        [XmlElement("line")]
        public List<Line> Lines { get; set; } = new();

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("platform")]
        public string? Platform { get; set; }

        [XmlAttribute("altIdosName")]
        public string AltIdosName { get; set; } = string.Empty;

        [XmlAttribute("lat")]
        public float Lat { get; set; }

        [XmlAttribute("lon")]
        public float Lon { get; set; }

        [XmlAttribute("jtskX")]
        public float JtskX { get; set; }

        [XmlAttribute("jtskY")]
        public float JtskY { get; set; }

        [XmlAttribute("zone")]
        public string Zone { get; set; } = string.Empty;

        [XmlAttribute("gtfsIds")]
        public string GtfsIds { get; set; } = string.Empty;

        [XmlAttribute("wheelchairAccess")]
        public WheelchairAccessType WheelchairAccess { get; set; }

        [XmlIgnore]
        public bool WheelchairAccessSpecified { get; set; }

        [XmlAttribute("isMetro")]
        public bool IsMetro { get; set; }

        [XmlIgnore]
        public bool IsMetroSpecified { get; set; }
    }

    public class Line
    {
        [XmlText]
        public string? Value { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public TrafficType Type { get; set; }

        [XmlAttribute("isNight")]
        public bool IsNight { get; set; }

        [XmlIgnore]
        public bool IsNightSpecified { get; set; }

        [XmlAttribute("direction")]
        public string Direction { get; set; } = string.Empty;

        [XmlAttribute("direction2")]
        public string? Direction2 { get; set; }
    }

    public enum WheelchairAccessType
    {
        unknown,
        notPossible,
        possible
    }

    public enum TrafficType
    {
        metro,
        tram,
        train,
        funicular,
        bus,
        ferry,
        trolleybus
    }
}
