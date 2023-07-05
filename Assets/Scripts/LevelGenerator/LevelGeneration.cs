using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class LevelGeneration : MonoBehaviour
{
    /// <summary>
    /// Dang sai phan tao mang Tile. Mai phai sua lai
    /// </summary>
    //X la chieu ngang, Y la chieu doc
    public SpriteRenderer bounds;
    public int roomX = 4;
    public int roomY = 4;
    public const int roomWidth = 10;
    public const int roomHeight = 8;

    [Header("Normal Room")]
    public Room[] normalRoom;

    [Header("Special Room")]
    public Room[] specialRoom;

    public Room[,] Rooms { get; private set; }
    public Tile[,] Tiles { get; private set; }

    private Dictionary<string, Tile> groundPrefabs;


    private Transform roomParent;
    //De 2 huong đe kiem tra xem phong tiep theo no co chan phong truoc do hay khong
    private Vector2 direction; // Kiem tra huong cua phong tiep theo
    private Vector2 lastDirection; //Kiem tra huong phong hien tai

    private Room firstRoom;
    private Room lastRoom;


    private Tile entrance;
    private Tile exit;

    public BackGround createBackGround;
    public GroundBounds createBounds;
    public Player player;
    public static LevelGeneration instance;
    public int level;
    public int score;
    private void Awake()
    {
        instance = this;
        //Tim prefab co class Tile
        Object[] resourseGround = Resources.LoadAll("Prefabs/Ground", typeof(Tile));
        if (resourseGround.Length == 0)
        {
            Debug.LogError("The path is wrong");
        }
        groundPrefabs = new Dictionary<string, Tile>();
        foreach (Object resource in resourseGround)
        {
            Tile ground = (Tile)resource;
            groundPrefabs.Add(ground.name, ground);
        }


        roomParent = GameObject.Find("Room").GetComponent<Transform>();

        Rooms = new Room[roomX, roomY];
        Tiles = new Tile[roomX * roomWidth, roomY * roomHeight];
    }

    private async void Start()
    {
        await Task.Delay(10);
        CreateLevel();
    }

    private void CreateLevel()
    {
        if (roomX <= 1 && roomY <= 1)
        {
            throw new System.Exception("1 level can co 2 phong");
        }
        //Tao phong co duong di tu entrace den exit.
        CreatePathRooms();

        //Tao cac phong con lai lap day o trong
        CreateRemainRooms();
        //Dua vao xac xuat de tao cac vat trong phong
        InitTile();

        //Ve them hoa tiet
        SetUpTile();

        //Tao dia diem cho entrance va exit
        PlaceEntranceandExit();

        //tao background
        createBackGround.CreateBackGround(roomX * roomWidth * Tile.Width, roomY * roomHeight * Tile.Height);

        //Tao vien xung quanh
        createBounds.CreateBounds(Tile.Width * roomWidth * roomX, Tile.Height * roomHeight * roomY);

        //Player xuat hien ben entrance
        Instantiate(player, entrance.transform.position + new Vector3(8, 8, 0), Quaternion.identity);
        //FindObjectOfType<GameManagement>().SpawnPlayer(entrance.transform.position);
    }

    private void InitTile()
    {
        Tile[] tmp = FindObjectsOfType<Tile>();
        foreach (Tile tile in tmp)
        {
            if (tile.spawnProbability <= Random.Range(0, 100))
            {
                //Destroy(tile.gameObject);
                tile.gameObject.SetActive(false);
                continue;
            }
            int x = ((int)tile.transform.position.x) / Tile.Width;
            int y = ((int)tile.transform.position.y) / Tile.Height;
            tile.InitTile(x, y);
        }
    }
    private void SetUpTile()
    {
        for (int x = 0; x < Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                if (Tiles[x, y] == null)
                {
                    continue;
                }
                Tiles[x, y].SetupTile();
            }
        }
    }

    private void PlaceEntranceandExit()
    {
        Tile spawnEntrance = firstRoom.GetEnstranceOrExit();
        entrance = Instantiate(groundPrefabs["Entrance"], spawnEntrance.transform.position + new Vector3(0, Tile.Height, 0), Quaternion.identity);
        Tile spawnExit = lastRoom.GetEnstranceOrExit();
        exit = Instantiate(groundPrefabs["Exit"], spawnExit.transform.position + new Vector3(0, Tile.Height, 0), Quaternion.identity);
    }
    private void CreatePathRooms()
    {
        //Xac dinh vi tri hien tai o tren cung hang dau la 1 trong 4 o
        Vector2 currentIndex = new Vector2(Random.Range(0, Rooms.GetLength(0)), Rooms.GetLength(1) - 1);
        transform.position = new Vector3(80, 64, 0);
        RandomDirection();
       
        firstRoom = null;
        lastRoom = null;

        bool stop = false;
        while (stop == false)
        {
            Vector2 indexCheck = new Vector2((int)currentIndex.x +(int)direction.x,(int) currentIndex.y + (int)direction.y);
            //O ngoai Bounds
            if (indexCheck.x < 0 || indexCheck.x >= Rooms.GetLength(0))
            {
                //lastDirection = direction;
                direction = Vector2.down;
            }
            //O vi tri cuoi cung
            else if (indexCheck.y < 0)
            {
                if (firstRoom == null)
                {
                    lastDirection = Vector2.zero;
                }
                direction = Vector2.zero;

                Room spawnRoom = FindRoom(currentIndex);
                Room placeRoom = PlaceRoom(spawnRoom, currentIndex);
                if (firstRoom == null)
                {
                    firstRoom = placeRoom;
                }
                currentIndex = indexCheck;
                stop = true;
                lastRoom = placeRoom;
            }
            else if (Rooms[(int)indexCheck.x, (int)indexCheck.y] == null)
            {
                if (firstRoom == null)
                {
                    lastDirection = Vector2.zero;
                }

                Room spawnRoom = FindRoom(currentIndex);
                if (spawnRoom != null)
                {
                    Room placeRoom = PlaceRoom(spawnRoom, currentIndex);
                    if (firstRoom == null)
                    {
                        firstRoom = placeRoom;
                    }
                    currentIndex = indexCheck;
                }
                RandomDirection();
            }
            else
            {
                Vector2 tmp = lastDirection;
                RandomDirection();
                lastDirection = tmp;
            }
        }
        lastDirection = Vector2.zero;
        direction = Vector2.zero;
    }

    //Sau khi co duong di cac phong ta can lap cho trong cac o con lai 
    private void CreateRemainRooms()
    {
        for (int x = 0; x < Rooms.GetLength(0); x++)
        {
            for (int y = 0; y < Rooms.GetLength(1); y++)
            {
                Vector2 currentIndex = new Vector2(x, y);
                if (Rooms[x, y] == null)
                {
                    Room spawnRoom = null;
                    if (Random.value < 0.1f)
                    {
                        spawnRoom = specialRoom[Random.Range(0, specialRoom.Length)];
                    }
                    else
                    {
                        spawnRoom = FindRoom(currentIndex);
                    }
                    PlaceRoom(spawnRoom, currentIndex);
                }

            }
        }
    }
    private Room PlaceRoom(Room spawnRoom, Vector2 currentIndex)
    {
        //if (direction == Vector2.down)
        //{
        //    SpriteRenderer spriteRenderer = Instantiate(bounds, CurrentPositon(currentIndex) + new Vector3(72, 0, 0), Quaternion.identity);
        //    spriteRenderer.size = new Vector2(112, 8);
        //    spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
        //}
        //else if (direction == Vector2.left)
        //{
        //    SpriteRenderer spriteRenderer = Instantiate(bounds, CurrentPositon(currentIndex) + new Vector3(0, 56, 0), Quaternion.identity);
        //    spriteRenderer.size = new Vector2(144, 8);
        //}
        //else if (direction == Vector2.right)
        //{
        //    SpriteRenderer spriteRenderer = Instantiate(bounds, CurrentPositon(currentIndex) + new Vector3(144, 56, 0), Quaternion.identity);
        //    spriteRenderer.size = new Vector2(144, 8);
        //}
        //Rooms[(int)currentIndex.x, (int)currentIndex.y] = Instantiate(spawnRoom,CurrentPositon(currentIndex), Quaternion.identity);
        Rooms[(int)currentIndex.x, (int)currentIndex.y] = ObjectPoolManager.instance.GetRoom(spawnRoom.name,CurrentPositon(currentIndex), Quaternion.identity);        
        Rooms[(int)currentIndex.x, (int)currentIndex.y].name = "Room [" + currentIndex.x + "," + currentIndex.y + "]";
        Rooms[(int)currentIndex.x, (int)currentIndex.y].index = currentIndex;
        Rooms[(int)currentIndex.x, (int)currentIndex.y].transform.parent = roomParent;
        return Rooms[(int)currentIndex.x, (int)currentIndex.y];
    }
    //Tim vi tri phong tiep theo de tao phong
    private Vector3 CurrentPositon(Vector2 currentIndex)
    {
        return new Vector3(currentIndex.x * roomWidth * Tile.Width, currentIndex.y * roomHeight * Tile.Height, 0);
    }
    private Room FindRoom(Vector2 currentIndex)
    {

        bool top = lastDirection == Vector2.down || direction == Vector2.up;
        bool right = lastDirection == Vector2.left || direction == Vector2.right;
        bool left = lastDirection == Vector2.right || direction == Vector2.left;
        bool down = direction == Vector2.down;
        //Neu vi tri hien tai o cuoi cung thi khong sinh phong duoi nua
        if (currentIndex.y == 0)
        {
            down = false;
        }
        //Kiem tra xem cac phong theo huong tiep theo co thoa man down left right rong hay khong 
        List<Room> listRoom = new List<Room>();
        foreach (Room room in normalRoom)
        {
            //lastdir.left=true
            //if (!(right && room.right && (left && room.left || down && room.down))||
            //    !(left && room.left && down && room.down)||!(top && room.top && (left && room.left || down && room.down || right && room.right)))
            //{
            //    continue;
            //}
            if (top && !room.top || down && !room.down || right && !room.right || left && !room.left)
            {
                continue;
            }
            listRoom.Add(room);
        }
        if (listRoom.Count > 0)
        {
            return listRoom[Random.Range(0, listRoom.Count)];
        }
        return null;
    }
    private void RandomDirection()
    {
        lastDirection = direction;
        if (Random.value < 0.8f)
        {
            if (Random.value < 0.5f)
            {
                direction = Vector2.right;
            }
            else
            {
                direction = Vector2.left;
            }
        }
        else
        {
            direction = Vector2.down;
        }      
    }
    public void RemoveTiles(Tile[] tilesToRemove)
    {
        int minX = int.MaxValue;
        int maxX = -1;
        int minY = int.MaxValue;
        int maxY = -1;
        foreach (Tile tile in tilesToRemove)
        {
            if (tile.x < minX)
            {
                minX = tile.x;
            }
            if (tile.x > maxX)
            {
                maxX = tile.x;
            }
            if (tile.y < minY)
            {
                minY = tile.y;
            }
            if (tile.y > maxY)
            {
                maxY = tile.y;
            }
            Tiles[tile.x, tile.y] = null;
            //print(1);
            tile.Remove();
        }

        minX--;
        maxX++;
        minY--;
        maxY++;

        if (minX < 0)
        {
            minX = 0;
        }
        if (maxX >= Tiles.GetLength(0))
        {
            maxX = Tiles.GetLength(0) - 1;
        }
        if (minY < 0)
        {
            minY = 0;
        }
        if (maxY >= Tiles.GetLength(1))
        {
            maxY = Tiles.GetLength(1) - 1;
        }

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // No tile.
                if (Tiles[x, y] == null)
                {
                    continue;
                }

                Tiles[x, y].SetupTile();
            }
        }
    }
}