using UnityEngine;
using System.Collections.Generic;

public class InGamePanel : MonoBehaviour {

    public int linesOfText = 20;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //Logger.Log("why");
	}

    //void OnGUI()
    //{
    //    GUI.enabled = true;
    //    //Camera camera = Camera.current;
    //    //Vector3 cameraLocation = camera.WorldToScreenPoint(new Vector3(0, 5, 0));
    //    //GUI.Label(new Rect(cameraLocation.x, Screen.height - cameraLocation.y, 150, 150), "test");
    //    GUI.Label(new Rect(Screen.width/2, Screen.height/2, 150, 150), "test");
    //}

    public void UpdateLog(LinkedList<LogMessage> messages)
    {
        string logText = "";
        LinkedListNode<LogMessage> currentNode;
        if (messages.Count > 0)
        {
            currentNode = messages.Last;
        }
        else
        {
            return;
        }

        for (int i = 0; i < linesOfText && i < messages.Count; i++)
        {
            logText += currentNode.Value.ToString() + "\n";
            currentNode = currentNode.Previous;
        }
        //Canvas bla = GetComponent<Canvas>();
        //UnityEngine.UI.Text ugh = GetComponentInChildren<UnityEngine.UI.Text>();
        //Component[] bla = GetComponents(typeof(Component));
        //foreach (Component blas in bla)
        //{
        //    Debug.Log("why" + blas);
        //}
        
        GetComponentInChildren<UnityEngine.UI.Text>().text = logText;
    }
}
