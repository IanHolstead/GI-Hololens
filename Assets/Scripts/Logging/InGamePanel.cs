using UnityEngine;
using System.Collections.Generic;

public class InGamePanel : MonoBehaviour {

    public int linesOfText = 20;
    public bool showPannel = true;
    Canvas canvas;

    private float counter = 0;
	// Use this for initialization
	void Start ()
    {
        canvas = GetComponent<Canvas>();
        SetState(showPannel);
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        
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
            for (int j = 0; j < 3 - Mathf.Floor(Mathf.Log10(messages.Count - i) + 1); j++)
            {
                logText += " ";
            }
            logText += messages.Count - i +" " + currentNode.Value.ToString() + "\n";
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

    public void Enable()
    {
        SetState(true);
    }

    public void Disable()
    {
        SetState(false);
    }

    private void SetState(bool enabled)
    {
        showPannel = enabled;
        canvas.enabled = enabled;
        Logger.useInGameLogger = enabled;
    }
}
