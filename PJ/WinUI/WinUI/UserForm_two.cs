﻿/***********************************************************
* 版本: V1.0
* 创建人：胡思旺
* 创建时间：2017年9月20日 
* 版权所有:胡思旺
* 说明：未经作者同意不得用于商业用途
* 联系方式:siwanghu@hotmail.com
************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using queuingMachinePrinter;
using System.IO.Ports;

namespace WinUI
{
    public partial class UserForm_two : Form
    {
        private int id_one = 1000;
        private int id_two = 2000;
        private int id_three = 3000;
        private int id_four = 4000;
        private bool flag_one = false;
        private bool flag_two = false;
        private bool flag_three = false;
        private bool flag_four = false;
        private ConfigureXml configureXml = null;
        private Printer printer = null;
        private SerialPortCom serialPort = null;
        private BusinessQueue businessQueue_one = null;
        private BusinessQueue businessQueue_two = null;
        private BusinessQueue businessQueue_three = null;
        private BusinessQueue businessQueue_four = null;
        private DataBase DB = null;

        public UserForm_two()
        {
            InitializeComponent();
            configureXml = ConfigureXml.getInstance();          
            printer = new Printer();
            serialPort = new SerialPortCom();
            DB = new DataBase();
        }

        private void transparent()
        {
            this.userFormTwo_button_panel.Parent = this.userFormTwo_background_pictureBox;
            this.userFormTwo_time_label.Parent = this.userFormTwo_background_pictureBox;
            this.userFormTwo_panel_one.Parent = this.userFormTwo_background_pictureBox;
            this.userFormTwo_panel_two.Parent = this.userFormTwo_background_pictureBox;
            this.userFormTwo_panel_three.Parent = this.userFormTwo_background_pictureBox;
            this.userFormTwo_panel_four.Parent = this.userFormTwo_background_pictureBox;
        }

        private void userForm_configure()
        {
            string path = configureXml.getUIForm_Background();
            string control_path = configureXml.getUIForm_Control_Background();
            string[] id_names = configureXml.getUIForm_id_name();
            if(path!="")
                userFormTwo_background_pictureBox.Image = new Bitmap(path);
            if(control_path!="")
            {
                userFormTwo_button_one.Image = new Bitmap(control_path);
                userFormTwo_button_two.Image = new Bitmap(control_path);
                userFormTwo_button_three.Image = new Bitmap(control_path);
                userFormTwo_button_four.Image = new Bitmap(control_path);
            }
            if(id_names!=null)
            {
                userFormTwo_button_one.Text = id_names[0];
                userFormTwo_button_two.Text = id_names[1];
                userFormTwo_button_three.Text = id_names[2];
                userFormTwo_button_four.Text = id_names[3];
            }
        }

        private void initSerialPort(string port)
        {
            serialPort.Port = port;
            serialPort.Baudrate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.Databits = 8;
            serialPort.Stopbits = (StopBits)1;
            serialPort.openSerialPort();
            serialPort.receiveSerialData(new UserFormTwo_CallBack(businessQueue_one, businessQueue_two, businessQueue_three,businessQueue_four));
        }

        private void initQueue()
        {
            businessQueue_one = new BusinessQueue(userFormTwo_button_one.Text);
            businessQueue_two = new BusinessQueue(userFormTwo_button_two.Text);
            businessQueue_three = new BusinessQueue(userFormTwo_button_three.Text);
            businessQueue_four = new BusinessQueue(userFormTwo_button_four.Text);
        }

        private void initPrint()
        {
            printer.openUsbPort();
        }

        private void initDB()
        {
            DB.openDB();
        }

        private void userFormTwo_load(object sender, EventArgs e)
        {
            transparent();
            userForm_configure();
            initQueue();
            initSerialPort(configureXml.getHardWareOfCaller());
            initPrint();
            initDB();
        }

        private void time_tick(object sender, EventArgs e)
        {
            this.userFormTwo_time_label.Text = DateTime.Now.ToString("yyyy-MM-dd:HH:mm:ss");
        }

        private void system_button_one_click(object sender, EventArgs e)
        {
            flag_one = true;
        }

        private void system_button_two_click(object sender, EventArgs e)
        {
            flag_two = true;
        }

        private void system_button_three_click(object sender, EventArgs e)
        {
            flag_three = true;
        }

        private void system_button_four_click(object sender, EventArgs e)
        {
            flag_four = true;
            if (flag_one && flag_two && flag_three && flag_four)
            {
                this.Hide();
                serialPort.closeSerialPort();
                DB.closeDB();
                new LoginForm().Show();               
            }
        }

        private void business_button_one_click(object sender, EventArgs e)
        {
            GeneralRecord record = new GeneralRecord(configureXml.getScreenForm_title(),
               false, userFormTwo_button_one.Text, ++id_one, DateTime.Today);
            businessQueue_one.EnBusinessQueue(record);
            DB.insertTable(false, record.Business_name, record.Queue_number);
            Speech.getInstance().speechText("请取票");
            printer.print(new StringBuilder(id_one.ToString()), new StringBuilder(userFormTwo_button_one.Text),
                new StringBuilder(((businessQueue_one.getGeneralQueueCount() + businessQueue_one.getVipQueueCount()) - 1).ToString()));
        }

        private void business_button_two_click(object sender, EventArgs e)
        {
            GeneralRecord record = new GeneralRecord(configureXml.getScreenForm_title(),
               false, userFormTwo_button_two.Text, ++id_two, DateTime.Today);
            businessQueue_two.EnBusinessQueue(record);
            DB.insertTable(false, record.Business_name, record.Queue_number);
            Speech.getInstance().speechText("请取票");
            printer.print(new StringBuilder(id_two.ToString()), new StringBuilder(userFormTwo_button_two.Text),
                new StringBuilder(((businessQueue_two.getGeneralQueueCount() + businessQueue_two.getVipQueueCount()) - 1).ToString()));
        }

        private void business_button_three_click(object sender, EventArgs e)
        {
            GeneralRecord record = new GeneralRecord(configureXml.getScreenForm_title(),
               false, userFormTwo_button_three.Text, ++id_three, DateTime.Today);
            businessQueue_three.EnBusinessQueue(record);
            DB.insertTable(false, record.Business_name, record.Queue_number);
            Speech.getInstance().speechText("请取票");
            printer.print(new StringBuilder(id_three.ToString()), new StringBuilder(userFormTwo_button_three.Text),
                new StringBuilder(((businessQueue_three.getGeneralQueueCount() + businessQueue_three.getVipQueueCount()) - 1).ToString()));
        }

        private void business_button_four_click(object sender, EventArgs e)
        {
            GeneralRecord record = new GeneralRecord(configureXml.getScreenForm_title(),
               false, userFormTwo_button_four.Text, ++id_four, DateTime.Today);
            businessQueue_four.EnBusinessQueue(record);
            DB.insertTable(false, record.Business_name, record.Queue_number);
            Speech.getInstance().speechText("请取票");
            printer.print(new StringBuilder(id_four.ToString()), new StringBuilder(userFormTwo_button_four.Text),
                new StringBuilder(((businessQueue_four.getGeneralQueueCount() + businessQueue_four.getVipQueueCount()) - 1).ToString()));
        }

        private void UserFormTwo_back(object sender, MouseEventArgs e)
        {
            serialPort.closeSerialPort();
            this.Hide();
            new MainForm().Show();
        }
    }
}
