using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct NavNode
{
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

    Queue<NavNode> navQueue;
    Vector3 nextNavPosition;
    float lastTolerance;
    public bool IsWalking { get; private set; }

    [SerializeField] float Speed = 2;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ActiveNavigations.Add(this);
    }

    public void Navigate(Interest interest)
    {
        Navigate(interest.Coord, interest.InteractableDistance);
    }

    public void Navigate(TileCoord target, float tolerance = 0)
    {
        var start = GridManager.Instance.GetTileCoordFromWorld(transform.position);
        var path = CalculatePath(start, target, tolerance);
        lastTolerance = tolerance;
        if (path == null)
        {
            Debug.LogWarning(gameObject.name + " can't navigate to " + target.ToString());
            IsWalking = false;
            animator.SetBool("Walking", false);
            return;
        }
        else
        {
            IsWalking = true;
            nextNavPosition = transform.position;
            navQueue = new Queue<NavNode>(path);
        }
    }

    public Interest GetClosestInterest(IEnumerable<Interest> interests)
    {
        Interest closest = null;
        float shortest = 9999999999f;
        foreach (var i in interests)
        {
            var distance = DistanceTo(i);
            if (distance >= 0 && distance < shortest)
            {
                shortest = distance;
                closest = i;
            }
        }
        return closest;
    }

    public float DistanceTo(Interest interest)
    {
        return DistanceTo(interest.Coord, interest.InteractableDistance);
    }

    public float DistanceTo(TileCoord target, float tolerance = 1)
    {
        var start = GridManager.Instance.GetTileCoordFromWorld(transform.position);
        var path = CalculatePath(start, target, tolerance);
        if (path == null)
        {
            return -1;
        }
        float distance = 0;
        foreach (var n in path)
        {
            distance += n.cost;
        }

        return distance;
    }

    public void Stop()
    {
        IsWalking = false;
        animator.SetBool("Walking", false);
    }

    private void Update()
    {
        if (IsWalking)
        {
            if (Vector3.Distance(transform.position, nextNavPosition) < 0.1f)
            {
                transform.position = nextNavPosition;
                IsWalking = navQueue.Count > 0;
                if (IsWalking)
                {
                    nextNavPosition = GridManager.Instance.GetWorldPosFromTile(navQueue.Dequeue().coord);

                }
                animator.SetBool("Walking", IsWalking);
            }
            else
            {
                transform.position += (nextNavPosition - transform.position).normalized * Speed * Time.deltaTime;
            }
        }
    }

    private void Recalculate()
    {
        if (IsWalking)
        {
            IsWalking = false;
            Navigate(navQueue.ToArray()[navQueue.Count - 1].coord, lastTolerance);
        }
    }

    public static void RecalculateAll()
    {
        foreach (var nav in ActiveNavigations)
        {
            nav.Recalculate();
        }
    }

    List<NavNode> CalculatePath(TileCoord start, TileCoord goal, float tolerance = 0)
    {
        var openSet = new List<TileCoord>();
        openSet.Add(start);

        var cameFrom = new Dictionary<TileCoord, NavNode>();

        var gScore = new Dictionary<TileCoord, float>();
        gScore[start] = 0;

        var fScore = new Dictionary<TileCoord, float>();
        fScore[start] = heuristicLine(start, goal);

        bool keepGoing = true;

        while (keepGoing && openSet.Count > 0)
        {
            openSet.Sort((a, b) => { var diff = fScore[a] - fScore[b]; if (diff > 0) return 1; if (diff < 0) return -1; return 0; });
            var current = openSet[0];
            if ((tolerance == 0 && current.Equals(goal)) || current.Distance(goal) <= tolerance)
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
                    cameFrom[neighbor] = new NavNode { coord = current, cost = navNode.cost };
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

    private List<NavNode> ReconstructPath(Dictionary<TileCoord, NavNode> cameFrom, TileCoord current)
    {
        var result = new List<NavNode>();
        result.Add(new NavNode { coord = current, cost = 0 });
        while (cameFrom.ContainsKey(current))
        {
            var nextNode = cameFrom[current];
            current = nextNode.coord;
            result.Add(nextNode);
        }
        result.Reverse();
        return result;
    }
}
