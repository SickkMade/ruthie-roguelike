using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int dungeonWidth = 64;
    [SerializeField] private int dungeonHeight = 64;
    [SerializeField] private int minRoomSize = 6;
    [SerializeField] private int maxRoomSize = 20;
    [SerializeField] private int maxIterations = 5;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase pathTile;
    [SerializeField] private List<TileBase> emptyTile = new();
    [SerializeField] private int corridorWidth = 1; // Width of the corridor


    private Node rootNode;

    void Start(){
        GenerateDungeon();
        GameManager.Instance.dungeonRootNode = rootNode;
        GameManager.Instance.CallSetPlayerSpawnEvent(GetRandomRoomFromNode(rootNode));
    }

    void GenerateDungeon(){
        RectInt initialRect = new(0, 0, dungeonWidth, dungeonHeight);
        rootNode = new Node(initialRect);

        SplitSpace(rootNode, maxIterations);

        CreateRooms(rootNode);

        DrawRooms(rootNode);

        ConnectRooms(rootNode);

    }

    void SplitSpace(Node node, int iterations){
        if(iterations <= 0 || node.rect.width < minRoomSize * 2 && node.rect.height < minRoomSize * 2){
            return;
        }


        bool splitHorizontal = Random.value > .5f;

        int max = (splitHorizontal ? node.rect.height : node.rect.width) - minRoomSize;
        if(max <= minRoomSize)return;
        int split = Random.Range(minRoomSize,max);
        if(splitHorizontal){
            node.left = new Node(new RectInt(node.rect.x, node.rect.y, node.rect.width, split));
            node.right = new Node(new RectInt(node.rect.x, node.rect.y + split, node.rect.width, node.rect.height-split));
        } else {
            node.left = new Node(new RectInt(node.rect.x, node.rect.y, split, node.rect.height));
            node.right = new Node(new RectInt(node.rect.x + split, node.rect.y, node.rect.width-split, node.rect.height));
        }

        SplitSpace(node.left, iterations-1);
        SplitSpace(node.right, iterations-1);
    }

    void CreateRooms(Node node){
        if(node.left == null && node.right == null){
            //only create rooms for leaf nodes
            int roomWidth = Random.Range(minRoomSize, Mathf.Min(node.rect.width, maxRoomSize));
            int roomHeight = Random.Range(minRoomSize, Mathf.Min(node.rect.height, maxRoomSize));

            int roomX = Random.Range(node.rect.x, node.rect.x + node.rect.width - roomWidth);
            int roomY = Random.Range(node.rect.y, node.rect.y + node.rect.height - roomHeight);

            node.room = new RectInt(roomX, roomY, roomWidth, roomHeight);
        }
        else {
            if(node.left != null){
                CreateRooms(node.left);
            }
            if(node.right != null){
                CreateRooms(node.right);
            }
        }
    }

    void ConnectRooms(Node node){
        if(node.left != null && node.right != null){
            ConnectRooms(node.left);
            ConnectRooms(node.right);

            //impliment get room if this shows to be an error
            RectInt rightRoom = GetRoom(node.right);
            RectInt leftRoom = GetRoom(node.left);

            CreateCorridor(leftRoom.center, rightRoom.center);
        }
    }
    RectInt GetRoom(Node node)
{
    if (node.room != null)
    {
        return node.room.Value;
    }
    else
    {
        // Recursively search for a room in the child nodes
        RectInt leftRoom = node.left != null ? GetRoom(node.left) : new RectInt();
        RectInt rightRoom = node.right != null ? GetRoom(node.right) : new RectInt();
        return leftRoom.size != Vector2Int.zero ? leftRoom : rightRoom;
    }
}


void CreateCorridor(Vector2 pointA, Vector2 pointB) {
    bool horizontalFirst = Random.value > .5f;

    if (horizontalFirst) {
        DrawCorridorLine((int)pointA.x, (int)pointA.y, (int)pointB.x, (int)pointA.y, corridorWidth);
        DrawCorridorLine((int)pointB.x, (int)pointA.y, (int)pointB.x, (int)pointB.y, corridorWidth);
    } else {
        DrawCorridorLine((int)pointA.x, (int)pointA.y, (int)pointA.x, (int)pointB.y, corridorWidth);
        DrawCorridorLine((int)pointA.x, (int)pointB.y, (int)pointB.x, (int)pointB.y, corridorWidth);
    }
}

void DrawCorridorLine(int x0, int y0, int x1, int y1, int width) {
    for (int i = -width / 2; i <= width / 2 - 1; i++) { // Offset corridor lines based on width
        TileBase tile = emptyTile[1];
        int offset = 0;

        if(i == -width/2 || i == width/2 - 1){
            tile = floorTile;
            offset=width/2;
        }
        if (x0 == x1) { // Vertical corridor
            DrawLine(x0 + i, y0-offset, x1 + i, y1+offset, tile);
        } else { // Horizontal corridor
            DrawLine(x0-offset, y0 + i, x1+offset, y1 + i, tile);
        }
    }
}


    void DrawLine(int x0, int y0, int x1, int y1, TileBase tile)
{
    // Bresenham's Line Algorithm for drawing corridors
    int dx = Mathf.Abs(x1 - x0);
    int dy = -Mathf.Abs(y1 - y0);
    int sx = x0 < x1 ? 1 : -1;
    int sy = y0 < y1 ? 1 : -1;
    int err = dx + dy;

    while (true)
    {
        if( !emptyTile.Contains(floorTilemap.GetTile(new Vector3Int(x0, y0, 0))) || tile == emptyTile[1]){
            floorTilemap.SetTile(new Vector3Int(x0, y0, 0), tile);
        }

        if (x0 == x1 && y0 == y1) break;

        int e2 = 2 * err;
        if (e2 >= dy)
        {
            err += dy;
            x0 += sx;
        }
        if (e2 <= dx)
        {
            err += dx;
            y0 += sy;
        }
    }
}

    void DrawRooms(Node node){
        if(node == null) return;
        if(node.room != null){
            RectInt room = node.room.Value;

            // Draw walls around the room
            for (int x = room.xMin; x <= room.xMax; x++)
            {
                floorTilemap.SetTile(new Vector3Int(x, room.yMin, 0), floorTile); // Bottom wall
                floorTilemap.SetTile(new Vector3Int(x, room.yMax, 0), floorTile); // Top wall
            }
            for (int y = room.yMin; y <= room.yMax; y++)
            {
                floorTilemap.SetTile(new Vector3Int(room.xMin, y, 0), floorTile); // Left wall
                floorTilemap.SetTile(new Vector3Int(room.xMax, y, 0), floorTile); // Right wall
            }

            for(int x = node.room.Value.x+1; x < node.room.Value.x + node.room.Value.width;x++){
                for(int y = node.room.Value.y+1; y < node.room.Value.y + node.room.Value.height;y++){
                    floorTilemap.SetTile(new Vector3Int(x,y,0), emptyTile[Random.Range(0, emptyTile.Count)]);
                }
            }
        }
        DrawRooms(node.left);
        DrawRooms(node.right);
    }

    public Node GetRandomRoomFromNode(Node node){
        while(node.left != null || node.right != null){
            if(Random.value > .5f && node.left != null){
                node = node.left;
            } else{
                node = node.right;
            }
        }
        return node;
    }
}
