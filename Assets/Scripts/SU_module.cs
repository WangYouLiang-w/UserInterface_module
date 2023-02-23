//Time��2022.06.29
//Editor��ANG
//Version��˫��͸����UDP���߷�����Neuroscan����

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

public class SU_module : MonoBehaviour
{
    public static int Sti_Total = 34;
    public int Sti_number = Sti_Total;
    public string[] Sti_name = new string[Sti_Total];
    public string[] Sti_frequency = { "30.0" ,"30.25", "30.5", "30.75", "31.0", "31.25", "31.5", "31.75", "32.0", "32.25", "32.5", "32.75", "33.0", "33.25", "33.5", "33.75","34.0","34.25",
                                      "30.0" ,"30.25", "30.5", "30.75", "31.0", "31.25", "31.5", "31.75", "32.0", "32.25", "32.5", "32.75", "33.0", "33.25", "33.5", "33.75"};
    public string[] Sti_phase = { "0", "1.95", "1.90", "1.85", "1.80", "1.75", "1.70", "1.65", "1.60", "1.55", "1.50", "1.45", "1.40", "1.35", "1.30","1.25", "1.20", "1.15",
                                  "0", "1.95", "1.90", "1.85", "1.80", "1.75", "1.70", "1.65", "1.60", "1.55", "1.50", "1.45", "1.40", "1.35", "1.30","1.25"};
    public string[] Sti_coordinate_X = new string[Sti_Total];
    public string[] Sti_coordinate_Y = new string[Sti_Total];
    public string[] Sti_size = new string[Sti_Total];
    public string pathfile = " ";
    public int Count = 0;
    public int qune = 0;
    public int Label_number = 0;
    public int Mode_Offline;//����ģʽѡ��
    public int portAddress; //�˴����ڶ��岢�ڣ����ڱ�ǩ�д��ǩ
    public int portAvailble = 0;
    public int[] Random_num = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33}; //���ǩ��α���
    public int Result;
    public float time_o = 0;
    public float time_sti = 0;
    public float gray_value = 255f;
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
    private UdpClient client;
    private Thread thread = null;
    private IPEndPoint endPoint;
    public string ip = "169.254.223.157";
    public int port = 8080;
    public Action<string> receiveMsg = null;
    public string receiveString = null;

    void Awake()
    {

        Mode_Offline = 2;//��ʾ̬
        qune = 0;
        //Application.targetFrameRate = 240; //����֡��
    }

    void Start()
    {
        thread = new Thread(ReceiveMsg);
        thread.Start();
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
            Sti_command[i].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
            //��ʼ��SSVEP��ʾ���ġ�+��
            Text_cross[i] = new GameObject();
            Text_cross[i].transform.parent = Sti_command[i].transform;
            Text_cross[i].name = "Text_cross" + i;
            Text_cross[i].AddComponent<Text>();
            Text_cross[i].GetComponent<Text>().text = "+";
            Text_cross[i].GetComponent<Text>().font = BuildInFont;
            Text_cross[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_cross[i].GetComponent<Text>().fontSize = 56;                                                                   
            Text_cross[i].GetComponent<Text>().color = Color.red;
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            //��ʼ��SSVEP��ʾָ������
            Text_command[i] = new GameObject();
            Text_command[i].transform.parent = Sti_command[i].transform;
            Text_command[i].name = "Text_command" + i;
            Text_command[i].AddComponent<Text>();
            Text_command[i].GetComponent<Text>().font = BuildInFont;
            Text_command[i].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            Text_command[i].GetComponent<Text>().fontSize = 32;
            Text_command[i].GetComponent<Text>().color = Color.black;          
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
            Text_feedback[i].SetActive(false);
        }
    }

    ////Update is called once per frame
    void Update()
    {
        Key_check();//�������
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
            if (Offline_flag == true)//����ʵ��
            {
                switch (Mode_Offline)
                {
                    case 1://�̼�̬                   
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
                            if (time_sti == 0) { SerialPort_Neuroscan(Random_num[qune] + 1); }
                            if (time_sti < 0.5)
                            {
                                for (int j = 0; j < Sti_number; j++)
                                {
                                    gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
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
                        if (time_o <= 1.0)
                        {
                            for (int j = 0; j < Sti_number; j++)
                            {
                                if (Random_num[qune] != j)
                                {
                                    Sti_command[j].GetComponent<Image>().color = new Color(255f, 225f, 255f, 255f);//��ɫ����ʾ 
                                    Text_command[j].SetActive(true);
                                }
                                else
                                {
                                    Sti_command[Random_num[j]].GetComponent<Image>().color = new Color(255f, 0, 0, 255f);//��ɫ����ʾ 
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

                if (Label_number < (10 * Sti_number * 2))
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
                                gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
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
               
                //Debug.Log("Stionline_lable" + Stionline_lable);
                if (time_sti == 0) 
                { 
                    ++Label_number;
                    StiOnline_Label(Sti_number);
                    SerialPort_Neuroscan(Stionline_lable);
                    Debug.Log("Stionline_lable" + Stionline_lable);
                }
                if (time_sti < 0.5)
                {
                    for (int j = 0; j < Sti_number; j++)
                    {
                        if (Result != j)
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
                            Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                            Text_feedback[j].SetActive(false);
                        }
                        else
                        {
                            gray_value = (float)Convert.ToDouble(((Math.Sin(2.0f * Math.PI * Double.Parse(Sti_frequency[j]) * time_sti + Math.PI * Double.Parse(Sti_phase[j])) + 1f)) / 2f);
                            Sti_command[j].GetComponent<Image>().color = new Color(gray_value, gray_value, gray_value, 255f);
                            Text_feedback[j].SetActive(true);

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
    }

    private void Sti_settings(string jsonStr)
    {
        //Debug.Log("��ȡJSON���0.5S");
        int lastNumSti = Sti_number;
        JsonData recvDataJson = JsonMapper.ToObject(jsonStr);
        if(recvDataJson == null)
        {
            Sti_number = 0;
            Sti_name = new string[0];
            Sti_coordinate_X = new string[0];
            Sti_coordinate_Y = new string[0];
            Sti_size = new string[0];
            
        }
        else
        {
            Sti_number = recvDataJson.Count;
            Sti_name = new string[Sti_Total];
            Sti_coordinate_X = new string[Sti_Total];
            Sti_coordinate_Y = new string[Sti_Total];
            Sti_size = new string[Sti_Total];

        }
        
        //Debug.Log("recvDataJson.Count " + Sti_number);
        for (int i = 0; i < Sti_number; i++)
        {
            //��JSON�ļ���ȡ�̼���Ϣ
            Sti_name[i] = recvDataJson[i][0].ToString();
            Sti_coordinate_X[i] = recvDataJson[i][1].ToString();
            Sti_coordinate_Y[i] = recvDataJson[i][2].ToString();
            Sti_size[i] = recvDataJson[i][3].ToString();
            //�̼���˸����Ĵ�С
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
            //�̼���˸�����λ��
            Sti_command[i].GetComponent<Image>().transform.GetComponent<RectTransform>().localPosition = new Vector3(int.Parse(Sti_coordinate_X[i]), int.Parse(Sti_coordinate_Y[i]), 0);
            //�̼���˸�����ָ������
            Text_command[i].GetComponent<Text>().text = Sti_name[i];//ָ������
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));//ָ������巽���С
            Text_command[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i])/3), 0);
            //�̼���˸����������ʾ��+��
            Text_cross[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
            //�̼���˸������ʾ��ɫ��
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(Sti_size[i]), int.Parse(Sti_size[i]));
            Text_feedback[i].GetComponent<Text>().transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -(int.Parse(Sti_size[i]) / 2), 0);

        }
        //��̬�̼����鸲��������
        if (Sti_number < lastNumSti)
        {
            for (int delete_index = 0; delete_index < lastNumSti; delete_index++)
            {
                //Debug.Log("delete_index " + delete_index);
                Sti_command[delete_index].GetComponent<Image>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_command[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                Text_cross[delete_index].GetComponent<Text>().transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
        }

        lastNumSti = Sti_number;


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
        Neuroscan_SerialPortLabel(0);
        StartCoroutine(WaitForSeconds(0.001f, () => { Neuroscan_SerialPortLabel(dataStr); }));
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
    void ReceiveMsg()
    {
        while (true)
        {
            // server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client = new UdpClient(port);
            byte[] receiveData = client.Receive(ref endPoint);
            receiveString = Encoding.UTF8.GetString(receiveData);
            Result = Int32.Parse(receiveString);
            Debug.Log("Result:" + Result);
            client.Close();
        }
    }
}