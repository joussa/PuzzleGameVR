using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    // Number of columns
    public int cols;

    // Number of rows
    public int rows;

    // width / height of the piece (square)
    public float pieceSize;

    // pieces
    public PieceController[] pieces;

    // successful completion
    public event Action OnCompleted;

    // failed completion
    public event Action OnFailed;

    // width
    float width;

    // height
    float height;

    // depth
    float depth;

    void Awake()
    {
        width = cols * pieceSize;
        height = rows * pieceSize;
    }

    // Get the cell (col, row) given a point in space
    public Vector2 GetCellFromPoint(Vector3 point)
    {
        //get local point
        Vector3 localPoint = transform.InverseTransformPoint(point);

        // save z
        depth = localPoint.z;

        //scale adjustment
        localPoint = Vector3.Scale(localPoint, transform.localScale);

        // get cell (col, row)
        // assumptions:
        // 1) the horizontal coordinate is x (not z, z is the depth)
        // 2) the anchor point of the puzzle is in the center
        // 3) size of the puzzle match exactly col * rows * pieceSize (equivalent: no padding)
        // 4) the face of the puzzle is not rotated about X or Z

        // get column (starts at 0)
        float column = Mathf.Floor((localPoint.x + width / 2) / pieceSize);

        // get row (starts at 0)
        float row = Mathf.Floor((localPoint.y + height / 2) / pieceSize);

        // return the col, row
        return new Vector2(column, row);
    }

    //Check whether a cell is taken or not
    public bool IsTaken(Vector2 cell)
    {
        //print(cell);
        // go through each piece, and check that the piece is not on that cell
        for (int i = 0; i < pieces.Length; i++)
        {
            // if we find a piece in there, it's taken!
            if (pieces[i].isPlaced && pieces[i].currCell == cell)
            {
                //print("Cell is taken!");                
                return true;
            }
        }
        // if not, it's free
        //print("Cell is free");
        return false;
    }

    // Give a cell's col,row, get the global coordinate of the center of the cell
    public Vector3 GetCellPosition(Vector2 cell)
    {
        // go from cell's col,row --> local point
        float x = (-width / 2 + pieceSize / 2 + cell.x * pieceSize) / transform.localScale.x;
        float y = (-height / 2 + pieceSize / 2 + cell.y * pieceSize) / transform.localScale.y;

        Vector3 localPoint = new Vector3(x, y, depth);

        // go from local point --> global point
        Vector3 globalPoint = transform.TransformPoint(localPoint);

        return globalPoint;
    }

    // Checks for completion of the puzzle
    public void CheckCompletion()
    {
        // keep track of correctness
        bool isCorrect = true;

        for (int i = 0; i < pieces.Length; i++)
        {
            // check that all the pieces are placed
            if (!pieces[i].isPlaced) return;

            // keep track of this "Correctness"
            isCorrect = isCorrect && pieces[i].CheckCorrect();
        }

        // that they are all correct
        if (isCorrect)
        {
            // call puzzle completion event
            if (OnCompleted != null) OnCompleted();
        }
        else
        {
            // call puzzle completion event
            if (OnFailed != null) OnFailed();
        }
    }
}
