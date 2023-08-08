using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace OSMParser
{
    public class Map
    {
        readonly Dictionary<long, Node> Nodes = new();
        readonly Dictionary<long, Way> Ways = new();
        readonly Dictionary<long, Relation> Relations = new();

        public void Parse(string path)
        {
            XmlDocument? doc = new XmlDocument();
            XmlElement? rootOsmElement = null;

            // Attempt to load xml and root OSM element
            try
            {
                doc.Load(path);
                rootOsmElement = doc["osm"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", ex.GetType().ToString(), ex.Message));
                return;
            }

            // Load nodes first
            XmlNodeList nodesElementList = rootOsmElement.GetElementsByTagName("node");
            foreach (XmlElement? nodeElement in nodesElementList)
            {
                // Parse node
                Node node = new()
                {
                    Id = Convert.ToInt64(nodeElement.Attributes["id"].Value),
                    Visible = Convert.ToBoolean(nodeElement.Attributes["visible"].Value),
                    Latitude = Convert.ToDouble(nodeElement.Attributes["lat"].Value),
                    Longitude = Convert.ToDouble(nodeElement.Attributes["lon"].Value),
                    Timestamp = nodeElement.Attributes["timestamp"].Value,
                };

                // Find any tags (if there are any)
                XmlNodeList tagsList = nodeElement.GetElementsByTagName("tag");
                foreach (XmlElement? tagElement in tagsList)
                {
                    node.Tags.Add(new Tag()
                    {
                        Key = tagElement.Attributes["k"].Value,
                        Value = tagElement.Attributes["v"].Value,
                    });
                }

                Nodes.Add(node.Id, node);
            }

            // Next parse ways
            XmlNodeList wayElementList = rootOsmElement.GetElementsByTagName("way");
            foreach (XmlElement? wayElement in wayElementList)
            {
                Way way = new()
                {
                    Id = Convert.ToInt64(wayElement.Attributes["id"].Value),
                    Visible = Convert.ToBoolean(wayElement.Attributes["visible"].Value),
                    Timestamp = wayElement.Attributes["timestamp"].Value,
                };

                // Create a list of nodes in way
                XmlNodeList nodeReferenceElemenets = wayElement.GetElementsByTagName("nd");
                foreach (XmlNode referenceElement in nodeReferenceElemenets)
                {
                    long nodeId = Convert.ToInt64(referenceElement.Attributes["ref"].Value);
                    if (Nodes.ContainsKey(nodeId))
                    {
                        way.Nodes.Add(Nodes[nodeId]);
                    }
                }

                // Parse any tags
                XmlNodeList tagsList = wayElement.GetElementsByTagName("tag");
                foreach (XmlElement? tagElement in tagsList)
                {
                    way.Tags.Add(new Tag()
                    {
                        Key = tagElement.Attributes["k"].Value,
                        Value = tagElement.Attributes["v"].Value,
                    });
                }

                Ways.Add(way.Id, way);
            }

            // Finally parse the relations
            XmlNodeList relationElementList = rootOsmElement.GetElementsByTagName("relation");
            foreach (XmlElement? relationElement in relationElementList)
            {
                Relation relation = new()
                {
                    Id = Convert.ToInt64(relationElement.Attributes["id"].Value),
                    Visible = Convert.ToBoolean(relationElement.Attributes["visible"].Value),
                    Timestamp = relationElement.Attributes["timestamp"].Value,
                };

                // Parse members of the relation
                XmlNodeList nodeReferenceElements = relationElement.GetElementsByTagName("mamber");
                foreach (XmlNode referenceElement in nodeReferenceElements)
                {
                    string memberType = referenceElement.Attributes["ref"].Value;
                    long memberId = Convert.ToInt64(referenceElement.Attributes["id"].Value);
                    string memberRole = referenceElement.Attributes["role"].Value;

                    switch (memberType)
                    {
                        case "way":
                            {
                                if (Ways.ContainsKey(memberId))
                                {
                                    relation.Ways.Add((Ways[memberId], memberRole));
                                }
                                break;
                            }
                        case "node":
                            {
                                if (Nodes.ContainsKey(memberId))
                                {
                                    relation.Nodes.Add((Nodes[memberId], memberRole));
                                }
                                break;
                            }
                    }
                }

                // Parse any tags
                XmlNodeList tagsList = relationElement.GetElementsByTagName("tag");
                foreach (XmlElement? tagElement in tagsList)
                {
                    relation.Tags.Add(new Tag()
                    {
                        Key = tagElement.Attributes["k"].Value,
                        Value = tagElement.Attributes["v"].Value,
                    });
                }

                Relations.Add(relation.Id, relation);
            }
        }
    }
}
