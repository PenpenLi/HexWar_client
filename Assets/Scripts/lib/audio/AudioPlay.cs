using UnityEngine;
using System.Collections;
using System;
using xy3d.tstd.lib.assetManager;
using xy3d.tstd.lib.localData;

namespace xy3d.tstd.lib.audio
{

	public class AudioPlay : MonoBehaviour
	{
		private static AudioPlay _Instance;

		public static AudioPlay Instance {

			get {

				if (_Instance == null) {

					GameObject go = new GameObject("AudioPlayGameObject");

					GameObject.DontDestroyOnLoad(go);

					_Instance = go.AddComponent<AudioPlay> ();
				}

				return _Instance;
			}
		}

		private AudioPlayScript script;

		private AudioSource musicSource;

		private const float musicVolumn = 0.6f;

		private const string MUSIC_PLAY_KEY = "MUSIC_PLAY_KEY";

		public bool isMusicPlay = true;

		private const string EFFECT_PLAY_KEY = "EFFECT_PLAY_KEY";

		public bool isEffectPlay = true;

		void Awake()
		{
			musicSource = gameObject.AddComponent<AudioSource> ();

			musicSource.loop = true;

			musicSource.volume = musicVolumn;

			script = gameObject.AddComponent<AudioPlayScript> ();

			if(LocalSettingData.HasKey(MUSIC_PLAY_KEY)){

				isMusicPlay = LocalSettingData.GetInt(MUSIC_PLAY_KEY) == 1;
			}

			if(LocalSettingData.HasKey(EFFECT_PLAY_KEY)){

				isEffectPlay = LocalSettingData.GetInt(EFFECT_PLAY_KEY) == 1;
			}
		}

		public void PlayMusic (string _path)
		{
			if(musicSource.clip != null){

				if(musicSource.clip.name.Equals(_path)){

					return;
				}

				AudioFactory.Instance.RemoveClip(musicSource.clip);
			}

			AudioFactory.Instance.GetClip (_path, GetMusicClip, false);
		}

		private void GetMusicClip (AudioClip _clip)
		{
			musicSource.clip = _clip;

			if (isMusicPlay) {

				musicSource.Play ();
			}
		}

		public void PlayEffect (string _path)
		{
			if (isEffectPlay) {

				AudioFactory.Instance.GetClip (_path, GetEffectClip);
			}
		}

		private void GetEffectClip (AudioClip _clip)
		{
			script.PlayEffect (_clip);
		}

		public void SetIsMusicPlay (bool _b)
		{
			isMusicPlay = _b;

			LocalSettingData.SetInt(MUSIC_PLAY_KEY,isMusicPlay ? 1 : 0);

			if (!isMusicPlay) {

				if (musicSource.isPlaying) {

					musicSource.Stop ();
				}

			} else {

				if (musicSource.clip != null) {

					musicSource.Play ();
				}
			}
		}

		public void SetIsEffectPlay (bool _b)
		{
			isEffectPlay = _b;

			LocalSettingData.SetInt(EFFECT_PLAY_KEY,isEffectPlay ? 1 : 0);
		}

		public void SetPlayEffectSpeed(float _speed){

			script.SetPlayEffectSpeed(_speed);
		}
	}
}