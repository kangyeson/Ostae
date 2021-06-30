using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenshotPreview : MonoBehaviour {
	public string savePath;
	public string albumName; //저장할 갤러리 앨범네임 설정
	public Image PictureSpace;
	string[] files = null;
	public static int filenum = 0;
	public static bool seven = false;

	public static ScreenshotPreview instance;

	void Awake()
    {
        instance = this;
    }

	//PC
	public void GetPopup()
	{
		files = Directory.GetFiles(savePath + @"\", "*.png" );
		if(files.Length > 0 && files.Length <= 6 && seven == false)
		{	
			if(files.Length == 7) {
				seven = true;
			}
			GetPictureAndShowIt();
			filenum++;
		}
		if(files.Length == 7 && seven == true) {
			filenum++;
			GetPictureAndShowItWhenSeven();	
			seven = false;
		}
		if(files.Length == 7 && seven == false) {
			GetPictureAndShowIt();
			filenum++;	
			if(filenum == 7){
				seven = true;
			}
		}
	}

	public void GetPictureAndShowIt()
	{
		string pathToFile = files[filenum];
		Texture2D texture = GetScreenshotImage(pathToFile);
		Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		PictureSpace.sprite = sp;
		Debug.Log("캡처넘버는 " + filenum);
	}

	public void GetPictureAndShowItWhenSeven()
	{
		string pathToFile = files[6];
		Texture2D texture = GetScreenshotImage(pathToFile);
		Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		PictureSpace.sprite = sp;
		filenum = 0;
		Debug.Log("캡처넘버는 " + filenum);
	}

	Texture2D GetScreenshotImage(string filePath)
	{
		Texture2D texture = null;
		byte[] fileBytes;
		if(File.Exists(filePath)) {
			fileBytes = File.ReadAllBytes(filePath);
			texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
			texture.LoadImage(fileBytes);
		}
		return texture;
	}


	//모바일
	// public void Start()
	// {
	// 	string pathToFile = GetPicture.GetLastPicturePath(albumName); 
	// 	if (pathToFile == null)
    //     {
    //         return;
    //     }
	// 	Texture2D texture = GetScreenshotImage(pathToFile);
		
	// 	Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	// 	PictureSpace.sprite = sp;
	// }

	// Texture2D GetScreenshotImage(string filePath)
	// {
	// 	Texture2D texture = null;
	// 	byte[] fileBytes;
	// 	if (File.Exists (filePath)) {
	// 		fileBytes = File.ReadAllBytes (filePath);
	// 		texture = new Texture2D (2, 2, TextureFormat.RGB24, false);
	// 		texture.LoadImage(fileBytes);
	// 	}
	// 	return texture;
	// }

	public void Close()
    {
       gameObject.SetActive(false);
    }

}
