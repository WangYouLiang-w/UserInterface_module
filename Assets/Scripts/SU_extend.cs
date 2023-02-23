using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Threading;
using LitJson;
using System.IO;
using Unity.VisualScripting;

public class SU_extend : MonoBehaviour
{
    private Camera Upper_Camera;
    private Camera Middle_Camera;
    //private GameObject Middle_CameraObj;
    public static Display[] displays;
    static int Sti_Total = 34;
    static int Sti_upper_num = 18;
    static int Sti_middle_num = 16;
    private int upper_count = 18;
    private int middle_count = 16;
    private GameObject[] Sti_command = new GameObject[Sti_Total];
    private GameObject[] Sti_upper = new GameObject[Sti_upper_num];
    private GameObject[] Sti_middle = new GameObject[Sti_middle_num];
    private GameObject[] Text_cross_upper = new GameObject[Sti_upper_num];
    private GameObject[] Text_cross_middle = new GameObject[Sti_middle_num];
    private GameObject[] Text_command_upper = new GameObject[Sti_upper_num];
    private GameObject[] Text_command_middle = new GameObject[Sti_middle_num];
    private GameObject[] Text_cross = new GameObject[Sti_Total];
    private GameObject[] Text_command = new GameObject[Sti_Total];
    private int Sti_number = Sti_Total;
    private int Sti_upper_number = Sti_upper_num;
    private int Sti_middle_number = Sti_middle_num;
    private int Result = 0;
    private string[] Sti_screen = new string[Sti_Total];
    private string[] Sti_name = new string[Sti_Total];

    private string[] Sti_frequency = {"30.0" ,"31.0", "32.0", "33.0", "34.0", "35.0", "36.0", "37.0", "38.0", "39.0", "40.0", "41.0", "42.0", "43.0", "44.0","45.0","46.0",
                                       "30.5" ,"31.5", "32.5", "33.5", "34.5", "35.5", "36.5", "37.5", "38.5", "39.5", "40.5", "41.5", "42.5", "43.5", "44.5","45.5","46.5",
                                                            };
    private string[] Sti_phase = { "1.05", "2.80", "0.70", "2.45", "1.75", "0.35", "0.00", "2.10", "1.40", "3.15", "3.50", "3.85", "1.05", "2.80", "0.70", "1.75", "0.35",
                                   "1.40", "3.15", "3.50", "3.85", "1.05", "2.80", "0.70", "1.05", "2.80", "0.70", "2.45", "1.75", "0.35", "0.00", "2.10","0.35", "0.00"};
    private string[] Sti_coordinate_X = new string[Sti_Total];
    private string[] Sti_coordinate_Y = new string[Sti_Total];
    private string[] Sti_size = new string[Sti_Total];
    private string pathfile = " ";

