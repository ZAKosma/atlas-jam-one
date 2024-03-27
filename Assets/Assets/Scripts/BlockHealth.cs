using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public int totalSquares = 4; // Total number of squares in the block
    private int activeSquares = 4; // Initially all squares are active

    public GameObject[] squares; // Array of child squares

    void Start()
    {
        // Initialize squares array
        squares = new GameObject[totalSquares];
        for (int i = 0; i < totalSquares; i++)
        {
            squares[i] = transform.GetChild(i).gameObject;
        }
    }

    public int GetActiveSquares()
    {
        return activeSquares;
    }

    public GameObject GetChildSquare(int index)
    {
        if (index >= 0 && index < totalSquares)
        {
            return squares[index];
        }
        return null;
    }

    public void DecreaseHealth()
    {
        activeSquares--;
        if (activeSquares <= 0)
        {
            // Handle block destruction
        }
    }
}