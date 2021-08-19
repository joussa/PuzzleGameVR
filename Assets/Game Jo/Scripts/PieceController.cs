using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenvaVR;

[RequireComponent(typeof(Draggable))]
public class PieceController : MonoBehaviour
{

    // puzzle
    public PuzzleController puzzle;

    // flag that tells us whether the piece is placed or not
    public bool isPlaced;

    // current cell
    public Vector2 currCell;

    // correct cell
    public Vector2 correctCell;

    // get a hold of our component
    Draggable drag;

    void Awake()
    {
        // get the drag component
        drag = GetComponent<Draggable>();
    }

    void OnEnable()
    {
        drag.OnDrag += HandleDrag;
        drag.OnDrop += HandleDrop;
        puzzle.OnCompleted += HandleCompletion;
        puzzle.OnFailed += HandleFailure;
    }

    void OnDisable()
    {
        drag.OnDrag -= HandleDrag;
        drag.OnDrop -= HandleDrop;
        puzzle.OnCompleted -= HandleCompletion;
        puzzle.OnFailed -= HandleFailure;
    }

    void HandleFailure()
    {
        // Set the flag to false
        isPlaced = false;

        // send the piece back to it's original position
        drag.SendToOriginalPos();
    }

    // handle a successful puzzle completion
    void HandleCompletion()
    {
        // block the piece
        drag.ToggleBlock(true);
    }

    void HandleDrop()
    {
        // Raycast hits
        RaycastHit[] hits;

        // Find elements
        hits = Physics.RaycastAll(transform.position, -transform.forward, 2);

        // check if the piece is being placed on the puzzle
        for (int i = 0; i < hits.Length; i++)
        {
            // check if the element we found was the puzzle
            if (hits[i].collider.gameObject.GetInstanceID() == puzzle.gameObject.GetInstanceID())
            {
                // the puzzle is there!
                HandlePuzzleDrop(hits[i].point);

                // exit the loop
                break;
            }

        }


    }

    void HandleDrag()
    {
        // Set the flag to false
        isPlaced = false;
    }

    // Takes care of what happens when you drop the piece on the puzzle
    void HandlePuzzleDrop(Vector3 droppedPoint)
    {
        // get cell (col, row)
        Vector2 cell = puzzle.GetCellFromPoint(droppedPoint);

        // check that the cell is not taken
        if (!puzzle.IsTaken(cell))
        {
            // position piece at the center of the cell
            Vector3 newPos = puzzle.GetCellPosition(cell);

            // position puzzle piece on that point
            transform.position = newPos;

            // make them face the oposite of the puzzle's forward
            transform.forward = -puzzle.transform.forward;

            // update cell's information
            isPlaced = true;
            currCell = cell;

            // check puzzle completion
            puzzle.CheckCompletion();
        }
        // if the cell is taken, send the piece back to it's original pos
        else
        {
            // send the piece back to it's original position
            drag.SendToOriginalPos();
        }
    }

    // check if the piece is correctly placed
    public bool CheckCorrect()
    {
        return currCell == correctCell;
    }
}
