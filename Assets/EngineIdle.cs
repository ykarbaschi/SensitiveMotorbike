using UnityEngine;
using System.Collections;

public class EngineIdle : MonoBehaviour
{

	    float IncVolume = 0.6f;
	    float DecVolume = 0.3f;
	    float VolumeRate = 0.2f;
	float pitchRate = 0.4f;
	    private int grad = 0;

	    void Update()
	    {
		        if (Input.GetKeyDown(KeyCode.W))
			        {
			            grad = 1;
			        }
		        if (Input.GetKeyUp(KeyCode.W))
			        {
			            grad = -1;
			        }
		        if (grad != 0)
			        {
			            GetComponent<AudioSource>().enabled = true;
			GetComponent<AudioSource>().pitch += grad * pitchRate * Time.deltaTime;
			            GetComponent<AudioSource>().volume += grad * VolumeRate * Time.deltaTime;
			            if (GetComponent<AudioSource>().pitch < 1.0 || GetComponent<AudioSource>().pitch > 2.0)
				            {
				                if (GetComponent<AudioSource>().pitch < 1.0)
					                {
					                    GetComponent<AudioSource>().pitch = 1.0f;
					                }
				                else {
					                    GetComponent<AudioSource>().pitch = 2.0f;
					                }
				                grad = 0;
				            }
			            if (GetComponent<AudioSource>().volume < 0.6 || GetComponent<AudioSource>().volume > 1.0)
				            {
				                if (GetComponent<AudioSource>().volume < 0.6)
					                {
					                    GetComponent<AudioSource>().volume = 0.6f;
					                }
				                else {
					                    GetComponent<AudioSource>().volume = 1.0f;
					                }
				                grad = 0;
				            }

			        }
		    }
}