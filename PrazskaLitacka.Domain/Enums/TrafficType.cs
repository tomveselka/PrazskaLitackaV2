using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PrazskaLitacka.Domain.Enums;
public enum TrafficType
{
    [XmlEnum("metro")]
    Metro,
    [XmlEnum("tram")]
    Tram,
    [XmlEnum("train_os")]
    TrainOs,
    [XmlEnum("train_r")]
    TrainR,
    [XmlEnum("funicular")]
    Funicular,
    [XmlEnum("bus_prague")]
    BusPrague,
    [XmlEnum("bus_regional")]
    BusRegional,
    [XmlEnum("ferry")]
    Ferry,
    [XmlEnum("trolleybus")]
    Trolleybus,
    [XmlEnum("bike")]
    Bike,
}