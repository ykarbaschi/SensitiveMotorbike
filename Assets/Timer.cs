using UnityEngine;
using System.Collections;


public class Timer : MonoBehaviour
{
	    private float startTime;
	    private string ellapsedTime;
	    void Awake()
	    {
		        startTime = Time.time;
		    }
	    // Update is called once per frame
	    void Update()
	    {
		    }
	    //void OnTriggerEnter(){
	    //  GUI.Label(new Rect(10, 10, 100, 20), (startTime));
	    //  float startTime = Time.time;
	    //  
	    //  guiText.text = ""+startTime;
	    //}
	    void OnGUI()
	    {
		        ellapsedTime = (Time.time - startTime).ToString(".0");
		        GUI.Label(new Rect(100, 100, 200, 120), ellapsedTime);
		    }

}