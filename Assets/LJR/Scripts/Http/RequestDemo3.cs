//----------------------------------------------
// LitJson Ruler
// © 2015 yedo-factory
// auto-generated
//----------------------------------------------
using UnityEngine;
using System;

namespace LJR
{
	public class RequestDemo3 : Request<ResponceDemo3>
	{
		public static readonly string URL = "Demo3.json";

		public Action<RequestDemo3, ResponceDemo3> onFinish = null;

		public void Send(Action<RequestDemo3, ResponceDemo3> onFinish)
		{
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
			Http.Send(this, URL, get, post, onFinish);
		}
	}

	[Serializable]
	public class ResponceDemo3 : Responce
	{
	}
}
