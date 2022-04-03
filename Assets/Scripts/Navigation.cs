using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct NavNode {
    public NavNode(TileCoord coord, float cost)
    {
        this.coord = coord;
        this.cost = cost;
    }
    public TileCoord coord;
    public float cost;
}

public class Navigation : MonoBehaviour
{
    static List<Navigation> ActiveNavigations = new List<Navigation>();

    Queue<TileCoord> navQueue;
    Vector3 nextNavPosition;
    bool moving = false;

    [SerializeField] float Speed = 2;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Navigate(new TileCoord(12, 1));
        ActiveNavigations.Add(this);
    }

    void Navigate(TileCoord target)
    {
        Debug.Log(GridManager.Instance.GetTile(new TileCoord(6, 8), TileLayer.OBJECT));
        Debug.Log(GridManager.Instance.GetTile(new TileCoord(4, 4), TileLayer.OBJECT));
        var start = GridManager.Instance.GetTileCoordFromWorld(transform.position);
        var path = CalculatePath(start, target);
        if(path == null)
        {
            Debug.LogWarning(gameObject.name + " can't navigate to " + target.ToString());
            moving = false;
            animator.SetBool("Walking", false);
            return;
        } else
        {
            moving = true;
            nextNavPosition = transform.position;
            navQueue = new Queue<TileCoord>(path);
        }
    }

    public void Stop()
    {
        moving = false;
    }

    private void Update()
    {
        if(moving)
        {
            if(Vector3.Distance(transform.position,nextNavPosition)< 0.1f)
            {
                transform.position = nextNavPosition;
                moving = navQueue.Count > 0;
                if(moving)
                {
                    nextNavPosition = GridManager.Instance.GetWorldPosFromTile(navQueue.Dequeue());

                }
                animator.SetBool("Walking", moving);
            } else
            {
                transform.position += (nextNavPosition - transform.position).normalized * Speed * Time.deltaTime;
            }
        }
    }

    private void Recalculate()
    {
        if(moving)
        {
            moving = false;
            Navigate(navQueue.ToArray()[navQueue.Count - 1]);
        }
    }

    public static void RecalculateAll()
    {
        foreach(var nav in ActiveNavigations)
        {
            nav.Recalculate();
        }
    }

    List<TileCoord> CalculatePath(TileCoord start, TileCoord goal)
    {
        var openSet = new List<TileCoord>();
        openSet.Add(start);

        var cameFrom = new Dictionary<TileCoord, TileCoord>();

        var gScore = new Dictionary<TileCoord, float>();
        gScore[start] = 0;

        var fScore = new Dictionary<TileCoord, float>();
        fScore[start] = heuristicLine(start, goal);

        bool keepGoing = true;

        while (keepGoing && openSet.Count > 0)
        {
            openSet.Sort((a, b) => { var diff = fScore[a] - fScore[b]; if (diff > 0) return 1; if (diff < 0) return -1; return 0; });
            var current = openSet[0];
            if (current.Equals(goal))
            {
                return ReconstructPath(cameFrom, current);
            }
            openSet.RemoveAt(0);
            var neighbors = GridManager.Instance.GetNeighbors(current);
            foreach (var navNode in neighbors)
            {
                var neighbor = navNode.coord;
                if (!gScore.ContainsKey(neighbor))
                {
                    gScore[neighbor] = 9999f; // BIG VALUE
                }
                if (!fScore.ContainsKey(neighbor))
                {
                    fScore[neighbor] = 9999f; // BIG VALUE
                }
                var tentativeGScore = gScore[current] + navNode.cost; // WEIGHT
                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + heuristicLine(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    private float heuristicLine(TileCoord a, TileCoord b)
    {
        var dX = a.X - b.X;
        var dY = a.Y - b.Y;
        return Mathf.Sqrt(dX * dX + dY * dY);
    }

    private List<TileCoord> ReconstructPath(Dictionary<TileCoord, TileCoord> cameFrom, TileCoord current)
    {
        var result = new List<TileCoord>();
        result.Add(current);
        while(cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            result.Add(current);
        }
        result.Reverse();
        return result;
    }
}
