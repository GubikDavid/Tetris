using UnityEngine;
public class TetrominoPrediction : MonoBehaviour
{
    private void Start()
    {
        MakePrediction();
    }
    public void RotateTetromino()
    {
        //if (transform.name.Contains("O Tetromino")) return;
        //transform.position += new Vector3(0, 10, 0);
        //transform.RotateAround(transform.GetChild(0).position, Vector3.forward, -90);
        transform.RotateAround(transform.GetChild(0).position, Vector3.forward, -90);
        string x = ValidMove();
        if (x == "xgrid")
            transform.position += new Vector3(transform.position.x < 4 ? transform.name.Contains("I Tetromino") ? 2 : 1 : transform.name.Contains("I Tetromino") ? -2 : -1, 0, 0);
        //else if (x == "object" || x == "ygrid")
        //transform.RotateAround(transform.GetChild(0).position, Vector3.forward, 90);
        MakePrediction();
    }
    public void MoveTetromino(int i)
    {
        transform.position += new Vector3(i, 0, 0);
        string x = ValidMove();
        if (x == "xgrid" || x == "object")
            transform.position += new Vector3(-i, 0, 0);
        MakePrediction();
    }
    private string ValidMove()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);

            if (x < 0 || x > Tetromino.width - 1)
                return "xgrid";
            else if (y < 0)
                return "ygrid";
            else if (x < Tetromino.width && y < Tetromino.height && Tetromino.grid[x, y] != null)
                return "object";
        }
        return "Success";
    }
    private void MakePrediction()
    {
        transform.position = FindObjectOfType<Tetromino>().transform.position;
        string x = ValidMove();
        while (x == "Success")
        {
            transform.position += new Vector3(0, -1, 0);
            x = ValidMove();
        }
        transform.position -= new Vector3(0, -1, 0);
    }
    public void DestroyPrediction()
    {
        Destroy(gameObject);
        enabled = false;
    }
}
