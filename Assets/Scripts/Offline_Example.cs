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
    //public float[] Sti_frequency = { 31.0f, 31.25f, 31.5f, 31.75f, 32.0f, 32.25f, 32.5f, 32.75f, 33.0f, 33.25f, 33.5f, 33.75f, 34.0f, 34.25f, 34.5f, 34.75f, 35.0f, 35.25f };//EXE_0716 ��һ��
    //public string[] Sti_frequency = { "30.0", "30.25", "30.5", "30.75", "31.0", "31.25", "31.5", "31.75", "32.0", "32.25", "32.5", "32.75", "33.0", "33.25", "33.5", "33.75", "34.0", "34.25" };
    // //public string[] Sti_phase = { "0", "1.95", "1.90", "1.85", "1.80", "1.75", "1.70", "1.65", "1.60", "1.55", "1.50", "1.45", "1.40", "1.35", "1.30","1.25", "1.20","1.15"};
    // //public string[] Sti_phase = { "0", "1.65", "1.30", "0.95", "0.60", "0.25", "1.90", "1.55", "1.20", "0.85", "0.50", "0.15", "1.80", "1.45", "1.10","0.75", "0.50" , "0.25" };
    // //public float[] Sti_phase = { 0f, 1.65f, 1.30f, 0.95f, 0.60f, 0.25f, 1.90f, 1.55f, 1.20f, 0.85f, 0.50f, 0.15f, 1.80f, 1.45f, 1.10f, 0.75f, 0.50f, 0.25f };//EXE_0716 ��һ��
    //public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
    //                                 31.25f, 32.25f, 33.25f, 34.25f, 35.25f,
    //                                 31.50f, 32.50f, 33.50f, 34.50f, 35.50f,
    //                                 31.75f,         33.75f,         35.75f };//EXE_0727_XJ - 6Ƶ������λ
    //
    //public float[] Sti_phase = {  0.00f, 0.00f, 0.00f, 0.00f,  0.00f,  0.00f,
    //                              0.35f, 1.75f, 1.15f, 0.55f, 1.95f,
    //                              0.70f, 0.10f, 1.50f, 0.90f, 0.30f,
    //                              1.05f,        1.85f,        0.65f };//////EXE_0727_XJ - 6Ƶ������λ
    //public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
    //                                 31.25f, 32.25f, 33.25f, 34.25f, 35.25f,
    //                                 31.50f, 32.50f, 33.50f, 34.50f, 35.50f,
    //                                 31.75f,         33.75f,         35.75f };//18����-18Ƶ����λ
    //public float[] Sti_phase = {  0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.35f, 1.75f, 1.15f, 0.55f, 1.95f,
    //                              0.70f, 0.10f, 1.50f, 0.90f, 0.30f,
    //                              1.05f,        1.85f,        0.65f };////18����-18Ƶ����λ
    public float[] Sti_frequency = { 31.00f, 32.00f, 33.00f, 34.00f,  35.00f, 36.00f,
                                     31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f,
                                     31.00f, 32.00f, 33.00f, 34.00f, 35.00f, 36.00f};//18����-18Ƶ����λ
    //public float[] Sti_phase = {  0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f,
    //                              0.00f, 1.40f, 0.80f, 0.20f,  1.60f,  1.00f};      ////18����-18Ƶ����λ
    public float[] Sti_phase = {  0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f,
                                  0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f,
                                 0.00f, 1.00f, 0.00f, 1.00f,  0.00f,  1.00f};      ////18����-18Ƶ����λ
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
    public int Mode_Offline;//����ģʽѡ��
    public int portAddress = 20220; //�˴����ڶ��岢�ڣ����ڱ�ǩ�д��ǩ
    public int portAvailble = 0;
    //public int[] Random_num = { 0, 5, 1, 4, 3, 2, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }; //���ǩ��α�����6ָ��
    //public int[] OnlineRandom_num = { 0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,}; //���ǩ��α���
    public int[] Random_num = { 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17 }; //���ǩ��α�����6ָ��
    public int[] OnlineRandom_num = { 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17, 0, 1, 3, 2, 4, 5, 11, 7, 6, 8, 10, 9, 12, 14, 13, 15, 16, 17 }; //���ǩ��α���
    //public int[] OnlineRandom_num = { 0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,  0, 1, 3, 2, 5, 4,}; //���ǩ��α���
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
    /*�˴����ڶ��崮�ڣ����ڱ�ǩ�д��ǩ*/
    public string portName = "COM4";
    public int baudRate = 115200;
    private SerialPort sp;
    public byte[] bit;
    private int Lable_count = 1;
    private int Hundreds_place;
    private int Stionline_lable;
    private string toSendString;
    /*�˴��Ƕ���������ش���ʾ*/
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

        Mode_Offline = 2;//��ʾ̬
        qune = 0;
        Application.targetFrameRate = 240; //����֡��
    }

    void Start()
    {
        SerialPort_Neuroscan(0);
        //thread = new Thread(ReceiveMsg);
        //thread.Start();

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.Bind(new IPEndPoint(IPAddress.Parse(UDP_IP), UDP_PORT));//�󶨶˿ںź�IP
        Debug.Log("������Ѿ�����");
        //Console.WriteLine("������Ѿ�����");
        //Thread t = new(ReceiveMsg);//����������Ϣ�߳�
        //t.Start();



        sp = new SerialPort(portName, baudRate)
        {
            ReadTimeout = 500
        };
        sp.Open();
        Sti_Camera = Camera.main;//��ȡ���������Camera���
        Sti_Camera.AddComponent<Canvas>();
        Sti_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas��������ģʽ���Ǹ���ģʽ��ScreenSpaceOverlay����ģʽ
        Sti_Camera.AddComponent<CanvasScaler>(); //canvasScaler������Կ���Canvas�����б����������ܶȣ�����ʱӰ������
        Sti_Camera.AddComponent<GraphicRaycaster>(); //��ͼ��Ԫ�ؽ�������Ͷ��    
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 2160);//���ε�λ�á���С��ê������ĵ�λ��
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        CreatGameObject(Sti_number);
    }


    //�������
    private void CreatGameObject(int Sti_num)
    {
        Font BuildInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        for (int i = 0; i < Sti_num; i++)
        {
            //��ʼ��SSVEP�̼���˸����
            Sti_command[i] = new GameObject();
            Sti_command[i].transform.parent = Sti_Camera.transform;
            Sti_command[i].name = "Sti_command" + i;
            Sti_command[i].AddComponent<Image>();
            Sti_command[i].GetComponent<Image>().color = Color.gray;
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //�̼���˸�����λ��
            Sti_command[i].SetActive(true);


            //��ʼ��SSVEP��ʾ���ġ�+��
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
            //��ʼ��SSVEP��ʾָ������
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
            //��ʼ��SSVEP������ɫ��ָʾ
            Text_feedback[i] = new GameObject();
            Text_feedback[i].transform.parent = Sti_command[i].transform;
            Text_feedback[i].name = "Text_feedback" + i;
            Text_feedback[i].AddComponent<Text>();
            Text_feedback[i].GetComponent<Text>().text = "��";
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

        Key_check();//�������
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
            if (Offline_flag == true)//����ʵ��
            {
                switch (Mode_Offline)
                {
                    case 1://�̼�̬                   
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
                                Count++;       //epoch����,ÿ�μ���10��ת�����ߡ�
                            }
                        }
                        break;

                    case 2://��ʾ
                        time_o += Time.deltaTime;
                        if (time_o <= 2)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (Random_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;//��ɫ����ʾ 
                                    Text_command[j].SetActive(true);
                                }
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//��ɫ����ʾ 
                                    Text_command[j].SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            time_o = 0;
                            Mode_Offline = 1;//�л����̼�̬
                            time_sti = 0;
                        }
                        break;

                    case 3://����
                        Destroy(gameObject, 5);
                        break;
                }
            }
            else if (StiOnline_flag == true)//ģ������ʵ��
            {
                /*������˸0.5*10*_NumStr*2=240s*/

                if (Label_number < (10 * Sti_number * 1))
                {
                    if (Count >= 10)
                    {
                        Count = 0;
                        qune++;
                        //��ʾ����ת�Ʊ�־
                        if (qune >= Sti_number) qune = 0;
                    }

                    else
                    {
                        /*�˴�Ӧ������ʾ����������Random_num[qune]*/

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
                            Count++; //epoch����,ÿ�μ���10��ת�����ߡ�
                        }
                    }
                }
                else
                {
                    Destroy(gameObject, 5);
                }
            }
            else if (Online_flag == true)//����ʵ��,ResultΪ�ش��ķ�����
            {

                switch (Mode_Offline)
                {
                    case 1://�̼�̬                   
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
                                Count++;       //epoch����,ÿ�μ���10��ת�����ߡ�
                            }
                        }
                        break;

                    case 2://��ʾ
                        time_o += Time.deltaTime;
                        if (time_o <= 2)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (OnlineRandom_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.gray;//��ɫ����ʾ 
                                    Text_command[j].SetActive(true);
                                    Text_feedback[j].SetActive(false);
                                }
                                else
                                {
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//��ɫ����ʾ 
                                    Text_command[j].SetActive(true);
                                    Text_feedback[j].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            time_o = 0;
                            Mode_Offline = 1;//�л����̼�̬
                            time_sti = 0;
                        }
                        break;

                    case 3://����UDP����

                        ReceiveMsg();
                        time_receive = 0;
                        Mode_Offline = 4;

                        break;

                    case 4://����̬

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
                            Mode_Offline = 2;//�л�����ʾ̬

                        }
                        break;
                    case 5://����
                        Destroy(gameObject, 5);
                        break;
                }
            }
        }
        //DateTime end = DateTime.Now;
        //TimeSpan ts = end - start;
        //Debug.Log("time" + ts.TotalMilliseconds.ToString());
    }

    void FixedUpdate()//�������ã�Ŀǰ��TimeStep=0.005S����0.005S����һ�Σ�ÿ��200֡
    {


    }

    void Quit()
    {
        //���ʱ����ʹ��
        //UnityEditor.EditorApplication.isPlaying = false;
        //����ʱ����ִ�У���������ִ��
        Application.Quit();
    }

    private void Key_check()//������⣬����ѡ���������͵�ʵ��
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))//���ͷ�������£�Ϊ����ʵ��
        {
            Offline_flag = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//�¼�ͷ�������£�Ϊģ������ʵ��
        {
            StiOnline_flag = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//�Ҽ�ͷ�������£�Ϊ����ʵ��
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
            Debug.Log("��ȡJSON���0.5S");
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
                //����˫������
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
                //���������
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
                    Debug.Log("Error:��" + i + "���̼��鲻���趨������");
                }




                //��JSON�ļ���ȡ�̼���Ϣ
                Sti_name[i] = recvDataJson[i][1].ToString();
                Sti_coordinate_X[i] = recvDataJson[i][2].ToString();
                Sti_coordinate_Y[i] = recvDataJson[i][3].ToString();
                Sti_size[i] = recvDataJson[i][4].ToString();
                Debug.Log(Sti_size[i] + "Ϊ��");
                //�̼���˸����Ĵ�С
                Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                //�̼���˸�����λ��
                Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[i]), int.Parse(Sti_coordinate_Y[i]), 0);
                //�̼���˸�����ָ������
                Text_command[i].GetComponent<Text>().text = Sti_name[i];//ָ������
                Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));//ָ������巽���С
                Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i]) / 3), 0);
                //�̼���˸����������ʾ��+��
                Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                //�̼���˸������ʾ��ɫ��
                Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
                Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i]) / 2), 0);
            }
            //��̬�̼����鸲��������

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

        //    //Sti_number = 0;//Debug.Log(jsonStr + "Ϊ��");
        //    for (int i = 0; i < Sti_number; i++)
        //    {
        //        Sti_command[i].SetActive(false);
        //    }
        //}
        //Debug.Log("recvDataJson.Count " + Sti_number);
    }



    //����ͨ��
    public class PortAccess

    {
        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        public static extern int IsInpOutDriverOpen();

        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        public static extern void paralleOutput(int paralleaddress, int parallevalue);
    };
    //Neuroscan���ڴ��ǩ
    void Neuroscan_ParallelPortLabel(int label)
    {
        PortAccess.paralleOutput(portAddress, 0);
        StartCoroutine(WaitForSeconds(0.001f, () => { PortAccess.paralleOutput(portAddress, label); }));
        //Invoke(nameof(PortAccess.paralleOutput), 0.001f);
        //PortAccess.paralleOutput(portAddress, label);
    }
    //Neuroscan��ǩ��ʱ�����õ͵�ƽ0��0.001s���øߵ�ƽ1
    IEnumerator WaitForSeconds(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    //Neuroscan���ڴ��ǩ
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
        //print("cc");//����ʹ��
    }

    //�˴�������������ת��
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
    //����ͨ�ŷ�������
    public void Send(byte[] command)
    {
        try
        {
            //д�����ݲ���ջ��棬��������һ�䲻ȷ���Ƿ���Ҫ
            sp.Write(command, 0, command.Length);
            sp.BaseStream.Flush();
        }
        catch (Exception e)
        {
            Debug.Log($"serial write error:{e}");
        }
    }
    //�����˳�ʱ�رմ���
    private void OnApplicationQuit()
    {
        if (sp.IsOpen)
            sp.Close();
    }

    //���߱�ǩ
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
    //�������߷���ָ���ʾ��ɫ��ʾ��
    private void ReceiveMsg()
    {
        while (true)
        {
            Debug.Log("Result:" + Result);
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//�������淢�ͷ���ip�Ͷ˿ں�
            byte[] buffer = new byte[1024];
            int length = server.ReceiveFrom(buffer, ref point);//�������ݱ�
            Debug.Log("����");
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            Result = Int32.Parse(message);

            if ((Result >= 0 && Result <= 17) || (Result == 20))
            {
                break;
            }

        }
    }
}
