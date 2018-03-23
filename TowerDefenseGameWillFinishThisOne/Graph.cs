using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Graph<TVertex, TEdge> 
    {
        private List<Vertex<TVertex, TEdge>> Vertices = new List<Vertex<TVertex, TEdge>>();
        private List<Edge<TVertex, TEdge>> Edges = new List<Edge<TVertex, TEdge>>();

        public Vertex<TVertex, TEdge> FindVertex(Vertex<TVertex, TEdge> vertexLikeThisOne)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i] == vertexLikeThisOne)
                {
                    return Vertices[i];
                } 
            }
            return null;
        }

        public void AddEdge(Vertex<TVertex, TEdge> firstVertex, Vertex<TVertex, TEdge> secondVertex, TEdge edgeType, TEdge edgeType1, int weight)
        {
            if (!DoesHaveVertex(firstVertex) || !DoesHaveVertex(secondVertex))
            {
                throw new InvalidOperationException("Vertex doesn't exist inside the Verticies list");
            }

            var edge = new Edge<TVertex, TEdge>(firstVertex, secondVertex, edgeType, weight);
            var edge1 = new Edge<TVertex, TEdge>(secondVertex, firstVertex, edgeType1, weight);

            if (DoesHaveEdge(edge) || DoesHaveEdge(edge1))
            {
                throw new InvalidOperationException("Edge already exists inside of the Edges list");
            }

            for (int i = 0; i < firstVertex.Edges.Count; i++)
            {
                if (firstVertex.Edges[i].EdgeType.Equals(edge.EdgeType))
                {
                    throw new InvalidTilePlacementException();
                }
            }

            for (int j = 0; j < secondVertex.Edges.Count; j++)
            {
                if (secondVertex.Edges[j].EdgeType.Equals(edge1.EdgeType))
                {
                    throw new InvalidTilePlacementException();
                }
            }

            firstVertex.Edges.Add(edge);
            secondVertex.Edges.Add(edge1);

            Edges.Add(edge);
            Edges.Add(edge1);
        }

        public void AddVertex(Vertex<TVertex, TEdge> vertex)
        {
            if (DoesHaveVertex(vertex))
            { 
                //THIS WILL THROW AN EXCEPTION IF YOU COMPLETE A SHAPE, THE REASON WHY IS BECAUSE IT GETS
                //ADDED TO THE VERTICES LIST TWICE IN THE IF STATEMENTS IN THE MAKE MAP SCREEN, EASY SOLUTION,
                //HAVE A BOOLEAN CHECKING WHICH IF STATEMENT THE TILE WENT INTO, IF IT WENT INTO MORE THAN 1,
                //IF STATEMENT, then dont let them place it
                //Vertex already exists inside of the Verticies list
                throw new InvalidOperationException("Vertex already exists inside the Verticies list");
            }
            Vertices.Add(vertex);
        }

        public void RemoveConnection(Vertex<TVertex, TEdge> firstVertex, Vertex<TVertex, TEdge> secondVertex, Edge<TVertex, TEdge> edge, Edge<TVertex, TEdge> edge1)
        {
            firstVertex.Edges.Remove(edge);
            secondVertex.Edges.Remove(edge1);
            Edges.Remove(edge);
            Edges.Remove(edge1);
        }

        public void RemoveVertex(Vertex<TVertex, TEdge> vertex)
        {
            for (int i = 0; i < vertex.Edges.Count; i++)
            {
                var other = vertex.Edges[i].secondVertex;
                for (int j = 0; j < other.Edges.Count; j++)
                {
                    if (other.Edges[j].secondVertex == vertex)
                    {
                        RemoveConnection(vertex, other, vertex.Edges[i], other.Edges[j]);
                        j--;
                        i--;
                        break;
                    }
                }
            }

            Vertices.Remove(vertex);
        }

        public bool DoesHaveVertex(Vertex<TVertex, TEdge> vertex)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Value.Equals(vertex.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DoesHaveEdge(Edge<TVertex, TEdge> edge)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if (Edges[i].firstVertex == edge.firstVertex && Edges[i].secondVertex == edge.secondVertex && Edges[i].Weight == edge.Weight)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddVertex(TVertex value)
        {
            Vertices.Add(new Vertex<TVertex, TEdge>(value));
        }
    }
}
