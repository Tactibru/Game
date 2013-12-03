/*

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Author: Ryan Taite 
/// 
/// Defines an offensive AI behavior.
/// The squad will select a target and move to attack it until it is dead or unavailable.
/// </summary>
[AddComponentMenu("Tactibru/AI/Squad Behaviors/Offensive Hunter Behavior")]
public class AIOffensiveHunterBehavior : AIUnitBehavior
{
    /// The player's actor we plan to move towards
    MovePointBehavior selectedActor = null;

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
            ActorBehavior actorOnNode = gameController.GetActorOnNode(node);
            
            if (actorOnNode == null || (actorOnNode.theSide != GameControllerBehaviour.UnitSide.player))
                continue;
            
            grid.ignoreList.Remove(node);
            List<MovePointBehavior> path = movePoint.FindPath(node, maxDistance, grid);
            path.RemoveAt(path.Count - 1);
            
            Actor.pathList = path;
            Actor.canMove = true;
            
            GridBehavior.preCombat = true;
            grid.currentActor = Actor.gameObject;
            grid.targetActor = actorOnNode.gameObject;
            
            grid.ignoreList.Add(node);
            
            return AIState.WaitingForCombat;
        }

        /// If we don't have a selected actor, find a new one
        if(selectedActor == null)
            selectedActor = ChooseRandomActorFromPlayerTeam();

        
        /// set the squads path to the random actor with have selected.
        List<MovePointBehavior> squadPath = movePoint.FindPath(selectedActor, maxDistance, grid);
        
        Actor.pathList = squadPath;
        Actor.canMove = true;
        
        return AIState.WaitingForMove;
    }

    /// <summary>
    /// Chooses a random actor from the player's team.
    /// </summary>
    /// <returns>Returns a MovePointBehvior of the actor we selected</returns>
    public MovePointBehavior ChooseRandomActorFromPlayerTeam()
    {
        /// Pick a random actor on the player's team.
        MovePointBehavior randomlySelectedActor = gameController.playerTeam[Random.Range(0, gameController.playerTeam.Count)].theGrid.targetNode;
        return randomlySelectedActor;
    }
}

*/
