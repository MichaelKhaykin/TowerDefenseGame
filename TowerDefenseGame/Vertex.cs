using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
{
    [JsonObject(IsReference = true)]
    public class Vertex<TVertex, TEdge> : IComparable<Vertex<TVertex, TEdge>>
    {
        public List<Edge<TVertex, TEdge>> Edges = new List<Edge<TVertex, TEdge>>();
        public TVertex Value { get; set; }
        public Pointy Point { get; set; }
        public Vertex<TVertex, TEdge> Founder { get; set; } = null;
        public double FScore { get; set; } = double.MaxValue;
        public double GScore { get; set; } = double.MaxValue;
        public bool hasBeenVisited = false;


        public Vertex(TVertex value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var vertex = obj as Vertex<TVertex, TEdge>;
            return vertex == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(Vertex<TVertex, TEdge> other)
        {
            //FOX THIS
            if (FScore > other.FScore)
            {
                return 1;
            }
            return 0;
        }

        public static bool operator ==(Vertex<TVertex, TEdge> lhs, Vertex<TVertex, TEdge> rhs)
        {
            if (lhs is null || rhs is null)
            {
                return false;
            }

            if (lhs.Value.Equals(rhs.Value))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Vertex<TVertex, TEdge> lhs, Vertex<TVertex, TEdge> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
