 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
  using UnityEngine.UI;

 public class Webcam : MonoBehaviour {

    private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;

	public RawImage background;
	public bool frontFacing;
	

	void Start () {
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0)
			return;

		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (curr.isFrontFacing == frontFacing)
			{
				cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
				break;
			}
		}	

		if (cameraTexture == null)
			return;

		cameraTexture.Play(); 
		background.texture = cameraTexture; 

		camAvailable = true; 
	}

	void Update () {
		if (!camAvailable)
			return;

		// //float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

		float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; 
		background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); 

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0,0, orient);
	}

	public void StopCamera()
	{
		cameraTexture.Stop();
		WebCamTexture.Destroy(cameraTexture);
		cameraTexture = null;
	}
}