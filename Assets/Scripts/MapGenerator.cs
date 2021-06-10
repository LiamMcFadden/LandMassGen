using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh };
    public DrawMode drawMode;

    public int mapWidth;  // width of map
    public int mapHeight;  // height of map
    public float noiseScale;  // distance to view noise map
    public int octaves;  // levels of detail 
    [Range(0,1)]
    public float persistance;  // determines how much each octave contributes to the overall shape (amplitude)
    public float lacunarity;  // determines detail of each octave (frequency)

    public bool autoUpdate;  

    public TerrainType[] regions;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x,y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay> ();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));

    }

    void OnValidate() {
        if (mapWidth < 1) 
            mapWidth = 1;
        if (mapHeight < 1) 
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}