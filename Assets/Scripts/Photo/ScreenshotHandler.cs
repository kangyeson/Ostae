using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour {
	public string savePath; //저장경로를 직접 설정, 이걸 직접 입력할 필요 없이 어떤 기기에서든 데이터를 바로 저장 가능하도록 json이나 DB사용해서 바꾸기
	public static int recordNumber = 0; 
    public float FadeTime = 2f; // Fade효과 재생시간
	public string albumName; //저장할 갤러리 앨범네임 설정
	public GameObject ScreenBtn;
	Image fadeImg;
    float start;
    float end;

    [SerializeField]
	GameObject blink;

	[SerializeField]
	GameObject popup; //팝업창
	public static ScreenshotHandler instance;

	void Awake()
    {
        instance = this;
    }

	public void TakeAShot()
	{
		ScreenBtn.gameObject.SetActive(false);
		StartCoroutine("CaptureIt");
	}


	IEnumerator CaptureIt()
	{
        yield return new WaitForEndOfFrame(); 
		Texture2D texture = new Texture2D(1330, 715, TextureFormat.RGB24, false);

		texture.ReadPixels(new Rect(295, 105, 1360, 715), 0, 0);
		texture.Apply();

		string fileName = recordNumber + ".png";

		//PC
		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(savePath + @"\" + fileName, bytes);
		Debug.Log("이미지넘버는 " + recordNumber);

		//모바일
		// NativeGallery.SaveImageToGallery(texture, albumName, fileName);

		blink.SetActive(true); //blink오브젝트를 활성화
		Invoke("blinkOut", 0.27f); //0.27초(blink가 완벽히 완료된 타임) 후 다시 비활성화
		recordNumber++;
		if(recordNumber == 7){
			recordNumber = 0;
		}
	}

	void blinkOut()
	{
		blink.SetActive(false); //blink오브젝트를 비활성화
		ScreenBtn.gameObject.SetActive(true);
		Invoke("getPopup", 1f);
	}

	void getPopup()
	{
		popup.SetActive(true);
		ScreenshotPreview.instance.GetPopup();
	}

}