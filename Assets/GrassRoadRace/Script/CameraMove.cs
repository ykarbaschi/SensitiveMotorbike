
using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class CameraMove : MonoBehaviour {

	private Vector3 initPosition;
	private Quaternion initRotation;
	public float moveSpeed;
	public GameObject mainCamera;
	public GameObject model;
	public GameObject self;
//	private SerialPort stream;
	private string[] sensors;
	private int flex;
	private int pressure;
	private float target_v = 0f;
	private float current_v = 0f;
	private float reduce_v_step = 0.1f;
	private Thread helperThread;
	float IncVolume = 0.6f;
	float DecVolume = 0.3f;
	float VolumeRate = 0.2f;
	float pitchRate = 0.4f;
	private int grad = 0;

	// Use this for initialization
	void Start () {
		
		initPosition = new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z);
		initRotation = new Quaternion(self.transform.rotation.x, self.transform.rotation.y, self.transform.rotation.z, self.transform.rotation.w);
//		mainCamera.transform.localPosition = new Vector3 ( 0, 0, 0 );
//		mainCamera.transform.localRotation = Quaternion.Euler (18, 180, 0);
//		stream = new SerialPort("/dev/tty.usbmodem1411", 9600);
//		stream.Open ();
//		StartCoroutine ("readFromArduino");

		helperThread = new Thread(DoInBackground)
		{
			IsBackground = true,
			Name = "HelperThread",
		};
		helperThread.Start();
	}

	void DoInBackground()
	{
		SerialPort serial = new SerialPort("/dev/tty.usbmodem1411", 9600);
		serial.ReadTimeout = 500;
		serial.Open ();

		while (true)
		{
			try
			{
//				Debug.Log("Test");
				string line = serial.ReadLine ();
//				Debug.Log (line);
				sensors = line.Split (new char[] { ' ' });
				if (sensors.Length == 2) {
					if (int.TryParse (sensors [1], out pressure) && int.TryParse (sensors [0], out flex)) {
						if (pressure > 10) {
							grad = 1;
						}
						else {
							grad = -1;
						}
						if (pressure / 40.0f > target_v) {
							target_v = pressure / 40.0f;
						} else if (pressure < 10) {
							target_v = 0f;
						}
						if (flex > 400) {
							flex = 400;
						} else if (flex < 200) {
							flex = 200;
						}
						flex = 400 - flex;

						if (flex < 50) {
							reduce_v_step = 0.1f;
						} else if ((flex > 50) && (flex < 150)) {
							target_v = flex / 200f;
							reduce_v_step = 0.5f;
							grad = -1;
						} else {
							target_v = 0;
							reduce_v_step = 1f;
							grad = -1;
						}
					}
				}
			}
			catch
			{
				//Most probably timeout exception
				Thread.Sleep(1);
			}
		}
	}


//	IEnumerator readFromArduino () {
//		while (true) {
//			string line = stream.ReadLine ();
//			Debug.Log (line);
//			sensors = line.Split (new char[] { ' ' });
//			if (sensors.Length == 2) {
//				if (int.TryParse (sensors [1], out pressure) && int.TryParse (sensors [0], out flex)) {
//					if (pressure / 50.0f > target_v) {
//						target_v = pressure / 50.0f;
//					} else if (pressure < 10) {
//						target_v = 0f;
//					}
//					if (flex > 400) {
//						flex = 400;
//					} else if (flex < 200) {
//						flex = 200;
//					}
//					flex = 400 - flex;
//
//					if (flex < 50) {
//						reduce_v_step = 0.1f;
//					} else if ((flex > 50) && (flex < 150)) {
//						target_v = flex / 200f;
//						reduce_v_step = 0.5f;
//					} else {
//						target_v = 0;
//						reduce_v_step = 1f;
//					}
//				}
//			}
////			yield return new WaitForSeconds (0.25f);
////			yield return new WaitForSeconds (1);
//			yield return null;
//		}
//	}

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		MoveObj ();
	}
	
	
	void MoveObj() {		
		if (current_v < target_v) {
			current_v += 0.2f;
		}
		float moveAmount = Time.smoothDeltaTime * (moveSpeed - current_v);

//		transform.Translate ( 0f, 0f, moveAmount );
//		Debug.Log(model.transform.rotation);
//		Debug.Log(moveAmount);
		if (moveAmount < 0) {
			transform.Translate ( -1f * Mathf.Sin(model.transform.localRotation.z), 0f, moveAmount );
		}
		if (current_v > target_v) {
			current_v -= reduce_v_step;
		}
		if (current_v < 0f) {
			current_v = 0f;
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
		if (self.transform.position.y < -1) {
			//TODO: Pause and then restart
			self.transform.position = initPosition;
			self.transform.rotation = initRotation;
			target_v = 0;
			current_v = 0;
			grad = 0;
			reduce_v_step = 0.1f;
		}
	}
}