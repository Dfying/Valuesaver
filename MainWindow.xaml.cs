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

namespace Valuesaver
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort _port;
                
        public MainWindow()
        {
            InitializeComponent();
            foreach (string s in SerialPort.GetPortNames())
            {
                port.Items.Add(s);
            }
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            if (_port.IsOpen)
            {
                _port.PortName = port.Name;
                _port.BaudRate = 9600;
                _port.DataBits = 8;
                _port.StopBits = StopBits.One;
                _port.Parity = Parity.None;
                _port.DataReceived += new SerialDataReceivedEventHandler(_portDataReceived);

                _port.Open();
                label.Content = "연결됨";
                port.IsEnabled = false;
            }
            else
            {
                label.Content = "이미 연결됨";
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            if(_port.IsOpen)
            {
                _port.Close();
                port.IsEnabled = true;

                label.Content = "연결종료";
            }
            else
            {
                label.Content = "이미 연결종료";
            }
        }

        private void _portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }
    }
}
