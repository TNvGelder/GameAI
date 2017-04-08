using Assets.Scripts;
using DataStructures.GraphStructure;
using System.Linq;
using UnityEngine;

public class GraphGenerator
{
    public Graph<Vector2D> Graph = new Graph<Vector2D>();
    
    public GraphGenerator()
    {
        GenerateGraph();
    }

    private void GenerateGraph()
    {
        var start = GameObject.FindGameObjectsWithTag("Roads")[0];

        find_adjacent_node(start);
    }

    /**
     * Look to the left, right, above and below a road piece
     * if there's another road piece, place node and connect edges
     * then look left,right above and below that road piece etc etc
     */
    void find_adjacent_node(GameObject obj)
    {
        var sp = obj.GetComponent<SpriteRenderer>().sprite;
        var width = sp.bounds.size.x;
        var height = sp.bounds.size.y;
        var x = obj.transform.position.x;
        var y = obj.transform.position.y;
        var spacing = 2;
        var widthInterval = spacing + width / 2;
        var heightInterval = spacing + height / 2;

        // right
        CheckAddNode(obj, x + widthInterval, y);

        // left
        CheckAddNode(obj, x - widthInterval, y);

        // above
        CheckAddNode(obj, x, y + heightInterval);

        // under
        CheckAddNode(obj, x, y - heightInterval);
    }

    private void CheckAddNode(GameObject source, float x, float y)
    {
        GameObject adjacentRoad = GetRoad(x, y);

        if (adjacentRoad != null)
        {
            if (PlaceRoadNode(adjacentRoad, source))
                find_adjacent_node(adjacentRoad);
        }
    }

    private bool DoesRoadHaveNode(GameObject obj)
    {
        Vector2D position = new Vector2D(obj.transform.position);
        return Graph.Nodes.ContainsKey(position);
    }

    public bool HasEdge(GameObject a, GameObject b)
    {
        Vector2D positionA = new Vector2D(a.transform.position);
        Vector2D positionB = new Vector2D(b.transform.position);
        return HasEdge(positionA, positionB);
    }

    public bool HasEdge(Vector2D positionA, Vector2D positionB)
    {
        return Graph.Nodes.Any(edge => edge.Key.Equals(positionA)  && edge.Value.Adjacent.Any(x => x.Destination.Value.Equals(positionB)));
    }

    private bool PlaceRoadNode(GameObject to, GameObject from)
    {
        Vector2D positionTo = new Vector2D(to.transform.position);
        Vector2D positionFrom = new Vector2D(from.transform.position);
        if (HasEdge(positionTo, positionFrom))
            return false;

        float cost = CalculateRoadCost(positionTo, positionFrom);
        Graph.AddEdge(positionTo, positionFrom, cost);
        Graph.AddEdge(positionFrom, positionTo, cost);
        return true;
    }

    /**
     * Can be made more sophisticated in the future
     */
    private float CalculateRoadCost(Vector2D v1, Vector2D v2)
    {
        var diff = (v2 - v1);

        return diff.Length();
    }

    public GameObject GetRoad(float x, float y)
    {
        foreach (var child in GameObject.FindGameObjectsWithTag("Roads"))
        {
            Vector3[] array = SpriteLocalToWorld(child);
            
            if (array[0].x <= x && array[1].x >= x &&
                array[0].y <= y && array[1].y >= y)
            {
                return child;
            }
        }

        return null;
    }

    private Vector3[] SpriteLocalToWorld(GameObject obj)
    {
        Sprite sp = obj.GetComponent<SpriteRenderer>().sprite;
        Vector3 pos = obj.transform.position;
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos + sp.bounds.min;
        // Bottom right
        array[1] = pos + sp.bounds.max;
        return array;
    }
}