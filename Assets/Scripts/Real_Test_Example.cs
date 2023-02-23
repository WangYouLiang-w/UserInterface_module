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
                                     11.00f, 11.50f, 12.00f, 12.50f, 0.00f};//18����-18Ƶ����λ
    public float[] Sti_phase = {0.35f, 0.70f, 1.05f, 1.40f,  1.75f,  2.10f,
                                  2.45f, 2.80f, 3.15f, 3.50f,  0.00f};      ////18����-18Ƶ����λ


    // �̼����λ��
    public int[] Sti_coordinate_X = { 525,525, 325, 0, -325, -525, -525, -325, 0, 325, 0 };
    public int[] Sti_coordinate_Y = { -125, 125, 350, 350, 350, 125, -125, -350, -350, -350,0 };
    public int Sti_size = 120;
    public string[] Sti_screen = new string[Sti_Total];


    public string pathfile = " ";
    public int Count = 0;
    public int qune = 0;
    public int Label_number = 0;
    public int Mode_Offline;//����ģʽѡ��


    public int[] Random_num = { 0, 1, 3, 2, 4, 5, 10, 7, 6, 8, 9 }; //���ǩ��α�����6ָ��
    public int[] OnlineRandom_num = { 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255, 127, 255 };//���ǩ��α���


    public int Result;          
    public float time_o = 0;
    public float time_sti = 0;

    public int portAddress = 0; //�˴����ڶ��岢�ڣ����ڱ�ǩ�д��ǩ
    public float time_sti_lengeth = 0.5f;   //�̼�����
    public float offline_time_cue = 1.0f;   //��ʾʱ��
    public int offline_sti_fragment = 1;    //�̼�Ƭ����
    public int offline_sti_round = 4;       //�̼��ִ�
    public int stionline_sti_fragment = 5; //ģ�����ߴ̼�Ƭ����
    public int stionline_sti_round = 2;     //ģ�����ߴ̼��ִ�

    public float online_time_cue = 0.0f;   //���ߵ���ʾʱ��
    public int online_sti_fragment = 1;    //���ߵĴ̼�Ƭ��
    public float online_time_reply = 0.0f; 

    public float time_receive = 0;
    public DateTime JsonLastTIME;
    public float gray_value;           // �Ҷ�ֵ
    public float frame = 0;            

    public bool Offline_flag = false;
    public bool StiOnline_flag = false;
    public bool Online_flag = false;
    public float Time_readJson = 0;

    public JsonData recvDataJson;
    public JsonData DataJson;

    public Camera Sti_Camera;
    public GameObject IC_Text;
    public GameObject[] Sti_command = new GameObject[Sti_Total];    // �̼���
    public GameObject[] Text_cross = new GameObject[Sti_Total];     // �̼���ʾ��
    public GameObject[] Text_command = new GameObject[Sti_Total];   // ��������
    public GameObject[] Text_feedback = new GameObject[Sti_Total];  // ����

    public GameObject Sti_nocommand;
    public GameObject Nocommand_cross;
    public GameObject Text_nocommand;

    public bool nocommand_flag = false;

    /*�˴����ڶ��崮�ڣ����ڱ�ǩ�д��ǩ*/
    public string portName = "COM8";
    public int baudRate = 115200;
    private SerialPort sp;
    public byte[] bit;
    private int Lable_count = 1;
    private int Hundreds_place;
    private int Stionline_lable;
    private string toSendString;

    /*�˴��Ƕ���������ش���ʾ*/
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
        Mode_Offline = 2;//��ʾ̬
        qune = 0;
        Application.targetFrameRate = 60; //����֡��
    }

    void Start()
    {
        //SerialPort_Neuroscan(0);
        //thread = new Thread(ReceiveMsg);
        //thread.Start();

        //server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //server.Bind(new IPEndPoint(IPAddress.Parse(UDP_IP), UDP_PORT));//�󶨶˿ںź�IP
        //Debug.Log("������Ѿ�����");

        //sp = new SerialPort(portName, baudRate)
        //{
        //    ReadTimeout = 500
        //};
        //sp.Open();
        Sti_Camera = Camera.main;//��ȡ���������Camera���
        Sti_Camera.AddComponent<Canvas>();
        Sti_Camera.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Canvas��������ģʽ���Ǹ���ģʽ��ScreenSpaceOverlay����ģʽ
        Sti_Camera.AddComponent<CanvasScaler>(); //canvasScaler������Կ���Canvas�����б����������ܶȣ�����ʱӰ������
        Sti_Camera.AddComponent<GraphicRaycaster>(); //��ͼ��Ԫ�ؽ�������Ͷ��    
        Sti_Camera.GetComponent<Canvas>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);//���ε�λ�á���С��ê������ĵ�λ��
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
        //��ʼ���̼������
        CreatGameObject(Sti_number-1);

        // ���ô̼����λ��
        Sti_settings(Sti_number-1);
    }


    ////Update is called once per frame
    void Update()

    {
        Key_check();//�������

        if (Offline_flag == true)//����ʵ��
        {
            switch (Mode_Offline)
            {
                case 1://�̼�̬                   
                    if (Count >= offline_sti_fragment)
                    {
                        Count = 0;
                        Mode_Offline = 2;

                        qune++;          //��ǩ������±�
                        Label_number++; //���²�ͬ��ǩ�Ĵ���

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
                        // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); } //���ǩ
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
                            Count++;       //epoch����,ÿ�μ���10��ת�����ߡ�
                        }
                    }
                    break;

                case 2://��ʾ
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
                                    Sti_command[j].GetComponent<Image>().color = Color.red;//��ɫ����ʾ 
                                }
                                Text_command[j].SetActive(true);
                            }
                            IC_Text.SetActive(false);
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

            if (Label_number < (stionline_sti_fragment * Sti_number * stionline_sti_round))
            {
                if (Count >= stionline_sti_fragment)
                {
                    Count = 0;
                    qune++;      //
                    //��ʾ����ת�Ʊ�־
                    if (qune >= Sti_number) qune = 0;
                }

                else
                {
                    /*�˴�Ӧ������ʾ����������Random_num[qune]*/
                   // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); Label_number++; }
                    if (time_sti < time_sti_lengeth)
                    {
                        if (Random_num[qune] == Sti_number - 1)  //��ʾ����״̬
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
            /*������˸0.5*10*_NumStr*2=240s*/
            if (Label_number < (stionline_sti_fragment * Sti_number * stionline_sti_round))
            {
                IC_Text.SetActive(true);

                //���ǩ
                // if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); Label_number++; }

                //����UDP����
                //ReceiveMsg();
                if (time_sti < time_sti_lengeth)
                {
                   
                    for (int j = 0; j < Sti_number - 1; j++)
                    {
                        if (Result == j)    //�յ�����
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
                    Count++; //epoch����,ÿ�μ���10��ת�����ߡ�
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
            Sti_command[i].SetActive(true);
            
            //��ʼ��SSVEP��ʾ���ġ�+��
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
            Text_feedback[i].GetComponent<Text>().fontSize = 60;
            Text_feedback[i].GetComponent<Text>().color = Color.green;
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            Text_feedback[i].SetActive(false);
        }
    }


    private void Sti_settings(int Sti_num)
    {
        int lastNumSti = Sti_num;
        IC_Text.GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080); //IC ���ڵ�λ��
        for (int i = 0; i < Sti_num; i++)
        {
            //�̼���˸����Ĵ�С
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            //�̼���˸�����λ��
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(Sti_coordinate_X[i], Sti_coordinate_Y[i], 0);
            //�̼���˸�����ָ������
            Text_command[i].GetComponent<Text>().text = Sti_name[i];//ָ������
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);//ָ������巽���С
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -Sti_size / 3, 0); //����ڸ������巽���λ��
            //�̼���˸����������ʾ��+��
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            //�̼���˸������ʾ��ɫ��
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Sti_size, Sti_size);
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -Sti_size/ 2, 0);
        }
        //��̬�̼����鸲��������
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
            Result = Int32.Parse(message) - 1;

            if ((Result >= 0 && Result <= 34) || (Result == 39))
            {
                break;
            }

        }
    }
}
