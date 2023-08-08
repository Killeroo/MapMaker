using OSMParser;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

//https://wiki.openstreetmap.org/wiki/Tags
//https://wiki.openstreetmap.org/wiki/Map_features
//https://wiki.openstreetmap.org/wiki/Relief_maps
//https://wiki.openstreetmap.org/wiki/OSM_XML
namespace OSM_test
{
    internal class Program
    {


        static void Main(string[] args)
        {

            string? osmPath = "C:\\Users\\Shadowfax\\Downloads\\map.osm";// Console.ReadLine();



            List<Tag> wayTags = new();
            foreach (var way in ways)
            {
                wayTags.AddRange(way.Value.Tags);
            }

            Dictionary<string, int> tagCount = new();
            foreach (var tag in wayTags)
            {
                if (!tagCount.ContainsKey(tag.Key))
                {
                    tagCount.Add(tag.Key, 0);
                }

                tagCount[tag.Key]++;
            }

            foreach (var tag in tagCount)
            {
                Console.WriteLine("{0} - {1}", tag.Key, tag.Value);
            }

            Console.ReadLine();

        }
    }
}