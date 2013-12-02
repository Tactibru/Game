using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implements the player's control for the game.
/// </summary>
[AddComponentMenu("Tactibru/Movement/Grid Controller")]
[RequireComponent(typeof(GridBehavior))]
[RequireComponent(typeof(GameControllerBehaviour))]
public class GridControlBehavior : MonoBehaviour
{
	/// <summary>
	/// Represents possible states for the Grid Controller.
	/// </summary>
	public enum GridControlState
	{
		/// <summary>
		/// Player is currently in the process of selecting her unit.
		/// </summary>
		SelectingUnit,

		/// <summary>
		/// Player is currently in the process of selecting where her selected unit will move.
		/// </summary>
		SelectingMovement,

		/// <summary>
		/// Player is waiting for the unit's movement to complete.
		/// </summary>
		AwaitingMovement,

		/// <summary>
		/// Player is currently in the process of selecting what target within her unit's attack range she will attack.
		/// </summary>
		SelectingTarget,

		/// <summary>
		/// Awaiting the completion of a combat sequence.
		/// </summary>
		InCombat,

		/// <summary>
		/// Awaiting the completion of the AI's turn.
		/// </summary>
		AwaitingEnemy,
	}

	/// <summary>
	/// A public accessor to retrieve the current state of the grid controller.
	/// </summary>
	public GridControlState State
	{
		get
		{
			return controlState;
		}
	}

	/// <summary>
	/// Stores the current state for the Grid Controller's FSM.
	/// </summary>
	/// <remarks>
	/// TODO: Change to private -- public is currently set so that the state can be monitored in the Inspector.
	/// </remarks>
	public GridControlState controlState = GridControlState.SelectingUnit;

	/// <summary>
	/// Used to store the currently-selected squad.
	/// </summary>
	private ActorBehavior selectedSquad = null;

	/// <summary>
	/// Used to store the starting point of the currently-selected squad.
	/// </summary>
	private MovePointBehavior startingPoint = null;

	/// <summary>
	/// Stores a reference to the GameControllerBehavior on the grid object.
	/// </summary>
	private GameControllerBehaviour controller = null;

	/// <summary>
	/// Stores a reference to the GridBehavior on the grid object.
	/// </summary>
	private GridBehavior grid = null;

	/// <summary>
	/// Captures a reference to the Grid and Game Controller components on the object.
	/// </summary>
	public void Start()
	{
		controller = GetComponent<GameControllerBehaviour>();
		if(controller == null)
			Debug.LogError ("GridControlBehavior is applied to an object that does not have the GameControllerBehavior!");

		grid = GetComponent<GridBehavior>();
		if(grid == null)
			Debug.LogError ("GridBehavior is applied to an object that does not have the GridBehavior!");
	}

	/// <summary>
	/// Performs the appropriate Update method based on the state machine.
	/// </summary>
	public void Update()
	{
		switch(controlState)
		{
		case GridControlState.SelectingUnit:
			updateSelectingUnit();
			return;

		case GridControlState.AwaitingMovement:
			if(!selectedSquad.canMove)
				controlState = GridControlState.SelectingTarget;
			return;

		case GridControlState.SelectingMovement:
			updateSelectingMovement();
			return;

		case GridControlState.SelectingTarget:
			updateSelectingTarget();
			return;

		case GridControlState.AwaitingEnemy:
			if(controller.currentTurn == GameControllerBehaviour.UnitSide.player)
				controlState = GridControlState.SelectingUnit;
			return;

		default:
			return;
		}
	}

	/// <summary>
	/// Waits for the left mouse button to be clicked, then performs a raycast to determine if the player has clicked
	/// on one of her own squads.
	/// </summary>
	/// <remarks>
	/// Transitions:
	/// 	Player Clicks Valid Squad -> SelectingMovement
	/// </remarks>
	private void updateSelectingUnit()
	{
		// Wait for the left mouse button to be pressed.
		if(Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Perform raycasting and store a list of all objects that have been selected.
			List<RaycastHit> hits = new List<RaycastHit>();
			hits.AddRange(Physics.RaycastAll (ray));

			// Iterate over the selection list to determine if the player has clicked on one of her squads.
			foreach(RaycastHit hitInfo in hits.OrderBy (l => l.distance))
			{
				// Capture the actor behavior on the hit object.
				ActorBehavior actor = hitInfo.transform.GetComponent<ActorBehavior>();
				if(actor == null)
					continue;

				// Ensure that the unit has not moved and belongs to the player.
				if(!actor.actorHasMovedThisTurn && actor.theSide == GameControllerBehaviour.UnitSide.player)
				{
					// Mark the actor as selected.
					selectedSquad = actor;
					startingPoint = actor.currentMovePoint;

					// Begin the scripted "Idle" animation.
					UnitIdleAnimationBehavior[] idles = selectedSquad.GetComponentsInChildren<UnitIdleAnimationBehavior>();
					foreach(UnitIdleAnimationBehavior idle in idles)
						idle.Active = true;

					// Enable rendering of valid target movement nodes.
					if(actor.currentMovePoint != null)
						actor.currentMovePoint.HighlightValidNodes(actor, grid);

					// STATE CHANGE: SelectingUnit -> SelectingMovement
					controlState = GridControlState.SelectingMovement;

					break;
				}
			}
		}
	}

