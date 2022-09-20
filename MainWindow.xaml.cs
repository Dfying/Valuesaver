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
using System.Windows.Threading;

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
        public const string inStream = "";

        public const string _MA = "MA"; // HEAD 1, 2 측정 명령 
        public const string _MS = "MS,01"; // HEAD 1 측정
        public const string _Zero01 = "VS,01"; // 
        public const string _Zero02 = "VS,02";
        public const string _ZeroAll = "VM,110000000000";

        public string MA = _MS + CR;
        public string Zero01 = _Zero01 + CR;
        public string Zero02 = _Zero02 + CR;
        public string SetZero = _ZeroAll + CR;

        string dt;

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
                _port.BaudRate = 38400;
                _port.DataBits = 8;
                _port.StopBits = StopBits.One;
                _port.Parity = Parity.None;
                _port.Handshake = 0;
                _port.ReadTimeout = 100;
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

        private void _portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                string ReceiveData = _port.ReadExisting();
                dt = string.Empty;
                dt = string.Format("{0:X2}", ReceiveData);
                
            }));
        }

        private void datasaver(object s, EventArgs e)
        {
            dtrcv.Text = dtrcv.Text + dt;
        }

        private void disconnect_Click(object sender, EventArgs e)
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
            if (!_port.IsOpen)
            {
                label.Text = "연결필요";
                if(dt == null)
                {
                    dtrcv.Text = "afa";
                }
                return;
            }
            else
            {
                _port.Write(MA);
                
            }
        }

        private void setZero_Click(object sender, RoutedEventArgs e)
        {
            _port.Write(SetZero);
            Thread.Sleep(10);            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dtrcv.Clear();
        }

        private void exportdata_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
