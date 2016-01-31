using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour{

    public GameObject basic_tile;

    private List<Sprite> basic_tiles;

    private Vector2 size;

    private GameObject[,] tiles;

    private const float SCALE_X = 2.0f, SCALE_Y = 2.0f;

    private bool full = false;

    void Start(){
        /*
        Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

        for (int i = 0; i < sprites.Length; i++) {
            if (sprites[i].name == "basic_tile01") {
                basic_tiles.Add(sprites[i]);
            }
            else if (sprites[i].name == "basic_tile02") {
                basic_tiles.Add(sprites[i]);
            }
        }*/


        //basic_tiles.Add(Resources.Load<Sprite>("basic_tile01"));
        //basic_tiles.Add(Resources.Load<Sprite>("basic_tile02"));
    }

    public void SetMap(List<Vector2> _tiles) {

        if (full) {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    GameObject.Destroy(tiles[x, y]);
                }
            }
        }
        
        float distXneg = 0.0f, distXpos = 0.0f;
        float distYneg = 0.0f, distYpos = 0.0f;

        for (int i = 0; i < _tiles.Count; i++) {
            for (int j = _tiles.Count-1; j >= 0; j--) {
                float nu_distX = _tiles[i].x - _tiles[j].x;
                float nu_distY = _tiles[i].y - _tiles[j].y;

                if (nu_distX < distXneg) { distXneg = nu_distX; }
                if (nu_distX > distXpos) { distXpos = nu_distX; }
                if (nu_distY < distYneg) { distYneg = nu_distY; }
                if (nu_distY > distYpos) { distYpos = nu_distY; }
            }
        }

        size = new Vector2(Mathf.Abs(distXneg) + distXpos, Mathf.Abs(distYneg) + distYpos);

        tiles = new GameObject[(int)(size.x),(int)(size.y)];
        
        int offX = (int)(Mathf.Abs(distXneg));
        int offY = (int)(Mathf.Abs(distYneg));

        Debug.Log("sizeX: " + size.x + " sizeY: " + size.y);

        for (int i = 0; i < _tiles.Count; i++) {
            tiles[(int)(_tiles[i].x + offX), (int)(_tiles[i].y + offY)] = GameObject.Instantiate(basic_tile, _tiles[i], Quaternion.identity) as GameObject;
            tiles[(int)(_tiles[i].x + offX), (int)(_tiles[i].y + offY)].transform.parent = this.transform;

            Tile t = tiles[(int)(_tiles[i].x + offX), (int)(_tiles[i].y + offY)].GetComponent<Tile>();
            t.SetGridPos((int)(_tiles[i].x + offX), (int)(_tiles[i].y + offY));

            int rand = Random.Range(1, 7); //6 tiles currently

            t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("basic_tile0" + rand);

            Vector3 nu_pos = t.gameObject.transform.position;
            nu_pos.x *= SCALE_X * t.GetComponent<SpriteRenderer>().sprite.texture.width / 100.0f;
            nu_pos.y *= SCALE_Y * t.GetComponent<SpriteRenderer>().sprite.texture.height / 100.0f;
            t.gameObject.transform.position = nu_pos;

            Vector3 nu_scale = t.gameObject.transform.localScale;
            nu_scale.x = SCALE_X;
            nu_scale.y = SCALE_Y;
            t.gameObject.transform.localScale = nu_scale;

            t.isWall = false;

            //wall needs to be scaled slightyly more then floor tiles
            //but they need to be at the same x as the floor
            //y position of walls will need to be tweaked to look correct
        }
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                if (tiles[x, y] != null) {
                    if (tiles[x, y].GetComponent<Tile>().isWall == false) {
                        if (tiles[x + 1, y] == null) {
                            SpawnWall(x + 1, y, offX, offY);
                        }
                        if (tiles[x - 1, y] == null) {
                            SpawnWall(x - 1, y, offX, offY);
                        }
                        if (tiles[x, y + 1] == null) {
                            SpawnWall(x, y + 1, offX, offY);
                        }
                        if (tiles[x, y - 1] == null) {
                            SpawnWall(x, y - 1, offX, offY);
                        }
                    }
                }
            }
        }
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                if (tiles[x, y] != null) {
                    if (tiles[x, y].GetComponent<Tile>().isWall == true && tiles[x, y].GetComponent<Tile>().empty == false) {
                        if (tiles[x + 1, y] == null) {
                            SpawnEmptyWall(x + 1, y, offX, offY);
                        }
                        if (tiles[x - 1, y] == null) {
                            SpawnEmptyWall(x - 1, y, offX, offY);
                        }
                        if (tiles[x, y + 1] == null) {
                            SpawnEmptyWall(x, y + 1, offX, offY);
                        }
                        if (tiles[x, y - 1] == null) {
                            SpawnEmptyWall(x, y - 1, offX, offY);
                        }
                    }
                }
            }
        }

        full = true;
    }

    public int Width() {
        return (int)(size.x);
    }
    public int Height() {
        return (int)(size.y);
    }

    public GameObject GetTile(int _x, int _y) {
        if (_x >= 0 && _x < size.x && _y >= 0 && _y < size.y) {
            return tiles[_x,_y];
        }
        Debug.Log("Tile does not exist, null returned.");
        return null;
    }

    public void DestoryMap() {
        if (full) {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    GameObject.Destroy(tiles[x, y]);
                }
            }
        }
    }
    //returns true if grid index is touching any tile that isn't null
    public int NearTile(int _x, int _y) {
        bool up = false, down = false, left = false, right = false;
        if (tiles[_x + 1, _y] != null) { if (!tiles[_x + 1, _y].GetComponent<Tile>().isWall){ right = true;} }
        if (tiles[_x - 1, _y] != null) { if (!tiles[_x - 1, _y].GetComponent<Tile>().isWall) { left = true; } }
        if (tiles[_x, _y + 1] != null) { if (!tiles[_x, _y + 1].GetComponent<Tile>().isWall) { up = true; } }
        if (tiles[_x, _y - 1] != null) { if (!tiles[_x, _y - 1].GetComponent<Tile>().isWall) { down = true; } }

        if (up && !down && !left && !right) {
            return 1;
        }
        else if (!up && right && !down && !left) {
            return 2;
        }
        else if (!up && !right && down && !left) {
            return 3;
        }
        else if (!up && !right && !down && left) {
            return 4;
        }
        else if (up && right && !down && left) {
            return 5;
        }
        else if (up && right && down && !left) {
            return 6;
        }
        else if (!up && right && down && left) {
            return 7;
        }
        else if (up && !right && down && left) {
            return 8;
        }
        else if (up && right && !down && !left) {
            return 9;
        }
        else if (!up && right && down && !left) {
            return 10;
        }
        else if (!up && !right && down && left) {
            return 11;
        }
        else if (up && !right && !down && left) {
            return 12;
        }
        else if (!up && right && !down && left) {
            return 14;
        }
        else if (up && !right && down && !left) {
            return 15;
        }
        else if (up && right && down && left) {
            return 13;
        }

        return 13; //pillar
    }
    public void SpawnWall(int x, int y, int offX, int offY) {
        tiles[x, y] = GameObject.Instantiate(basic_tile, new Vector3(x - offX, y - offY, -1.0f), Quaternion.identity) as GameObject;
        tiles[x, y].transform.parent = this.transform;

        int wallT = NearTile(x, y);

        tiles[x, y].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("wall" + wallT);

        Vector3 nu_pos = tiles[x, y].transform.position;
        nu_pos.x *= SCALE_X * 1.28f;
        nu_pos.y *= SCALE_Y * 1.28f;
        tiles[x, y].transform.position = nu_pos;

        Vector3 nu_scale = tiles[x, y].transform.localScale;
        nu_scale.x = SCALE_X;
        nu_scale.y = SCALE_Y;
        tiles[x, y].gameObject.transform.localScale = nu_scale;

        tiles[x, y].GetComponent<Tile>().SetGridPos(x, y);

        if (wallT == 3 || wallT == 6 || wallT == 7 || wallT == 8 || wallT == 10 || wallT == 11 || wallT == 13 || wallT == 15) {
            //spawn flat wall
            GameObject flatWall = GameObject.Instantiate(basic_tile, Vector2.zero, Quaternion.identity) as GameObject;
            flatWall.transform.parent = tiles[x, y].transform;
            flatWall.transform.localPosition = Vector2.zero;
            flatWall.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            flatWall.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flat_wall2");

            flatWall.transform.Translate(0.0f, -1.12f * SCALE_Y, 0.0f);
        }

        tiles[x, y].transform.Translate(0.0f, 1.28f * 1.2f, 0.0f);
    }
    public void SpawnEmptyWall(int x, int y, int offX, int offY) {
        tiles[x, y] = GameObject.Instantiate(basic_tile, new Vector2(x - offX, y - offY), Quaternion.identity) as GameObject;
        tiles[x, y].transform.parent = this.transform;

        tiles[x, y].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty_wall");

        Vector3 nu_pos = tiles[x, y].transform.position;
        nu_pos.x *= SCALE_X * tiles[x, y].GetComponent<SpriteRenderer>().sprite.texture.width / 100.0f;
        nu_pos.y *= SCALE_Y * tiles[x, y].GetComponent<SpriteRenderer>().sprite.texture.height / 100.0f;
        tiles[x, y].transform.position = nu_pos;

        Vector3 nu_scale = tiles[x, y].transform.localScale;
        nu_scale.x = SCALE_X;
        nu_scale.y = SCALE_Y;
        tiles[x, y].gameObject.transform.localScale = nu_scale;

        tiles[x, y].GetComponent<Tile>().SetGridPos(x, y);

        tiles[x, y].GetComponent<Tile>().empty = true;
    }
}
