using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{

    //this fields are related to the map generation method, which is excluded*************************************************
    [SerializeField] private Transform map;
    [SerializeField] private Texture2D[] mapData;
    [SerializeField] private MapElement[] mapElements;
    [SerializeField] private Sprite[] defaultTile;


    private Vector3 WorldStartPosition
    {
        get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }
    }

    //this fields are related to the map generation method, which is excluded*************************************************
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        //GenerateMap method is excluded because i'm using integrated tilemap
        GenerateMap();
    }


    private void GenerateMap()
    {
        for(int i = 0; i< mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y= 0; y < mapData[i].height; y++)
                {
                    //gets pixel color
                    Color c = mapData[i].GetPixel(x, y);
                    //searches for related tile color
                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);
                    //if find a match
                    if (newElement !=null)
                    {
                        float xPos = WorldStartPosition.x + (defaultTile.Length * x);
                        float yPos = WorldStartPosition.y + (defaultTile.Length * y);

                        //instantiating tile
                        GameObject go = Instantiate(newElement.MyElementPrefab);
                        //setting position
                        go.transform.position = new Vector2(xPos, yPos);
                        //sets map game object as parent of the tile
                        go.transform.parent = map;
                        Debug.Log(newElement.MyElementPrefab.name + "x; " + xPos + ", y;" + yPos);

                    }
                }
            }
        }
    }
}

[Serializable]
public class MapElement
{
    [SerializeField] private string tileTag;
    [SerializeField] private Color color;
    [SerializeField] private GameObject elementPrefab;

    public string MyTileTag 
    { 
        get { return tileTag; } 
    }
    public Color MyColor 
    { 
        get { return color; } 
    }
    public GameObject MyElementPrefab 
    { 
        get { return elementPrefab; } 
    }
}