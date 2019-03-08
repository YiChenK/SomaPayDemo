using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class Soma : MonoBehaviour {

	private string openid = "";

	private string app_order_id = "";

	private string image = "https://p2.music.126.net/VLz0OVK0zP2qzCJ-I7LQVQ==/109951163353477446.jpg?param=200y200";

	private string app_secret = "EqgUWwZIjZB2xyLTBn8ccPW6QlTG82w1iCDbh7fzl0oitpnhwjLnAWjmG1Bnj5OT";

	private AndroidJavaObject mainActivity;

    public Transform parent;

    public GameObject prefab;

	void Awake () {
		AndroidJavaClass ajc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");

		mainActivity = ajc.GetStatic<AndroidJavaObject> ("currentActivity");

		mainActivity.Call ("initSoma");

		initFc ();

        Message("初始化");
    }

	void initFc() {
		// 绑定 android call unity 方法，
		mainActivity.Call ("initUnityCall","MainCamera","callback");
	}

	public void callback (string data){
        Message("callbackdata___" + data);
		JsonData jdata = JsonMapper.ToObject (data);

		int type = int.Parse (jdata ["type"].ToString ());

		int mess = int.Parse (jdata ["mess"].ToString ()); 

		if (mess == 1) {
            Message(type + "成功");
        } else if (mess == 0) {
            Message(type + "取消");
            return;
		} else {
			//if (mess == -1)
            Message(type + "失败");
            return;
		}

		switch (type) {

		case 1:

			openid = jdata ["data"] ["open_id"].ToString ();
            Message("登陆回调————" + openid);

            break;
		
		case 2:
			app_order_id = jdata ["data"] ["app_order_id"].ToString ();
			Message("支付回调————" + app_order_id);

            break;
		}
	}

	public void Login () {
		mainActivity.Call ("somaLogin");
	}

	/// <summary>
	/// open_id: 玩家 id
	/// app_secret:
	/// app_order_id:订单号，唯一，随机字符串（数字，字母大小写都可）
	/// product_id:商品的 ID，即商品 SKU 
	/// product_name:商品的名字，用于页面展示 
	/// description:商品的描述，用于页面展示 
	/// unit_img:商品的图标，用于页面展示
	/// unit_price:商品的单价
	/// quantity:商品的数量(int 类型)
	/// money_amount:商品的总价（总价要等于 unit_price * quantity）
	/// merchant_name:商店的名字
	/// </summary>
	public void SomaPay () {
		mainActivity.Call ("somaPay", openid, app_secret, "kkowj4loj3ofndpeo19eekeh", "121212", "Coin1", "AHAHA", image, "0.01", "1", "0.01", "SomaPayTest");
	}

    void Message (string debug)
    {
        Debug.Log(debug);

        GameObject obj = Instantiate(prefab);

        obj.GetComponent<Text>().text = debug;

        obj.transform.SetParent(parent);

        obj = null;
    }
}