	/// <summary>
	/// Waits for the left mouse button to be clicked, then performs a raycast to determine if the player has clicked
	/// on a valid movement point.
	/// </summary>
	/// <remarks>
	/// Transitions:
	/// 	Player presses Escape key -> SelectingUnit
	/// 	Player presses Space key -> SelectingTarget
	/// 
	/// 	Path to movement point cannot be found -> SelectingUnit
	/// 	Player selects a valid movement point -> AwaitingMovement
	/// </remarks>
	private void updateSelectingMovement()
	{
		// Allow the player to press the Escape key to deselect their current unit.
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			deselectSquad();
			controlState = GridControlState.SelectingUnit;
			return;
		}

		// Allow the player to press the Space bar to skip the movement step.
		if(Input.GetKeyDown (KeyCode.Space))
		{
			selectedSquad.actorHasMovedThisTurn = true;
			grid.HideMovePoints();
			controlState = GridControlState.SelectingTarget;
			return;
		}

		// Wait for the left mouse button to be pressed.
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Perform raycasting and store a list of all objects that have been selected.
			List<RaycastHit> hits = new List<RaycastHit>();
			hits.AddRange(Physics.RaycastAll (ray));
			
			// Iterate over the selection list to determine if the player has clicked on a valid movementpoint.
			foreach(RaycastHit hitInfo in hits.OrderBy (l => l.distance))
			{
				// Retrieve the movepoint behavior from the hit target.
				MovePointBehavior movePoint = hitInfo.transform.GetComponent<MovePointBehavior>();
				if(movePoint == null || !movePoint.renderer.enabled)
					continue;

				// Retrieve the combat squad behavior from the currently-selected squad.
				CombatSquadBehavior combatSquad = selectedSquad.GetComponent<CombatSquadBehavior>();
				if(combatSquad == null)
					Debug.LogError ("Selected a unit with no combat squad attached!");

				// Retrieve the maximum movement distance of the selected squad.
				int distance = (combatSquad == null ? 0 : combatSquad.Squad.Speed);

				// Mark the actor as having moved.
				selectedSquad.actorHasMovedThisTurn = true;

				// Retrieve a path from the starting point to the selected node.
				List<MovePointBehavior> pathList = startingPoint.FindPath(movePoint, distance, grid);
				if(pathList != null)
				{
					selectedSquad.pathList = pathList;
					selectedSquad.canMove = true;

					// Remove the squad's current movement point from the ignore list.
					grid.ignoreList.Remove (startingPoint);

					// Add the squad's target movement point to the ignore list.
					grid.ignoreList.Add (movePoint);

					// Hide visible nodes on the grid.
					grid.HideMovePoints();

					// Transition into the AwaitingMovement state.
					controlState = GridControlState.AwaitingMovement;
				}
				else
				{
					Debug.LogError("No path to target!");

					// Deselect the current squad, but do not allow it to move again!
					deselectSquad();

					// Return to the SelectingUnit state.
					controlState = GridControlState.SelectingUnit;
				}
			}
		}
	}

	/// <summary>
	/// Waits for the left mouse button to be clicked, then performs a raycast to determine if the player has clicked
	/// on a valid enemy unit.
	/// </summary>
	/// <remarks>
	/// Transitions:
	/// 	Player presses Escape key -> SelectingMovement
	/// 	Player presses Space key -> SelectingUnit or AwaitingEnemy
	/// </remarks>
	private void updateSelectingTarget()
	{
		// Allow the player to press the escape key to undo their movement.
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			// Unflag the actor as having moved.
			selectedSquad.actorHasMovedThisTurn = false;

			// Remove the squad's current movement point from the ignore list.
			grid.ignoreList.Remove (selectedSquad.currentMovePoint);

			// Add the squad's starting movement point to the ignore list.
			grid.ignoreList.Add (startingPoint);

			// Forcibly move the actor to the starting point.
			selectedSquad.currentMovePoint = startingPoint;
			selectedSquad.transform.position = startingPoint.transform.position;

			// Re-highlight valid movement nodes.
			startingPoint.HighlightValidNodes(selectedSquad, grid);

			// Return to the SelectingMovement state.
			controlState = GridControlState.SelectingMovement;

			return;
		}

		// Allow the player to press the space bar to skip attacking.
		if(Input.GetKeyDown (KeyCode.Space))
		{
			// Hide the target movement points.
			grid.HideMovePoints();

			// Decrement the number of moves left for this turn.
			controller.leftToMoveThis--;

			// Switch to the SelectingUnits state if the player still has moves left, otherwise end the turn.
			controlState = (controller.leftToMoveThis > 0 ?
			                GridControlState.SelectingUnit :
			                GridControlState.AwaitingEnemy);

			// Deselect the current squad.
			deselectSquad();

			return;
		}
	}

	/// <summary>
	/// Performs necessary steps to deselect the current squad.
	/// </summary>
	private void deselectSquad()
	{
		UnitIdleAnimationBehavior[] idles = selectedSquad.GetComponentsInChildren<UnitIdleAnimationBehavior>();
		foreach(UnitIdleAnimationBehavior idle in idles)
			idle.Active = false;
		
		selectedSquad = null;
		startingPoint = null;
		grid.HideMovePoints();
	}
}
