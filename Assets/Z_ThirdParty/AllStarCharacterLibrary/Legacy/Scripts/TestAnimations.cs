
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class TestAnimations : MonoBehaviour 
{
	//WeaponState stuff
	public int weaponIndex;
	[SerializeField]
	public WeaponState[] weapons;
	
	public int currentAnimIndex;
	public int totalAnimations;
	public List<String> animationNames;

	void Start() 
    {
		weapons = new WeaponState[9];
		//Change this for your own code
		//I am hard coding it here for simplicity sake
		Renderer[] renders = GetComponentsInChildren<Renderer>();
		foreach(Renderer render in renders)
		{
			if (render.name.Contains("Broadsword02"))
			{
				weapons[0] = new WeaponState();
				weapons[0].name = "1H";
				weapons[0].weaponModels = new List<GameObject>();
				weapons[0].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("Longsword"))
			{
				weapons[1] = new WeaponState();
				weapons[1].name = "2H";
				weapons[1].weaponModels = new List<GameObject>();
				weapons[1].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("Bow"))
			{
				if (weapons[2]==null)
				{
					weapons[2] = new WeaponState();
					weapons[2].name = "Bow";
					weapons[2].weaponModels = new List<GameObject>();
				}
				weapons[2].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("Arrow"))
			{
				if (weapons[2]==null)
				{
					weapons[2] = new WeaponState();
					weapons[2].name = "Bow";
					weapons[2].weaponModels = new List<GameObject>();
				}
				weapons[2].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("gladius02"))
			{
				if (weapons[3]==null)
				{
					weapons[3] = new WeaponState();
					weapons[3].name = "Dual";
					weapons[3].weaponModels = new List<GameObject>();
				}
				weapons[3].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("Gladius"))
			{
				if (weapons[3]==null)
				{
					weapons[3] = new WeaponState();
					weapons[3].name = "Dual";
					weapons[3].weaponModels = new List<GameObject>();
				}
				weapons[3].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("SpacePistol"))
			{
				if (weapons[4]==null)
				{
					weapons[4] = new WeaponState();
					weapons[4].name = "Pistol";
					weapons[4].weaponModels = new List<GameObject>();
				}
				weapons[4].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("SpaceRifle"))
			{
				if (weapons[5]==null)
				{
					weapons[5] = new WeaponState();
					weapons[5].name = "Rifle";
					weapons[5].weaponModels = new List<GameObject>();
				}
				weapons[5].weaponModels.Add(render.gameObject);
			}
			if (render.name.Contains("Spear"))
			{
				if (weapons[6]==null)
				{
					weapons[6] = new WeaponState();
					weapons[6].name = "Spear";
					weapons[6].weaponModels = new List<GameObject>();
				}
				weapons[6].weaponModels.Add(render.gameObject);
			}
			if (render.name == "Broadsword")
			{
				if (weapons[7]==null)
				{
					weapons[7] = new WeaponState();
					weapons[7].name = "SS";
					weapons[7].weaponModels = new List<GameObject>();
				}
				weapons[7].weaponModels.Add(render.gameObject);
			}
			if (render.name == "Shield01")
			{
				if (weapons[7]==null)
				{
					weapons[7] = new WeaponState();
					weapons[7].name = "SS";
					weapons[7].weaponModels = new List<GameObject>();
				}
				weapons[7].weaponModels.Add(render.gameObject);
			}
		}
		weapons[8] = new WeaponState();
		weapons[8].name = "UN";
		weapons[8].weaponModels = new List<GameObject>();
		weaponIndex = 8;
		/// END WEAPON STATE HARD CODING
		
        // get the names of all the clips
        Animation animation = this.GetComponent<Animation>();
        totalAnimations = animation.GetClipCount();
        animationNames = new List<string>();
        foreach (AnimationState anim in animation)
        {
            animationNames.Add(anim.name);
        }
		currentAnimIndex = 0;
		ChangeWeaponState();
	}
	
	void OnGUI()
	{
		float aIndex = GUI.HorizontalSlider(new Rect(10, 60, 900, 20), (float)currentAnimIndex, 0.0F, (float)totalAnimations);
		if((int)aIndex!= currentAnimIndex)
		{
			currentAnimIndex = (int)aIndex;
			if (currentAnimIndex > (totalAnimations - 1))
			{
				currentAnimIndex = totalAnimations - 1;
			}
			this.GetComponent<Animation>().CrossFade(animationNames[currentAnimIndex], 0.0f);
			if(!animationNames[currentAnimIndex].StartsWith(weapons[weaponIndex].name))
			{
				ChangeWeaponState();
			}
		}
		string tempString = "Animation: " + animationNames[currentAnimIndex];
		GUI.Label (new Rect (400, 80,1000, 20), tempString);
		tempString = "use this slider to change the current animation";
		GUI.Label (new Rect (10, 80,1000, 20), tempString);
	}
	
	void Update()
	{
		
	}
	
	void ChangeWeaponState()
	{
		print("changing weapon state for " + animationNames[currentAnimIndex]);
		//go through all the weaponstates we are calling "weapons" here...
		bool weaponFound = false;
		for(int wi = 0; wi<weapons.Length;wi++)
		{
			//go through all the gamemodels in this weapon state
			foreach(GameObject go in weapons[wi].weaponModels)
			{
				if(!animationNames[currentAnimIndex].StartsWith(weapons[wi].name))
				{
					//turn off any renderers for this weaponstate
					go.GetComponent<Renderer>().enabled = false;
				}
				else
				{
					//turn on any renderers for this weaponstate
					go.GetComponent<Renderer>().enabled = true;
					weaponIndex = wi;
					weaponFound = true;
				}
			}
		}
		if(!weaponFound)
		{
			weaponIndex = 8;
		}
	}
}


public class WeaponState
{
	public string name;
	public List<GameObject> weaponModels;
}