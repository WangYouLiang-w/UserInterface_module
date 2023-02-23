using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using LitJson;
using System.IO;
using Unity.VisualScripting;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
public class Offline_Example : MonoBehaviour
{
    public static int Sti_Total = 34;
    public static int Sti_number = 34;
    public string[] Sti_name = new string[Sti_Total];
    //public string[] Sti_frequency = {  "31.0", "32.0", "33.0", "34.0", "35.0", "36.0", "37.0", "38.0", "39.0", "40.0", "41.0", "42.0", "43.0", "44.0", "45.0", "46.0" , "47.0" , "48.0" };
    //public float[] Sti_frequency = { 31.0f, 32.0f, 33.0f, 34.0f, 35.0f, 36.0f, 37.0f, 38.0f, 39.0f, 40.0f, 41.0f, 42.0f, 43.0f, 44.0f, 45.0f, 46.0f, 47.0f, 48.0f };
    //public float[] Sti_frequency = { 31.0f, 31.25f, 31.5f, 31.75f, 32.0f, 32.25f, 32.5f, 32.75f, 33.0f, 33.25f, 33.5f, 33.75f, 34.0f, 34.25f, 34.5f, 34.75f, 35.0f, 35.25f };//EXE_0716 第一次
    //public string[] Sti_frequency = { "30.0", "30.25", "30.5", "30.75", "31.0", "31.25", "31.5", "31.75", "32.0", "32.25", "32.5", "32.75", "33.0", "33.25", "33.5", "33.75", "34.0", "34.25" };
    // //public string[] Sti_phase = { "0", "1.95", "1.90", "1.85", "1.80", "1.75", "1.70", "1.65", "1.60", "1.55", "1.50", "1.45", "1.40", "1.35", "1.30","1.25", "1.20","1.15"};
    // //public string[] Sti_phase = { "0", "1.65", "1.30", "0.95", "0.60", "0.25", "1.90", "1.55", "1.20", "0.85", "0.50", "0.15", "1.80", "1.45", "1.10","0.75", "0.50" , "0.25" };
    // //public float[] Sti_phase = { 0f, 1.65f, 1.30f, 0.95f, 0.60f, 0.25f, 1.90f, 1.55f, 1.20f, 0.85f, 0.50f, 0.15f, 1.80f, 1.45f, 1.10f, 0.75f, 0.50f, 0.25f };//EXE_0716 第一次
    //public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
    //                                 31.25f, 32.25f, 33.25f, 34.25f, 35.25f,
    //                                 31.50f, 32.50f, 33.50f, 34.50f, 35.50f,
    //                                 31.75f,         33.75f,         35.75f };//EXE_0727_XJ - 6频率零相位
    //
    //public float[] Sti_phase = {  0.00f, 0.00f, 0.00f, 0.00f,  0.00f,  0.00f,
    //                              0.35f, 1.75f, 1.15f, 0.55f, 1.95f,
    //                              0.70f, 0.10f, 1.50f, 0.90f, 0.30f,
    //                              1.05f,        1.85f,        0.65f };//////EXE_0727_XJ - 6频率零相位
    //public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
    //                                 31.25f, 32.25f, 33.25f, 34.25f, 35.25f,
    //                                 31.50f, 32.50f, 33.50f, 34.50f, 35.50f,
    //                                 31.75f,         33.75f,         35.75f };//18分类-18频率相位
    //public float[] Sti_phase = {  0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.35f, 1.75f, 1.15f, 0.55f, 1.95f,
    //                              0.70f, 0.10f, 1.50f, 0.90f, 0.30f,
    //                              1.05f,        1.85f,        0.65f };////18分类-18频率相位
    public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f,  35.00f, 36.00f,
                                     31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
                                     31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f};//18分类-18频率相位
    //public float[] Sti_phase = {  0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f};      ////18分类-18频率相位
    public float[] Sti_phase = {  0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f,
                                  0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f,
                                 0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f};      ////18分类-18频率相位
    //public string[] Sti_Highfre = new string[Sti_Total];
    //public string[] Sti_Highphase = new string[Sti_Total];
    public float[] Sti_Highfre = new float[Sti_Total];
    public float[] Sti_Highphase = new float[Sti_Total];
    public string[] Sti_coordinate_X = new string[Sti_Total];
    public string[] Sti_coordinate_Y = new string[Sti_Total];
    public string[] Sti_size = new string[Sti_Total];
    public string[] Sti_screen = new string[Sti_Total];
    public string pathfile = " ";
    public int Count = 0;
    public int qune = 0;
    public int Label_number = 0;
    public int Mode_Offline;//离线模式选择
    public int portAddress = 20220; //此处用于定义并口，用于标签盒打标签
    public int portAvailble = 0;
    //public int[] Random_num = { 0, 5, 1, 4, 3, 2, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }; //打标签，伪随机，6指令
    //public int[] OnlineRandom_num = { 0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,}; //打标签，伪随机
    public int[] Random_num = { 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17 }; //打标签，伪随机，6指令
    public int[] OnlineRandom_num = { 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17, 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17 }; //打标签，伪随机
    //public int[] OnlineRandom_num = { 0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,}; //打标签，伪随机
    //public int Result;
    public int Result;
    public float time_o = 0;
    public float time_sti = 0;
    public float time_receive = 0;
    public DateTime JsonLastTIME;
    public float gray_value;
    public float frame = 0;
    public bool Offline_flag = false;
    public bool StiOnline_flag = false;
    public bool Online_flag = false;
    public float Time_readJson = 0;
    public JsonData recvDataJson;
    public Camera Sti_Camera;
    public GameObject[] Sti_command = new GameObject[Sti_Total];
    public GameObject[] Text_cross = new GameObject[Sti_Total];
    public GameObject[] Text_command = new GameObject[Sti_Total];
    public GameObject[] Text_feedback = new GameObject[Sti_Total];
    /*此处用于定义串口，用于标签盒打标签*/
    public string portName = "COM4";
    public int baudRate = 115200;
    private SerialPort sp;
    public byte[] bit;
    private int Lable_count = 1;
    private int Hundreds_place;
    private int Stionline_lable;
    private string toSendString;
    /*此处是定义分类结果回传提示*/
    //private UdpClient client;
    //private Thread thread = null;
    //private IPEndPoint endPoint;
    public string UDP_IP = "169.254.26.10";
    public int UDP_PORT = 7809;
    public Action<string> receiveMsg = null;
    public string receiveString = null;
    static Socket server;
    FileInfo fi;

    void Awake()
    {

        Mode_Offline = 2;//提示态
        qune = 0;
        Application.targetFrameRate = 240; //设置帧率
    }

    void Start()
    {
        SerialPort_Neuroscan(0);
        //thread = new Thread(ReceiveMsg);
        //thread.Start();

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.Bind(new IPEndPoint(IPAddress.Parse(UDP_IP), UDP_PORT));//绑定端口号和IP
        Debug.Log("服务端已经开启");
        //Console.WriteLine("服务端已经开启");
        //Thread t = new(ReceiveMsg);//开启接收消息线程
        //t.Start();



        sp = new SerialPort(portName, baudRate)
        {
            ReadTimeout = 500
        };
        sp.Open();
        Sti_Camera = Camera.main;//获取主摄像机的Camera组件
        Sti_Camera.AddComponent<Canvas>();
        Sti_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas处于世界模式还是覆盖模式；ScreenSpaceOverlay覆盖模式
        Sti_Camera.AddComponent<CanvasScaler>(); //canvasScaler组件用以控制Canvas的行列比例和像素密度，缩放时影响所有
        Sti_Camera.AddComponent<GraphicRaycaster>(); //对图表元素进行射线投放    
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 2160);//矩形的位置、大小、锚点和轴心的位置
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        CreatGameObject(Sti_number);
    }


    //创建组件
    private void CreatGameObject(int Sti_num)
    {
        Font BuildInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        for (int i = 0; i < Sti_num; i++)
        {
            //初始化SSVEP刺激闪烁方块
            Sti_command[i] = new GameObject();
            Sti_command[i].transform.parent = Sti_Camera.transform;
            Sti_command[i].name = "Sti_command" + i;
            Sti_command[i].AddComponent<Image>();
            Sti_command[i].GetComponent<Image>().color = Color.gray;
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //刺激闪烁方块的位置
            Sti_command[i].SetActive(true);


            //初始化SSVEP提示中心“+”
            Text_cross[i] = new GameObject();
            Text_cross[i].transform.parent = Sti_command[i].transform;
            Text_cross[i].name = "Text_cross" + i;
            Text_cross[i].AddComponent<Text>();
            Text_cross[i].GetComponent<Text>().text = "+";
            Text_cross[i].GetComponent<Text>().font = BuildInFont;
            Text_cross[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_cross[i].GetComponent<Text>().fontSize = 45;
            Text_cross[i].GetComponent<Text>().color = Color.red;
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            Text_cross[i].SetActive(true);
            //初始化SSVEP提示指令文字
            Text_command[i] = new GameObject();
            Text_command[i].transform.parent = Sti_command[i].transform;
            Text_command[i].name = "Text_command" + i;
            Text_command[i].AddComponent<Text>();
            Text_command[i].GetComponent<Text>().font = BuildInFont;
            Text_command[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_command[i].GetComponent<Text>().fontSize = 25;
            Text_command[i].GetComponent<Text>().color = Color.black;
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            Text_command[i].SetActive(true);
            //初始化SSVEP反馈绿色球指示
            Text_feedback[i] = new GameObject();
            Text_feedback[i].transform.parent = Sti_command[i].transform;
            Text_feedback[i].name = "Text_feedback" + i;
            Text_feedback[i].AddComponent<Text>();
            Text_feedback[i].GetComponent<Text>().text = "●";
            Text_feedback[i].GetComponent<Text>().font = BuildInFont;
            Text_feedback[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_feedback[i].GetComponent<Text>().fontSize = 50;
            Text_feedback[i].GetComponent<Text>().color = Color.green;
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            Text_feedback[i].SetActive(false);
        }
    }

    ////Update is called once per frame
    void Update()

    {

        //Debug.Log("Time.deltaTime:" + Time.deltaTime);
        //DateTime JsonLastTIME = DateTime.Now;

        Key_check();//按键检测
        if (Time_readJson == 0)
        {
            //FileInfo fi = new("../Brain-Control-Project/json_config1/Sti_merges/Sti_merges.json");
            fi = new("../Offline_Example.json");
            //DateTime JsonLastTIME = DateTime.Now;

            Debug.Log("fi.LastWriteTime" + fi.LastWriteTime);
            Debug.Log("JsonLastTIME" + JsonLastTIME);

            if (fi.LastWriteTime != JsonLastTIME)
            {
                Debug.Log("111111111");
                //pathfile = File.ReadAllText("../Brain-Control-Project/json_config1/Sti_merges/Sti_merges.json");
                pathfile = File.ReadAllText("../Offline_Example.json");
                Sti_settings(pathfile);
            }

            JsonLastTIME = fi.LastWriteTime;
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
                        if (Count >= 1)
                        //if (Count >= 5)
                        {
                            Count = 0;
                            Mode_Offline = 2;
                            qune++;
                            Label_number++;

                            if (Label_number == (Sti_number * 18))
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
                            if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); }
                            if (time_sti < 0.5)
                            {
                                for (int j = 0; j < Sti_number; j++)
                                {
                                    gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_Highfre[j] * time_sti + Math.PI * Sti_Highphase[j]) + 1f) / 2f);
                                    Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                                    Text_command[j].SetActive(true);
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
                        if (time_o <= 2)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (Random_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;//红色，提示 
                                    Text_command[j].SetActive(true);
                                }
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//红色，提示 
                                    Text_command[j].SetActive(true);
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

                if (Label_number < (10 * Sti_number * 1))
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

                        if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); Label_number++; }
                        if (time_sti < 0.5)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_Highfre[j] * time_sti + Math.PI * Sti_Highphase[j]) + 1f) / 2f);
                                Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);

                                if (Random_num[qune] != j)
                                {
                                    Text_command[j].SetActive(false);
                                    Text_cross[j].SetActive(false);
                                }
                                else
                                {
                                    Text_command[j].SetActive(true);
                                    Text_cross[j].SetActive(true);
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

                switch (Mode_Offline)
                {
                    case 1://刺激态                   
                        if (Count >= 1)
                        //if (Count >= 5)
                        {
                            Count = 0;
                            Mode_Offline = 3;
                            qune++;
                            Label_number++;

                            if (Label_number == OnlineRandom_num.Length)
                            {
                                Mode_Offline = 5;
                            }

                        }

                        else
                        {
                            if (time_sti == 0) { SerialPort_Neuroscan(OnlineRandom_num[qune] + 1); }
                            if (time_sti < 0.5)
                            {
                                for (int j = 0; j < Sti_number; j++)
                                {
                                    gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_Highfre[j] * time_sti + Math.PI * Sti_Highphase[j]) + 1f) / 2f);
                                    Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                                    Text_command[j].SetActive(true);
                                    Text_feedback[j].SetActive(false);
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
                        if (time_o <= 2)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (OnlineRandom_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;//红色，提示 
                                    Text_command[j].SetActive(true);
                                    Text_feedback[j].SetActive(false);
                                }
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//红色，提示 
                                    Text_command[j].SetActive(true);
                                    Text_feedback[j].SetActive(false);
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

                    case 3://接收UDP反馈

                        ReceiveMsg();
                        time_receive = 0;
                        Mode_Offline = 4;

                        break;

                    case 4://反馈态

                        if (time_receive <= 2)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (Result != j)
                                {
                                    //Sti_command[j].GetComponent<Image>().color = Color.green;
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;
                                    Text_feedback[j].SetActive(false);
                                }
                                //else if (Result == 20)
                                //{
                                //    Sti_command[j].GetComponent<Image>().color = Color.gray;
                                //    Text_feedback[j].SetActive(false);
                                //}
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;
                                    Text_feedback[j].SetActive(true);

                                }
                            }
                            time_receive += Time.deltaTime;
                        }
                        else
                        {
                            time_o = 0;
                            Mode_Offline = 2;//切换到提示态

                        }
                        break;
                    case 5://销毁
                        Destroy(gameObject, 5);
                        break;
                }
            }
        }
        //DateTime end = DateTime.Now;
        //TimeSpan ts = end - start;
        //Debug.Log("time" + ts.TotalMilliseconds.ToString());
    }

    void FixedUpdate()//可以设置，目前的TimeStep=0.005S，即0.005S运行一次，每秒200帧
    {


    }

    void Quit()
    {
        //打包时不能使用
        //UnityEditor.EditorApplication.isPlaying = false;
        //测试时不能执行，打包后可以执行
        Application.Quit();
    }

    private void Key_check()//按键检测，用于选择哪种类型的实验
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
        if (Input.GetKeyDown(KeyCode.Escape) /*|| Input.GetKeyDown(KeyCode.Home)*/)
        {
            Quit();
        }
    }

    private void Sti_settings(string jsonStr)
    {

        int lastNumSti = Sti_number;
        FileStream fs = new FileStream("../Offline_Example.json", FileMode.Open);
        StreamReader sr = new StreamReader(fs);

        if (sr.ReadToEnd() != string.Empty)
        {

            //if (fi.Length != 0)
            Debug.Log("读取JSON间隔0.5S");
            //int upper_count = 0;
            //int middle_count = 0;
            int RegionOne_count = 0;
            int RegionTwo_count = 0;
            int RegionThree_count = 0;
            int RegionFour_count = 0;
            int RegionFive_count = 0;
            recvDataJson = JsonMapper.ToObject(jsonStr);
            Sti_number = recvDataJson.Count;
            //Sti_name = new string[Sti_number];
            //Sti_coordinate_X = new string[Sti_number];
            //Sti_coordinate_Y = new string[Sti_number];
            //Sti_size = new string[Sti_number];


            for (int i = 0; i < Sti_number; i++)
            {
                Sti_screen[i] = recvDataJson[i][0].ToString();/*ToString(); *//*Substring(0, 1);*/
                //上中双屏分屏
                //if (Sti_screen[i] == "U")
                //{
                //    Sti_Highfre[i] = Sti_frequency[upper_count];
                //    Sti_Highphase[i] = Sti_phase[upper_count];
                //    upper_count += 1;
                //    //Debug.Log(i + "Sti_Highfre[i]" +  Sti_Highfre[i]);
                //}
                //else if (Sti_screen[i] == "M")
                //{

                //    Sti_Highfre[i] = Sti_frequency[middle_count];
                //    Sti_Highphase[i] = Sti_phase[middle_count];
                //    middle_count += 1;
                //}
                //分区域分屏
                if (Sti_screen[i] == "1")
                {
                    Sti_Highfre[i] = Sti_frequency[RegionOne_count];
                    Sti_Highphase[i] = Sti_phase[RegionOne_count];
                    RegionOne_count += 1;
                    //Debug.Log(i + "Sti_Highfre[i]" +  Sti_Highfre[i]);
                }
                else if (Sti_screen[i] == "2")
                {

                    Sti_Highfre[i] = Sti_frequency[RegionTwo_count];
                    Sti_Highphase[i] = Sti_phase[RegionTwo_count];
                    RegionTwo_count += 1;
                }
                else if (Sti_screen[i] == "3")
                {

                    Sti_Highfre[i] = Sti_frequency[RegionThree_count];
                    Sti_Highphase[i] = Sti_phase[RegionThree_count];
                    RegionThree_count += 1;
                }
                else if (Sti_screen[i] == "4")
                {

                    Sti_Highfre[i] = Sti_frequency[RegionFour_count];
                    Sti_Highphase[i] = Sti_phase[RegionFour_count];
                    RegionFour_count += 1;
                }
                else if (Sti_screen[i] == "5")
                {

                    Sti_Highfre[i] = Sti_frequency[RegionFive_count];
                    Sti_Highphase[i] = Sti_phase[RegionFive_count];
                    RegionFive_count += 1;
                }
                else
                {
                    Debug.Log("Error:第" + i + "个刺激块不在设定分区内");
                }




                //从JSON文件读取刺激信息
                Sti_name[i] = recvDataJson[i][1].ToString();
                Sti_coordinate_X[i] = recvDataJson[i][2].ToString();
                Sti_coordinate_Y[i] = recvDataJson[i][3].ToString();
                Sti_size[i] = recvDataJson[i][4].ToString();
                Debug.Log(Sti_size[i] + "为空");
                //刺激闪烁方块的大小
                Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                //刺激闪烁方块的位置
                Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[i]), int.Parse(Sti_coordinate_Y[i]), 0);
                //刺激闪烁方块的指令文字
                Text_command[i].GetComponent<Text>().text = Sti_name[i];//指令名称
                Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));//指令父级物体方块大小
                Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i]) / 3), 0);
                //刺激闪烁方块中心提示“+”
                Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                //刺激闪烁反馈提示绿色球
                Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i]) / 2), 0);
            }
            //动态刺激方块覆盖问题解决

            if (Sti_number < lastNumSti)
            {
                for (int delete_index = Sti_number; delete_index < lastNumSti; delete_index++)
                {
                    //Debug.Log("delete_index " + delete_index);
                    Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    Text_feedback[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    //Sti_command[delete_index].SetActive(false);
                    //Text_command[delete_index].SetActive(false);
                    //Text_cross[delete_index].SetActive(false);
                    //Text_command[delete_index].SetActive(false);
                    //Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    //Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
            }
            lastNumSti = Sti_number;
        }

        //else
        //{

        //    //Sti_number = 0;//Debug.Log(jsonStr + "为空");
        //    for (int i = 0; i < Sti_number; i++)
        //    {
        //        Sti_command[i].SetActive(false);
        //    }
        //}
        //Debug.Log("recvDataJson.Count " + Sti_number);
    }



    //并口通信
    public class PortAccess

    {
        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        public static extern int IsInpOutDriverOpen();

        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        public static extern void paralleOutput(int paralleaddress, int parallevalue);
    };
    //Neuroscan并口打标签
    void Neuroscan_ParallelPortLabel(int label)
    {
        PortAccess.paralleOutput(portAddress, 0);
        StartCoroutine(WaitForSeconds(0.001f, () => { PortAccess.paralleOutput(portAddress, label); }));
        //Invoke(nameof(PortAccess.paralleOutput), 0.001f);
        //PortAccess.paralleOutput(portAddress, label);
    }
    //Neuroscan标签延时：先置低电平0，0.001s后置高电平1
    IEnumerator WaitForSeconds(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    //Neuroscan串口打标签
    public void SerialPort_Neuroscan(int dataStr)
    {

        StartCoroutine(WaitForSeconds(0.005f, () => { Neuroscan_SerialPortLabel(dataStr); }));
        Neuroscan_SerialPortLabel(0);
    }
    public void Neuroscan_SerialPortLabel(int dataStr)
    {

        toSendString = dataStr.ToString("X2");
        bit = HexStringToByteArray(toSendString);
        Send(bit);
        // byte[] v= SerialPortItem.SerialPortLabelBuffer;
        //print("cc");//测试使用
    }

    //此处用于数据类型转变
    public byte[] HexStringToByteArray(string s)
    {
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
        {
            buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        }
        return buffer;
    }
    //串口通信发送数据
    public void Send(byte[] command)
    {
        try
        {
            //写入数据并清空缓存，不过后面一句不确定是否需要
            sp.Write(command, 0, command.Length);
            sp.BaseStream.Flush();
        }
        catch (Exception e)
        {
            Debug.Log($"serial write error:{e}");
        }
    }
    //程序退出时关闭串口
    private void OnApplicationQuit()
    {
        if (sp.IsOpen)
            sp.Close();
    }

    //在线标签
    private void StiOnline_Label(int Stionline_number)
    {
        //Lable_count = 1;
        if (Lable_count % 2 == 0)
        {
            Hundreds_place = 1;
            Stionline_lable = Hundreds_place * 100 + Stionline_number;
        }
        else
        {
            Hundreds_place = 2;
            Stionline_lable = Hundreds_place * 100 + Stionline_number;
        }
        Lable_count++;
    }
    //接受在线反馈指令，显示绿色提示球
    private void ReceiveMsg()
    {
        while (true)
        {
            Debug.Log("Result:" + Result);
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
            byte[] buffer = new byte[1024];
            int length = server.ReceiveFrom(buffer, ref point);//接收数据报
            Debug.Log("阻塞");
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            Result = Int32.Parse(message);

            if ((Result >= 0 && Result <= 17) || (Result == 20))
            {
                break;
            }

        }
    }
}
