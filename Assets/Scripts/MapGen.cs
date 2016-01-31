using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {

    public GameObject basicTile;

    private int mapWidth = 25, mapHeight = 15;
    private int roomSizeMin = 5, roomSizeMax = 10;

    private GameObject map;

    private int roomWidth, roomHeight, roomX, roomY;

    private List<Vector2> floor = new List<Vector2>();
    private List<Connection> possibleConnections = new List<Connection>();
    private List<Vector2> connections = new List<Vector2>();
    private List<Connection> pathEnds = new List<Connection>();

    private int roomCount = 0, desiredRoomCount = 25;

    struct Connection {
        public Vector2 pos;
        public Vector2 dir;

        public Connection(Vector2 _pos, Vector2 _dir) {
            pos = _pos;
            dir = _dir;
        }
    }

	// Use this for initialization
	void Start () {

        map = new GameObject();
        map.name = "Map";

        map.AddComponent<Map>();
        map.GetComponent<Map>().basic_tile = basicTile;

        CreateMap();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CreateMap();
        }
	}

    public void CreateMap() {

        //resets
        roomCount = 0;
        floor.Clear();
        possibleConnections.Clear();
        connections.Clear();
        pathEnds.Clear();
        

        roomWidth = Random.Range(roomSizeMin, roomSizeMax);
        roomHeight = Random.Range(roomSizeMin, roomSizeMax);

        //place first room
        roomX = 0;
        roomY = 0;

        AddCurrentRoom();
        roomCount++;

        bool addNewRoom = true;

        while (addNewRoom) {
            roomWidth = Random.Range(roomSizeMin, roomSizeMax);
            roomHeight = Random.Range(roomSizeMin, roomSizeMax);

            int rand = Random.Range(0, possibleConnections.Count);

            Connection c;

            List<int> rands = new List<int>();
            for (int i = 0; i < 5; i++) {
                rands.Add(Random.Range(0, possibleConnections.Count));
            }
            c = possibleConnections[rands[0]];
            rand = rands[0];
            for (int i = 1; i < 5; i++) {
                if (Vector2.Distance(Vector2.zero, c.pos) > Vector2.Distance(Vector2.zero, possibleConnections[rands[i]].pos)) {
                    c = possibleConnections[rands[i]];
                    rand = rands[i];
                }
            }

            roomX = (int)(possibleConnections[rand].pos.x);
            roomY = (int)(possibleConnections[rand].pos.y);

            RemoveAdjacentConnections(c, rand);

            if (!floor.Contains(c.pos + c.dir)) {
                float chance = Random.Range(0.0f, 1.0f);
                if (chance <= 0.2f) {
                    //attempt path spawn
                    roomX = (int)(c.pos.x);
                    roomY = (int)(c.pos.y);

                    int pathLength = (c.dir.x == 0) ? roomWidth : roomHeight;

                    bool pTest = true;

                    for (int i = 0; i < pathLength; i++) {
                        if (floor.Contains(new Vector2(roomX, roomY) + c.dir * i)){
                            pTest = false;
                        }
                    }
                    if (pTest) {
                        for (int i = 0; i < pathLength; i++) {
                            floor.Add(new Vector2(roomX, roomY) + c.dir * i);
                        }
                        
                        //if end of path isn't already floor add it to possible connections
                        if (!floor.Contains(new Vector2(roomX, roomY) + c.dir * pathLength)) {
                            possibleConnections.Add(new Connection(new Vector2(roomX, roomY) + c.dir * pathLength, c.dir));
                            pathEnds.Add(new Connection(new Vector2(roomX, roomY) + c.dir * pathLength, c.dir));
                        }

                        //add the end of the path to a list of path ends
                        //when ever a new room OR path is placed check against this list and remove if it is there

                        //the list is all connections[so they have a pos AND dir]
                        //once the map is done placing rooms/paths
                        //loop through path ends and see if they have the possiblity of connecting to a area when extended
                        //if yes extend path *

                        pathEnds.Remove(c);

                        possibleConnections.Remove(c);
                    }
                }
                else {
                    //attempt room spawn
                    int rSize = (c.dir.x == 0) ? roomWidth : roomHeight;
                    int rand_offset = Random.Range(0, rSize);

                    roomX = (int)((c.pos + c.dir).x);
                    roomY = (int)((c.pos + c.dir).y);

                    if (c.dir.y == -1.0f) {
                        roomX -= rand_offset;
                        roomY += (int)((roomHeight - 1) * c.dir.y);
                    }
                    else if (c.dir.x == -1.0f) {
                        roomY -= rand_offset;
                        roomX += (int)((roomWidth - 1) * c.dir.x);
                    }

                    if (AddCurrentRoom()) {
                        RemoveAdjacentConnections(c, rand);
                        //remove adjacent connections and add door to list
                        connections.Add(c.pos);
                        possibleConnections.Remove(c);
                        pathEnds.Remove(c);

                        roomCount++;
                        if (roomCount >= desiredRoomCount) {
                            addNewRoom = false;
                        }
                    }
                    else {
                        Debug.Log("New room was almost added above a previous room!");
                    }
                }
            }
        }

        

        foreach (Connection vec in pathEnds) {
            //remove any path ends from possible connections
            possibleConnections.Remove(vec);
        }

        ExtendPaths();

        foreach (Connection vec in possibleConnections) {
            //GameObject ob = GameObject.Instantiate(basicTile, vec.pos, Quaternion.identity) as GameObject;
            //ob.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        foreach (Vector2 vec in connections) {
            //GameObject ob = GameObject.Instantiate(basicTile, vec, Quaternion.identity) as GameObject;
            //ob.GetComponent<SpriteRenderer>().color = Color.blue;

            floor.Add(vec);
        }

        map.GetComponent<Map>().SetMap(floor);

        foreach (Vector2 vec in floor) {

            //GameObject ob = GameObject.Instantiate(basicTile, vec, Quaternion.identity) as GameObject;
            //ob.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public void SetMapSize(int _width, int _height){
        mapWidth = _width;
        mapHeight = _height;
    }

    public bool AddCurrentRoom() {
        for (int x = roomX; x < roomX + roomWidth; x++) {
            for (int y = roomY; y < roomY + roomHeight; y++) {
               if (floor.Contains(new Vector2(x, y))){
                   return false;
               }
            }
        }
        for (int x = roomX; x < roomX + roomWidth; x++) {
            for (int y = roomY; y < roomY + roomHeight; y++) {
                floor.Add(new Vector2(x, y));
            }
        }
        for (int x = roomX; x < roomX + roomWidth; x++) {
            possibleConnections.Add(new Connection(new Vector2(x, roomY - 1), new Vector2(0, -1)));
            possibleConnections.Add(new Connection(new Vector2(x, roomY + roomHeight), new Vector2(0, 1)));
        }
        for (int y = roomY; y < roomY + roomHeight; y++) {
            possibleConnections.Add(new Connection(new Vector2(roomX - 1, y), new Vector2(-1, 0)));
            possibleConnections.Add(new Connection(new Vector2(roomX + roomWidth, y), new Vector2(1, 0)));
        }

        return true;
    }

    private void RemoveAdjacentConnections(Connection _c, int _rand) {

        for (int i = possibleConnections.Count - 1; i >= 0; i--) {
            if (i != _rand) {
                if (_c.dir.x == 0) {
                    if (possibleConnections[i].pos.y == _c.pos.y) {
                        possibleConnections.RemoveAt(i);
                    }
                }
                else {
                    if (possibleConnections[i].pos.x == _c.pos.x) {
                        possibleConnections.RemoveAt(i);
                    }
                }
            }
        }
    }

    private void ExtendPaths() {
        foreach (Connection vec in pathEnds) {

            Vector2 p = vec.pos;

            int neededPath = 0;

            bool test = false;

            for (int i = 0; i < 10; i++) {
                p = vec.pos + vec.dir * i;
                if (floor.Contains(p) || possibleConnections.Contains(new Connection(p, vec.dir))) {
                    test = true;
                    neededPath = i;
                    break;
                }
            }
            if (test) {
                for (int i = 0; i < neededPath; i++) {
                    p = vec.pos + vec.dir * i;
                    floor.Add(p);

                    //GameObject ob = GameObject.Instantiate(basicTile, p, Quaternion.identity) as GameObject;
                    //ob.GetComponent<SpriteRenderer>().color = Color.yellow;
                    //ob.transform.Translate(0, 0, -5);
                }
            }
            else {
                //display dead ends
                //GameObject ob = GameObject.Instantiate(basicTile, vec.pos, Quaternion.identity) as GameObject;
                //ob.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
