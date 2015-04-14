﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public enum soundTrackType {BACKGROUND,MAIN_MENU,MELD,STD_PFRM,CNV_PFRM,GLUE,BOMB,BLACKHOLE,PORTAL};

public class Jukebox : MonoBehaviour {

	private Dictionary<soundTrackType,AudioSource> soundBank; 
	private static Jukebox instance = new Jukebox();
	
	//call AudioListener.Instance.<Function to Call>() to communicate with game mngr
	public static Jukebox Instance {
		get {return instance;}
	}
	
	// Use this for initialization
	void Start () {
	
		//Constructors
		soundBank = new Dictionary<soundTrackType,AudioSource>();
		
		//Load audio files into soundBank
		string[] audioFiles = Directory.GetFiles("Resources/AudioFiles");
		foreach(string soundByte in audioFiles) {
		
			//convert file name into soundTrackTypeEnum
			soundTrackType sType = (soundTrackType)Enum.Parse(typeof(soundTrackType),soundByte);
			soundBank.Add(sType,(AudioSource)Resources.Load(soundByte));
		}
	
	}
	
	public void song(soundTrackType sRequest){
	
		if(Enum.IsDefined(typeof(soundTrackType),sRequest)){
			soundBank[sRequest].Play();
		} else {
			Debug.LogWarning(sRequest + " song type not in SoundBank");
		}
	}
}
