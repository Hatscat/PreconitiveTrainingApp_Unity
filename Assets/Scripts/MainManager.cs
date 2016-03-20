using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour
{
	public const string FIREBASE_URL = "https://crackling-torch-3895.firebaseio.com"; // "https://crackling-torch-3895.firebaseio.com";

	public static MainManager singleton;
	public static IFirebase firebase_root;
	public static IFirebase firebase_users;
	public static string user_id;
	public static JSONObject user_data;

	public Transform loginCanvas, clientsCanvas;

	void Awake ()
	{
		if (singleton != null)
		{
			Destroy(singleton.gameObject);
		}
		singleton = this as MainManager;

		firebase_root = Firebase.CreateNew(FIREBASE_URL);
		firebase_users = firebase_root.Child("users");

		user_id = "";

		//print("firebase_users: " + firebase_users);
		//firebase_users.Child("bob123").SetValue("test!");
		//print("key: " + firebase_users.Key);
	}

	public void SetUserId (string _user_id)
	{
		user_id = _user_id;
		StartCoroutine(EnterTheApp());
	}

	IEnumerator EnterTheApp ()
	{
		yield return StartCoroutine(GetUserData());
		yield return StartCoroutine(ChangeCameraTarget(clientsCanvas, 1f));

	}

	IEnumerator GetUserData ()
	{
		WWW www = new WWW(FIREBASE_URL + "/users/" + user_id + ".json?auth=" + user_id);
		yield return www;

		Debug.Log(www.text);
		user_data = new JSONObject(www.text);
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(ChangeCameraTarget(clientsCanvas, 1f));
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(ChangeCameraTarget(loginCanvas, 1f));
		}
	}

	IEnumerator ChangeCameraTarget (Transform _target, float transition_duration)
	{
		Vector3 initial_position = Camera.main.transform.position;
		Vector3 final_position = _target.position - _target.forward * 350f;
		Quaternion initial_rotation = Camera.main.transform.rotation;
		Quaternion final_rotation = Quaternion.LookRotation(_target.position - final_position, _target.up);
		float timer = 0f;

		while (timer < transition_duration)
		{
			float k = timer / transition_duration;

			Camera.main.transform.position = Vector3.Lerp(initial_position, final_position, k);
			Camera.main.transform.rotation = Quaternion.Lerp(initial_rotation, final_rotation, k);

			timer += Time.deltaTime;
			yield return null;
		}
		Camera.main.transform.position = final_position;
		Camera.main.transform.rotation = final_rotation;
	}
}
