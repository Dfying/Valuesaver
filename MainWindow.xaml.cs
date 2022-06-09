using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace Valuesaver
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort _port = new SerialPort();
        public const string ETX = "\r\n";
        public const char STX = (char)0x02;
        public const char CR = (char)0x0D;
        public const char LF = (char)0x0A;
        public const string inStream = "inStream";

        public const string MA = "MA";
        

        string receivedData;

        public MainWindow()
        {
            InitializeComponent();
            string[] Portno = SerialPort.GetPortNames();
            foreach (string portnum in Portno)
            {
                port.Items.Add(portnum);
            }
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {

            if (!_port.IsOpen)
            {
                if (port.SelectedItem == null)
                {
                    label.Text = "선택 가능한 PORT 없음";
                    return;
                }

                _port.PortName = port.SelectedItem.ToString();
                _port.BaudRate = 9600;
                _port.DataBits = 8;
                _port.StopBits = StopBits.One;
                _port.Parity = Parity.None;
                _port.DataReceived += new SerialDataReceivedEventHandler(_portDataReceived);
                _port.Open();

                port.IsEnabled = false;
                label.Text = "연결됨";
                dtrcv.Text = "Start";
            }
            else
            {
                label.Text = "이미 연결됨";
            }

        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (_port.IsOpen == true)
            {
                _port.Close();
                port.IsEnabled = true;
                label.Text = "연결종료";
            }
            else
            {
                label.Text = "이미 연결종료";
            }
        }

        private void datasaver_Click(object sender, RoutedEventArgs e)
        {
            if (_port.IsOpen)
            {
                _port.Write(MA);
                if(receivedData == null)
                {
                    dtrcv.Text += '\n'+"데이터 미수신";
                }
                dtrcv.Text += string.Format("{0:X2}", receivedData);
            }
            else
            {
                return;
            }
        }
        
        private void _portDataReceived(object sender, EventArgs e)
        {
            int readCnt = 0;
            byte recvByte = 0;
            byte[] recvBuf = new byte[1024];

            //수신 데이터 문자수 확인
            string strRecData = string.Empty;

            //수신 값을 모두 읽어 온다.
            strRecData = _port.ReadExisting();

            //수신된 문자열을 파일에 저장한다.

            //UI Cross thread invoke
            
            readCnt = _port.Read(recvBuf, 0, 1024);
            recvByte = recvBuf[readCnt - 1];
            dtrcv.Text.Contains(readCnt.ToString());

            receivedData = _port.ReadExisting();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dtrcv.Clear();
        }
    }
}
