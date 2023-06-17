using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;

	[SerializeField] Menu[] menus;

	void Awake()
	{
		Instance = this;
		for (int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == "Settings"){

			}
		}
	}

	public IEnumerator SettingsCo()
	{
		OpenMenu("Settings");
		yield return new WaitForSeconds(0.1f);
		CloseMenu("Settings");
	}

	public void OpenMenu(string menuName)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == menuName)
			{
				menus[i].Open();
			}
			else if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
	}

	public void OpenMenu(Menu menu)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
	public void CloseMenu(string menuName)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == menuName)
			{
				menus[i].Close();
			}
		}
	}

	public void CloseAll()
	{
		for(int i = 0; i < menus.Length; i++)
		{
			menus[i].Close();
		}
	}
	public void CloseGame()
	{
		Application.Quit();
	}
}