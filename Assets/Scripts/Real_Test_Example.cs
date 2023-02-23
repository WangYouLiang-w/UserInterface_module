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

public class Real_Test_Example : MonoBehaviour
{
    public static int Sti_Total = 11;
    public static int Sti_number = 11;
    public string[] Sti_name = { "Right", "CW", "TakeOff", "Forward", "Up", "CCW", "Left", "Down", "Back", "Land" };
    public float[] Sti_fre = { 8.00f, 8.50f, 9.00f, 9.50f, 10.00f, 10.50f,
                                     11.00f, 11.50f, 12.00f, 12.50f, 0.00f};//18分类-18频率相位
    public float[] Sti_phase = {0.35f, 0.70f, 1.05f, 1.40f,  1.75f,  2.10f,
                                  2.45f, 2.80f, 3.15f, 3.50f,  0.00f};      ////18分类-18频率相位


    // 刺激块的位置
    public int[] Sti_coordinate_X = { 525,525, 325, 0, -325, -525, -525, -325, 0, 325, 0 };
    public int[] Sti_coordinate_Y = { -125, 125, 350, 350, 350, 125, -125, -350, -350, -350,0 };
    public int Sti_size = 120;
    public string[] Sti_screen = new string[Sti_Total];


    public string pathfile = " ";
    public int Count = 0;
    public int qune = 0;
    public int Label_number = 0;
    public int Mode_Offline;//离线模式选择


    public int[] Random_num = { 0, 1, 3, 2, 4, 5, 10, 7, 6, 8, 9 }; //打标签，伪随机，6指令
    public int[] OnlineRandom_num = { 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255 };//打标签，伪随机


    public int Result;          
    public float time_o = 0;
    public float time_sti = 0;

    public int portAddress = 0; //此处用于定义并口，用于标签盒打标签
    public float time_sti_lengeth = 0.5f;   //刺激长度
    public float offline_time_cue = 1.0f;   //提示时间
    public int offline_sti_fragment = 1;    //刺激片段数
    public int offline_sti_round = 4;       //刺激轮次
    public int stionline_sti_fragment = 5; //模拟在线刺激片段数
    public int stionline_sti_round = 2;     //模拟在线刺激轮次

    public float online_time_cue = 0.0f;   //在线的提示时间
    public int online_sti_fragment = 1;    //在线的刺激片段
    public float online_time_reply = 0.0f; 

    public float time_receive = 0;
    public DateTime JsonLastTIME;
    public float gray_value;           // 灰度值
    public float frame = 0;            

    public bool Offline_flag = false;
    public bool StiOnline_flag = false;
    public bool Online_flag = false;
    public float Time_readJson = 0;

    public JsonData recvDataJson;
    public JsonData DataJson;

    public Camera Sti_Camera;
    public GameObject IC_Text;
    public GameObject[] Sti_command = new GameObject[Sti_Total];    // 刺激块
    public GameObject[] Text_cross = new GameObject[Sti_Total];     // 刺激提示符
    public GameObject[] Text_command = new GameObject[Sti_Total];   // 命令文字
    public GameObject[] Text_feedback = new GameObject[Sti_Total];  // 反馈

    public GameObject Sti_nocommand;
    public GameObject Nocommand_cross;
    public GameObject Text_nocommand;

    public bool nocommand_flag = false;

    /*此处用于定义串口，用于标签盒打标签*/
    public string portName = "COM8";
    public int baudRate = 115200;
    private SerialPort sp;
    public byte[] bit;
    private int Lable_count = 1;
    private int Hundreds_place;
    private int Stionline_lable;
    private string toSendString;

    /*此处是定义分类结果回传提示*/
    private UdpClient client;
    private Thread thread = null;
    private IPEndPoint endPoint;

    public string UDP_IP = "192.168.1.15";
    public int UDP_PORT = 7810;
    public Action<string> receiveMsg = null;
    public string receiveString = null;
    static Socket server;

    FileInfo fi;
    void Awake()
    {
        Mode_Offline = 2;//提示态
        qune = 0;
        Application.targetFrameRate = 60; //设置帧率
    }

    void Start()
    {
        //SerialPort_Neuroscan(0);
        //thread = new Thread(ReceiveMsg);
        //thread.Start();

        //server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //server.Bind(new IPEndPoint(IPAddress.Parse(UDP_IP), UDP_PORT));//绑定端口号和IP
        //Debug.Log("服务端已经开启");

        //sp = new SerialPort(portName, baudRate)
        //{
        //    ReadTimeout = 500
        //};
        //sp.Open();
        Sti_Camera = Camera.main;//获取主摄像机的Camera组件
        Sti_Camera.AddComponent<Canvas>();
        Sti_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas处于世界模式还是覆盖模式；ScreenSpaceOverlay覆盖模式
        Sti_Camera.AddComponent<CanvasScaler>(); //canvasScaler组件用以控制Canvas的行列比例和像素密度，缩放时影响所有
        Sti_Camera.AddComponent<GraphicRaycaster>(); //对图表元素进行射线投放    
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);//矩形的位置、大小、锚点和轴心的位置
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        //
        Font BuildInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        IC_Text = new GameObject();
        IC_Text.transform.parent = Sti_Camera.transform;
        IC_Text.name = "IC_Text";
        IC_Text.AddComponent<Text>();
        IC_Text.GetComponent<Text>().text = "+";
        IC_Text.GetComponent<Text>().font = BuildInFont;
        IC_Text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        IC_Text.GetComponent<Text>().fontSize = 100;
        IC_Text.GetComponent<Text>().color = Color.red;
        IC_Text.GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        IC_Text.GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        IC_Text.SetActive(false);
        //初始化刺激快组件
        CreatGameObject(Sti_number-1);

