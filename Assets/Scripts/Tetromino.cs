using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width,height];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move Left
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1,0,0);
            if(!ValidMove()) {transform.position -= new Vector3(-1,0,0);}
        }
        //Move Right
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1,0,0);
            if(!ValidMove()) {transform.position -= new Vector3(1,0,0);}
        }
        //Slam Down
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            while(ValidMove())
            {
                transform.position += new Vector3(0,-1,0);
            }
            if(!ValidMove()) {transform.position += new Vector3(0,1,0);}
        }
        //Rotate Left
        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
            if(!ValidMove())
            {
                transform.position += new Vector3(1,0,0);
                //Checking all possible valid rotation positions
                if(!ValidMove()) {transform.position += new Vector3(-2,0,0);}
                if(!ValidMove()) {transform.position += new Vector3(1,1,0);}
                if(!ValidMove()) {transform.position += new Vector3(1,0,0);}
                if(!ValidMove()) {transform.position += new Vector3(-2,0,0);}
                if(!ValidMove()) {
                    transform.position += new Vector3(1,-1,0);
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
                }
            }
        }
        //Rotate Right
        else if(Input.GetKeyDown(KeyCode.X))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            if(!ValidMove())
            {
                transform.position += new Vector3(1,0,0);
                if(!ValidMove()) {transform.position += new Vector3(-2,0,0);}
                if(!ValidMove()) {transform.position += new Vector3(1,1,0);}
                if(!ValidMove()) {transform.position += new Vector3(1,0,0);}
                if(!ValidMove()) {transform.position += new Vector3(-2,0,0);}
                if(!ValidMove()) {
                    transform.position += new Vector3(1,-1,0);
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
                }
            }
        }

        // Fall down + Move Down
        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime)) {
            transform.position += new Vector3(0, -1, 0);
            if(!ValidMove()) {
                transform.position -= new Vector3(0,-1,0);
                AddToGrid();
                this.enabled = false;
                FindObjectOfType<Spawner>().NewTetromino();
            }
            previousTime = Time.time;
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x - 0.001f);
            int roundedY = Mathf.RoundToInt(children.transform.position.y - 0.001f);

            grid[roundedX,roundedY] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x - 0.001f);
            int roundedY = Mathf.RoundToInt(children.transform.position.y - 0.001f);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= (height + 2))
            {
                return false;
            }

            if (grid[roundedX,roundedY] != null) {return false;}
        }

        return true;
    }
}
