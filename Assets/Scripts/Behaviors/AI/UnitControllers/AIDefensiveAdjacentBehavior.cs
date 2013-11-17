using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Author: Ryan Taite
/// 
/// Defines a defensive AI behavior. The squad will not move unless an enemy unit is
/// in its movement area and there is a unit adjacent to the target unit.
/// </summary>
[AddComponentMenu("Tactibru/AI/Squad Behaviors/Defensive Adjacent Behavior")]
public class AIDefensiveAdjacentBehavior : AIUnitBehavior
{
    /// <summary>
    /// Determines the target for this squad.
    /// </summary>
    /// <returns>AI state the controller should enter after determining the target.</returns>
    public override AIState DetermineTarget()
    {
        MovePointBehavior movePoint = Actor.currentMovePoint;
        
        // Get the maximum movement distance.
        CombatSquadBehavior squadBehavior = Actor.GetComponent<CombatSquadBehavior>();

        if (squadBehavior == null)
        {
            Debug.LogError(string.Format("AI Squad ({0}) does not have a CombatSquadBehavior!", Actor.transform.name));
            Actor.actorHasMovedThisTurn = true;
            
            return AIState.PickingSquad;
        }

        int maxDistance = squadBehavior.Squad.Speed;
        
        // Retrieve a list of nodes within appropriate distance.
        List<MovePointBehavior> graph = new List<MovePointBehavior>();
        movePoint.BuildGraph(maxDistance, 0, grid, ref graph, true);
        
        Actor.actorHasMovedThisTurn = true;
        
        // Iterate over the nodes to determine if any node has an actor.
        foreach (MovePointBehavior node in graph)
        {
            /// Node currently being checked
            ActorBehavior actorOnNode = gameController.GetActorOnNode(node);

            /// If the actor is null OR the actor is not on the player's team, find another node to check
            if (actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
            {
                continue;
            }
                
            /// Let's us see that we found an acceptable target
            Debug.Log("Found target actor!");

            /// Iterate over each node in the found target actor's neighborList for friendly units
            foreach(MovePointBehavior neighborNode in actorOnNode.theGrid.targetNode.neighborList)
            {
                /// Node currently being checked
                ActorBehavior neighborActorOnNode = gameController.GetActorOnNode(neighborNode);

                /// If the actor is null OR the actor is not on the player's team, find another neighbor node to check
                if (neighborActorOnNode == null || (neighborActorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
                {
                    continue;
                }

                /// Let's us see that there was a unit adjacent to the target actor
                Debug.Log("Found actor adjacent to target actor!");
                
                grid.ignoreList.Remove(node);
                /// Create a path to the target actor
                List<MovePointBehavior> path = movePoint.FindPath(node, maxDistance, grid);
                path.RemoveAt(path.Count - 1);
                
                /// Attach path to our actor
                Actor.pathList = path;
                /// Actor can now move to his target
                Actor.canMove = true;
                
                GridBehavior.preCombat = true;
                grid.currentActor = Actor.gameObject;
                grid.targetActor = actorOnNode.gameObject;
                
                grid.ignoreList.Add(node);
                
                return AIState.WaitingForCombat;
            }
        }
        
        return AIState.PickingSquad;
    }
}
