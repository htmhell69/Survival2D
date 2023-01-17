using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class WorldTile
{
    [SerializeField] Vector2Int position;
    [SerializeField] NoiseValue noiseValues;
    [SerializeField] TilePlacement tile;
    [SerializeField] Placement detail;
    [SerializeField] Structure structure;
    bool placed;
    PlacementInstance detailInstance;
    TilePlacementInstance tileInstance;

    public WorldTile(Vector2Int position, NoiseValue noiseValues, TilePlacement tile)
    {
        this.position = position;
        this.noiseValues = noiseValues;
        this.tile = tile;
        tile.SelectDetail(position, out detail, noiseValues);
        this.structure = null;
    }

    public bool Traversable(Entity entity)
    {
        if (tile != null && !tile.IsTraversable(entity))
        {
            return false;
        }
        if (detail != null && !detail.IsTraversable(entity))
        {
            return false;
        }
        return true;
    }
    public void Place(WorldInstance world)
    {
        if (!placed)
        {
            placed = true;
        }
        if (tile != null)
        {
            tileInstance = tile.Place(world, position, noiseValues);
        }
        if (detail != null)
        {
            detailInstance = detail.Place(world, position);
        }
    }

    public void Destroy(WorldInstance world)
    {
        if (placed)
        {
            if (tileInstance != null)
            {
                tileInstance.Destroy(world);
            }

            if (detailInstance != null)
            {
                detailInstance.Destroy(world);
            }
        }
    }

    public void Interact(Entity entity)
    {
        if (placed)
        {
            if (tileInstance != null)
            {
                tileInstance.Interact(entity);
            }

            if (detailInstance != null)
            {
                detailInstance.Interact(entity);
            }
        }
    }

    public bool AddTransition(Vector3Int adjacentPosition, TilePlacementInstance tile, WorldInstance world)
    {
        if (tileInstance != null)
        {
            GenTile adjacentGenTile = tile.GetTile();
            GenTile genTile = tileInstance.GetTile();
            if (genTile.baseTile != adjacentGenTile)
            {
                if (genTile.priority > adjacentGenTile.priority)
                {
                    tile.AddTransition(new Vector3Int(position.x, position.y, 0), world);
                }
                else if (genTile.priority < adjacentGenTile.priority)
                {
                    tileInstance.AddTransition(adjacentPosition, world);
                }
                return true;
            }
            return false;
        }

        return false;
    }
    public Vector2Int GetPosition()
    {
        return position;
    }

    public TilePlacementInstance GetTile()
    {
        return tileInstance;
    }
}
