using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMParser
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

    class Way
    {
        public long Id;
        public bool Visible;
        public string? Timestamp;

        public List<Node> Nodes = new();
        public List<Tag> Tags = new();
    }

    class Relation
    {
        public long Id;
        public bool Visible;
        public string? Timestamp;

        public List<(Node Node, string Role)> Nodes = new();
        public List<(Way Way, string Role)> Ways = new();
        public List<Tag> Tags = new();
    }
}
