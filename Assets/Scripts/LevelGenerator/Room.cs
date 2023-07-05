using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
                                
public class Room : MonoBehaviour
{
    public Vector2 index;

    public bool top, down, right, left;
    public Tile[] GetRoomTiles()
    {
        List<Tile> roomTiles = new List<Tile>();
        for(int i = (int)index.x * LevelGeneration.roomWidth; i < (int)(index.x + 1) * LevelGeneration.roomWidth; i++)
        {
            for(int j=(int)index.y*LevelGeneration.roomHeight;j < (int)(index.y + 1) * LevelGeneration.roomHeight; j++)
            {
                if (LevelGeneration.instance.Tiles[i, j] == null)
                {
                    continue;
                }
                roomTiles.Add(LevelGeneration.instance.Tiles[i, j]);
            }
        }
        return roomTiles.ToArray();
    }

    public Tile GetEnstranceOrExit()
    {
        Tile[] romTiles = GetRoomTiles();
        List<Tile> listTile = new List<Tile>();
        foreach(Tile tile in romTiles)
        {
            if (tile.name.Contains("Dirt") == false)
            {
                continue;
            }
            int yPosition = tile.y + 1;
            int roomYPosition = (int)(index.y + 1) * LevelGeneration.roomHeight - 1;
            if (yPosition < roomYPosition && yPosition < LevelGeneration.instance.Tiles.GetLength(1) - 1 && LevelGeneration.instance.Tiles[tile.x, yPosition] == null)
            {
                listTile.Add(tile);
            }
        }
        return listTile.Count > 0 ? listTile[Random.Range(0, listTile.Count)] : null;
    }
}
