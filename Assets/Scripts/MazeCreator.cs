using UnityEngine;

public class MazeCreator {
    
    public static void CreateMaze(Sprite wall, Sprite floor, Material red, Material green)
    {
        float unit = 0.64f;
        int[,] maze;
        float org_x, org_y;
        GameObject maze_obj;

        maze_obj = new GameObject("Maze");

        org_x = 0;
        org_y = 0;
        maze = new int[8,9] { 
                                { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                { 1, 2, 0, 0, 0, 0, 0, 0, 1 },
                                { 1, 1, 1, 1, 1, 0, 1, 1, 1 },
                                { 1, 0, 0, 0, 0, 0, 0, 0, 1 },
                                { 1, 1, 0, 1, 1, 1, 1, 1, 1 },
                                { 1, 0, 0, 0, 0, 0, 0, 0, 1 },
                                { 1, 1, 1, 1, 1, 3, 1, 1, 1 },
                                { 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                            };

        int i, j;
        for (i = 0; i < maze.GetLength(0); i++)
        {
            for (j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 0)
                {
                    GameObject floor_obj = new GameObject("floor");
                    floor_obj.transform.SetParent(maze_obj.transform);
                    floor_obj.transform.position = new Vector2(org_x, org_y);
                    SpriteRenderer renderer = floor_obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = floor;
                }
                else if (maze[i, j] == 1)
                {
                    GameObject wall_obj = new GameObject("wall");
                    wall_obj.layer = LayerMask.NameToLayer("Wall");
                    wall_obj.transform.SetParent(maze_obj.transform);
                    wall_obj.transform.position = new Vector3(org_x, org_y);
                    SpriteRenderer renderer = wall_obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = wall;
                    wall_obj.AddComponent<BoxCollider2D>();
                }
                else if (maze[i, j] == 2)
                {
                    GameObject start_obj = new GameObject("start");
                    start_obj.layer = LayerMask.NameToLayer("Start");
                    start_obj.transform.SetParent(maze_obj.transform);
                    start_obj.transform.position = new Vector2(org_x, org_y);
                    SpriteRenderer renderer = start_obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = floor;
                    renderer.material = red;
                }
                else if (maze[i, j] == 3)
                {
                    GameObject exit_obj = new GameObject("exit");
                    exit_obj.layer = LayerMask.NameToLayer("Exit");
                    exit_obj.transform.SetParent(maze_obj.transform);
                    exit_obj.transform.position = new Vector2(org_x, org_y);
                    SpriteRenderer renderer = exit_obj.AddComponent<SpriteRenderer>();
                    BoxCollider2D exit_collider = exit_obj.AddComponent<BoxCollider2D>();
                    exit_collider.isTrigger = true;
                    renderer.sprite = floor;
                    renderer.material = green;
                }
                org_x += unit;
            }
            org_x = 0;
            org_y += -unit;
        }
    }

}