    /*此处是定义分类结果回传提示*/
    //private UdpClient client;
    //private Thread thread = null;
    //private IPEndPoint endPoint;
    //private Socket TcpSocket;
    public int Count = 0;
    public int qune = 0;
    public int Label_number = 0;
    public int Mode_Offline;//离线模式选择
    public int port = 8080;
    public int portAddress; //此处用于定义并口，用于标签盒打标签
    public int portAvailble = 0;
    public int[] Random_num = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                               15, 16, 17,18,19,20,21,22,23,24,25,26, 27, 28, 29};//打标签，伪随机
    public float time_o = 0;
    public float time_sti = 0;
    private float gray_value = 255f;
    public float frame = 0;
    /*此处Bool型变量，用于按键检测，选择实验类型*/
    public bool Offline_flag = false;
    public bool StiOnline_flag = false;
    public bool Online_flag = false;
    public string ip = "127.0.0.1";
    public string receiveString = null;
    public Action<string> receiveMsg = null;
    public float Time_readJson = 0;
    private JsonData recvDataJson;
   

    void Awake()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Sti_Camera = new GameObject();  //创建空的游戏对象
        //Sti_Camera.name = "Sti_Camera";
        //mainCamera= GameObject.Find("MainCamera").GetComponent<Camera>();//获取主摄像机的Camera组件,等同于Camera.main

        //upper camera inital 
        Upper_Camera = Camera.main;//获取主摄像机的Camera组件
        Upper_Camera.name = "Upper_Camera";
        Upper_Camera.targetDisplay = 0;
        Upper_Camera.clearFlags = CameraClearFlags.SolidColor;
        Upper_Camera.backgroundColor = new Color(0, 0, 0, 255f);
        Upper_Camera.depth = -1;


        Upper_Camera.AddComponent<Canvas>();
        Upper_Camera.AddComponent<CanvasScaler>(); //canvasScaler组件用以控制Canvas的行列比例和像素密度，缩放时影响所有
        Upper_Camera.AddComponent<GraphicRaycaster>(); //对图表元素进行射线投放
        Upper_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas处于世界模式还是覆盖模式；ScreenSpaceOverlay覆盖模式           
        Upper_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(2560, 1440);//矩形的位置、大小、锚点和轴心的位置
        Upper_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Upper_Camera.GetComponent<Canvas>().targetDisplay = 1;

        //middle camera inital 
        //Middle_CameraObj = new GameObject("Middle_Camera");
        Middle_Camera = GameObject.Find("Camera").GetComponent<Camera>();
        //Camera Middle_Camera = Middle_CameraObj.AddComponent<Camera>();
        Middle_Camera.targetDisplay = 1;
        Middle_Camera.clearFlags = CameraClearFlags.SolidColor;
        Middle_Camera.backgroundColor = new Color(0, 0, 0, 255f);
        Middle_Camera.depth = -1;
        Middle_Camera.AddComponent<Canvas>();
        Middle_Camera.AddComponent<CanvasScaler>(); //canvasScaler组件用以控制Canvas的行列比例和像素密度，缩放时影响所有
        Middle_Camera.AddComponent<GraphicRaycaster>(); //对图表元素进行射线投放   
        Middle_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas处于世界模式还是覆盖模式；ScreenSpaceOverlay覆盖模式        
        Middle_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(2560, 1440);//矩形的位置、大小、锚点和轴心的位置
        Middle_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Middle_Camera.GetComponent<Canvas>().targetDisplay = 0;
        CreatGameObject(Sti_upper_num, Sti_middle_num);

    }


    private void CreatGameObject(int Sti_upper_num ,int Sti_middle_num)
    {
        Font BuildInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        //upper screen
        for (int upper_index = 0; upper_index < Sti_upper_num; upper_index++)
        {          
            //upper image initial 
            
            Sti_upper[upper_index] = new GameObject
            {               
                name = " Sti_upper" + upper_index
            };
            Sti_upper[upper_index].transform.parent = Upper_Camera.transform;
            Sti_upper[upper_index].AddComponent<Image>();
            Sti_upper[upper_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);//创建Image对应的游戏对象
            Sti_upper[upper_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
            Sti_upper[upper_index].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            //upper  "+" initial 
            Text_cross_upper[upper_index] = new GameObject
            {
                name = "Text_cross_upper" + upper_index
            };
            Text_cross_upper[upper_index].transform.parent = Sti_upper[upper_index].transform;            
            Text_cross_upper[upper_index].AddComponent<Text>();
            Text_cross_upper[upper_index].GetComponent<Text>().font = BuildInFont;
            Text_cross_upper[upper_index].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_cross_upper[upper_index].GetComponent<Text>().fontSize = 60;                                                                   //初始化SSVEP的文字大小
            Text_cross_upper[upper_index].GetComponent<Text>().color = Color.red;
            Text_cross_upper[upper_index].GetComponent<Text>().text = "+";
            Text_cross_upper[upper_index].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            //upper  command initial 
            Text_command_upper[upper_index] = new GameObject
            {
                name = "Text_command_upper" + upper_index
            };
            Text_command_upper[upper_index].transform.parent = Sti_upper[upper_index].transform;           
            Text_command_upper[upper_index].AddComponent<Text>();
            Text_command_upper[upper_index].GetComponent<Text>().font = BuildInFont;
            Text_command_upper[upper_index].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_command_upper[upper_index].GetComponent<Text>().fontSize = 42;  //初始化SSVEP的文字大小
            Text_command_upper[upper_index].GetComponent<Text>().color = Color.black;                                                                  //
            Text_command_upper[upper_index].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -56, 0);
        }

        //middle screen
        for (int middle_index = 0; middle_index < Sti_middle_num; middle_index++)
        {
            //middle image initial          
            Sti_middle[middle_index] = new GameObject
            {
                name = "Sti_middle" + middle_index
            };
            Sti_middle[middle_index].transform.parent = Middle_Camera.transform;
            Sti_middle[middle_index].AddComponent<Image>();
            Sti_middle[middle_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);//创建Image对应的游戏对象
            Sti_middle[middle_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 180);
            Sti_middle[middle_index].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            //middle  "+" initial 
            Text_cross_middle[middle_index] = new GameObject
            {
                name = "Text_cross_middle" + middle_index
            };
            Text_cross_middle[middle_index].transform.parent = Sti_middle[middle_index].transform;
            Text_cross_middle[middle_index].AddComponent<Text>();
            Text_cross_middle[middle_index].GetComponent<Text>().font = BuildInFont;
            Text_cross_middle[middle_index].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_cross_middle[middle_index].GetComponent<Text>().fontSize = 60;
            Text_cross_middle[middle_index].GetComponent<Text>().color = Color.red;
            Text_cross_middle[middle_index].GetComponent<Text>().text = "+";//初始化SSVEP的文字大小
            Text_cross_middle[middle_index].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);


            //middle  command initial 
            Text_command_middle[middle_index] = new GameObject
            {
                name = "Text_command_middle" + middle_index
            };
            Text_command_middle[middle_index].transform.parent = Sti_middle[middle_index].transform;
            Text_command_middle[middle_index].AddComponent<Text>();
            Text_command_middle[middle_index].GetComponent<Text>().font = BuildInFont;
            Text_command_middle[middle_index].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_command_middle[middle_index].GetComponent<Text>().fontSize = 42;
            Text_command_middle[middle_index].GetComponent<Text>().color = Color.black;//初始化SSVEP的文字大小         
            Text_command_middle[middle_index].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -56, 0);
        }
    }




    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Time_readJson"+Time_readJson);
        key_check();//按键检测
        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetKeyDown(KeyCode.Home)*/)
        {
            Quit();
        }
        if (Time_readJson == 0)
        {
            pathfile = File.ReadAllText("../json_config/Sti_merges/Sti_merges.json");
            Sti_settings(pathfile);
        }
        if (Time_readJson >= 0.5)
        {
            Time_readJson = 0;
        }
        else
        {
            if (Offline_flag == true)//离线实验
            {
                switch (Mode_Offline)
                {
                    case 1://刺激态                   
                        if (Count >= 5)
                        {
                            Count = 0;
                            Mode_Offline = 2;
                            qune++;
                            Label_number++;

                            if (Label_number == (Sti_number * 6))
                            {
                                Mode_Offline = 3;
                            }
                            if (qune == Sti_number)

                            {
                                qune = 0;
                            }
                        }

                        else
                        {
                            if (time_sti == 0) { sendLabel(Random_num[qune] + 1); }
                            if (time_sti < 0.5)
                            {
                                for (int up_index= 0; up_index < Sti_number; up_index++)
                                {
                                    gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[up_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[up_index])) + 1f)) / 2f);
                                    Sti_upper[up_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);                         
                                    Text_command_upper[up_index].SetActive(true);
                                  

                                }
                                for (int mi_index = 0; mi_index < Sti_number; mi_index++)
                                {
                                    gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[mi_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[mi_index])) + 1f)) / 2f);                                    
                                    Sti_middle[mi_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);                                  
                                    Text_command_middle[mi_index].SetActive(true);

                                }
                                time_sti += Time.deltaTime;
                            }
                            else
                            {
                                time_sti = 0;
                                Count++;       //epoch计数,每次计数10次转移视线。
                            }
                        }
                        break;
                    case 2://提示
                        time_o += Time.deltaTime;
                        if (time_o <= 1.0)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (Random_num[qune] != j)
                                {
                                    Sti_upper[j].GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                                    Sti_middle[j].GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                                    Text_command_upper[j].SetActive(true);
                                    Text_command_middle[j].SetActive(true);
                                }
                                else
                                {
                                    Sti_upper[j].GetComponent<Image>().color = new Color(255f, 0,  0, 255f);//红色，提示 
                                    Sti_middle[j].GetComponent<Image>().color = new Color(255f, 0, 0, 255f);
                                    Text_command_upper[j].SetActive(true);
                                    Text_command_middle[j].SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            time_o = 0;
                            Mode_Offline = 1;//切换到刺激态
                            time_sti = 0;
                        }
                        break;

                    case 3://销毁
                        Destroy(gameObject, 5);
                        break;
                }
            }
            else if (StiOnline_flag == true)//模拟在线实验
            {
                /*连续闪烁0.5*10*_NumStr*2=240s*/

                if (Label_number < (10 * Sti_number * 2))
                {
                    if (Count >= 10)
                    {
                        Count = 0;
                        qune++;
                        //提示线索转移标志
                        if (qune >= Sti_number) qune = 0;
                    }

                    else
                    {
                        /*此处应该是提示线索，满足Random_num[qune]*/

                        if (time_sti == 0) { sendLabel(Random_num[qune] + 1); Label_number++; }
                        if (time_sti < 0.5)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
                                //Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                                Sti_upper[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                                Sti_middle[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);

                                if (Random_num[qune] != j)
                                {
                                    //Text_command[j].SetActive(false);
                                    //Text_cross[j].SetActive(false);
                                    Text_command_upper[j].SetActive(false);
                                    Text_command_middle[j].SetActive(false);
                                    Text_cross_upper[j].SetActive(false);
                                    Text_cross_middle[j].SetActive(false);

                                }
                                else
                                {
                                    Text_command_upper[j].SetActive(true);
                                    Text_command_middle[j].SetActive(true);
                                    Text_cross_upper[j].SetActive(true);
                                    Text_cross_middle[j].SetActive(true);
                                }
                            }
                            time_sti += Time.deltaTime;
                        }
                        else
                        {
                            time_sti = 0;
                            Count++; //epoch计数,每次计数10次转移视线。
                        }
                    }
                }
                else
                {
                    Destroy(gameObject, 5);
                }
            }



            else if (Online_flag == true)//在线实验,Result为回传的分类结果
            {
                if (time_sti == 0) { ++Label_number; sendLabel(Label_number); /*Debug.Log(Label_number)*/; }
                if (time_sti < 0.5)
                {
                    //for (int j = 0; j < Sti_number; j++)
                    //{
                    //    if (Result != j)
                    //    {
                    //        gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
                    //        Sti_upper[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                    //        Sti_middle[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);

                    //    }
                    //    else
                    //    {
                    //        gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
                    //        Sti_upper[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                    //        Sti_middle[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);


                    //    }
                    //}

                    for (int up_index = 0; up_index < upper_count; up_index++)
                    {
                        if (Result != up_index)
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[up_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[up_index])) + 1f)) / 2f);
                            Sti_upper[up_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                           

                        }
                        else
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[up_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[up_index])) + 1f)) / 2f);
                            Sti_upper[up_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                           


                        }
                    }

                    for (int mi_index = 0; mi_index < middle_count; mi_index++)
                    {
                        if (Result != mi_index)
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[mi_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[mi_index])) + 1f)) / 2f);
                         
                            Sti_middle[mi_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);

                        }
                        else
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[mi_index]) * time_sti + Math.PI * Double.Parse(Sti_phase[mi_index])) + 1f)) / 2f);                           
                            Sti_middle[mi_index].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);


                        }
                    }








                    time_sti += Time.deltaTime;
                }
                else
                {
                    time_sti = 0;
                    if (Label_number > 250) Label_number = 0;

                }

            }
            else
            {
                gameObject.SetActive(true);
            }
            Time_readJson += Time.deltaTime;
        }

    }
    void Quit()
    {
        //打包时不能使用
        //UnityEditor.EditorApplication.isPlaying = false;
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }


    private void key_check()//按键检测，用于选择哪种类型的实验
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))//左箭头按键按下，为离线实验
        {
            Offline_flag = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//下箭头按键按下，为模拟在线实验
        {
            StiOnline_flag = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//右箭头按键按下，为在线实验
        {
            Online_flag = true;
        }


    }



    /*此处用于数据类型转变*/
    //public byte[] HexStringToByteArray(string s)
    //{
    //    s = s.Replace(" ", "");
    //    byte[] buffer = new byte[s.Length / 2];
    //    for (int i = 0; i < s.Length; i += 2)
    //    {
    //        buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
    //    }
    //    return buffer;
    //}



    //void ReceiveMsg()
    //{
    //    while (true)
    //    {
    //        // server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    //        client = new UdpClient(port);
    //        byte[] receiveData = client.Receive(ref endPoint);
    //        receiveString = Encoding.UTF8.GetString(receiveData);
    //        Result = Int32.Parse(receiveString);
    //        Debug.Log("Result:" + Result);
    //        client.Close();
    //    }
    //}
    private void Json_read(string jsonStr)

    {

        recvDataJson = JsonMapper.ToObject(jsonStr);
    }

    private void Sti_settings(string jsonStr)
    {
        //Debug.Log("读取JSON间隔0.5S");
        int lastNumSti_upper = upper_count;
        int lastNumSti_middle = middle_count;
        JsonData recvDataJson = JsonMapper.ToObject(jsonStr);
        Sti_number = recvDataJson.Count;
        Sti_name = new string[Sti_Total];
        Sti_coordinate_X = new string[Sti_Total];
        Sti_coordinate_Y = new string[Sti_Total];
        Sti_size = new string[Sti_Total];
        upper_count = 0;
        middle_count = 0;
        //Debug.Log("recvDataJson.Count " + Sti_number);
        for (int sti_index = 0; sti_index < Sti_number; sti_index++)
        {
            
            Sti_screen[sti_index] = recvDataJson[sti_index][0].ToString();/*ToString(); *//*Substring(0, 1);*/
            Debug.Log("Sti_screen[i]" + recvDataJson[sti_index][0].ToString());
            Sti_name[sti_index] = recvDataJson[sti_index][1].ToString();
            Sti_coordinate_X[sti_index] = recvDataJson[sti_index][2].ToString();
            Sti_coordinate_Y[sti_index] = recvDataJson[sti_index][3].ToString();
            Sti_size[sti_index] = recvDataJson[sti_index][4].ToString();

            //Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
            //Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[i]), int.Parse(Sti_coordinate_Y[i]), 0);
            //Text_command[i].GetComponent<Text>().text = Sti_name[i];
            //Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
            //Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));

            if (Sti_screen[sti_index] == "U")               
            {
                Sti_upper[upper_count].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                Sti_upper[upper_count].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[sti_index]), int.Parse(Sti_coordinate_Y[sti_index]), 0);
                Text_command_upper[upper_count].GetComponent<Text>().text = Sti_name[sti_index];
                Text_command_upper[upper_count].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                Text_cross_upper[upper_count].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                upper_count += 1;
            }
            else if (Sti_screen[sti_index] == "M")
            {
                Sti_middle[middle_count].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                Sti_middle[middle_count].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[sti_index]), int.Parse(Sti_coordinate_Y[sti_index]), 0);
                Text_command_middle[middle_count].GetComponent<Text>().text = Sti_name[sti_index];
                Text_command_middle[middle_count].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                Text_cross_middle[middle_count].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[sti_index]), int.Parse(Sti_size[sti_index]));
                middle_count += 1;
            }
            //创建Image控件，到游戏对象上                 //获取相应的Image控件的位置信息
           

        }

        //if (Sti_number < lastNumSti)
        //{
        //    for (int delete_index = 0; delete_index < lastNumSti; delete_index++)
        //    {
        //        //Debug.Log("delete_index " + delete_index);
        //        //Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //        //Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //        //Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //        Sti_upper[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //        Text_command_upper[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //        Text_cross_upper[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        //    }
        //}
        if (upper_count < lastNumSti_upper|| middle_count < lastNumSti_middle)
        {
            for (int delete_index = 0; delete_index < lastNumSti_upper; delete_index++)
            {
                //Debug.Log("delete_index " + delete_index);
                //Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                //Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                //Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Sti_upper[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_command_upper[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_cross_upper[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
        
        //if (middle_count < lastNumSti_middle)
        //{
            for (int delete_index = 0; delete_index < lastNumSti_middle; delete_index++)
            {
                //Debug.Log("delete_index " + delete_index);
                //Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                //Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                //Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Sti_middle[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_command_middle[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_cross_middle[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

            }
        }


        lastNumSti_upper = upper_count;
        lastNumSti_middle = middle_count;


    }

    public class PortAccess//并口通信

    {
        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        public static extern int IsInpOutDriverOpen();

        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        public static extern void paralleOutput(int paralleaddress, int parallevalue);
    };

    void sendLabel(int label)
    {
        //PortAccess.paralleOutput(portAddress, 0);
        //StartCoroutine(waitForSeconds(0.001f, () => { PortAccess.paralleOutput(portAddress, label); }));
        ////Invoke(nameof(PortAccess.paralleOutput), 0.001f);
        ////PortAccess.paralleOutput(portAddress, label);
    }


    IEnumerator waitForSeconds(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }


}
