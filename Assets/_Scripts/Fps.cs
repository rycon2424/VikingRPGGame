using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fps : MonoBehaviour
{

	string label = "";
	string avg = "";
	string graphicsSettings = "";
	string vSyncSettings = "";
	float count;
	float average;
	float total;

	IEnumerator Start()
	{
		GUI.depth = 2;
		int i = 0;
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				count = (1 / Time.deltaTime);
				total += count;
				label = "FPS :" + (Mathf.Round(count));
				avg = "AVG :" + (Mathf.Round(total / i));
			}
			else
			{
				label = "Pause";
			}
			yield return new WaitForSeconds(0.5f);
			i++;
		}
	}

    private void Update()
    {
		graphicsSettings = "currentquality: " + QualitySettings.GetQualityLevel().ToString();
        if (Input.GetKeyDown(KeyCode.V))
		{
			if (QualitySettings.vSyncCount == 1)
			{
				QualitySettings.vSyncCount = 0;
			}
            else
			{
				QualitySettings.vSyncCount = 1;
			}
		}
		vSyncSettings = "Press V to toggle Vsync (" + QualitySettings.vSyncCount.ToString() + ")";
	}

    void OnGUI()
	{
		GUI.color = Color.black;
		GUI.skin.label.fontSize = 50;
		GUI.Label(new Rect(10, 80, 400, 500), label);
		GUI.Label(new Rect(10, 160, 400, 500), avg);
		GUI.skin.label.fontSize = 35;
		GUI.Label(new Rect(10, 240, 400, 500), graphicsSettings);
		GUI.Label(new Rect(10, 300, 400, 500), vSyncSettings);
	}
}