using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Node
{
	private Node previous=null;
	private string id;
	public Node(string id)
	{
		this.id = id;
	}
	public void setPrevious(Node n)
	{
		previous = n;
	}
	public string getId()
	{
		return id;
	}
	public bool hasPrevious()
	{
		return previous != null;
	}
	public Node getPrevious()
	{
		return previous;
	}
}
public class Control : MonoBehaviour {
	public List<string>rooms;
	public GameObject A;
	public GameObject B;
	List<string> unvisited;
	List<Node> path;
	List<string> solution;
	public GameObject school;
	// Use this for initialization
	void Start () {





		rooms = new List<string> ();
		foreach (Transform t in school.transform)
			rooms.Add (t.gameObject.name);
		foreach (string n in rooms) {
			GameObject.Find (n + "").GetComponent<LineRenderer> ().enabled = false;
		}
		unvisited = new List<string> ();
		foreach (string n in rooms) {
			unvisited.Add (n);
			GameObject.Find (n + "").GetComponent<Room> ().tDistance = 1000000;
			GameObject.Find (n + "").GetComponent<Room> ().visited=false;
		}
		path = new List<Node> ();

	}
	
	void OnDrawGizmosSelected()
	{
		for(int i=0; i<rooms.Count; i++)
		{
			
			for (int j = 0; i < GameObject.Find(rooms[i]).GetComponent<Room>().neighbors.Length; i++) {
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (GameObject.Find(rooms[i]).transform.position,GameObject.Find(GameObject.Find(rooms[i]).GetComponent<Room>().neighbors[j]).transform.position);
			}
		}
	}
	void Update () {
		
			
		
	}

	public void OnClick()
	{
        try
        {
            PathAB(A.GetComponent<InputField>().text.ToUpper(), B.GetComponent<InputField>().text.ToUpper());

        }
        catch
        {
            Debug.LogError("Text is wrong");
        }

    }

	bool isNeighbor(string id, string[]neighbors)
	{
		foreach (string s in neighbors)
			if (s == id)
				return true;
		return false;
	}
	void PathAB(string a, string b)
	{	
		foreach (string n in rooms) {
			unvisited.Add (n);
			GameObject.Find (n + "").GetComponent<Room> ().tDistance = 1000000;
			GameObject.Find (n + "").GetComponent<Room> ().visited=false;
		}
		foreach (string n in rooms) {
			GameObject.Find (n + "").GetComponent<LineRenderer> ().enabled = false;
		}
		path = new List<Node> ();
		string currentNode = a;
		Node startNode = new Node (a);
		string destinationNode = b;
		bool cont = true;
		solution = new List<string> ();
		GameObject.Find(a).GetComponent<Room>().tDistance=0;
		float tentativeDistance=0;
		path.Add (startNode);

		while (GameObject.Find (destinationNode + "").GetComponent<Room> ().visited == false && cont == true) {
			//Debug.Log (currentNode);
			tentativeDistance = GameObject.Find (currentNode + "").GetComponent<Room> ().tDistance;
		
			string[] neighbors = GameObject.Find (currentNode + "").GetComponent<Room> ().neighbors;
			for (int i =0; i<neighbors.Length; i++) {
				//Debug.Log (GameObject.Find (currentNode + "").GetComponent<Room> ().distances[0]);
				float prevTDist = GameObject.Find (neighbors [i] + "").GetComponent<Room> ().tDistance;
				GameObject.Find(neighbors[i]+"").GetComponent<Room>().tDistance=(Mathf.Min(tentativeDistance+GameObject.Find (currentNode + "").GetComponent<Room> ().distances[i],GameObject.Find(neighbors[i]+"").GetComponent<Room>().tDistance));
				//Debug.Log ("check");

				if (prevTDist != GameObject.Find (neighbors [i] + "").GetComponent<Room> ().tDistance) {
					Node nextNode = new Node (neighbors [i]);
					nextNode.setPrevious (startNode);
					path.Add (nextNode);
				}

			}
			GameObject.Find (currentNode + "").GetComponent<Room> ().visited=true;

			for (int z = 0; z < unvisited.Count; z++) {
				if (unvisited [z] == currentNode) {
					unvisited.Remove (unvisited [z]);
					break;
				}
			}
			int index = 0;
			float min = 1000000;
			for (int j = 0; j < unvisited.Count; j++)
					if (GameObject.Find (unvisited [j] + "").GetComponent<Room> ().tDistance < min ) {
					min = GameObject.Find (unvisited [j] + "").GetComponent<Room> ().tDistance;
					index = j;
					//Debug.Log("visit "+currentNode+" to "+unvisited [j]);
				}
			if (min == 1000000 && unvisited.Count != 0) {
				cont = false;
				GameObject.Find (b + "").GetComponent<Room> ().tDistance = 1000000;
			}
			if (unvisited.Count > 0) {
				currentNode = unvisited [index];
				foreach (Node n in path)
					if (n.getId() == unvisited [index])
						startNode = n;
			}

		}
		//if(GameObject.Find (b + "").GetComponent<Room> ().tDistance!=1000000)
			//Debug.Log (GameObject.Find (b + "").GetComponent<Room> ().tDistance+" solution");
		int findex = 0;
		for (int i = 0; i < path.Count; i++)
			if (path [i].getId() == b) {
				findex = i;
				break;
			}
		Node currentFNode = path [findex];
		Debug.Log ("point");
		solution.Add (currentFNode.getId());
		while(currentFNode.hasPrevious ()) {
			currentFNode = currentFNode.getPrevious ();
			solution.Add (currentFNode.getId ());

		} 

		for(int i=solution.Count-1; i>=1; i--) {
			Debug.Log (solution [i]);
			LineRenderer lineRenderer = GameObject.Find (solution[i] + "").GetComponent<LineRenderer>();
			Vector3[]positions=new Vector3[2];
			positions [0] = GameObject.Find (solution[i] + "").transform.position;
			positions[1]=GameObject.Find (solution[i-1] + "").transform.position;
			lineRenderer.enabled = true;
			lineRenderer.SetPositions (positions);

		}
	}
}
