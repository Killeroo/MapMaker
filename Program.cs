using System.Diagnostics;
using System.Xml;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

//https://wiki.openstreetmap.org/wiki/Tags
//https://wiki.openstreetmap.org/wiki/Map_features
//https://wiki.openstreetmap.org/wiki/Relief_maps
//https://wiki.openstreetmap.org/wiki/OSM_XML
namespace OSM_test
{
    internal class Program
    {
        class Tag
        {
            public string? Key;
            public string? Value;
        }

        class Node
        {
            public long Id;
            public bool Visible;
            public double Latitude;
            public double Longitude;
            public string? Timestamp;

            public List<Tag> Tags = new();
        }

        static void Main(string[] args)
        {

            string? osmPath = "C:\\Users\\Shadowfax\\Downloads\\map.osm";// Console.ReadLine();

            XmlDocument? doc = new XmlDocument();
            XmlElement? rootOsmElement = null;
            
            // Attempt to load xml and root OSM element
            try
            {
                doc.Load(osmPath);
                rootOsmElement = doc["osm"];
            }
            catch (Exception ex) 
            {
                Console.WriteLine(string.Format("{0} - {1}", ex.GetType().ToString(), ex.Message));
                return;
            }

            Dictionary<long, Node> nodes = new();

            // Load nodes first
            XmlNodeList nodesElementList = rootOsmElement.GetElementsByTagName("node");
            foreach (XmlElement? nodeElement in nodesElementList)
            {
                // Parse node
                Node node = new Node()
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

                //Console.WriteLine("-> Added {0}, {1} tags included", node.Id, node.Tags.Count);

                // Add to collection
                nodes.Add(
                     Convert.ToInt64(nodeElement.Attributes["id"].Value), 
                    node);
            }

            List<Tag> tags = new List<Tag>();
            foreach (Node n in nodes.Values)
            {
                tags.AddRange(n.Tags);
            }

            tags.DistinctBy(x => x.Key).ToList().ForEach(x => Console.WriteLine(x.Key));

            Console.ReadLine();

        }
    }
}