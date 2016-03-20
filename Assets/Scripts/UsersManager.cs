using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UsersManager : MonoBehaviour
{
	public static UsersManager singleton;
	//public static string user_id;
	public InputField emailInputField, pwdInputField;

	string tmp_debug;

	void Awake ()
	{
		if (singleton != null)
		{
			Destroy(singleton.gameObject);
		}
		singleton = this as UsersManager;

		tmp_debug = "debug...";

		StartCoroutine("getUsers");

		//emailInputField.onValueChange.AddListener(setEmail);
		//pwdInputField.onValueChange.AddListener(setPwd);
	}
/*
	public void setEmail (string _email)
	{
		tmp_debug = "email: " + _email;
	}

	public void setPwd (string _pwd)
	{
	}
*/
	public void Login ()
	{
		Debug.Log("Login req");
		MainManager.firebase_root.AuthWithPassword(emailInputField.text, pwdInputField.text, (AuthData auth) =>
		{
			Debug.Log("auth success!! " + auth.Uid);
			tmp_debug = "auth success!! " + auth.Uid;
			//MainManager.user_id = auth.Uid;
			MainManager.singleton.SetUserId(auth.Uid);

		}, (FirebaseError e) =>
		{
			Debug.Log("auth failure!!");
			tmp_debug = "auth failure: " + e.Message; 
		});

	}

	public void Signup ()
	{
		Debug.Log("Signup req");
		//MainManager.firebase_root.Child("users").SetValue(new Dictionary("benwu@google.com", "password"));
		//signup_email_text.GetComponent<Text>

#if UNITY_ANDROID 
		MainManager.firebase_root.CreateUser(emailInputField.text, pwdInputField.text, () =>
		{
			Debug.Log("User created");
			tmp_debug = "User created";
			Login();
		}, (FirebaseError e) =>
		{
			Debug.Log("user creation fail!!");
			tmp_debug = "user creation fail: " + e.Message; 
		});
#endif


	}


	public void Logout ()
	{

	}

	void OnGUI ()
	{
		GUI.Box(new Rect(Screen.width * .05f, Screen.height * .05f, Screen.width * .9f, Screen.height * .2f), tmp_debug);
	}

	IEnumerator getUsers ()
	{
		WWW www = new WWW("https://crackling-torch-3895.firebaseio.com/users.json");
		yield return www;

		Debug.Log(www.text);
		tmp_debug = "users: " + www.text;
	}
}
