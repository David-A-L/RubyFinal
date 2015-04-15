using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class Jukebox{

	private static Jukebox instance; 
	public static Jukebox Instance { 
		get {
			if (instance == null){
				instance = new Jukebox();
				backgroundPlayer = Object.Instantiate((GameObject) Resources.Load("Prefabs/MusicPlayer"));
				gameAudioPlayer = Object.Instantiate((GameObject) Resources.Load("Prefabs/MusicPlayer"));
				Object.DontDestroyOnLoad (backgroundPlayer);
				Object.DontDestroyOnLoad (gameAudioPlayer);

			}
			return instance; 
		} 
	}

	public bool firstMenuVisit = true;
	public bool firstLevelVisit = true;


	//CODE FOR BACKGROUND AUDIO MANAGEMENT
	private static GameObject backgroundPlayer;

	public void playMainMenu(){
		AudioSource backgroundAudioSource = backgroundPlayer.GetComponent<AudioSource> ();
		backgroundAudioSource.volume = 1;
		backgroundAudioSource.clip = (AudioClip) Resources.Load("AudioFiles/MAIN_MENU");
		backgroundAudioSource.Play ();
	}

	public void playLevel(){
		AudioSource backgroundAudioSource = backgroundPlayer.GetComponent<AudioSource> ();
		backgroundAudioSource.volume = 1;
		backgroundAudioSource.clip = (AudioClip) Resources.Load("AudioFiles/BACKGROUND");
		backgroundAudioSource.Play ();
	}

	//IN-GAME AUDIO
	private static GameObject gameAudioPlayer;

	public void playASound(string soundToPlay){
		string fullpath = "AudioFiles/" + soundToPlay;
		AudioClip ac = (AudioClip)Resources.Load (fullpath);
		AudioSource gameAudioSource = gameAudioPlayer.GetComponent<AudioSource> ();
		gameAudioSource.PlayOneShot (ac);
	}
	
	public void setVolume(string songToSet,int volume){
		
	}
	
	
}



//	private Dictionary<soundTrackType,AudioClip> soundBank; 
//	private AudioClip inLevelMusic;


//public enum soundTrackType {BACKGROUND,MAIN_MENU,MELD,STD_PFRM,CNV_PFRM,GLUE,BOMB,BLACKHOLE,PORTAL};


//		//Constructors
//		soundBank = new Dictionary<soundTrackType,AudioClip>();
//		
//		//Load audio files into soundBank
//		string[] audioFiles = Directory.GetFiles("Resources/AudioFiles");
//		foreach(string soundByte in audioFiles) {
//			//convert file name into soundTrackTypeEnum
//			soundTrackType sType = (soundTrackType)Enum.Parse(typeof(soundTrackType),soundByte);
//			soundBank.Add(sType,(AudioClip)Resources.Load(soundByte));
//		}

//public void playSong(soundTrackType sRequest){
//	
//	if(Enum.IsDefined(typeof(soundTrackType),sRequest)){
//		backgroundAudioSource.clip = soundBank[sRequest];
//		backgroundAudioSource.Play();
//	} else {
//		Debug.LogWarning(sRequest + " song type not in SoundBank");
//	}
//}