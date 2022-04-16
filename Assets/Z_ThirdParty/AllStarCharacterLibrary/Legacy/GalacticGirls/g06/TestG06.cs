
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class TestG06 : MonoBehaviour 
{
	
	public int currentAnimIndex;
	public int totalAnimations;
	public List<String> animationNames;

	void Start() 
    {
        // get the names of all the clips
        Animation animation = this.GetComponent<Animation>();
        totalAnimations = animation.GetClipCount();
        animationNames = new List<string>();
        foreach (AnimationState anim in animation)
        {
            animationNames.Add(anim.name);
        }
		currentAnimIndex = 0;
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
		}
		string tempString = "Animation: " + animationNames[currentAnimIndex];
		GUI.Label (new Rect (30, 80,1000, 20), tempString);
		tempString = "ALT+LMB to orbit,   ALT+RMB to zoom,   ALT+MMB to pan";
		GUI.Label (new Rect (10, 5,1000, 20), tempString);
		tempString = "use this slider to change the current animation";
		GUI.Label (new Rect (10, 30,1000, 20), tempString);
	}
	
	void Update()
	{
		
	}
}