        // 设置刺激块的位置
        Sti_settings(Sti_number-1);
    }


    ////Update is called once per frame
    void Update()

    {
        Key_check();//按键检测

        if (Offline_flag == true)//离线实验
        {
            switch (Mode_Offline)
            {
                case 1://刺激态                   
                    if (Count >= offline_sti_fragment)
                    {
                        Count = 0;
                        Mode_Offline = 2;

                        qune++;          //标签数组的下标
                        Label_number++; //打下不同标签的次数

                        if (Label_number == (Sti_number * offline_sti_round))
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
                        // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); } //打标签
                        if (time_sti < time_sti_lengeth)
                        {
                            for (int j = 0; j < Sti_number-1; j++)
                            {
                                gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_fre[j] * time_sti + Math.PI * Sti_phase[j]) + 1f) / 2f);
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
                    if (time_o <= offline_time_cue)
                    {
                        if (Random_num[qune] == Sti_number-1)
                        {
                            IC_Text.SetActive(true);
                            for (int j = 0; j < Sti_number-1; j++)
                            {
                                Sti_command[j].GetComponent<Image>().color = Color.gray;
                                Text_command[j].SetActive(true);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Sti_number-1; j++)
                            {
                                if (Random_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;
                                }
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//红色，提示 
                                }
                                Text_command[j].SetActive(true);
                            }
                            IC_Text.SetActive(false);
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

            if (Label_number < (stionline_sti_fragment * Sti_number * stionline_sti_round))
            {
                if (Count >= stionline_sti_fragment)
                {
                    Count = 0;
                    qune++;      //
                    //提示线索转移标志
                    if (qune >= Sti_number) qune = 0;
                }

                else
                {
                    /*此处应该是提示线索，满足Random_num[qune]*/
                   // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); Label_number++; }
                    if (time_sti < time_sti_lengeth)
                    {
                        if (Random_num[qune] == Sti_number - 1)  //提示空闲状态
                        {
                            IC_Text.SetActive(true);
                            for (int j = 0; j < Sti_number - 1; j++)
                            {
                                gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_fre[j] * time_sti + Math.PI * Sti_phase[j]) + 1f) / 2f);
                                Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                                Text_command[j].SetActive(false);
                                Text_cross[j].SetActive(false);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Sti_number-1; j++)
                            {
                                gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_fre[j] * time_sti + Math.PI * Sti_phase[j]) + 1f) / 2f);
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
                            IC_Text.SetActive(false);
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
            /*连续闪烁0.5*10*_NumStr*2=240s*/
            if (Label_number < (stionline_sti_fragment * Sti_number * stionline_sti_round))
            {
                IC_Text.SetActive(true);

                //打标签
                // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); Label_number++; }

                //接收UDP反馈
                //ReceiveMsg();
                if (time_sti < time_sti_lengeth)
                {
                   
                    for (int j = 0; j < Sti_number - 1; j++)
                    {
                        if (Result == j)    //收到反馈
                        {
                           Text_feedback[j].SetActive(true);
                        }
                        else
                        {
                           Text_feedback[j].SetActive(false);
                        }
                        gray_value = (float)((Math.Sin(2.0f * Math.PI * Sti_fre[j] * time_sti + Math.PI * Sti_phase[j]) + 1f) / 2f);
                        Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                        Text_command[j].SetActive(true);
                        Text_cross[j].SetActive(true);
                        Text_feedback[j].SetActive(false);
                    }
                    time_sti += Time.deltaTime;
                }   
                else
                {
                    time_sti = 0;
                    Count++; //epoch计数,每次计数10次转移视线。
                }
            }
            else
            {
                Destroy(gameObject, 5);
            }

        }
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
            Sti_command[i].SetActive(true);
            
            //初始化SSVEP提示中心“+”
            Text_cross[i] = new GameObject();
            Text_cross[i].transform.parent = Sti_command[i].transform;
            Text_cross[i].name = "Text_cross" + i;
            Text_cross[i].AddComponent<Text>();
            Text_cross[i].GetComponent<Text>().text = "+";
            Text_cross[i].GetComponent<Text>().font = BuildInFont;
            Text_cross[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_cross[i].GetComponent<Text>().fontSize = 50;
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
            Text_feedback[i].GetComponent<Text>().fontSize = 60;
            Text_feedback[i].GetComponent<Text>().color = Color.green;
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            Text_feedback[i].SetActive(false);
        }
    }


    private void Sti_settings(int Sti_num)
    {
        int lastNumSti = Sti_num;
        IC_Text.GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080); //IC 所在的位置
        for (int i = 0; i < Sti_num; i++)
        {
            //刺激闪烁方块的大小
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            //刺激闪烁方块的位置
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(Sti_coordinate_X[i], Sti_coordinate_Y[i], 0);
            //刺激闪烁方块的指令文字
            Text_command[i].GetComponent<Text>().text = Sti_name[i];//指令名称
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);//指令父级物体方块大小
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -Sti_size / 3, 0); //相对于父级物体方块的位置
            //刺激闪烁方块中心提示“+”
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            //刺激闪烁反馈提示绿色球
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -Sti_size/ 2, 0);
        }
        //动态刺激方块覆盖问题解决
        if (Sti_num < lastNumSti)
        {
            for (int delete_index = Sti_number; delete_index < lastNumSti; delete_index++)
            {
                //Debug.Log("delete_index " + delete_index);
                Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_feedback[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
        }
        lastNumSti = Sti_num;
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
            Result = Int32.Parse(message) - 1;

            if ((Result >= 0 && Result <= 34) || (Result == 39))
            {
                break;
            }

        }
    }
}
