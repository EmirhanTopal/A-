using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PathMarker // Marker = işaretçi
{
    public MapLocation location;
    public float G; // Cost from start
    public float H; // Cost to goal
    public float F; // F = G + H 
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, GameObject marker, PathMarker p)
    {
        location = l;
        G = g; // The cost of moving from the start to the PathMarker.
        H = h; // The estimated cost of moving from the PathMarker to the goal.
        F = f; // The total cost of moving from the start to the goal via the PathMarker.
        this.marker = marker; // The GameObject that represents the PathMarker.
        parent = p; // The parent PathMarker.
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker)obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
public class FindPathStart : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;

    private List<PathMarker> open = new List<PathMarker>();
    private List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    private PathMarker goalNode;
    private PathMarker startNode;
    private PathMarker lastPos;

    private bool done = false;

    private void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }

    private void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        for (int z = 1; z < maze.depth - 1; z++)
        {
            for (int x = 1; x < maze.width -1 ; x++)
            {
                if (maze.map[x,z] != 1)
                {
                    locations.Add(new MapLocation(x,z));
                } 
            }
        }
        
        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z),0, 0, 0,
            Instantiate(start, startLocation, Quaternion.identity), null);
        
        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z),0, 0, 0,
            Instantiate(end, goalLocation, Quaternion.identity), null);
            
        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode == null)
        {
            return;
        }
        if (thisNode.Equals(goalNode))
        {
            done = true; // goal has been found
            return;
        }
        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;
            if (maze.map[neighbour.x, neighbour.z] == 1)
            {
                continue;
            }

            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth)
            {
                continue;
            }

            if (IsClosed(neighbour))
            {
                continue;
            }

            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock = Instantiate(pathP,new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G: " + G.ToString("0.00");
            values[1].text = "G: " + H.ToString("0.00");
            values[2].text = "G: " + F.ToString("0.00");

            if (!UpdateMarker(neighbour, G, H, F, thisNode))
            {
                open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
            }

            open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();
            PathMarker pm = (PathMarker) open.ElementAt(0);
            closed.Add(pm);
            
            open.RemoveAt(0);
            pm.marker.GetComponent<Renderer>().material = closedMaterial;

            lastPos = pm;
        }
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }

        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker p in closed)
        {
            if (p.location.Equals(marker))
            {
                return true;
            }
        }
        return false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Search(lastPos);
        }
    }
}
