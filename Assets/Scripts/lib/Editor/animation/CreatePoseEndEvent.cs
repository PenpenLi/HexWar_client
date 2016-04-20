using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class CreatePoseEndEvent{

	[MenuItem("Animation/在动作最后一帧抛出结束事件,在第一帧加入开始事件")]
	public static void Start(){

		UnityEngine.Object[] gos = Selection.objects;

		foreach(UnityEngine.Object go in gos){

			if(go is AnimationClip){

				AnimationClip clip = (AnimationClip) go;

				if(!clip.isLooping){

					AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);

                    List<AnimationEvent> evtList = new List<AnimationEvent>(events);

                    AnimationEvent end = null;
                    if (events.Length > 0)
                    {
                        end = events[events.Length - 1];
                    }
					if (end == null || (end != null && end.stringParameter != "0$poseStop"))
                    {
                        end = new AnimationEvent();

                        end.functionName = "DispatchAnimationEvent";
                        end.stringParameter = "0$poseStop";

                        end.time = clip.length;

                        evtList.Add(end);
                    }

                    AnimationEvent start = null;
                    if(events.Length > 0)
                    {
                        start = events[0];
                    }
					if (start == null || (start != null && start.stringParameter != "0$poseStart"))
                    {
                        start = new AnimationEvent();

                        start.functionName = "DispatchAnimationEvent";
						start.stringParameter = "0$poseStart";

                        start.time = 0;

                        evtList.Insert(0, start);
                    }

                    //Array.Resize(ref events, events.Length + 1);

                    events = evtList.ToArray();//[events.Length - 1] = e;

                    AnimationUtility.SetAnimationEvents(clip, events);
				}
			}
		}
	}

	[MenuItem("Animation/在动作最后一帧抛出ChangeHeadRandom")]
	public static void Start2(){
		
		UnityEngine.Object[] gos = Selection.objects;
		
		foreach(UnityEngine.Object go in gos){
			
			if(go is AnimationClip){
				
				AnimationClip clip = (AnimationClip) go;
					
				AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);
				
				List<AnimationEvent> evtList = new List<AnimationEvent>(events);
				
				AnimationEvent end = null;

				if (events.Length > 0)
				{
					end = events[events.Length - 1];
				}

				if (end == null || (end != null && end.stringParameter != "0$ChangeHeadRandom"))
				{
					end = new AnimationEvent();
					
					end.functionName = "DispatchAnimationEvent";
					end.stringParameter = "0$ChangeHeadRandom";
					
					end.time = clip.length;
					
					evtList.Add(end);
				}

				events = evtList.ToArray();//[events.Length - 1] = e;
				
				AnimationUtility.SetAnimationEvents(clip, events);
			}
		}
	}


    [MenuItem("Animation/在动作第一帧抛出SetPartIndex$1")]
    public static void Start3()
    {

        UnityEngine.Object[] gos = Selection.objects;

        foreach (UnityEngine.Object go in gos)
        {

            if (go is AnimationClip)
            {

                AnimationClip clip = (AnimationClip)go;

                AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);

                List<AnimationEvent> evtList = new List<AnimationEvent>(events);

                AnimationEvent start = null;
                if (events.Length > 0)
                {
                    start = events[0];
                }
                if (start == null || (start != null && start.stringParameter != "0$SetPartIndex$1"))
                {
                    start = new AnimationEvent();

                    start.functionName = "DispatchAnimationEvent";
                    start.stringParameter = "0$SetPartIndex$1";

                    start.time = 0;

                    evtList.Insert(0, start);
                }

                events = evtList.ToArray();

                AnimationUtility.SetAnimationEvents(clip, events);
            }
        }
    }

    [MenuItem("Animation/在动作第一帧抛出SetPartIndex$2")]
    public static void Start4()
    {

        UnityEngine.Object[] gos = Selection.objects;

        foreach (UnityEngine.Object go in gos)
        {

            if (go is AnimationClip)
            {

                AnimationClip clip = (AnimationClip)go;

                AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);

                List<AnimationEvent> evtList = new List<AnimationEvent>(events);

                AnimationEvent start = null;
                if (events.Length > 0)
                {
                    start = events[0];
                }
                if (start == null || (start != null && start.stringParameter != "0$SetPartIndex$2"))
                {
                    start = new AnimationEvent();

                    start.functionName = "DispatchAnimationEvent";
                    start.stringParameter = "0$SetPartIndex$2";

                    start.time = 0;

                    evtList.Insert(0, start);
                }

                events = evtList.ToArray();

                AnimationUtility.SetAnimationEvents(clip, events);
            }
        }
    }

    [MenuItem("Animation/在动作第一帧抛出SetPartIndex$3")]
    public static void Start5()
    {

        UnityEngine.Object[] gos = Selection.objects;

        foreach (UnityEngine.Object go in gos)
        {

            if (go is AnimationClip)
            {

                AnimationClip clip = (AnimationClip)go;

                AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);

                List<AnimationEvent> evtList = new List<AnimationEvent>(events);

                AnimationEvent start = null;
                if (events.Length > 0)
                {
                    start = events[0];
                }
                if (start == null || (start != null && start.stringParameter != "0$SetPartIndex$3"))
                {
                    start = new AnimationEvent();

                    start.functionName = "DispatchAnimationEvent";
                    start.stringParameter = "0$SetPartIndex$3";

                    start.time = 0;

                    evtList.Insert(0, start);
                }

                events = evtList.ToArray();

                AnimationUtility.SetAnimationEvents(clip, events);
            }
        }
    }


}
