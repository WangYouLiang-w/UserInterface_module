//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
// auto-generated
//----------------------------------------------
using UnityEngine;
using System;

namespace LJR
{
	public class RequestDemo1 : Request<ResponceDemo1>
	{
		public static readonly string URL = "Demo1.json";

		public string id = "";
		public Action<RequestDemo1, ResponceDemo1> onFinish = null;

		public void Send(string id, Action<RequestDemo1, ResponceDemo1> onFinish)
		{
			this.id = id;
			this.onFinish = onFinish;
			Send();
		}

        [Obsolete]
#pragma warning disable CS0809 // 过时成员重写未过时成员
        public override void Send()
#pragma warning restore CS0809 // 过时成员重写未过时成员
        {
			get.Clear ();
			post.Clear ();
			post.Add("id", id);
			Http.Send(this, URL, get, post, onFinish);
		}
	}

	[Serializable]
	public class ResponceDemo1 : Responce
	{
		public int user_id;
		public string user_name;
		public float power_ratio;
		public Game_data game_data;
	}

	[Serializable]
	public class Game_data
	{
		public int game_version;
		public int[] parameter;
	}
}
