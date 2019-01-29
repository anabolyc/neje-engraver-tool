using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using NejeEngraverApp.Properties;

namespace NejeEngraverApp
{
    public class Form_Main : Form
    {
        private enum LANG
        {
            EN,
            CN,
            JP,
            FR,
            IT,
            DE
        }

        private enum MODE
        {
            NULL,
            OLD_KBOT,
            OLD_NOR,
            OLD_LIT,
            OLD_BLE,
            NEW_NOR,
            NEW_LIT,
            NEW_BLE,
            MASTER,
            WING
        }

        private enum SOFTWARE_TYPE
        {
            ALL,
            KBOT,
            NOR,
            LIT,
            BLE,
            MASTER
        }

        private enum STATE
        {
            BAN_ALL,
            IDLE,
            AT_EDIT,
            BEFORE_CARVING,
            CARVING,
            AT_PAUSE
        }

        private bool DEBUG = true;

        private bool Is_OnlyCN;

        private Form_Main.SOFTWARE_TYPE CURRENT_SOFTWARE_TYPE = Form_Main.SOFTWARE_TYPE.NOR;

        private const string Current_Software_Version = "V4.0";
     
        private bool Check_Online = true;

        private bool Online = true;

        private int page;

        private List<string> imgLink = new List<string>();

        private List<string> Alt_Name = new List<string>();

        private List<List<string>> Alt = new List<List<string>>();

        private List<string> CollectionFileLink = new List<string>();

        private int Collection_page;

        private bool Collection_Picture_Need_To_Refresh;

        private List<Point> pointBuff = new List<Point>();

        private bool NEW_IMAGE;

        private float Zoom = 4f;

        private int MAX_WIDTH = 2000;

        private int MAX_HIGHT = 2000;

        private int Location_x;

        private int Location_y;

        private Bitmap img_black_origin;

        private Bitmap img_shake_origin;

        private Bitmap img_black;

        private Bitmap img_shake;

        private Bitmap img_carve;

        private Bitmap img_reload;

        private int times_at_carving;

        private int Fan_RPM;

        private int Fan_Precent;

        private Form_Loading form_sending;

        private string WEB_SOFTWARE_VERSION = "";

        private string Firmware_Version;

        private string Master_Laser_Type;

        private Form_Main.MODE MACHINE_MODE;

        private Form_Main.STATE SOFTWARE_STATE;

        private Form_Main.LANG Current_language;

        private Form_Main.STATE Current_State;

        private string image_buff_path = "C:\\NEJE\\Image";

        private string[] portNameArray;

        private int portName_i;

        private byte[] remain_cmd;

        private byte[] cmd_todecode;

        private int disp_x;

        private int disp_y;

        private int Last_Tempture;

        private int carvr_time_value_mouse_down;

        private int cursor_x_start;

        private int cursor_y_start;

        private int cursor_x_offset;

        private int cursor_y_offset;

        private string Connected_COM;

        private int time;

        private byte[] img_array_to_send;

        private Bitmap stringimg;

        private IContainer components;

        private TextBox textBox1;

        private Label label_Connection;

        private SerialPort serialPort1;

        private Button button_driver;

        private TrackBar trackBar_carveTime;

        private Label label_trackbar;

        private TabControl tabControl1;

        private TabPage tabPage0;

        private TabPage tabPage1;

        private TabPage tabPage2;

        private PictureBox pictureBox1;

        private RadioButton radioButton_Black;

        private RadioButton radioButton_Shake;

        private TextBox textBox_size;

        private Label label_size;

        private Button button_size;

        private Panel panel_size;

        private System.Windows.Forms.Timer timer_connect;

        private System.Windows.Forms.Timer timer_isconnection_ok_check;

        private System.Windows.Forms.Timer timer_picturebox_refresh;

        private GroupBox groupBox_InsertText;

        private CheckBox checkBox_Insert;

        private Button button_Font;

        private TextBox textBox_Edit;

        private Label label_current;

        private Label label_temperature;

        private Label label_power;

        private Label label_PWM;

        private WebBrowser webBrowser1;

        private PictureBox pictureBox2;

        private PictureBox pictureBox3;

        private PictureBox pictureBox13;

        private PictureBox pictureBox12;

        private PictureBox pictureBox11;

        private PictureBox pictureBox10;

        private PictureBox pictureBox9;

        private PictureBox pictureBox8;

        private PictureBox pictureBox7;

        private PictureBox pictureBox6;

        private PictureBox pictureBox5;

        private PictureBox pictureBox4;

        private Label label_page;

        private Button button_next_page;

        private Button button_pre_page;

        private Label label_machine_firmversion;

        private Label label_machine_mode;

        private Label label_software_version;

        private Label label1;

        private ComboBox comboBox_Language;

        private GroupBox groupBox1;

        private Label label_filter;

        private ComboBox comboBox_Alt;

        private Button button_right;

        private Button button_down;

        private Button button_left;

        private Button button_up;

        private FontDialog fontDialog_Edit;

        private Button button_reload;

        private Button button_clear;

        private Panel panel_direction;

        private Label label_width;

        private Panel panel_control;

        private Button button_stop;

        private Label label_times;

        private NumericUpDown numericUpDown_times;

        private Button button_anypoint_location;

        private Button button_preview;

        private Button button_pause;

        private Button button_start;

        private Button button_Send_Pic;

        private TrackBar trackBar_PWM;

        private Label label_connection_logo;

        private Panel panel_Laser_PWM;

        private TabPage tabPage3;

        private Label label_collection_pages;

        private Button button_collection_next;

        private Button button_collection_pre;

        private PictureBox pictureBox14;

        private PictureBox pictureBox15;

        private PictureBox pictureBox16;

        private PictureBox pictureBox17;

        private PictureBox pictureBox18;

        private PictureBox pictureBox19;

        private PictureBox pictureBox20;

        private PictureBox pictureBox21;

        private PictureBox pictureBox22;

        private PictureBox pictureBox23;

        private PictureBox pictureBox24;

        private PictureBox pictureBox25;

        private ContextMenuStrip contextMenu1;

        private ToolStripMenuItem menu_add_collection;

        private ContextMenuStrip contextMenu2;

        private ToolStripMenuItem menu_delete;

        private System.Windows.Forms.Timer timer_check_send_pic;

        private StatusStrip statusStrip1;

        private ToolStripStatusLabel StatusLabel_state;

        private Label label_fan_RPM;

        private Label label_laser_power;

        private Label label_laser_length;

        private void set_software_state(Form_Main.STATE state)
        {
            this.Current_State = state;
            switch (state)
            {
                case Form_Main.STATE.BAN_ALL:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = false;
                    this.radioButton_Shake.Enabled = false;
                    this.groupBox_InsertText.Enabled = false;
                    this.button_Send_Pic.Enabled = false;
                    this.button_up.Enabled = false;
                    this.button_down.Enabled = false;
                    this.button_left.Enabled = false;
                    this.button_right.Enabled = false;
                    this.button_preview.Enabled = false;
                    this.button_anypoint_location.Enabled = false;
                    this.button_start.Enabled = false;
                    this.button_pause.Enabled = false;
                    this.button_stop.Enabled = false;
                    this.StatusLabel_state.Text = "    STATE : NOT READY";
                    return;
                case Form_Main.STATE.IDLE:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = false;
                    this.radioButton_Shake.Enabled = false;
                    this.groupBox_InsertText.Enabled = true;
                    this.button_Send_Pic.Enabled = false;
                    this.button_up.Enabled = false;
                    this.button_down.Enabled = false;
                    this.button_left.Enabled = false;
                    this.button_right.Enabled = false;
                    this.button_preview.Enabled = false;
                    this.button_anypoint_location.Enabled = false;
                    this.button_start.Enabled = true;
                    this.button_pause.Enabled = true;
                    this.button_stop.Enabled = true;
                    this.button_preview.Text = "";
                    this.button_preview.BackgroundImage = Resources.preview;
                    this.button_anypoint_location.Text = "";
                    this.button_anypoint_location.BackgroundImage = Resources.anypoint;
                    this.pictureBox1.Cursor = Cursors.Default;
                    this.StatusLabel_state.Text = "    STATE : IDLE";
                    return;
                case Form_Main.STATE.AT_EDIT:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = true;
                    this.radioButton_Shake.Enabled = true;
                    this.groupBox_InsertText.Enabled = true;
                    this.button_Send_Pic.Enabled = true;
                    this.button_up.Enabled = true;
                    this.button_down.Enabled = true;
                    this.button_left.Enabled = true;
                    this.button_right.Enabled = true;
                    this.button_anypoint_location.Enabled = true;
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT || this.MACHINE_MODE == Form_Main.MODE.OLD_NOR || this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
                    {
                        this.button_preview.Enabled = false;
                        this.button_start.Enabled = false;
                        this.button_pause.Enabled = false;
                    }
                    else
                    {
                        this.button_preview.Enabled = true;
                        this.button_start.Enabled = true;
                        this.button_pause.Enabled = true;
                    }
                    this.button_stop.Enabled = true;
                    this.StatusLabel_state.Text = "    STATE : AT EDIT";
                    return;
                case Form_Main.STATE.BEFORE_CARVING:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = false;
                    this.radioButton_Shake.Enabled = false;
                    this.groupBox_InsertText.Enabled = false;
                    this.button_Send_Pic.Enabled = false;
                    this.button_up.Enabled = true;
                    this.button_down.Enabled = true;
                    this.button_left.Enabled = true;
                    this.button_right.Enabled = true;
                    this.button_anypoint_location.Enabled = true;
                    this.button_preview.Enabled = true;
                    this.button_start.Enabled = true;
                    this.button_pause.Enabled = true;
                    this.button_stop.Enabled = true;
                    this.StatusLabel_state.Text = "    STATE : BEFORE CARVING";
                    return;
                case Form_Main.STATE.CARVING:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = false;
                    this.radioButton_Shake.Enabled = false;
                    this.groupBox_InsertText.Enabled = false;
                    this.button_Send_Pic.Enabled = false;
                    this.button_up.Enabled = false;
                    this.button_down.Enabled = false;
                    this.button_left.Enabled = false;
                    this.button_right.Enabled = false;
                    this.button_anypoint_location.Enabled = false;
                    this.button_preview.Enabled = false;
                    this.button_start.Enabled = true;
                    this.button_pause.Enabled = true;
                    this.button_stop.Enabled = true;
                    this.StatusLabel_state.Text = "    STATE : AT CARVING";
                    return;
                case Form_Main.STATE.AT_PAUSE:
                    this.SOFTWARE_STATE = state;
                    this.radioButton_Black.Enabled = false;
                    this.radioButton_Shake.Enabled = false;
                    this.groupBox_InsertText.Enabled = false;
                    this.button_Send_Pic.Enabled = false;
                    this.button_up.Enabled = false;
                    this.button_down.Enabled = false;
                    this.button_left.Enabled = false;
                    this.button_right.Enabled = false;
                    this.button_anypoint_location.Enabled = false;
                    this.button_preview.Enabled = false;
                    this.button_start.Enabled = true;
                    this.button_pause.Enabled = true;
                    this.button_stop.Enabled = true;
                    this.StatusLabel_state.Text = "    STATE : AT PAUSE";
                    return;
                default:
                    return;
            }
        }

        private void Adjust_Window()
        {
            if (this.Is_OnlyCN)
            {
                this.comboBox_Language.SelectedIndex = 1;
                this.comboBox_Language.Enabled = false;
                this.Current_language = Form_Main.LANG.CN;
            }
            switch (this.CURRENT_SOFTWARE_TYPE)
            {
                case Form_Main.SOFTWARE_TYPE.ALL:
                    this.webBrowser1.Navigate("http://neje.club/tutorials/general_help.htm");
                    return;
                case Form_Main.SOFTWARE_TYPE.KBOT:
                    this.button_driver.Visible = true;
                    this.Text = "KK创意工作室 K-Bot 控制台";
                    this.webBrowser1.Navigate("http://www.kbot.vip/help.html");
                    return;
                case Form_Main.SOFTWARE_TYPE.NOR:
                    if (this.Current_language == Form_Main.LANG.CN)
                    {
                        this.webBrowser1.Navigate("http://neje.club/tutorials/cn_kz_help.htm");
                        return;
                    }
                    this.webBrowser1.Navigate("http://neje.club/tutorials/en_kz_help.htm");
                    return;
                case Form_Main.SOFTWARE_TYPE.LIT:
                    this.webBrowser1.Navigate("http://neje.club/tutorials/en_fkz_help.htm");
                    return;
                case Form_Main.SOFTWARE_TYPE.BLE:
                    this.webBrowser1.Navigate("http://neje.club/tutorials/en_bl_help.htm");
                    return;
                case Form_Main.SOFTWARE_TYPE.MASTER:
                    if (this.Current_language == Form_Main.LANG.CN)
                    {
                        this.webBrowser1.Navigate("http://neje.club/tutorials/cn_btw_help.htm");
                        return;
                    }
                    this.webBrowser1.Navigate("http://neje.club/tutorials/en_btw_help.htm");
                    return;
                default:
                    return;
            }
        }

        private void Init_window()
        {
            switch (this.MACHINE_MODE)
            {
                case Form_Main.MODE.NULL:
                    this.label_machine_mode.Text = "--------";
                    this.label_machine_firmversion.Text = "--------";
                    this.label_temperature.Text = "--------";
                    this.label_current.Text = "--------";
                    this.label_power.Text = "--------";
                    this.label_laser_length.Text = "--------";
                    this.label_laser_power.Text = "--------";
                    this.label_fan_RPM.Text = "--------";
                    break;
                case Form_Main.MODE.OLD_KBOT:
                    this.Zoom = 1f;
                    this.MAX_WIDTH = 490;
                    this.MAX_HIGHT = 490;
                    this.label_machine_mode.Text = "Model : K-Bot V3S";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.OLD_NOR:
                    this.Zoom = 1f;
                    this.MAX_WIDTH = 490;
                    this.MAX_HIGHT = 490;
                    this.label_machine_mode.Text = "Model : DK-8-KZ";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.OLD_LIT:
                    this.Zoom = 1f;
                    this.MAX_WIDTH = 490;
                    this.MAX_HIGHT = 490;
                    this.label_machine_mode.Text = "Model : DK-8-FKZ";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.OLD_BLE:
                    this.Zoom = 1.1f;
                    this.MAX_WIDTH = 550;
                    this.MAX_HIGHT = 550;
                    this.label_machine_mode.Text = "Model : NEJE-BL";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.NEW_NOR:
                    this.Zoom = 1f;
                    this.MAX_WIDTH = 490;
                    this.MAX_HIGHT = 490;
                    this.label_machine_mode.Text = "Model : DK-8-KZ";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.NEW_LIT:
                    this.Zoom = 1f;
                    this.MAX_WIDTH = 490;
                    this.MAX_HIGHT = 490;
                    this.label_machine_mode.Text = "Model : DK-8-FKZ";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.NEW_BLE:
                    this.Zoom = 1.1f;
                    this.MAX_WIDTH = 550;
                    this.MAX_HIGHT = 550;
                    this.label_machine_mode.Text = "Model : NEJE-BL";
                    this.label_machine_firmversion.Text = "Firmware : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.MASTER:
                    this.Zoom = 4f;
                    this.MAX_WIDTH = 2000;
                    this.MAX_HIGHT = 2000;
                    this.label_machine_mode.Text = "Model : NEJE-MASTER";
                    this.label_machine_firmversion.Text = "Firmware version : " + this.Firmware_Version;
                    break;
                case Form_Main.MODE.WING:
                    this.Zoom = 2f;
                    this.MAX_WIDTH = 1000;
                    this.MAX_HIGHT = 1000;
                    break;
            }
            this.img_carve = new Bitmap(this.MAX_WIDTH, this.MAX_HIGHT, PixelFormat.Format24bppRgb);
            Graphics.FromImage(this.img_carve).Clear(Color.White);
            if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT || this.MACHINE_MODE == Form_Main.MODE.OLD_NOR || this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
            {
                this.panel_Laser_PWM.Visible = false;
                this.button_Send_Pic.Visible = true;
                this.button_Send_Pic.Location = new Point(596, 241);
                this.panel_direction.Location = new Point(591, 283);
                this.panel_control.Location = new Point(591, 382);
            }
            if (this.MACHINE_MODE == Form_Main.MODE.NEW_BLE || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_NOR || this.MACHINE_MODE == Form_Main.MODE.MASTER)
            {
                this.panel_Laser_PWM.Visible = true;
                this.button_Send_Pic.Visible = false;
                this.panel_Laser_PWM.Location = new Point(534, 485);
                this.panel_direction.Location = new Point(591, 243);
                this.panel_control.Location = new Point(591, 342);
            }
            if (this.MACHINE_MODE == Form_Main.MODE.NULL)
            {
                this.set_software_state(Form_Main.STATE.BAN_ALL);
                this.trackBar_carveTime.Enabled = false;
                this.trackBar_PWM.Enabled = false;
                this.tabControl1_Selected(null, null);
                return;
            }
            this.set_software_state(Form_Main.STATE.IDLE);
            this.trackBar_carveTime.Enabled = true;
            this.trackBar_PWM.Enabled = true;
            this.tabControl1_Selected(null, null);
        }

        public void addText(string Text)
        {
            this.textBox1.AppendText(Text + "\r\n");
        }

        private void Display_Picture(Bitmap image)
        {
            Bitmap bitmap = new Bitmap(500, 500);
            Graphics expr_16 = Graphics.FromImage(bitmap);
            expr_16.DrawImage(image, new Rectangle((int)((float)this.Location_x / this.Zoom), (int)((float)this.Location_y / this.Zoom), (int)((float)image.Width / this.Zoom), (int)((float)image.Height / this.Zoom)));
            expr_16.DrawRectangle(new Pen(Color.Blue, 1f), new Rectangle((int)((float)this.Location_x / this.Zoom), (int)((float)this.Location_y / this.Zoom), (int)((float)image.Width / this.Zoom) - 1, (int)((float)image.Height / this.Zoom) - 1));
            this.textBox_size.Text = ((double)image.Width * 0.075).ToString();
            Image expr_E7 = this.pictureBox1.BackgroundImage;
            if (expr_E7 != null)
            {
                expr_E7.Dispose();
            }
            this.pictureBox1.BackgroundImage = (Bitmap)bitmap.Clone();
            this.pictureBox1.Refresh();
            expr_16.Dispose();
            bitmap.Dispose();
        }

        private void image_download()
        {
            if (!Directory.Exists("C:\\NEJE"))
            {
                Directory.CreateDirectory("C:\\NEJE");
            }
            if (!Directory.Exists(this.image_buff_path))
            {
                Directory.CreateDirectory(this.image_buff_path);
            }
            WebClient webClient = new WebClient();
            foreach (string current in this.Alt[0])
            {
                string text = this.image_buff_path + "\\" + current.Substring(current.LastIndexOf("/") + 1);
                try
                {
                    if (!File.Exists(text))
                    {
                        webClient.DownloadFile(current, text);
                    }
                }
                catch
                {
                }
            }
        }

        private void Check_update()
        {
            string text = "";
            string update_link = "";
            switch (this.CURRENT_SOFTWARE_TYPE)
            {
                case Form_Main.SOFTWARE_TYPE.ALL:
                    text = Web.getWebSource("http://neje.club/CheckUpdate/general_CheckUpdate.htm");
                    update_link = "http://neje.club/CheckUpdate/general_New_Version_Software.htm";
                    break;
                case Form_Main.SOFTWARE_TYPE.KBOT:
                    return;
                case Form_Main.SOFTWARE_TYPE.NOR:
                    if (this.Is_OnlyCN)
                    {
                        text = Web.getWebSource("http://neje.club/CheckUpdate/cn_kz_CheckUpdate.htm");
                        update_link = "http://neje.club/CheckUpdate/cn_kz_New_Version_Software.htm";
                    }
                    else
                    {
                        text = Web.getWebSource("http://neje.club/CheckUpdate/en_kz_CheckUpdate.htm");
                        update_link = "http://neje.club/CheckUpdate/en_kz_New_Version_Software.htm";
                    }
                    break;
                case Form_Main.SOFTWARE_TYPE.LIT:
                    text = Web.getWebSource("http://neje.club/CheckUpdate/en_fkz_CheckUpdate.htm");
                    update_link = "http://neje.club/CheckUpdate/en_fkz_New_Version_Software.htm";
                    break;
                case Form_Main.SOFTWARE_TYPE.BLE:
                    text = Web.getWebSource("http://neje.club/CheckUpdate/en_bl_CheckUpdate.htm");
                    update_link = "http://neje.club/CheckUpdate/en_bl_New_Version_Software.htm";
                    break;
                case Form_Main.SOFTWARE_TYPE.MASTER:
                    if (this.Is_OnlyCN)
                    {
                        text = Web.getWebSource("http://neje.club/CheckUpdate/cn_btw_CheckUpdate.htm");
                        update_link = "http://neje.club/CheckUpdate/cn_btw_New_Version_Software.htm";
                    }
                    else
                    {
                        text = Web.getWebSource("http://neje.club/CheckUpdate/en_btw_CheckUpdate.htm");
                        update_link = "http://neje.club/CheckUpdate/en_btw_New_Version_Software.htm";
                    }
                    break;
            }
            if (text != "")
            {
                if (text.Contains("LATEST-SOFTWARE-VERSION"))
                {
                    text = text.Remove(0, text.IndexOf("LATEST-SOFTWARE-VERSION=") + 24);
                    this.WEB_SOFTWARE_VERSION = text.Substring(0, text.IndexOf(";"));
                    if (this.WEB_SOFTWARE_VERSION != "V4.0" && Settings.Default.Upgrade_remind)
                    {
                        Form_update form_update = new Form_update("V4.0", this.WEB_SOFTWARE_VERSION, update_link);
                        form_update.TopMost = true;
                        form_update.Location = new Point(base.Location.X + (base.Width - form_update.Width) / 2, base.Location.Y + (base.Height - form_update.Height) / 2);
                        form_update.Show();
                        return;
                    }
                }
            }
            else
            {
                this.addText("Failed to connect network, unable to check update.");
            }
        }

        private void get_web_picture_list()
        {
            List<string> list = new List<string>();
            this.Alt.Clear();
            this.Alt_Name.Clear();
            this.Alt.Add(list);
            this.comboBox_Alt.Items.Clear();
            this.comboBox_Alt.Items.Add("ALL");
            string text;
            if (this.MACHINE_MODE == Form_Main.MODE.NULL)
            {
                if (this.CURRENT_SOFTWARE_TYPE == Form_Main.SOFTWARE_TYPE.MASTER)
                {
                    text = Web.getWebSource("http://neje.club/neje_pic_2000.htm");
                }
                else
                {
                    text = Web.getWebSource("http://neje.club/neje_pic_490.htm");
                }
            }
            else if (this.MACHINE_MODE == Form_Main.MODE.MASTER)
            {
                text = Web.getWebSource("http://neje.club/neje_pic_2000.htm");
            }
            else
            {
                text = Web.getWebSource("http://neje.club/neje_pic_490.htm");
            }
            if (text != "")
            {
                while (text.IndexOf("<img src=\"") != -1)
                {
                    text = text.Remove(0, text.IndexOf("<img src=\"") + 10);
                    string item = text.Substring(0, text.IndexOf("\""));
                    list.Add(item);
                    text = text.Remove(0, text.IndexOf("alt=\"") + 5);
                    string item2 = text.Substring(0, text.IndexOf("\""));
                    if (!this.Alt_Name.Contains(item2))
                    {
                        this.Alt_Name.Add(item2);
                        this.Alt.Add(new List<string>());
                    }
                    this.Alt[this.Alt_Name.LastIndexOf(item2) + 1].Add(item);
                }
                if (this.DEBUG)
                {
                    this.addText("获取网络图片总数：" + this.Alt[0].Count.ToString());
                }
                if (this.DEBUG)
                {
                    this.addText("分类数目：" + this.Alt_Name.Count.ToString());
                }
                foreach (string current in this.Alt_Name)
                {
                    this.comboBox_Alt.Items.Add(current);
                }
                this.comboBox_Alt.SelectedIndex = 0;
                new Thread(new ThreadStart(this.image_download)).Start();
                return;
            }
            this.addText("Failed to connect network, unable to get network picture.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.comboBox_Language.SelectedIndex = Settings.Default.Language;
            this.Adjust_Window();
            this.Init_window();
            this.auto_connect();
            //this.get_web_picture_list();
            this.Load_Collection();
            //this.Check_update();
            this.check_driver_is_exists();
            this.label_software_version.Text = "Software Version : " + "V4.0".ToString();
        }

        private void check_driver_is_exists()
        {
            if (!Directory.Exists("C:\\NEJE"))
            {
                Directory.CreateDirectory("C:\\NEJE");
            }
            if (!Directory.Exists("C:\\NEJE\\Driver"))
            {
                Directory.CreateDirectory("C:\\NEJE\\Driver");
            }
            if (!File.Exists("C:\\NEJE\\Driver\\driver.exe"))
            {
                byte[] driver = Resources.driver;
                FileStream expr_4B = new FileStream("C:\\NEJE\\Driver\\driver.exe", FileMode.CreateNew);
                expr_4B.Write(driver, 0, driver.Length);
                expr_4B.Close();
            }
        }

        private void Load_Collection()
        {
            this.CollectionFileLink.Clear();
            if (!Directory.Exists("C:\\NEJE"))
            {
                Directory.CreateDirectory("C:\\NEJE");
            }
            if (!Directory.Exists("C:\\NEJE\\Collection"))
            {
                Directory.CreateDirectory("C:\\NEJE\\Collection");
            }
            FileInfo[] files = new DirectoryInfo("C:\\NEJE\\Collection").GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo arg = files[i];
                this.CollectionFileLink.Add("C:\\NEJE\\Collection\\" + arg);
            }
            if (this.Collection_page == 0)
            {
                this.Collection_page = 1;
            }
            this.showCollectionPicture(this.CollectionFileLink, this.Collection_page);
        }

        public void Send_CMD(byte D1, byte D2, byte D3)
        {
            try
            {
                if (this.serialPort1.IsOpen)
                {
                    byte[] buffer = new byte[]
                    {
                        255,
                        D1,
                        D2,
                        D3
                    };
                    this.serialPort1.Write(buffer, 0, 4);
                    this.addText($" > 255 {D1} {D2} {D3}");
                }
            }
            catch
            {
                this.addText("An error occurred in sending data.");
            }
        }

        public void Send_CMD(byte D1, byte D2, byte D3, byte D4, byte D5, byte D6)
        {
            if (this.serialPort1.IsOpen)
            {
                byte[] buffer = new byte[]
                {
                    255,
                    D1,
                    D2,
                    D3,
                    D4,
                    D5,
                    D6
                };
                this.serialPort1.Write(buffer, 0, 7);
                this.addText($" > 255 {D1} {D2} {D3} {D4} {D5} {D6}");
            }
        }

        public Form_Main()
        {
            this.InitializeComponent();
        }

        private void auto_connect()
        {
            this.serialPort1.BaudRate = 57600;
            this.portName_i = 0;
            this.timer_connect.Enabled = true;
            this.portNameArray = null;
            this.label_Connection.Text = "CONNECTING...";
            this.label_connection_logo.Image = Resources.connecting;
        }

        private void timer_connect_Tick(object sender, EventArgs e)
        {
            if (this.portName_i != 0)
            {
                if (this.DEBUG)
                {
                    this.addText("验证失败，正在尝试下一个串口...");
                }
                this.serialPort1.Close();
            }
            if (this.portNameArray == null || this.portName_i >= this.portNameArray.Length)
            {
                this.portName_i = 0;
                this.portNameArray = SerialPort.GetPortNames();
                if (this.DEBUG)
                {
                    this.addText("准备刷新数组，再来一遍...");
                }
            }
            if (this.portNameArray.Length != 0)
            {
                try
                {
                    if (this.portNameArray[this.portName_i].ToString() == "COM1")
                    {
                        this.portName_i++;
                        if (this.DEBUG)
                        {
                            this.addText("COM1 跳过验证");
                        }
                    }
                    if (this.DEBUG)
                    {
                        this.addText("准备连接：" + this.portNameArray[this.portName_i].ToString());
                    }
                    this.serialPort1.PortName = this.portNameArray[this.portName_i];
                    this.serialPort1.Open();
                    if (this.DEBUG)
                    {
                        this.addText("打开端口：" + this.portNameArray[this.portName_i].ToString() + "成功！正在验证...");
                    }
                    this.Send_CMD(9, 0, 0);
                }
                catch
                {
                    if (this.DEBUG)
                    {
                        this.addText("串口" + this.portNameArray[this.portName_i].ToString() + "打开失败");
                    }
                }
                this.portName_i++;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] array = new byte[this.serialPort1.BytesToRead];
            this.serialPort1.Read(array, 0, array.Length);
            if (this.remain_cmd != null)
            {
                this.cmd_todecode = new byte[array.Length + this.remain_cmd.Length];
                this.remain_cmd.CopyTo(this.cmd_todecode, 0);
                array.CopyTo(this.cmd_todecode, this.remain_cmd.Length);
            }
            else
            {
                this.cmd_todecode = new byte[array.Length];
                array.CopyTo(this.cmd_todecode, 0);
            }
            this.remain_cmd = null;
            if (this.cmd_todecode.Length % 4 != 0)
            {
                this.remain_cmd = new byte[this.cmd_todecode.Length % 4];
                for (int i = 0; i < this.remain_cmd.Length; i++)
                {
                    this.remain_cmd[i] = this.cmd_todecode[this.cmd_todecode.Length - this.remain_cmd.Length + i];
                }
                byte[] array2 = new byte[this.cmd_todecode.Length];
                this.cmd_todecode.CopyTo(array2, 0);
                this.cmd_todecode = new byte[array2.Length - this.remain_cmd.Length];
                for (int j = 0; j < array2.Length - this.remain_cmd.Length; j++)
                {
                    this.cmd_todecode[j] = array2[j];
                }
            }
            for (int k = 0; k < this.cmd_todecode.Length; k++)
            {
                if (this.cmd_todecode[k] == 255)
                {
                    try
                    {
                        this.decode(this.cmd_todecode[k + 1], this.cmd_todecode[k + 2], this.cmd_todecode[k + 3]);
                    }
                    catch
                    {
                        if (this.DEBUG)
                        {
                            this.addText("指令不符合格式");
                            this.addText("不符合格式的指令是：");
                            byte[] array3 = this.cmd_todecode;
                            for (int l = 0; l < array3.Length; l++)
                            {
                                byte b = array3[l];
                                this.addText(b.ToString());
                            }
                        }
                        this.remain_cmd = null;
                    }
                }
            }
        }

        public void decode(byte D1, byte D2, byte D3)
        {
            this.addText($" < 255 {D1} {D2} {D3}");
            switch (D1)
            {
                case 0:
                    this.Online = true;
                    return;
                case 1:
                    break;
                case 2:
                    this.timer_connect.Enabled = false;
                    this.label_connection_logo.Image = Resources.OK;
                    this.label_Connection.Text = "CONNECTED";
                    this.addText("CONNECTION OK !");
                    this.Connected_COM = this.serialPort1.PortName;
                    this.MACHINE_MODE = Form_Main.MODE.NULL;
                    if (D2 == 10 && D3 == 1)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.OLD_KBOT;
                        if (this.DEBUG)
                        {
                            this.addText("基础款雕刻机已连接！");
                        }
                        this.Firmware_Version = "V1.0";
                    }
                    if (D2 == 11 && D3 == 1)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.OLD_NOR;
                        if (this.DEBUG)
                        {
                            this.addText("基础款雕刻机已连接！");
                        }
                        this.Firmware_Version = "V1.0";
                    }
                    if (D2 == 13 && D3 == 1)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.OLD_LIT;
                        if (this.DEBUG)
                        {
                            this.addText("精简版已连接！");
                        }
                        this.Firmware_Version = "V1.0";
                    }
                    if (D2 == 1 && D3 == 0)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.OLD_BLE;
                        if (this.DEBUG)
                        {
                            this.addText("蓝牙雕刻机已连接！");
                        }
                        this.Firmware_Version = "V1.0";
                    }
                    if (D2 == 11 && D3 == 2)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.NEW_NOR;
                        if (this.DEBUG)
                        {
                            this.addText("新款量产版雕刻机已连接！");
                        }
                        this.Firmware_Version = "V2.0";
                    }
                    if (D2 == 13 && D3 == 2)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.NEW_LIT;
                        if (this.DEBUG)
                        {
                            this.addText("新款精简版雕刻机已连接！");
                        }
                        this.Firmware_Version = "V2.0";
                    }
                    if (D2 == 1 && D3 == 10)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.NEW_BLE;
                        if (this.DEBUG)
                        {
                            this.addText("新款蓝牙雕刻机已连接！");
                        }
                        this.Firmware_Version = "V2.0";
                    }
                    if (D2 == 14 && D3 == 1)
                    {
                        this.MACHINE_MODE = Form_Main.MODE.MASTER;
                        if (this.DEBUG)
                        {
                            this.addText("NEJE-DK-Master已连接！");
                        }
                        this.Firmware_Version = "V3.0";
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.NULL)
                    {
                        this.addText("Unrecognized machine, \r\nPlease upgrade your software!");
                        this.label_Connection.Text = "NOT SUPPORT";
                        this.label_connection_logo.Image = Resources.ERROR;
                    }
                    this.Init_window();
                    this.get_web_picture_list();
                    return;
                case 3:
                    this.disp_x = (int)(D2 * 100 + D3);
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
                    {
                        this.pointBuff.Add(new Point(this.disp_x - 1, this.disp_y));
                        return;
                    }
                    break;
                case 4:
                    this.disp_y = (int)(D2 * 100 + D3);
                    if (this.MACHINE_MODE != Form_Main.MODE.OLD_BLE)
                    {
                        this.pointBuff.Add(new Point(this.disp_x, this.disp_y));
                        return;
                    }
                    break;
                case 5:
                    if (D2 == 0 && D3 == 0)
                    {
                        this.Check_Online = false;
                        MessageBox.Show("You need to quit this software when you operate the mobile APP.");
                        base.Close();
                    }
                    if (D2 == 0 && D3 == 1)
                    {
                        this.Check_Online = false;
                        MessageBox.Show("You need to quit this software when you operate the mobile APP.");
                        base.Close();
                    }
                    if (D2 == 1 && D3 == 0)
                    {
                        this.addText("Sending picture...");
                        this.serialPort1.Write(this.img_array_to_send, 0, this.img_array_to_send.Length);
                        this.addText("Sending complete !");
                        //this.addText("Verifying...");
                        //new Thread(new ThreadStart(this.UpLoadImageToServer)).Start();
                    }
                    if (D2 == 1 && D3 == 1)
                    {
                        this.Check_Online = false;
                        this.addText("Sending picture at high speed...");
                        this.serialPort1.Write(this.img_array_to_send, 0, this.img_array_to_send.Length);
                        this.addText("Sending complete !");
                        //this.addText("Verifying...");
                        //new Thread(new ThreadStart(this.UpLoadImageToServer)).Start();
                    }
                    if (!this.form_sending.IsDisposed)
                    {
                        this.form_sending.Dispose();
                        return;
                    }
                    break;
                case 6:
                    this.addText("Engraving complete !");
                    if (this.times_at_carving < this.numericUpDown_times.Value)
                    {
                        this.addText("The next carving will be start soon...");
                        this.Send_CMD(1, 1, 0);
                        this.times_at_carving++;
                        this.show_carving_times_infomation();
                        return;
                    }
                    this.addText(this.times_at_carving.ToString() + " times engravings have been completed !");
                    this.set_software_state(Form_Main.STATE.IDLE);
                    this.times_at_carving = 1;
                    return;
                case 7:
                    if (D2 == 1)
                    {
                        this.addText("-------------------");
                        this.addText("This machine turns off automatically because of low battery. Please recharge for at least half an hour.");
                        return;
                    }
                    break;
                case 8:
                    this.addText("DEBUG:" + ((int)(D2 * 100 + D3)).ToString());
                    return;
                case 9:
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_BLE)
                    {
                        switch (D2)
                        {
                            case 0:
                                this.label_power.Text = "Battery Power : 0%";
                                return;
                            case 1:
                                this.label_power.Text = "Battery Power : 25%";
                                return;
                            case 2:
                                this.label_power.Text = "Battery Power : 50%";
                                return;
                            case 3:
                                this.label_power.Text = "Battery Power : 75%";
                                return;
                            case 4:
                                this.label_power.Text = "Battery Power : 100%";
                                return;
                            case 5:
                                this.label_power.Text = "Battery Power : 100%";
                                return;
                            default:
                                return;
                        }
                    }
                    break;
                case 10:
                    this.trackBar_carveTime.Value = (int)(D2 * 100 + D3);
                    this.label_trackbar.Text = "Burning Time: " + this.trackBar_carveTime.Value.ToString() + "mS";
                    return;
                case 11:
                    this.addText("Verify OK !");
                    if (this.serialPort1.BaudRate != 57600 && this.MACHINE_MODE != Form_Main.MODE.MASTER)
                    {
                        this.serialPort1.BaudRate = 57600;
                    }
                    this.Check_Online = true;
                    this.timer_check_send_pic.Enabled = false;
                    if (this.MACHINE_MODE == Form_Main.MODE.NEW_NOR || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_BLE || this.MACHINE_MODE == Form_Main.MODE.MASTER)
                    {
                        this.show_carving_times_infomation();
                        return;
                    }
                    break;
                case 12:
                    if (D3 == 1)
                    {
                        this.addText("ERROR! CODE:12.1");
                    }
                    if (D3 == 2)
                    {
                        this.addText("ERROR! CODE:12.2");
                    }
                    if (D3 == 3)
                    {
                        this.addText("- - - - - - - - - - - - - - - - - - - - - - - ");
                        this.addText("The inclination of the machine is too large. The engraving has been suspended for safety.");
                        if (this.SOFTWARE_STATE == Form_Main.STATE.CARVING)
                        {
                            this.set_software_state(Form_Main.STATE.AT_PAUSE);
                        }
                    }
                    if (D3 == 4)
                    {
                        this.addText("- - - - - - - - - - - - - - - - - - - - - - - ");
                        this.addText("The inclination has been restored but it is recommended to restart carving again.");
                    }
                    if (D3 == 5)
                    {
                        this.addText("Emergency stopped !");
                        MessageBox.Show("Emergency stopped ! You need to reboot this software .");
                        base.Close();
                        return;
                    }
                    break;
                case 13:
                    this.trackBar_PWM.Value = (int)((D2 * 100 + D3) / 10);
                    this.label_PWM.Text = "Laser Power : " + (this.trackBar_PWM.Value * 10).ToString() + "%";
                    return;
                case 14:
                    this.label_temperature.Text = "Laser Temperature : " + D3.ToString() + "℃";
                    return;
                case 15:
                    this.label_current.Text = "Charging Current : " + ((int)(D2 * 100 + D3)).ToString() + "mA";
                    return;
                case 16:
                    if (D2 == 1 && D3 == 0)
                    {
                        this.panel_Laser_PWM.Visible = false;
                    }
                    if (D2 == 1 && D3 == 1)
                    {
                        this.panel_Laser_PWM.Visible = true;
                    }
                    if (D2 == 2 && D3 == 0)
                    {
                        this.addText("Laser initialization errork, Unrecognized laser type.");
                    }
                    if (D2 == 2 && D3 == 1)
                    {
                        this.label_laser_length.Text = "Laser Wavelength: 450NM";
                        this.Master_Laser_Type = "4PIN";
                        this.tabControl1_Selected(null, null);
                    }
                    if (D2 == 2 && D3 == 2)
                    {
                        this.label_laser_length.Text = "Laser Wavelength: 405NM";
                        this.Master_Laser_Type = "7PIN";
                        this.tabControl1_Selected(null, null);
                    }
                    if (D2 == 2 && D3 == 3)
                    {
                        this.label_laser_length.Text = "Laser Wavelength: 450NM";
                        this.Master_Laser_Type = "7PIN";
                        this.tabControl1_Selected(null, null);
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.MASTER)
                    {
                        this.Send_CMD(14, 0, 4);
                        Thread.Sleep(10);
                        this.serialPort1.BaudRate = 750000;
                        Thread.Sleep(10);
                        return;
                    }
                    break;
                case 17:
                    this.Fan_RPM = (int)(D2 * 100 + D3);
                    this.label_fan_RPM.Text = string.Concat(new string[]
                    {
                    "Fan Speed: ",
                    this.Fan_RPM.ToString(),
                    " RPM   ",
                    this.Fan_Precent.ToString(),
                    "%"
                    });
                    return;
                case 18:
                    if (D2 == 0)
                    {
                        this.Fan_Precent = (int)D3;
                        this.label_fan_RPM.Text = string.Concat(new string[]
                        {
                        "Fan Speed: ",
                        this.Fan_RPM.ToString(),
                        " RPM     ",
                        this.Fan_Precent.ToString(),
                        "%"
                        });
                    }
                    break;
                default:
                    return;
            }
        }

        private void button_driver_Click(object sender, EventArgs e)
        {
            new Process
            {
                StartInfo =
                {
                    FileName = "C:\\NEJE\\Driver\\driver.exe"
                }
            }.Start();
        }

        private void UpLoadImageToServer()
        {
            if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT)
            {
                return;
            }
            string text = string.Concat(new string[]
            {
                "Windows_",
                "V4.0".ToString(),
                "_",
                this.comboBox_Language.Text,
                "_",
                this.MACHINE_MODE.ToString().Replace("_", "-"),
                "_",
                DateTime.Now.ToString().Replace(":", ".").Replace("/", "-"),
                ".bmp"
            });
            this.img_carve.Save(text);
            WebClient webClient = new WebClient();
            try
            {
                webClient.UploadFile("http://neje.club/Upload_NEJE_Windows.php", "POST", text);
            }
            catch
            {
            }
            File.Delete(text);
        }

        private void showNetPicture(List<string> list, int page)
        {
            int num;
            if (list.Count % 12 == 0)
            {
                num = list.Count / 12;
            }
            else
            {
                num = list.Count / 12 + 1;
            }
            this.label_page.Text = page.ToString() + "/" + num.ToString();
            TabPage tabPage = this.tabControl1.TabPages[1];
            foreach (Control control in tabPage.Controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).Image = Resources.load;
                    ((PictureBox)control).Cursor = Cursors.Hand;
                    foreach (Control control2 in ((PictureBox)control).Controls)
                    {
                        if (control2 is Label)
                        {
                            control2.Dispose();
                        }
                    }
                }
            }
            int num2 = 0;
            try
            {
                foreach (Control control3 in tabPage.Controls)
                {
                    if (control3 is PictureBox)
                    {
                        string text = list[(page - 1) * 12 + num2++];
                        string text2 = this.image_buff_path + "\\" + text.Substring(text.LastIndexOf("/") + 1);
                        if (File.Exists(text2))
                        {
                            ((PictureBox)control3).ImageLocation = text2;
                        }
                        else
                        {
                            ((PictureBox)control3).ImageLocation = text;
                        }
                    }
                }
            }
            catch
            {
                foreach (Control control4 in tabPage.Controls)
                {
                    if (control4 is PictureBox)
                    {
                        num2--;
                        if (num2 < 1)
                        {
                            ((PictureBox)control4).Image = Resources.Blank;
                            ((PictureBox)control4).Cursor = Cursors.Default;
                        }
                    }
                }
            }
        }

        private void showCollectionPicture(List<string> list, int page)
        {
            int num;
            if (list.Count % 12 == 0)
            {
                num = list.Count / 12;
            }
            else
            {
                num = list.Count / 12 + 1;
            }
            this.label_collection_pages.Text = page.ToString() + "/" + num.ToString();
            TabPage tabPage = this.tabControl1.TabPages[2];
            foreach (Control control in tabPage.Controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).Image = Resources.Blank;
                    ((PictureBox)control).Cursor = Cursors.Hand;
                    foreach (Control control2 in ((PictureBox)control).Controls)
                    {
                        if (control2 is Label)
                        {
                            control2.Dispose();
                        }
                    }
                }
            }
            int num2 = 0;
            try
            {
                foreach (Control control3 in tabPage.Controls)
                {
                    if (control3 is PictureBox)
                    {
                        string text = list[(page - 1) * 12 + num2++];
                        string text2 = this.image_buff_path + "\\" + text.Substring(text.LastIndexOf("/") + 1);
                        if (File.Exists(text2))
                        {
                            ((PictureBox)control3).ImageLocation = text2;
                        }
                        else
                        {
                            ((PictureBox)control3).ImageLocation = text;
                        }
                    }
                }
            }
            catch
            {
                foreach (Control control4 in tabPage.Controls)
                {
                    if (control4 is PictureBox)
                    {
                        num2--;
                        if (num2 < 1)
                        {
                            ((PictureBox)control4).Image = Resources.Blank;
                            ((PictureBox)control4).Cursor = Cursors.Default;
                        }
                    }
                }
            }
            GC.Collect();
        }

        private void trackBar_carveTime_MouseDown(object sender, MouseEventArgs e)
        {
            this.carvr_time_value_mouse_down = this.trackBar_carveTime.Value;
        }

        private void trackBar_carveTime_Scroll(object sender, EventArgs e)
        {
            this.label_trackbar.Text = "Burning Time : " + this.trackBar_carveTime.Value.ToString() + "mS";
        }

        private void trackBar_carveTime_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.trackBar_carveTime.Value <= 5 && MessageBox.Show("The setting of burning time is too short, the engraving effect may not be obvious, still continue ?", "Message :", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                this.trackBar_carveTime.Value = this.carvr_time_value_mouse_down;
                this.label_trackbar.Text = "Burning Time : " + this.carvr_time_value_mouse_down.ToString() + "mS";
                return;
            }
            this.label_trackbar.Text = "Burning Time : " + this.trackBar_carveTime.Value.ToString() + "mS";
            this.Send_CMD(5, BitConverter.GetBytes(this.trackBar_carveTime.Value)[0], 0);
        }

        private void trackBar_PWM_Scroll(object sender, EventArgs e)
        {
            this.label_PWM.Text = "Laser Power : " + (this.trackBar_PWM.Value * 10).ToString() + "%";
            this.Send_CMD(13, 0, (byte)(this.trackBar_PWM.Value * 10));
        }

        private void tabPage3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                return;
            }
            e.Effect = DragDropEffects.None;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void tabPage3_DragDrop(object sender, DragEventArgs e)
        {
            Bitmap image;
            try
            {
                Bitmap bitmap = new Bitmap(((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
                image = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb);
            }
            catch
            {
                this.textBox1.AppendText("Sorry! Unsupported file formats.\r\n");
                return;
            }
            this.Load_pic(image);
        }

        private void Load_pic(Bitmap image)
        {
            if (this.MACHINE_MODE == Form_Main.MODE.NULL)
            {
                this.addText("Need to connect the machine before operating the software.");
                return;
            }
            if (this.SOFTWARE_STATE == Form_Main.STATE.CARVING)
            {
                this.addText("The current engraving is not complete, please wait for the engraving to finish or pause the engraving.");
                return;
            }
            if (this.SOFTWARE_STATE == Form_Main.STATE.BAN_ALL)
            {
                this.addText("The engraving machine is not ready.");
                return;
            }
            this.set_software_state(Form_Main.STATE.AT_EDIT);
            this.numericUpDown_times.Value = decimal.One;
            this.times_at_carving = 1;
            this.img_reload = (Bitmap)image.Clone();
            this.img_black_origin = ImageProcess.getOriginPicture(image, 128, this.MAX_WIDTH, this.MAX_HIGHT);
            this.img_shake_origin = ImageProcess.getOriginPicture(image, 30, this.MAX_WIDTH, this.MAX_HIGHT);
            this.img_black = ImageProcess.toBlack(this.img_black_origin, 127);
            this.img_shake = ImageProcess.toShake(this.img_shake_origin);
            if (this.radioButton_Black.Checked)
            {
                this.img_carve = (Bitmap)this.img_black.Clone();
            }
            if (this.radioButton_Shake.Checked)
            {
                this.img_carve = (Bitmap)this.img_shake.Clone();
            }
            this.Location_x = (this.MAX_WIDTH - this.img_carve.Width) / 2;
            this.Location_y = (this.MAX_HIGHT - this.img_carve.Height) / 2;
            image.Dispose();
            this.Display_Picture(this.img_carve);
            if (this.DEBUG)
            {
                this.addText("图片处理完成！");
            }
            this.panel_size.Visible = true;
            this.pictureBox1.Cursor = Cursors.SizeAll;
            this.NEW_IMAGE = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            if (this.pictureBox1.Cursor == Cursors.SizeAll && e.Button == MouseButtons.Left)
            {
                this.cursor_x_start = Cursor.Position.X;
                this.cursor_y_start = Cursor.Position.Y;
                this.cursor_x_offset = 0;
                this.cursor_y_offset = 0;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            if (this.pictureBox1.Cursor == Cursors.SizeAll && e.Button == MouseButtons.Left)
            {
                this.Location_x -= this.cursor_x_offset;
                this.Location_y -= this.cursor_y_offset;
                this.cursor_x_offset = (int)((float)(Cursor.Position.X - this.cursor_x_start) * this.Zoom);
                this.cursor_y_offset = (int)((float)(Cursor.Position.Y - this.cursor_y_start) * this.Zoom);
                this.Location_x += this.cursor_x_offset;
                this.Location_y += this.cursor_y_offset;
                if (this.Location_x < 0)
                {
                    this.Location_x = 0;
                }
                if (this.Location_y < 0)
                {
                    this.Location_y = 0;
                }
                if (this.Location_x + this.img_carve.Width > this.MAX_WIDTH)
                {
                    this.Location_x = this.MAX_WIDTH - this.img_carve.Width;
                }
                if (this.Location_y + this.img_carve.Height > this.MAX_HIGHT)
                {
                    this.Location_y = this.MAX_HIGHT - this.img_carve.Height;
                }
                this.Display_Picture(this.img_carve);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.checkBox_Insert.Checked)
            {
                this.set_software_state(Form_Main.STATE.AT_EDIT);
                int num = (int)((float)e.Location.X * this.Zoom - (float)(this.stringimg.Width / 2));
                int num2 = (int)((float)e.Location.Y * this.Zoom - (float)(this.stringimg.Height / 2));
                int location_x = this.Location_x;
                int location_y = this.Location_y;
                int num3 = num + this.stringimg.Width;
                int num4 = num2 + this.stringimg.Height;
                int num5 = location_x + this.img_carve.Width;
                int num6 = location_y + this.img_carve.Height;
                int num7;
                if (num < location_x)
                {
                    num7 = num;
                }
                else
                {
                    num7 = location_x;
                }
                int num8;
                if (num2 < location_y)
                {
                    num8 = num2;
                }
                else
                {
                    num8 = location_y;
                }
                int num9;
                if (num3 < num5)
                {
                    num9 = num5;
                }
                else
                {
                    num9 = num3;
                }
                int num10;
                if (num4 < num6)
                {
                    num10 = num6;
                }
                else
                {
                    num10 = num4;
                }
                int num11 = num9 - num7;
                int num12 = num10 - num8;
                if (num11 > this.MAX_WIDTH)
                {
                    num11 = this.MAX_WIDTH;
                }
                if (num12 > this.MAX_WIDTH)
                {
                    num12 = this.MAX_WIDTH;
                }
                Bitmap bitmap = new Bitmap(num11, num12, PixelFormat.Format24bppRgb);
                Graphics expr_132 = Graphics.FromImage(bitmap);
                expr_132.Clear(Color.White);
                expr_132.DrawImage(this.img_carve, location_x - num7, location_y - num8);
                expr_132.DrawImage(this.stringimg, num - num7, num2 - num8);
                this.img_carve.Dispose();
                this.img_carve = (Bitmap)bitmap.Clone();
                this.Location_x = (this.MAX_WIDTH - this.img_carve.Width) / 2;
                this.Location_y = (this.MAX_HIGHT - this.img_carve.Height) / 2;
                if (!this.NEW_IMAGE)
                {
                    this.Load_pic(this.img_carve);
                }
                this.Display_Picture(this.img_carve);
                this.checkBox_Insert.Checked = false;
                this.pictureBox1.Cursor = Cursors.SizeAll;
                if (this.radioButton_Black.Checked)
                {
                    this.img_black = (Bitmap)this.img_carve.Clone();
                }
                if (this.radioButton_Shake.Checked)
                {
                    this.img_shake = (Bitmap)this.img_carve.Clone();
                }
                return;
            }
            if (this.pictureBox1.Cursor == Cursors.SizeAll)
            {
                if (e.Button == MouseButtons.Left && (this.MACHINE_MODE == Form_Main.MODE.NEW_NOR || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_BLE || this.MACHINE_MODE == Form_Main.MODE.MASTER))
                {
                    this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                    this.Send_CMD(110, 2, (byte)(this.img_carve.Width / 100), (byte)(this.img_carve.Width % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
                }
                return;
            }
            if (this.pictureBox1.Cursor == Cursors.Cross)
            {
                this.addText("Laser AT: " + e.Location.X.ToString() + "," + e.Location.Y.ToString());
                this.Send_CMD(10, (byte)((float)e.Location.X * this.Zoom / 100f), (byte)((float)e.Location.X * this.Zoom % 100f));
                this.Send_CMD(11, (byte)((float)e.Location.Y * this.Zoom / 100f), (byte)((float)e.Location.Y * this.Zoom % 100f));
                return;
            }
        }

        private void timer_isconnection_ok_check_Tick(object sender, EventArgs e)
        {
            if (this.Connected_COM == null)
            {
                return;
            }
            this.portNameArray = SerialPort.GetPortNames();
            if (this.portNameArray.Contains(this.Connected_COM))
            {
                this.time++;
                if (this.time % 3 == 0 && this.MACHINE_MODE == Form_Main.MODE.NEW_BLE && this.Check_Online)
                {
                    if (!this.Online)
                    {
                        this.label_Connection.Text = "POWER-OFF";
                        this.label_connection_logo.Image = Resources.PowerOff;
                        this.set_software_state(Form_Main.STATE.BAN_ALL);
                        this.pictureBox1.BackgroundImage = null;
                    }
                    else
                    {
                        if (this.label_Connection.Text == "POWER-OFF")
                        {
                            this.set_software_state(Form_Main.STATE.IDLE);
                        }
                        this.label_Connection.Text = "CONNECTED";
                        this.label_connection_logo.Image = Resources.OK;
                    }
                    this.Send_CMD(0, 0, 0);
                    this.Online = false;
                    return;
                }
            }
            else
            {
                this.addText("CONNECTION BREAK! \r\nRetrying...");
                this.Connected_COM = null;
                this.auto_connect();
                this.MACHINE_MODE = Form_Main.MODE.NULL;
                this.Init_window();
                this.Online = true;
            }
        }

        private void timer_picturebox_refresh_Tick(object sender, EventArgs e)
        {
            if (this.pointBuff.Count == 0)
            {
                return;
            }
            Bitmap bitmap = new Bitmap(500, 500);
            Graphics graphics = Graphics.FromImage(bitmap);
            while (this.pointBuff.Count != 0)
            {
                try
                {
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT || this.MACHINE_MODE == Form_Main.MODE.OLD_NOR || this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
                    {
                        this.img_carve.SetPixel(this.pointBuff[0].X - this.Location_x, this.pointBuff[0].Y - this.Location_y, Color.Red);
                    }
                    else
                    {
                        this.img_carve.SetPixel(this.pointBuff[0].X - this.Location_x, this.pointBuff[0].Y - this.Location_y, Color.Red);
                    }
                }
                catch
                {
                    if (this.pointBuff[0].Y != 0 && this.DEBUG)
                    {
                        this.addText("收到错误的点阵数据：" + (this.pointBuff[0].X - this.Location_x).ToString() + "," + (this.pointBuff[0].Y - this.Location_y).ToString());
                    }
                }
                this.pointBuff.RemoveAt(0);
            }
            graphics.DrawImage(this.img_carve, new Rectangle((int)((float)this.Location_x / this.Zoom), (int)((float)this.Location_y / this.Zoom), (int)((float)this.img_carve.Width / this.Zoom), (int)((float)this.img_carve.Height / this.Zoom)));
            Image expr_1D9 = this.pictureBox1.BackgroundImage;
            if (expr_1D9 != null)
            {
                expr_1D9.Dispose();
            }
            this.pictureBox1.BackgroundImage = (Bitmap)bitmap.Clone();
            this.pictureBox1.Refresh();
            graphics.Dispose();
            bitmap.Dispose();
        }

        private void pictureBox_load_Click(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Cursor == Cursors.Default)
            {
                return;
            }
            Bitmap bitmap = new Bitmap(((PictureBox)sender).Image);
            bitmap = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb);
            this.Load_pic(bitmap);
            this.tabControl1.SelectTab(3);
        }

        private void button_pre_page_Click(object sender, EventArgs e)
        {
            if (this.page == 1)
            {
                return;
            }
            this.page--;
            this.showNetPicture(this.Alt[this.comboBox_Alt.SelectedIndex], this.page);
        }

        private void button_next_page_Click(object sender, EventArgs e)
        {
            int num;
            if (this.Alt[this.comboBox_Alt.SelectedIndex].Count % 12 == 0)
            {
                num = this.Alt[this.comboBox_Alt.SelectedIndex].Count / 12;
            }
            else
            {
                num = this.Alt[this.comboBox_Alt.SelectedIndex].Count / 12 + 1;
            }
            if (this.page == num)
            {
                return;
            }
            this.page++;
            this.showNetPicture(this.Alt[this.comboBox_Alt.SelectedIndex], this.page);
        }

        private void button_collection_pre_Click(object sender, EventArgs e)
        {
            if (this.Collection_page == 1)
            {
                return;
            }
            this.Collection_page--;
            this.showCollectionPicture(this.CollectionFileLink, this.Collection_page);
        }

        private void button_collection_next_Click(object sender, EventArgs e)
        {
            int num;
            if (this.CollectionFileLink.Count % 12 == 0)
            {
                num = this.CollectionFileLink.Count / 12;
            }
            else
            {
                num = this.CollectionFileLink.Count / 12 + 1;
            }
            if (this.Collection_page == num)
            {
                return;
            }
            this.Collection_page++;
            this.showCollectionPicture(this.CollectionFileLink, this.Collection_page);
        }

        private void comboBox_Alt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DEBUG)
            {
                this.addText("当前分类名称：" + this.comboBox_Alt.SelectedItem.ToString());
            }
            if (this.DEBUG)
            {
                this.addText("当前类目下图片数量：" + this.Alt[this.comboBox_Alt.SelectedIndex].Count.ToString());
            }
            this.page = 1;
            this.showNetPicture(this.Alt[this.comboBox_Alt.SelectedIndex], this.page);
        }

        private void comboBox_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Current_language = (Form_Main.LANG)this.comboBox_Language.SelectedIndex;
            this.set_language(this.Current_language);
            Settings.Default.Language = (int)this.Current_language;
            Settings.Default.Save();
            this.tabControl1_Selected(null, null);
        }

        private void button_Send_Pic_Click(object sender, EventArgs e)
        {
            this.NEW_IMAGE = false;
            this.Check_Online = false;
            this.timer_check_send_pic.Enabled = true;
            if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT || this.MACHINE_MODE == Form_Main.MODE.OLD_NOR || this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
            {
                this.set_software_state(Form_Main.STATE.BEFORE_CARVING);
            }
            else
            {
                this.set_software_state(Form_Main.STATE.CARVING);
            }
            this.groupBox_InsertText.Enabled = false;
            this.radioButton_Black.Enabled = false;
            this.radioButton_Shake.Enabled = false;
            this.panel_size.Visible = false;
            this.pictureBox1.Cursor = Cursors.Default;
            this.form_sending = new Form_Loading();
            this.form_sending.TopMost = true;
            this.form_sending.Location = new Point(base.Location.X + (base.Width - this.form_sending.Width) / 2, base.Location.Y + (base.Height - this.form_sending.Height) / 2);
            //this.form_sending.Show();
            switch (this.MACHINE_MODE)
            {
                case Form_Main.MODE.OLD_KBOT:
                case Form_Main.MODE.OLD_NOR:
                case Form_Main.MODE.OLD_LIT:
                    {
                        this.img_array_to_send = new byte[32768];
                        Bitmap bitmap = new Bitmap(512, 512, PixelFormat.Format24bppRgb);
                        Graphics expr_169 = Graphics.FromImage(bitmap);
                        expr_169.Clear(Color.White);
                        expr_169.DrawImage(this.img_carve, new Point(this.Location_x, this.Location_y));
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, 512, 512), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                        IntPtr arg_1D2_0 = bitmapData.Scan0;
                        int num = bitmapData.Stride * bitmapData.Height;
                        byte[] array = new byte[num];
                        Marshal.Copy(arg_1D2_0, array, 0, num);
                        for (int i = 0; i < bitmap.Height; i++)
                        {
                            for (int j = 0; j < bitmap.Width; j++)
                            {
                                int num2 = i * bitmapData.Stride + j * 3;
                                if (array[num2] == 0)
                                {
                                    byte[] expr_210_cp_0 = this.img_array_to_send;
                                    int expr_210_cp_1 = i * 64 + j / 8;
                                    expr_210_cp_0[expr_210_cp_1] |= (byte)(128 >> j % 8);
                                }
                            }
                        }
                        this.Send_CMD(6, 1, 0);
                        break;
                    }
                case Form_Main.MODE.OLD_BLE:
                    {
                        this.img_array_to_send = new byte[39744];
                        Bitmap bitmap2 = new Bitmap(576, 552, PixelFormat.Format24bppRgb);
                        Graphics expr_27E = Graphics.FromImage(bitmap2);
                        expr_27E.Clear(Color.White);
                        expr_27E.DrawImage(this.img_carve, new Point(this.Location_x, this.Location_y));
                        BitmapData bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, 576, 552), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
                        IntPtr arg_2ED_0 = bitmapData2.Scan0;
                        int num3 = bitmapData2.Stride * bitmapData2.Height;
                        byte[] array2 = new byte[num3];
                        Marshal.Copy(arg_2ED_0, array2, 0, num3);
                        for (int k = 0; k < bitmap2.Height; k++)
                        {
                            for (int l = 0; l < bitmap2.Width; l++)
                            {
                                int num4 = k * bitmapData2.Stride + l * 3;
                                if (array2[num4] == 0)
                                {
                                    byte[] expr_32C_cp_0 = this.img_array_to_send;
                                    int expr_32C_cp_1 = k * 72 + l / 8;
                                    expr_32C_cp_0[expr_32C_cp_1] |= (byte)(128 >> l % 8);
                                }
                            }
                        }
                        this.Send_CMD(6, 1, 0);
                        break;
                    }
                case Form_Main.MODE.NEW_NOR:
                case Form_Main.MODE.NEW_LIT:
                case Form_Main.MODE.NEW_BLE:
                    {
                        int num5;
                        if (this.img_carve.Width % 8 == 0)
                        {
                            num5 = this.img_carve.Width / 8;
                        }
                        else
                        {
                            num5 = this.img_carve.Width / 8 + 1;
                        }
                        this.img_array_to_send = new byte[num5 * this.img_carve.Height];
                        if (this.DEBUG)
                        {
                            this.addText("Location_x: " + this.Location_x.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("Location_y: " + this.Location_y.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("num5 * 8: " + (num5 * 8).ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("img_carve.Height: " + this.img_carve.Height.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("img_array_to_send.Length: " + this.img_array_to_send.Length.ToString() + "B");
                        }
                        Bitmap bitmap3 = new Bitmap(num5 * 8, this.img_carve.Height, PixelFormat.Format24bppRgb);
                        Graphics graphics = Graphics.FromImage(bitmap3);
                        graphics.Clear(Color.White);
                        graphics.DrawImage(this.img_carve, 0, 0);
                        BitmapData bitmapData3 = bitmap3.LockBits(new Rectangle(0, 0, num5 * 8, this.img_carve.Height), ImageLockMode.ReadOnly, bitmap3.PixelFormat);
                        IntPtr pixelData = bitmapData3.Scan0;
                        int num6 = bitmapData3.Stride * bitmapData3.Height;
                        byte[] array3 = new byte[num6];
                        Marshal.Copy(pixelData, array3, 0, num6);
                        for (int x = 0; x < bitmap3.Height; x++)
                        {
                            for (int y = 0; y < bitmap3.Width; y++)
                            {
                                int num7 = x * bitmapData3.Stride + y * 3;
                                if (array3[num7] == 0)
                                {
                                    byte[] expr_544_cp_0 = this.img_array_to_send;
                                    int expr_544_cp_1 = x * num5 + y / 8;
                                    expr_544_cp_0[expr_544_cp_1] |= (byte)(128 >> y % 8);
                                }
                            }
                        }
                        this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                        this.Send_CMD(110, 2, (byte)(num5 * 8 / 100), (byte)(num5 * 8 % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
                        this.Send_CMD(14, 0, 1);
                        Thread.Sleep(10);
                        this.serialPort1.BaudRate = 115200;
                        Thread.Sleep(10);
                        this.Send_CMD(6, 1, 1);
                        break;
                    }
                case Form_Main.MODE.MASTER:
                    {
                        int num8;
                        if (this.img_carve.Width % 8 == 0)
                        {
                            num8 = this.img_carve.Width / 8;
                        }
                        else
                        {
                            num8 = this.img_carve.Width / 8 + 1;
                        }
                        this.img_array_to_send = new byte[num8 * this.img_carve.Height];
                        if (this.DEBUG)
                        {
                            this.addText("位置X：" + this.Location_x.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("位置Y：" + this.Location_y.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("图片宽度：" + (num8 * 8).ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("图片高度：" + this.img_carve.Height.ToString());
                        }
                        if (this.DEBUG)
                        {
                            this.addText("数据长度是：" + this.img_array_to_send.Length.ToString() + "字节");
                        }
                        Bitmap bitmap4 = new Bitmap(num8 * 8, this.img_carve.Height, PixelFormat.Format24bppRgb);
                        Graphics expr_748 = Graphics.FromImage(bitmap4);
                        expr_748.Clear(Color.White);
                        expr_748.DrawImage(this.img_carve, 0, 0);
                        BitmapData bitmapData4 = bitmap4.LockBits(new Rectangle(0, 0, num8 * 8, this.img_carve.Height), ImageLockMode.ReadOnly, bitmap4.PixelFormat);
                        IntPtr arg_7AD_0 = bitmapData4.Scan0;
                        int num9 = bitmapData4.Stride * bitmapData4.Height;
                        byte[] array4 = new byte[num9];
                        Marshal.Copy(arg_7AD_0, array4, 0, num9);
                        for (int num10 = 0; num10 < bitmap4.Height; num10++)
                        {
                            for (int num11 = 0; num11 < bitmap4.Width; num11++)
                            {
                                if (num10 % 2 == 0)
                                {
                                    int num12 = num10 * bitmapData4.Stride + num11 * 3;
                                    if (array4[num12] == 0)
                                    {
                                        byte[] expr_7F8_cp_0 = this.img_array_to_send;
                                        int expr_7F8_cp_1 = num10 * num8 + num11 / 8;
                                        expr_7F8_cp_0[expr_7F8_cp_1] |= (byte)(128 >> num11 % 8);
                                    }
                                }
                                else
                                {
                                    int num12 = (num10 + 1) * bitmapData4.Stride - (num11 + 1) * 3;
                                    if (array4[num12] == 0)
                                    {
                                        byte[] expr_83E_cp_0 = this.img_array_to_send;
                                        int expr_83E_cp_1 = num10 * num8 + num11 / 8;
                                        expr_83E_cp_0[expr_83E_cp_1] |= (byte)(128 >> num11 % 8);
                                    }
                                }
                            }
                        }
                        this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                        this.Send_CMD(110, 2, (byte)(num8 * 8 / 100), (byte)(num8 * 8 % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
                        this.Send_CMD(6, 1, 1);
                        break;
                    }
            }
            GC.Collect();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (this.NEW_IMAGE)
            {
                if (this.MACHINE_MODE != Form_Main.MODE.NEW_NOR && this.MACHINE_MODE != Form_Main.MODE.NEW_LIT && this.MACHINE_MODE != Form_Main.MODE.NEW_BLE && this.MACHINE_MODE != Form_Main.MODE.MASTER)
                {
                    this.addText("Please send pictures before starting engraving...");
                    return;
                }
                this.button_Send_Pic_Click(null, null);
            }
            else
            {
                this.show_carving_times_infomation();
                this.set_software_state(Form_Main.STATE.CARVING);
                this.Send_CMD(1, 1, 0);
            }
            this.panel_size.Visible = false;
            this.pictureBox1.Cursor = Cursors.Default;
            this.button_preview.Text = "";
            this.button_preview.BackgroundImage = Resources.preview;
            this.button_anypoint_location.Text = "";
            this.button_anypoint_location.BackgroundImage = Resources.anypoint;
        }

        private void show_carving_times_infomation()
        {
            string text = "carving tasks: " + this.numericUpDown_times.Value.ToString() + " time(s)";
            string text2 = "at carving: " + this.times_at_carving.ToString();
            switch (this.times_at_carving % 10)
            {
                case 1:
                    text2 += "st";
                    break;
                case 2:
                    text2 += "nd";
                    break;
                case 3:
                    text2 += "rd";
                    break;
                default:
                    text2 += "th";
                    break;
            }
            this.addText("**************************");
            this.addText(text);
            this.addText(text2);
            this.addText("**************************");
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT || this.MACHINE_MODE == Form_Main.MODE.OLD_NOR || this.MACHINE_MODE == Form_Main.MODE.OLD_LIT || this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
            {
                if (sender == this.button_up)
                {
                    this.Send_CMD(3, 1, 0);
                }
                if (sender == this.button_down)
                {
                    this.Send_CMD(3, 2, 0);
                }
                if (sender == this.button_left)
                {
                    this.Send_CMD(3, 3, 0);
                }
                if (sender == this.button_right)
                {
                    this.Send_CMD(3, 4, 0);
                    return;
                }
            }
            else
            {
                if (sender == this.button_up)
                {
                    this.Location_y -= 4;
                }
                if (sender == this.button_down)
                {
                    this.Location_y += 4;
                }
                if (sender == this.button_left)
                {
                    this.Location_x -= 4;
                }
                if (sender == this.button_right)
                {
                    this.Location_x += 4;
                }
                if (this.Location_x < 0)
                {
                    this.Location_x = 0;
                }
                if (this.Location_y < 0)
                {
                    this.Location_y = 0;
                }
                if (this.Location_x + this.img_carve.Width > this.MAX_WIDTH)
                {
                    this.Location_x = this.MAX_WIDTH - this.img_carve.Width;
                }
                if (this.Location_y + this.img_carve.Height > this.MAX_HIGHT)
                {
                    this.Location_y = this.MAX_HIGHT - this.img_carve.Height;
                }
                this.Display_Picture(this.img_carve);
                this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                this.Send_CMD(110, 2, (byte)(this.img_carve.Width / 100), (byte)(this.img_carve.Width % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
            }
        }

        private void checkBox_Insert_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox_Insert.Checked)
            {
                this.pictureBox1.Cursor = Cursors.Arrow;
                return;
            }
            this.textBox_Edit.Text = this.textBox_Edit.Text.Trim();
            if (this.textBox_Edit.Text.Length == 0)
            {
                this.checkBox_Insert.Checked = false;
                return;
            }
            this.stringimg = (Bitmap)this.GenerateImageFromString(this.textBox_Edit.Text, Settings.Default.Font).Clone();
            Bitmap bitmap = new Bitmap((int)((float)this.stringimg.Width / this.Zoom), (int)((float)this.stringimg.Height / this.Zoom));
            Graphics.FromImage(bitmap).DrawImage(this.stringimg, 0, 0, bitmap.Width, bitmap.Height);
            this.pictureBox1.Cursor = new Cursor(bitmap.GetHicon());
        }

        private Bitmap GenerateImageFromString(string Str, Font strFont)
        {
            SizeF sizeF = base.CreateGraphics().MeasureString(Str, strFont);
            Bitmap expr_36 = new Bitmap(sizeF.ToSize().Width, sizeF.ToSize().Height, PixelFormat.Format24bppRgb);
            Graphics expr_3C = Graphics.FromImage(expr_36);
            expr_3C.Clear(Color.White);
            expr_3C.DrawString(Str, strFont, new SolidBrush(Color.Black), 1f, 2f);
            return ImageProcess.toBlack(expr_36, 200);
        }

        private void button_Font_Click(object sender, EventArgs e)
        {
            this.fontDialog_Edit.Font = Settings.Default.Font;
            if (this.fontDialog_Edit.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.Font = this.fontDialog_Edit.Font;
                Settings.Default.Save();
            }
        }

        private void button_preview_Click(object sender, EventArgs e)
        {
            if (this.button_preview.Text == "")
            {
                if (this.MACHINE_MODE == Form_Main.MODE.NEW_NOR || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_BLE || this.MACHINE_MODE == Form_Main.MODE.MASTER)
                {
                    this.pictureBox1.Cursor = Cursors.SizeAll;
                    int num;
                    if (this.img_carve.Width % 8 == 0)
                    {
                        num = this.img_carve.Width / 8;
                    }
                    else
                    {
                        num = this.img_carve.Width / 8 + 1;
                    }
                    this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                    this.Send_CMD(110, 2, (byte)(num * 8 / 100), (byte)(num * 8 % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
                }
                this.Send_CMD(2, 2, 0);
                this.button_preview.Text = ">STOP<";
                this.button_preview.BackgroundImage = null;
                this.button_anypoint_location.Text = "";
                this.button_anypoint_location.BackgroundImage = Resources.anypoint;
                return;
            }
            this.pictureBox1.Cursor = Cursors.Default;
            this.Send_CMD(10, 0, 0);
            this.Send_CMD(11, 0, 0);
            this.Send_CMD(2, 1, 0);
            this.button_preview.Text = "";
            this.button_preview.BackgroundImage = Resources.preview;
        }

        private void button_anypoint_location_Click(object sender, EventArgs e)
        {
            if (this.button_anypoint_location.Text == "")
            {
                this.pictureBox1.Cursor = Cursors.Cross;
                this.button_anypoint_location.Text = ">EXIT<";
                this.button_anypoint_location.BackgroundImage = null;
                this.button_preview.Text = "";
                this.button_preview.BackgroundImage = Resources.preview;
                return;
            }
            this.pictureBox1.Cursor = Cursors.SizeAll;
            this.button_anypoint_location.Text = "";
            this.button_anypoint_location.BackgroundImage = Resources.anypoint;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            this.set_software_state(Form_Main.STATE.IDLE);
            this.pictureBox1.BackgroundImage = new Bitmap(10, 10);
            this.pictureBox1.Cursor = Cursors.Default;
            this.Send_CMD(4, 1, 0);
        }

        private void button_reload_Click(object sender, EventArgs e)
        {
            if (this.img_reload == null)
            {
                return;
            }
            this.Load_pic(this.img_reload);
        }

        private void textBox_Edit_Click(object sender, EventArgs e)
        {
            if (this.textBox_Edit.Tag == null)
            {
                this.textBox_Edit.Tag = "A";
                this.textBox_Edit.Text = "";
            }
        }

        private void contextMenu1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string filename = "C:\\NEJE\\Collection\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + DateTime.Now.ToLongTimeString().ToString().Replace(":", "") + ".bmp";
            if ((PictureBox)((ContextMenuStrip)sender).SourceControl == this.pictureBox1)
            {
                if (this.radioButton_Black.Checked && this.img_black != null)
                {
                    this.Collection_Picture_Need_To_Refresh = true;
                    this.img_black.Save(filename);
                    this.addText("add to my collection success!");
                }
                if (this.radioButton_Shake.Checked && this.img_shake != null)
                {
                    this.Collection_Picture_Need_To_Refresh = true;
                    this.img_shake.Save(filename);
                    this.addText("add to my collection success!");
                    return;
                }
            }
            else if (((PictureBox)((ContextMenuStrip)sender).SourceControl).Image != null)
            {
                this.Collection_Picture_Need_To_Refresh = true;
                ((PictureBox)((ContextMenuStrip)sender).SourceControl).Image.Save(filename);
                this.addText("add to my collection success!");
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                if (this.MACHINE_MODE == Form_Main.MODE.NULL)
                {
                    this.Adjust_Window();
                }
                else
                {
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_KBOT)
                    {
                        this.webBrowser1.Navigate("http://www.kbot.vip/help.html");
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_NOR)
                    {
                        if (this.Current_language == Form_Main.LANG.CN)
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/cn_kz.htm");
                        }
                        else
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/en_kz.htm");
                        }
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_LIT)
                    {
                        if (this.Current_language == Form_Main.LANG.CN)
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/cn_fkz.htm");
                        }
                        else
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/en_fkz.htm");
                        }
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.OLD_BLE)
                    {
                        this.webBrowser1.Navigate("http://neje.club/tutorials/en_bl.htm");
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.NEW_NOR)
                    {
                        if (this.Current_language == Form_Main.LANG.CN)
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/cn_kz.htm");
                        }
                        else
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/en_kz.htm");
                        }
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.NEW_LIT)
                    {
                        if (this.Current_language == Form_Main.LANG.CN)
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/cn_fkz.htm");
                        }
                        else
                        {
                            this.webBrowser1.Navigate("http://neje.club/tutorials/en_fkz.htm");
                        }
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.NEW_BLE)
                    {
                        this.webBrowser1.Navigate("http://neje.club/tutorials/en_bl.htm");
                    }
                    if (this.MACHINE_MODE == Form_Main.MODE.MASTER)
                    {
                        if (this.Current_language == Form_Main.LANG.CN)
                        {
                            if (this.Master_Laser_Type == "4PIN")
                            {
                                this.webBrowser1.Navigate("http://neje.club/tutorials/cn_btw.htm");
                            }
                            if (this.Master_Laser_Type == "7PIN")
                            {
                                this.webBrowser1.Navigate("http://neje.club/tutorials/en_master.htm");
                            }
                        }
                        else
                        {
                            if (this.Master_Laser_Type == "4PIN")
                            {
                                this.webBrowser1.Navigate("http://neje.club/tutorials/en_btw.htm");
                            }
                            if (this.Master_Laser_Type == "7PIN")
                            {
                                this.webBrowser1.Navigate("http://neje.club/tutorials/en_master.htm");
                            }
                        }
                    }
                }
            }
            if (this.tabControl1.SelectedIndex == 2 && this.Collection_Picture_Need_To_Refresh)
            {
                this.Collection_Picture_Need_To_Refresh = false;
                this.Load_Collection();
            }
        }

        private void contextMenu2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (((PictureBox)((ContextMenuStrip)sender).SourceControl).Cursor == Cursors.Default)
            {
                this.addText("There is no need to delete the picture.");
                return;
            }
            File.Delete(((PictureBox)((ContextMenuStrip)sender).SourceControl).ImageLocation);
            this.Load_Collection();
        }

        private void timer_check_send_pic_Tick(object sender, EventArgs e)
        {
            this.timer_check_send_pic.Enabled = false;
            this.addText("");
            this.addText("");
            this.addText("---ERROR---");
            this.addText("A serious error occurred while sending the picture.");
            this.addText("Do the following steps can return to normal.");
            this.addText("1.Re-power the machine.");
            this.addText("2.Restart this software.");
            this.set_software_state(Form_Main.STATE.BAN_ALL);
        }

        private void pictureBox2_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Label label = new Label();
            ((PictureBox)sender).Controls.Add(label);
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.BackColor = Color.White;
            label.Text = ((PictureBox)sender).Image.Width.ToString() + "*" + ((PictureBox)sender).Image.Height.ToString();
            label.Location = new Point(((PictureBox)sender).Width - label.Width - 5, ((PictureBox)sender).Height - 20);
            if (((PictureBox)sender).Cursor == Cursors.Default)
            {
                ((PictureBox)sender).Image = Resources.Blank;
                label.Dispose();
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            this.NEW_IMAGE = false;
            Graphics.FromImage(this.img_carve).Clear(Color.White);
            this.pictureBox1.BackgroundImage = Resources.Blank;
            this.pictureBox1.Cursor = Cursors.Default;
        }

        private void textBox_Edit_DoubleClick(object sender, EventArgs e)
        {
            this.textBox_Edit.Text = "";
        }

        private void radioButton_Black_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_Black.Checked)
            {
                this.img_carve = (Bitmap)this.img_black.Clone();
                this.Location_x = this.MAX_WIDTH / 2 - this.img_carve.Width / 2;
                this.Location_y = this.MAX_HIGHT / 2 - this.img_carve.Height / 2;
                this.Display_Picture(this.img_carve);
            }
            if (this.radioButton_Shake.Checked)
            {
                this.img_carve = (Bitmap)this.img_shake.Clone();
                this.Location_x = this.MAX_WIDTH / 2 - this.img_carve.Width / 2;
                this.Location_y = this.MAX_HIGHT / 2 - this.img_carve.Height / 2;
                this.Display_Picture(this.img_carve);
            }
        }

        private void button_size_Click(object sender, EventArgs e)
        {
            if (this.img_black == null || this.img_shake == null)
            {
                return;
            }
            if (this.radioButton_Black.Checked)
            {
                int num = (int)((double)float.Parse(this.textBox_size.Text) / 0.075);
                if (num > this.MAX_WIDTH)
                {
                    num = this.MAX_WIDTH;
                }
                if (this.img_black_origin.Height * num / this.img_black_origin.Width > this.MAX_HIGHT)
                {
                    num = this.img_black_origin.Width * this.MAX_HIGHT / this.img_black_origin.Height;
                }
                Bitmap bitmap = new Bitmap(num, this.img_black_origin.Height * num / this.img_black_origin.Width, PixelFormat.Format24bppRgb);
                Graphics.FromImage(bitmap).DrawImage(this.img_black_origin, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                this.img_black = ImageProcess.toBlack(bitmap, 127);
                this.img_carve = this.img_black;
                this.label_size.Text = "mm " + Math.Round((double)((float)this.img_carve.Width / (float)this.img_black_origin.Width * 100f), 0).ToString() + "%";
            }
            if (this.radioButton_Shake.Checked)
            {
                int num2 = (int)((double)float.Parse(this.textBox_size.Text) / 0.075);
                if (num2 > this.MAX_WIDTH)
                {
                    num2 = this.MAX_WIDTH;
                }
                if (this.img_shake_origin.Height * num2 / this.img_shake_origin.Width > this.MAX_HIGHT)
                {
                    num2 = this.img_shake_origin.Width * this.MAX_HIGHT / this.img_shake_origin.Height;
                }
                Bitmap bitmap2 = new Bitmap(num2, this.img_shake_origin.Height * num2 / this.img_shake_origin.Width, PixelFormat.Format24bppRgb);
                Graphics.FromImage(bitmap2).DrawImage(this.img_shake_origin, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
                this.img_shake = ImageProcess.toShake(bitmap2);
                this.img_carve = this.img_shake;
                this.label_size.Text = "mm " + Math.Round((double)((float)this.img_carve.Width / (float)this.img_black_origin.Width * 100f), 0).ToString() + "%";
            }
            this.Location_x = this.MAX_WIDTH / 2 - this.img_carve.Width / 2;
            this.Location_y = this.MAX_HIGHT / 2 - this.img_carve.Height / 2;
            this.Display_Picture(this.img_carve);
            if (this.MACHINE_MODE == Form_Main.MODE.NEW_NOR || this.MACHINE_MODE == Form_Main.MODE.NEW_LIT || this.MACHINE_MODE == Form_Main.MODE.NEW_BLE || this.MACHINE_MODE == Form_Main.MODE.MASTER)
            {
                this.Send_CMD(110, 1, (byte)(this.Location_x / 100), (byte)(this.Location_x % 100), (byte)(this.Location_y / 100), (byte)(this.Location_y % 100));
                this.Send_CMD(110, 2, (byte)(this.img_carve.Width / 100), (byte)(this.img_carve.Width % 100), (byte)(this.img_carve.Height / 100), (byte)(this.img_carve.Height % 100));
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            this.set_software_state(Form_Main.STATE.AT_PAUSE);
            this.Send_CMD(1, 2, 0);
        }

        private void set_language(Form_Main.LANG lang)
        {
            switch (lang)
            {
                case Form_Main.LANG.EN:
                    this.button_start.Text = "Start";
                    this.button_pause.Text = "Pause";
                    this.button_stop.Text = "RESET";
                    this.radioButton_Black.Text = "Effect A";
                    this.radioButton_Shake.Text = "Effect B";
                    this.button_Send_Pic.Text = "Send Picture";
                    this.button_clear.Text = "Clear";
                    this.button_reload.Text = "Reload";
                    this.button_Font.Text = "Font";
                    this.groupBox_InsertText.Text = "Insert Text";
                    this.checkBox_Insert.Text = "Insert Text";
                    this.textBox_Edit.Text = "Enter Text Here...";
                    this.tabControl1.TabPages[0].Text = "Home";
                    this.tabControl1.TabPages[1].Text = "Photo Gallery";
                    this.tabControl1.TabPages[2].Text = "My Collection";
                    this.tabControl1.TabPages[3].Text = "Control";
                    this.label_width.Text = "Width:";
                    break;
                case Form_Main.LANG.CN:
                    this.button_start.Text = "开始";
                    this.button_pause.Text = "暂停";
                    this.button_stop.Text = "停止";
                    this.radioButton_Black.Text = "黑白雕刻";
                    this.radioButton_Shake.Text = "抖动雕刻";
                    this.button_Send_Pic.Text = "发送图片";
                    this.button_clear.Text = "清除";
                    this.button_reload.Text = "重载";
                    this.button_Font.Text = "字体";
                    this.groupBox_InsertText.Text = "插入文字";
                    this.checkBox_Insert.Text = "插入文字";
                    this.textBox_Edit.Text = "在这里输入文字...";
                    this.tabControl1.TabPages[0].Text = "主页";
                    this.tabControl1.TabPages[1].Text = "图片库";
                    this.tabControl1.TabPages[2].Text = "我的收藏";
                    this.tabControl1.TabPages[3].Text = "控制";
                    this.label_width.Text = "宽度:";
                    break;
                case Form_Main.LANG.JP:
                    this.button_start.Text = "始める";
                    this.button_pause.Text = "一時停止";
                    this.button_stop.Text = "停止";
                    this.radioButton_Black.Text = "効果 A";
                    this.radioButton_Shake.Text = "効果 B";
                    this.button_Send_Pic.Text = "写真を送る";
                    this.button_clear.Text = "クリア";
                    this.button_reload.Text = "更新する";
                    this.button_Font.Text = "フォント";
                    this.groupBox_InsertText.Text = "テキストを挿入する";
                    this.checkBox_Insert.Text = "挿入";
                    this.textBox_Edit.Text = "ここにテキストを入力してください...";
                    this.tabControl1.TabPages[0].Text = "ホームページ";
                    this.tabControl1.TabPages[1].Text = "フォトギャラリー";
                    this.tabControl1.TabPages[2].Text = "私のコレクション";
                    this.tabControl1.TabPages[3].Text = "コントロール";
                    this.label_width.Text = "幅:";
                    break;
                case Form_Main.LANG.FR:
                    this.button_start.Text = "Commencer";
                    this.button_pause.Text = "Suspendu";
                    this.button_stop.Text = "Arrêter";
                    this.radioButton_Black.Text = "Effets A";
                    this.radioButton_Shake.Text = "Effets B";
                    this.button_Send_Pic.Text = "Envoyer une photo";
                    this.button_clear.Text = "Effacer";
                    this.button_reload.Text = "Recharger";
                    this.button_Font.Text = "Font";
                    this.groupBox_InsertText.Text = "Insérer un texte";
                    this.checkBox_Insert.Text = "Insert";
                    this.textBox_Edit.Text = "Entrez le texte ici..";
                    this.tabControl1.TabPages[0].Text = "Home";
                    this.tabControl1.TabPages[1].Text = "Galerie de photos";
                    this.tabControl1.TabPages[2].Text = "ma collection";
                    this.tabControl1.TabPages[3].Text = "Contrôles";
                    this.label_width.Text = "Largeur:";
                    break;
                case Form_Main.LANG.IT:
                    this.button_start.Text = "Iniziare";
                    this.button_pause.Text = "Pausa";
                    this.button_stop.Text = "Reset";
                    this.radioButton_Black.Text = "effetto A";
                    this.radioButton_Shake.Text = "effetto B";
                    this.button_Send_Pic.Text = "Invia foto";
                    this.button_clear.Text = "rimuovere";
                    this.button_reload.Text = "ricaricare";
                    this.button_Font.Text = "Caratteri";
                    this.groupBox_InsertText.Text = "Inserire il testo";
                    this.checkBox_Insert.Text = "Inserire";
                    this.textBox_Edit.Text = "Inserisci il testo qui...";
                    this.tabControl1.TabPages[0].Text = "Home";
                    this.tabControl1.TabPages[1].Text = "Galleria fotografica";
                    this.tabControl1.TabPages[2].Text = "la mia collezione";
                    this.tabControl1.TabPages[3].Text = "Controllo";
                    this.label_width.Text = "larghezza:";
                    break;
                case Form_Main.LANG.DE:
                    this.button_start.Text = "Beginnen";
                    this.button_pause.Text = "Suspendiert";
                    this.button_stop.Text = "Stoppen";
                    this.radioButton_Black.Text = "Effekte A";
                    this.radioButton_Shake.Text = "Effekte B";
                    this.button_Send_Pic.Text = "Bild senden";
                    this.button_clear.Text = "Löschen";
                    this.checkBox_Insert.Text = "";
                    this.button_reload.Text = "Laden neu laden";
                    this.button_Font.Text = "Schriftart";
                    this.groupBox_InsertText.Text = "Fügen Sie Text ein";
                    this.checkBox_Insert.Text = "Einfügen";
                    this.textBox_Edit.Text = "Gib hier einen Text ein...";
                    this.tabControl1.TabPages[0].Text = "Home";
                    this.tabControl1.TabPages[1].Text = "Fotogallerie";
                    this.tabControl1.TabPages[2].Text = "meine Sammlung";
                    this.tabControl1.TabPages[3].Text = "Kontrollen";
                    this.label_width.Text = "Breite:";
                    break;
            }
            this.tabControl1.TabPages[0].Text = "   " + this.tabControl1.TabPages[0].Text + "   ";
            this.tabControl1.TabPages[1].Text = "   " + this.tabControl1.TabPages[1].Text + "   ";
            this.tabControl1.TabPages[2].Text = "   " + this.tabControl1.TabPages[2].Text + "   ";
            this.tabControl1.TabPages[3].Text = "   " + this.tabControl1.TabPages[3].Text + "   ";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.textBox1 = new TextBox();
            this.label_Connection = new Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.button_driver = new Button();
            this.trackBar_carveTime = new TrackBar();
            this.label_trackbar = new Label();
            this.tabControl1 = new TabControl();
            this.tabPage0 = new TabPage();
            this.webBrowser1 = new WebBrowser();
            this.tabPage1 = new TabPage();
            this.label_filter = new Label();
            this.comboBox_Alt = new ComboBox();
            this.label_page = new Label();
            this.button_next_page = new Button();
            this.button_pre_page = new Button();
            this.pictureBox2 = new PictureBox();
            this.contextMenu1 = new ContextMenuStrip(this.components);
            this.menu_add_collection = new ToolStripMenuItem();
            this.pictureBox3 = new PictureBox();
            this.pictureBox4 = new PictureBox();
            this.pictureBox5 = new PictureBox();
            this.pictureBox6 = new PictureBox();
            this.pictureBox7 = new PictureBox();
            this.pictureBox8 = new PictureBox();
            this.pictureBox9 = new PictureBox();
            this.pictureBox10 = new PictureBox();
            this.pictureBox11 = new PictureBox();
            this.pictureBox12 = new PictureBox();
            this.pictureBox13 = new PictureBox();
            this.tabPage3 = new TabPage();
            this.label_collection_pages = new Label();
            this.button_collection_next = new Button();
            this.button_collection_pre = new Button();
            this.pictureBox14 = new PictureBox();
            this.contextMenu2 = new ContextMenuStrip(this.components);
            this.menu_delete = new ToolStripMenuItem();
            this.pictureBox15 = new PictureBox();
            this.pictureBox16 = new PictureBox();
            this.pictureBox17 = new PictureBox();
            this.pictureBox18 = new PictureBox();
            this.pictureBox19 = new PictureBox();
            this.pictureBox20 = new PictureBox();
            this.pictureBox21 = new PictureBox();
            this.pictureBox22 = new PictureBox();
            this.pictureBox23 = new PictureBox();
            this.pictureBox24 = new PictureBox();
            this.pictureBox25 = new PictureBox();
            this.tabPage2 = new TabPage();
            this.panel_Laser_PWM = new Panel();
            this.trackBar_PWM = new TrackBar();
            this.label_PWM = new Label();
            this.button_Send_Pic = new Button();
            this.panel_control = new Panel();
            this.button_stop = new Button();
            this.label_times = new Label();
            this.numericUpDown_times = new NumericUpDown();
            this.button_anypoint_location = new Button();
            this.button_preview = new Button();
            this.button_pause = new Button();
            this.button_start = new Button();
            this.panel_direction = new Panel();
            this.button_left = new Button();
            this.button_down = new Button();
            this.button_right = new Button();
            this.button_up = new Button();
            this.button_reload = new Button();
            this.button_clear = new Button();
            this.groupBox_InsertText = new GroupBox();
            this.checkBox_Insert = new CheckBox();
            this.button_Font = new Button();
            this.textBox_Edit = new TextBox();
            this.panel_size = new Panel();
            this.label_width = new Label();
            this.textBox_size = new TextBox();
            this.button_size = new Button();
            this.label_size = new Label();
            this.radioButton_Black = new RadioButton();
            this.radioButton_Shake = new RadioButton();
            this.pictureBox1 = new PictureBox();
            this.timer_connect = new System.Windows.Forms.Timer(this.components);
            this.timer_isconnection_ok_check = new System.Windows.Forms.Timer(this.components);
            this.timer_picturebox_refresh = new System.Windows.Forms.Timer(this.components);
            this.label_current = new Label();
            this.label_temperature = new Label();
            this.label_power = new Label();
            this.label_software_version = new Label();
            this.label_machine_mode = new Label();
            this.label_machine_firmversion = new Label();
            this.label1 = new Label();
            this.comboBox_Language = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.label_fan_RPM = new Label();
            this.label_laser_power = new Label();
            this.label_laser_length = new Label();
            this.fontDialog_Edit = new FontDialog();
            this.label_connection_logo = new Label();
            this.timer_check_send_pic = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new StatusStrip();
            this.StatusLabel_state = new ToolStripStatusLabel();
            ((ISupportInitialize)(this.trackBar_carveTime)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage0.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.contextMenu1.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox14)).BeginInit();
            this.contextMenu2.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((ISupportInitialize)(this.pictureBox18)).BeginInit();
            ((ISupportInitialize)(this.pictureBox19)).BeginInit();
            ((ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((ISupportInitialize)(this.pictureBox21)).BeginInit();
            ((ISupportInitialize)(this.pictureBox22)).BeginInit();
            ((ISupportInitialize)(this.pictureBox23)).BeginInit();
            ((ISupportInitialize)(this.pictureBox24)).BeginInit();
            ((ISupportInitialize)(this.pictureBox25)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel_Laser_PWM.SuspendLayout();
            ((ISupportInitialize)(this.trackBar_PWM)).BeginInit();
            this.panel_control.SuspendLayout();
            ((ISupportInitialize)(this.numericUpDown_times)).BeginInit();
            this.panel_direction.SuspendLayout();
            this.groupBox_InsertText.SuspendLayout();
            this.panel_size.SuspendLayout();
            ((ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(14, 53);
            this.textBox1.Margin = new Padding(3, 5, 3, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(198, 267);
            this.textBox1.TabIndex = 0;
            // 
            // label_Connection
            // 
            this.label_Connection.Location = new System.Drawing.Point(67, 17);
            this.label_Connection.Name = "label_Connection";
            this.label_Connection.Size = new System.Drawing.Size(141, 15);
            this.label_Connection.TabIndex = 2;
            this.label_Connection.Text = "CONNECTING";
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 57600;
            this.serialPort1.DtrEnable = true;
            this.serialPort1.ReadBufferSize = 512000;
            this.serialPort1.WriteBufferSize = 512000;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // button_driver
            // 
            this.button_driver.Location = new System.Drawing.Point(166, 12);
            this.button_driver.Name = "button_driver";
            this.button_driver.Size = new System.Drawing.Size(46, 23);
            this.button_driver.TabIndex = 3;
            this.button_driver.Text = "驱动";
            this.button_driver.UseVisualStyleBackColor = true;
            this.button_driver.Visible = false;
            this.button_driver.Click += new System.EventHandler(this.button_driver_Click);
            // 
            // trackBar_carveTime
            // 
            this.trackBar_carveTime.Location = new System.Drawing.Point(26, 530);
            this.trackBar_carveTime.Maximum = 250;
            this.trackBar_carveTime.Name = "trackBar_carveTime";
            this.trackBar_carveTime.Size = new System.Drawing.Size(696, 45);
            this.trackBar_carveTime.TabIndex = 4;
            this.trackBar_carveTime.Scroll += new System.EventHandler(this.trackBar_carveTime_Scroll);
            this.trackBar_carveTime.MouseDown += new MouseEventHandler(this.trackBar_carveTime_MouseDown);
            this.trackBar_carveTime.MouseUp += new MouseEventHandler(this.trackBar_carveTime_MouseUp);
            // 
            // label_trackbar
            // 
            this.label_trackbar.AutoSize = true;
            this.label_trackbar.Location = new System.Drawing.Point(721, 534);
            this.label_trackbar.Name = "label_trackbar";
            this.label_trackbar.Size = new System.Drawing.Size(87, 15);
            this.label_trackbar.TabIndex = 5;
            this.label_trackbar.Text = "Burning Time :";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(230, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(853, 607);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Selected += new TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.webBrowser1);
            this.tabPage0.Location = new System.Drawing.Point(4, 24);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Padding = new Padding(3);
            this.tabPage0.Size = new System.Drawing.Size(845, 579);
            this.tabPage0.TabIndex = 0;
            this.tabPage0.Text = "主页";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(839, 573);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label_filter);
            this.tabPage1.Controls.Add(this.comboBox_Alt);
            this.tabPage1.Controls.Add(this.label_page);
            this.tabPage1.Controls.Add(this.button_next_page);
            this.tabPage1.Controls.Add(this.button_pre_page);
            this.tabPage1.Controls.Add(this.pictureBox2);
            this.tabPage1.Controls.Add(this.pictureBox3);
            this.tabPage1.Controls.Add(this.pictureBox4);
            this.tabPage1.Controls.Add(this.pictureBox5);
            this.tabPage1.Controls.Add(this.pictureBox6);
            this.tabPage1.Controls.Add(this.pictureBox7);
            this.tabPage1.Controls.Add(this.pictureBox8);
            this.tabPage1.Controls.Add(this.pictureBox9);
            this.tabPage1.Controls.Add(this.pictureBox10);
            this.tabPage1.Controls.Add(this.pictureBox11);
            this.tabPage1.Controls.Add(this.pictureBox12);
            this.tabPage1.Controls.Add(this.pictureBox13);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(845, 581);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "网络";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_filter
            // 
            this.label_filter.AutoSize = true;
            this.label_filter.Location = new System.Drawing.Point(748, 12);
            this.label_filter.Name = "label_filter";
            this.label_filter.Size = new System.Drawing.Size(37, 15);
            this.label_filter.TabIndex = 33;
            this.label_filter.Text = "Filter:";
            // 
            // comboBox_Alt
            // 
            this.comboBox_Alt.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Alt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Alt.FormattingEnabled = true;
            this.comboBox_Alt.Items.AddRange(new object[] {
            "ALL"});
            this.comboBox_Alt.Location = new System.Drawing.Point(751, 32);
            this.comboBox_Alt.Name = "comboBox_Alt";
            this.comboBox_Alt.Size = new System.Drawing.Size(88, 23);
            this.comboBox_Alt.TabIndex = 32;
            this.comboBox_Alt.SelectedIndexChanged += new System.EventHandler(this.comboBox_Alt_SelectedIndexChanged);
            // 
            // label_page
            // 
            this.label_page.AutoSize = true;
            this.label_page.Location = new System.Drawing.Point(780, 282);
            this.label_page.Name = "label_page";
            this.label_page.Size = new System.Drawing.Size(24, 15);
            this.label_page.TabIndex = 15;
            this.label_page.Text = "1/X";
            // 
            // button_next_page
            // 
            this.button_next_page.BackgroundImage = global::NejeEngraverApp.Properties.Form_Main.button_next_page;
            this.button_next_page.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_next_page.Location = new System.Drawing.Point(774, 323);
            this.button_next_page.Name = "button_next_page";
            this.button_next_page.Size = new System.Drawing.Size(41, 119);
            this.button_next_page.TabIndex = 14;
            this.button_next_page.UseVisualStyleBackColor = true;
            this.button_next_page.Click += new System.EventHandler(this.button_next_page_Click);
            // 
            // button_pre_page
            // 
            this.button_pre_page.BackgroundImage = global::NejeEngraverApp.Properties.Form_Main.button_pre_page;
            this.button_pre_page.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_pre_page.Location = new System.Drawing.Point(774, 138);
            this.button_pre_page.Name = "button_pre_page";
            this.button_pre_page.Size = new System.Drawing.Size(41, 119);
            this.button_pre_page.TabIndex = 13;
            this.button_pre_page.UseVisualStyleBackColor = true;
            this.button_pre_page.Click += new System.EventHandler(this.button_pre_page_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox2.ContextMenuStrip = this.contextMenu1;
            this.pictureBox2.Cursor = Cursors.Hand;
            this.pictureBox2.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox2;
            this.pictureBox2.Location = new System.Drawing.Point(7, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(180, 180);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "1";
            this.pictureBox2.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Items.AddRange(new ToolStripItem[] {
            this.menu_add_collection});
            this.contextMenu1.Name = "contextMenu1";
            this.contextMenu1.Size = new System.Drawing.Size(188, 26);
            this.contextMenu1.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu1_ItemClicked);
            // 
            // menu_add_collection
            // 
            this.menu_add_collection.Name = "menu_add_collection";
            this.menu_add_collection.Size = new System.Drawing.Size(187, 22);
            this.menu_add_collection.Text = "Add to My Collection";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox3.ContextMenuStrip = this.contextMenu1;
            this.pictureBox3.Cursor = Cursors.Hand;
            this.pictureBox3.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox3;
            this.pictureBox3.Location = new System.Drawing.Point(193, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(180, 180);
            this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox4.ContextMenuStrip = this.contextMenu1;
            this.pictureBox4.Cursor = Cursors.Hand;
            this.pictureBox4.Location = new System.Drawing.Point(379, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(180, 180);
            this.pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox5.ContextMenuStrip = this.contextMenu1;
            this.pictureBox5.Cursor = Cursors.Hand;
            this.pictureBox5.Location = new System.Drawing.Point(565, 12);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(180, 180);
            this.pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 4;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox5.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox6.ContextMenuStrip = this.contextMenu1;
            this.pictureBox6.Cursor = Cursors.Hand;
            this.pictureBox6.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox6;
            this.pictureBox6.Location = new System.Drawing.Point(7, 198);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(180, 180);
            this.pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 5;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox7
            // 
            this.pictureBox7.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox7.ContextMenuStrip = this.contextMenu1;
            this.pictureBox7.Cursor = Cursors.Hand;
            this.pictureBox7.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox7;
            this.pictureBox7.Location = new System.Drawing.Point(193, 198);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(180, 180);
            this.pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 6;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox7.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox8.ContextMenuStrip = this.contextMenu1;
            this.pictureBox8.Cursor = Cursors.Hand;
            this.pictureBox8.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox8;
            this.pictureBox8.Location = new System.Drawing.Point(379, 198);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(180, 180);
            this.pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 7;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox8.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox9
            // 
            this.pictureBox9.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox9.ContextMenuStrip = this.contextMenu1;
            this.pictureBox9.Cursor = Cursors.Hand;
            this.pictureBox9.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox9;
            this.pictureBox9.Location = new System.Drawing.Point(565, 198);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(180, 180);
            this.pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox9.TabIndex = 8;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox9.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox10
            // 
            this.pictureBox10.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox10.ContextMenuStrip = this.contextMenu1;
            this.pictureBox10.Cursor = Cursors.Hand;
            this.pictureBox10.Location = new System.Drawing.Point(7, 384);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(180, 180);
            this.pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox10.TabIndex = 9;
            this.pictureBox10.TabStop = false;
            this.pictureBox10.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox10.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox11
            // 
            this.pictureBox11.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox11.ContextMenuStrip = this.contextMenu1;
            this.pictureBox11.Cursor = Cursors.Hand;
            this.pictureBox11.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox11;
            this.pictureBox11.Location = new System.Drawing.Point(193, 384);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(180, 180);
            this.pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox11.TabIndex = 10;
            this.pictureBox11.TabStop = false;
            this.pictureBox11.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox11.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox12
            // 
            this.pictureBox12.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox12.ContextMenuStrip = this.contextMenu1;
            this.pictureBox12.Cursor = Cursors.Hand;
            this.pictureBox12.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox12;
            this.pictureBox12.Location = new System.Drawing.Point(379, 384);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(180, 180);
            this.pictureBox12.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox12.TabIndex = 11;
            this.pictureBox12.TabStop = false;
            this.pictureBox12.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox12.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox13
            // 
            this.pictureBox13.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox13.ContextMenuStrip = this.contextMenu1;
            this.pictureBox13.Cursor = Cursors.Hand;
            this.pictureBox13.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox13;
            this.pictureBox13.Location = new System.Drawing.Point(565, 384);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(180, 180);
            this.pictureBox13.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox13.TabIndex = 12;
            this.pictureBox13.TabStop = false;
            this.pictureBox13.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox13.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label_collection_pages);
            this.tabPage3.Controls.Add(this.button_collection_next);
            this.tabPage3.Controls.Add(this.button_collection_pre);
            this.tabPage3.Controls.Add(this.pictureBox14);
            this.tabPage3.Controls.Add(this.pictureBox15);
            this.tabPage3.Controls.Add(this.pictureBox16);
            this.tabPage3.Controls.Add(this.pictureBox17);
            this.tabPage3.Controls.Add(this.pictureBox18);
            this.tabPage3.Controls.Add(this.pictureBox19);
            this.tabPage3.Controls.Add(this.pictureBox20);
            this.tabPage3.Controls.Add(this.pictureBox21);
            this.tabPage3.Controls.Add(this.pictureBox22);
            this.tabPage3.Controls.Add(this.pictureBox23);
            this.tabPage3.Controls.Add(this.pictureBox24);
            this.tabPage3.Controls.Add(this.pictureBox25);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(845, 581);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "收藏";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label_collection_pages
            // 
            this.label_collection_pages.AutoSize = true;
            this.label_collection_pages.Location = new System.Drawing.Point(780, 282);
            this.label_collection_pages.Name = "label_collection_pages";
            this.label_collection_pages.Size = new System.Drawing.Size(24, 15);
            this.label_collection_pages.TabIndex = 30;
            this.label_collection_pages.Text = "1/X";
            // 
            // button_collection_next
            // 
            this.button_collection_next.BackgroundImage = global::NejeEngraverApp.Properties.Form_Main.button_collection_next;
            this.button_collection_next.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_collection_next.Location = new System.Drawing.Point(774, 323);
            this.button_collection_next.Name = "button_collection_next";
            this.button_collection_next.Size = new System.Drawing.Size(41, 119);
            this.button_collection_next.TabIndex = 29;
            this.button_collection_next.UseVisualStyleBackColor = true;
            this.button_collection_next.Click += new System.EventHandler(this.button_collection_next_Click);
            // 
            // button_collection_pre
            // 
            this.button_collection_pre.BackgroundImage = global::NejeEngraverApp.Properties.Form_Main.button_collection_pre;
            this.button_collection_pre.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_collection_pre.Location = new System.Drawing.Point(774, 138);
            this.button_collection_pre.Name = "button_collection_pre";
            this.button_collection_pre.Size = new System.Drawing.Size(41, 119);
            this.button_collection_pre.TabIndex = 28;
            this.button_collection_pre.UseVisualStyleBackColor = true;
            this.button_collection_pre.Click += new System.EventHandler(this.button_collection_pre_Click);
            // 
            // pictureBox14
            // 
            this.pictureBox14.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox14.ContextMenuStrip = this.contextMenu2;
            this.pictureBox14.Cursor = Cursors.Hand;
            this.pictureBox14.Location = new System.Drawing.Point(7, 12);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(180, 180);
            this.pictureBox14.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox14.TabIndex = 16;
            this.pictureBox14.TabStop = false;
            this.pictureBox14.Tag = "1";
            this.pictureBox14.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox14.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // contextMenu2
            // 
            this.contextMenu2.Items.AddRange(new ToolStripItem[] {
            this.menu_delete});
            this.contextMenu2.Name = "contextMenu2";
            this.contextMenu2.Size = new System.Drawing.Size(108, 26);
            this.contextMenu2.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenu2_ItemClicked);
            // 
            // menu_delete
            // 
            this.menu_delete.Name = "menu_delete";
            this.menu_delete.Size = new System.Drawing.Size(107, 22);
            this.menu_delete.Text = "Delete";
            // 
            // pictureBox15
            // 
            this.pictureBox15.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox15.ContextMenuStrip = this.contextMenu2;
            this.pictureBox15.Cursor = Cursors.Hand;
            this.pictureBox15.Location = new System.Drawing.Point(193, 12);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(180, 180);
            this.pictureBox15.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox15.TabIndex = 17;
            this.pictureBox15.TabStop = false;
            this.pictureBox15.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox15.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox16
            // 
            this.pictureBox16.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox16.ContextMenuStrip = this.contextMenu2;
            this.pictureBox16.Cursor = Cursors.Hand;
            this.pictureBox16.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox16;
            this.pictureBox16.Location = new System.Drawing.Point(379, 12);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(180, 180);
            this.pictureBox16.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox16.TabIndex = 18;
            this.pictureBox16.TabStop = false;
            this.pictureBox16.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox16.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox17
            // 
            this.pictureBox17.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox17.ContextMenuStrip = this.contextMenu2;
            this.pictureBox17.Cursor = Cursors.Hand;
            this.pictureBox17.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox17;
            this.pictureBox17.Location = new System.Drawing.Point(565, 12);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(180, 180);
            this.pictureBox17.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox17.TabIndex = 19;
            this.pictureBox17.TabStop = false;
            this.pictureBox17.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox17.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox18
            // 
            this.pictureBox18.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox18.ContextMenuStrip = this.contextMenu2;
            this.pictureBox18.Cursor = Cursors.Hand;
            this.pictureBox18.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox18;
            this.pictureBox18.Location = new System.Drawing.Point(7, 198);
            this.pictureBox18.Name = "pictureBox18";
            this.pictureBox18.Size = new System.Drawing.Size(180, 180);
            this.pictureBox18.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox18.TabIndex = 20;
            this.pictureBox18.TabStop = false;
            this.pictureBox18.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox18.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox19
            // 
            this.pictureBox19.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox19.ContextMenuStrip = this.contextMenu2;
            this.pictureBox19.Cursor = Cursors.Hand;
            this.pictureBox19.Location = new System.Drawing.Point(193, 198);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(180, 180);
            this.pictureBox19.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox19.TabIndex = 21;
            this.pictureBox19.TabStop = false;
            this.pictureBox19.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox19.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox20
            // 
            this.pictureBox20.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox20.ContextMenuStrip = this.contextMenu2;
            this.pictureBox20.Cursor = Cursors.Hand;
            this.pictureBox20.Location = new System.Drawing.Point(379, 198);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(180, 180);
            this.pictureBox20.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox20.TabIndex = 22;
            this.pictureBox20.TabStop = false;
            this.pictureBox20.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox20.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox21
            // 
            this.pictureBox21.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox21.ContextMenuStrip = this.contextMenu2;
            this.pictureBox21.Cursor = Cursors.Hand;
            this.pictureBox21.Location = new System.Drawing.Point(565, 198);
            this.pictureBox21.Name = "pictureBox21";
            this.pictureBox21.Size = new System.Drawing.Size(180, 180);
            this.pictureBox21.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox21.TabIndex = 23;
            this.pictureBox21.TabStop = false;
            this.pictureBox21.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox21.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox22
            // 
            this.pictureBox22.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox22.ContextMenuStrip = this.contextMenu2;
            this.pictureBox22.Cursor = Cursors.Hand;
            this.pictureBox22.Location = new System.Drawing.Point(7, 384);
            this.pictureBox22.Name = "pictureBox22";
            this.pictureBox22.Size = new System.Drawing.Size(180, 180);
            this.pictureBox22.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox22.TabIndex = 24;
            this.pictureBox22.TabStop = false;
            this.pictureBox22.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox22.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox23
            // 
            this.pictureBox23.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox23.ContextMenuStrip = this.contextMenu2;
            this.pictureBox23.Cursor = Cursors.Hand;
            this.pictureBox23.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox23;
            this.pictureBox23.Location = new System.Drawing.Point(193, 384);
            this.pictureBox23.Name = "pictureBox23";
            this.pictureBox23.Size = new System.Drawing.Size(180, 180);
            this.pictureBox23.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox23.TabIndex = 25;
            this.pictureBox23.TabStop = false;
            this.pictureBox23.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox23.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox24
            // 
            this.pictureBox24.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox24.ContextMenuStrip = this.contextMenu2;
            this.pictureBox24.Cursor = Cursors.Hand;
            this.pictureBox24.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox24;
            this.pictureBox24.Location = new System.Drawing.Point(379, 384);
            this.pictureBox24.Name = "pictureBox24";
            this.pictureBox24.Size = new System.Drawing.Size(180, 180);
            this.pictureBox24.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox24.TabIndex = 26;
            this.pictureBox24.TabStop = false;
            this.pictureBox24.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox24.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // pictureBox25
            // 
            this.pictureBox25.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox25.ContextMenuStrip = this.contextMenu2;
            this.pictureBox25.Cursor = Cursors.Hand;
            this.pictureBox25.InitialImage = global::NejeEngraverApp.Properties.Form_Main.pictureBox25;
            this.pictureBox25.Location = new System.Drawing.Point(565, 384);
            this.pictureBox25.Name = "pictureBox25";
            this.pictureBox25.Size = new System.Drawing.Size(180, 180);
            this.pictureBox25.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox25.TabIndex = 27;
            this.pictureBox25.TabStop = false;
            this.pictureBox25.LoadCompleted += new AsyncCompletedEventHandler(this.pictureBox2_LoadCompleted);
            this.pictureBox25.Click += new System.EventHandler(this.pictureBox_load_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.AllowDrop = true;
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.panel_Laser_PWM);
            this.tabPage2.Controls.Add(this.button_Send_Pic);
            this.tabPage2.Controls.Add(this.panel_control);
            this.tabPage2.Controls.Add(this.panel_direction);
            this.tabPage2.Controls.Add(this.button_reload);
            this.tabPage2.Controls.Add(this.button_clear);
            this.tabPage2.Controls.Add(this.groupBox_InsertText);
            this.tabPage2.Controls.Add(this.panel_size);
            this.tabPage2.Controls.Add(this.radioButton_Black);
            this.tabPage2.Controls.Add(this.radioButton_Shake);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Controls.Add(this.trackBar_carveTime);
            this.tabPage2.Controls.Add(this.label_trackbar);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(845, 579);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "控制";
            this.tabPage2.Click += new System.EventHandler(this.tabPage3_Click);
            this.tabPage2.DragDrop += new DragEventHandler(this.tabPage3_DragDrop);
            this.tabPage2.DragEnter += new DragEventHandler(this.tabPage3_DragEnter);
            // 
            // panel_Laser_PWM
            // 
            this.panel_Laser_PWM.Controls.Add(this.trackBar_PWM);
            this.panel_Laser_PWM.Controls.Add(this.label_PWM);
            this.panel_Laser_PWM.Location = new System.Drawing.Point(910, 470);
            this.panel_Laser_PWM.Name = "panel_Laser_PWM";
            this.panel_Laser_PWM.Size = new System.Drawing.Size(304, 50);
            this.panel_Laser_PWM.TabIndex = 39;
            // 
            // trackBar_PWM
            // 
            this.trackBar_PWM.LargeChange = 1;
            this.trackBar_PWM.Location = new System.Drawing.Point(3, 3);
            this.trackBar_PWM.Minimum = 1;
            this.trackBar_PWM.Name = "trackBar_PWM";
            this.trackBar_PWM.Size = new System.Drawing.Size(184, 45);
            this.trackBar_PWM.TabIndex = 37;
            this.trackBar_PWM.Value = 10;
            this.trackBar_PWM.Scroll += new System.EventHandler(this.trackBar_PWM_Scroll);
            // 
            // label_PWM
            // 
            this.label_PWM.AutoSize = true;
            this.label_PWM.Location = new System.Drawing.Point(187, 7);
            this.label_PWM.Name = "label_PWM";
            this.label_PWM.Size = new System.Drawing.Size(61, 15);
            this.label_PWM.TabIndex = 25;
            this.label_PWM.Text = "激光功率：";
            // 
            // button_Send_Pic
            // 
            this.button_Send_Pic.Location = new System.Drawing.Point(596, 241);
            this.button_Send_Pic.Name = "button_Send_Pic";
            this.button_Send_Pic.Size = new System.Drawing.Size(192, 40);
            this.button_Send_Pic.TabIndex = 30;
            this.button_Send_Pic.Text = "发送图片";
            this.button_Send_Pic.UseVisualStyleBackColor = true;
            this.button_Send_Pic.Click += new System.EventHandler(this.button_Send_Pic_Click);
            // 
            // panel_control
            // 
            this.panel_control.Controls.Add(this.button_stop);
            this.panel_control.Controls.Add(this.label_times);
            this.panel_control.Controls.Add(this.numericUpDown_times);
            this.panel_control.Controls.Add(this.button_anypoint_location);
            this.panel_control.Controls.Add(this.button_preview);
            this.panel_control.Controls.Add(this.button_pause);
            this.panel_control.Controls.Add(this.button_start);
            this.panel_control.Location = new System.Drawing.Point(591, 382);
            this.panel_control.Name = "panel_control";
            this.panel_control.Size = new System.Drawing.Size(200, 138);
            this.panel_control.TabIndex = 36;
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(107, 93);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(90, 40);
            this.button_stop.TabIndex = 41;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // label_times
            // 
            this.label_times.AutoSize = true;
            this.label_times.Location = new System.Drawing.Point(155, 62);
            this.label_times.Name = "label_times";
            this.label_times.Size = new System.Drawing.Size(42, 15);
            this.label_times.TabIndex = 40;
            this.label_times.Text = "Times";
            // 
            // numericUpDown_times
            // 
            this.numericUpDown_times.Location = new System.Drawing.Point(111, 60);
            this.numericUpDown_times.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_times.Name = "numericUpDown_times";
            this.numericUpDown_times.Size = new System.Drawing.Size(38, 21);
            this.numericUpDown_times.TabIndex = 39;
            this.numericUpDown_times.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button_anypoint_location
            // 
            this.button_anypoint_location.BackgroundImage = global::NejeEngraverApp.Properties.Resources.anypoint;
            this.button_anypoint_location.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_anypoint_location.Location = new System.Drawing.Point(107, 5);
            this.button_anypoint_location.Name = "button_anypoint_location";
            this.button_anypoint_location.Size = new System.Drawing.Size(90, 40);
            this.button_anypoint_location.TabIndex = 38;
            this.button_anypoint_location.UseVisualStyleBackColor = true;
            this.button_anypoint_location.Click += new System.EventHandler(this.button_anypoint_location_Click);
            // 
            // button_preview
            // 
            this.button_preview.BackgroundImage = global::NejeEngraverApp.Properties.Form_Main.button_preview;
            this.button_preview.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_preview.Location = new System.Drawing.Point(5, 5);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(90, 40);
            this.button_preview.TabIndex = 37;
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // button_pause
            // 
            this.button_pause.Location = new System.Drawing.Point(5, 93);
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size(90, 40);
            this.button_pause.TabIndex = 36;
            this.button_pause.Text = "暂停";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler(this.button_pause_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(5, 49);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(90, 40);
            this.button_start.TabIndex = 35;
            this.button_start.Text = "开始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // panel_direction
            // 
            this.panel_direction.Controls.Add(this.button_left);
            this.panel_direction.Controls.Add(this.button_down);
            this.panel_direction.Controls.Add(this.button_right);
            this.panel_direction.Controls.Add(this.button_up);
            this.panel_direction.Location = new System.Drawing.Point(591, 283);
            this.panel_direction.Name = "panel_direction";
            this.panel_direction.Size = new System.Drawing.Size(200, 93);
            this.panel_direction.TabIndex = 35;
            // 
            // button_left
            // 
            this.button_left.BackgroundImage = global::NejeEngraverApp.Properties.Resources.left;
            this.button_left.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_left.Location = new System.Drawing.Point(5, 49);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(60, 40);
            this.button_left.TabIndex = 27;
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_down
            // 
            this.button_down.BackgroundImage = global::NejeEngraverApp.Properties.Resources.down;
            this.button_down.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_down.Location = new System.Drawing.Point(71, 49);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(60, 40);
            this.button_down.TabIndex = 28;
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_right
            // 
            this.button_right.BackgroundImage = global::NejeEngraverApp.Properties.Resources.right;
            this.button_right.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_right.Location = new System.Drawing.Point(137, 49);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(60, 40);
            this.button_right.TabIndex = 29;
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_up
            // 
            this.button_up.BackgroundImage = global::NejeEngraverApp.Properties.Resources.up;
            this.button_up.BackgroundImageLayout = ImageLayout.Zoom;
            this.button_up.Location = new System.Drawing.Point(71, 3);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(60, 40);
            this.button_up.TabIndex = 26;
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_reload
            // 
            this.button_reload.Location = new System.Drawing.Point(698, 47);
            this.button_reload.Name = "button_reload";
            this.button_reload.Size = new System.Drawing.Size(90, 27);
            this.button_reload.TabIndex = 31;
            this.button_reload.Text = "重载";
            this.button_reload.UseVisualStyleBackColor = true;
            this.button_reload.Click += new System.EventHandler(this.button_reload_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(596, 47);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(90, 27);
            this.button_clear.TabIndex = 30;
            this.button_clear.Text = "清除";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // groupBox_InsertText
            // 
            this.groupBox_InsertText.Controls.Add(this.checkBox_Insert);
            this.groupBox_InsertText.Controls.Add(this.button_Font);
            this.groupBox_InsertText.Controls.Add(this.textBox_Edit);
            this.groupBox_InsertText.Location = new System.Drawing.Point(596, 86);
            this.groupBox_InsertText.Margin = new Padding(2);
            this.groupBox_InsertText.Name = "groupBox_InsertText";
            this.groupBox_InsertText.Padding = new Padding(2);
            this.groupBox_InsertText.Size = new System.Drawing.Size(192, 150);
            this.groupBox_InsertText.TabIndex = 25;
            this.groupBox_InsertText.TabStop = false;
            this.groupBox_InsertText.Text = "插入文字";
            // 
            // checkBox_Insert
            // 
            this.checkBox_Insert.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.checkBox_Insert.Location = new System.Drawing.Point(96, 116);
            this.checkBox_Insert.Name = "checkBox_Insert";
            this.checkBox_Insert.Size = new System.Drawing.Size(92, 21);
            this.checkBox_Insert.TabIndex = 3;
            this.checkBox_Insert.Text = "插入文字";
            this.checkBox_Insert.UseVisualStyleBackColor = true;
            this.checkBox_Insert.CheckedChanged += new System.EventHandler(this.checkBox_Insert_CheckedChanged);
            // 
            // button_Font
            // 
            this.button_Font.Location = new System.Drawing.Point(10, 111);
            this.button_Font.Margin = new Padding(2);
            this.button_Font.Name = "button_Font";
            this.button_Font.Size = new System.Drawing.Size(65, 29);
            this.button_Font.TabIndex = 2;
            this.button_Font.Text = "字体";
            this.button_Font.UseVisualStyleBackColor = true;
            this.button_Font.Click += new System.EventHandler(this.button_Font_Click);
            // 
            // textBox_Edit
            // 
            this.textBox_Edit.AcceptsReturn = true;
            this.textBox_Edit.AcceptsTab = true;
            this.textBox_Edit.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Edit.Location = new System.Drawing.Point(4, 18);
            this.textBox_Edit.Margin = new Padding(2);
            this.textBox_Edit.Multiline = true;
            this.textBox_Edit.Name = "textBox_Edit";
            this.textBox_Edit.ScrollBars = ScrollBars.Vertical;
            this.textBox_Edit.Size = new System.Drawing.Size(184, 80);
            this.textBox_Edit.TabIndex = 0;
            this.textBox_Edit.Text = "请输入文字：";
            this.textBox_Edit.Click += new System.EventHandler(this.textBox_Edit_Click);
            this.textBox_Edit.DoubleClick += new System.EventHandler(this.textBox_Edit_DoubleClick);
            // 
            // panel_size
            // 
            this.panel_size.BackColor = System.Drawing.SystemColors.Window;
            this.panel_size.Controls.Add(this.label_width);
            this.panel_size.Controls.Add(this.textBox_size);
            this.panel_size.Controls.Add(this.button_size);
            this.panel_size.Controls.Add(this.label_size);
            this.panel_size.Location = new System.Drawing.Point(189, 482);
            this.panel_size.Name = "panel_size";
            this.panel_size.Size = new System.Drawing.Size(233, 29);
            this.panel_size.TabIndex = 16;
            // 
            // label_width
            // 
            this.label_width.Location = new System.Drawing.Point(3, 5);
            this.label_width.Name = "label_width";
            this.label_width.Size = new System.Drawing.Size(53, 15);
            this.label_width.TabIndex = 16;
            this.label_width.Text = "Width:";
            this.label_width.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_size
            // 
            this.textBox_size.Location = new System.Drawing.Point(57, 3);
            this.textBox_size.Name = "textBox_size";
            this.textBox_size.Size = new System.Drawing.Size(58, 21);
            this.textBox_size.TabIndex = 15;
            this.textBox_size.Text = "0";
            this.textBox_size.TextAlign = HorizontalAlignment.Center;
            // 
            // button_size
            // 
            this.button_size.Location = new System.Drawing.Point(186, 3);
            this.button_size.Name = "button_size";
            this.button_size.Size = new System.Drawing.Size(44, 23);
            this.button_size.TabIndex = 13;
            this.button_size.Text = "OK";
            this.button_size.UseVisualStyleBackColor = true;
            this.button_size.Click += new System.EventHandler(this.button_size_Click);
            // 
            // label_size
            // 
            this.label_size.AutoSize = true;
            this.label_size.Location = new System.Drawing.Point(121, 5);
            this.label_size.Name = "label_size";
            this.label_size.Size = new System.Drawing.Size(64, 15);
            this.label_size.TabIndex = 14;
            this.label_size.Text = "mm 100%";
            this.label_size.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButton_Black
            // 
            this.radioButton_Black.AutoSize = true;
            this.radioButton_Black.Checked = true;
            this.radioButton_Black.Location = new System.Drawing.Point(606, 18);
            this.radioButton_Black.Name = "radioButton_Black";
            this.radioButton_Black.Size = new System.Drawing.Size(73, 19);
            this.radioButton_Black.TabIndex = 12;
            this.radioButton_Black.TabStop = true;
            this.radioButton_Black.Text = "黑白雕刻";
            this.radioButton_Black.UseVisualStyleBackColor = true;
            this.radioButton_Black.CheckedChanged += new System.EventHandler(this.radioButton_Black_CheckedChanged);
            // 
            // radioButton_Shake
            // 
            this.radioButton_Shake.AutoSize = true;
            this.radioButton_Shake.Location = new System.Drawing.Point(703, 18);
            this.radioButton_Shake.Name = "radioButton_Shake";
            this.radioButton_Shake.Size = new System.Drawing.Size(73, 19);
            this.radioButton_Shake.TabIndex = 11;
            this.radioButton_Shake.Text = "抖动雕刻";
            this.radioButton_Shake.UseVisualStyleBackColor = true;
            this.radioButton_Shake.CheckedChanged += new System.EventHandler(this.radioButton_Black_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.None;
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.ContextMenuStrip = this.contextMenu1;
            this.pictureBox1.Cursor = Cursors.Default;
            this.pictureBox1.Location = new System.Drawing.Point(26, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(502, 502);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // timer_connect
            // 
            this.timer_connect.Interval = 1000;
            this.timer_connect.Tick += new System.EventHandler(this.timer_connect_Tick);
            // 
            // timer_isconnection_ok_check
            // 
            this.timer_isconnection_ok_check.Enabled = true;
            this.timer_isconnection_ok_check.Interval = 1000;
            this.timer_isconnection_ok_check.Tick += new System.EventHandler(this.timer_isconnection_ok_check_Tick);
            // 
            // timer_picturebox_refresh
            // 
            this.timer_picturebox_refresh.Enabled = true;
            this.timer_picturebox_refresh.Interval = 500;
            this.timer_picturebox_refresh.Tick += new System.EventHandler(this.timer_picturebox_refresh_Tick);
            // 
            // label_current
            // 
            this.label_current.AutoSize = true;
            this.label_current.Location = new System.Drawing.Point(18, 128);
            this.label_current.Name = "label_current";
            this.label_current.Size = new System.Drawing.Size(85, 15);
            this.label_current.TabIndex = 29;
            this.label_current.Text = "电池充电电流：";
            // 
            // label_temperature
            // 
            this.label_temperature.AutoSize = true;
            this.label_temperature.Location = new System.Drawing.Point(18, 77);
            this.label_temperature.Name = "label_temperature";
            this.label_temperature.Size = new System.Drawing.Size(61, 15);
            this.label_temperature.TabIndex = 28;
            this.label_temperature.Text = "激光温度：";
            // 
            // label_power
            // 
            this.label_power.AutoSize = true;
            this.label_power.Location = new System.Drawing.Point(18, 103);
            this.label_power.Name = "label_power";
            this.label_power.Size = new System.Drawing.Size(61, 15);
            this.label_power.TabIndex = 27;
            this.label_power.Text = "剩余电量：";
            // 
            // label_software_version
            // 
            this.label_software_version.AutoSize = true;
            this.label_software_version.Location = new System.Drawing.Point(17, 572);
            this.label_software_version.Name = "label_software_version";
            this.label_software_version.Size = new System.Drawing.Size(102, 15);
            this.label_software_version.TabIndex = 28;
            this.label_software_version.Text = "Software Version:";
            // 
            // label_machine_mode
            // 
            this.label_machine_mode.AutoSize = true;
            this.label_machine_mode.Location = new System.Drawing.Point(18, 27);
            this.label_machine_mode.Name = "label_machine_mode";
            this.label_machine_mode.Size = new System.Drawing.Size(61, 15);
            this.label_machine_mode.TabIndex = 30;
            this.label_machine_mode.Text = "机器型号：";
            // 
            // label_machine_firmversion
            // 
            this.label_machine_firmversion.AutoSize = true;
            this.label_machine_firmversion.Location = new System.Drawing.Point(18, 53);
            this.label_machine_firmversion.Name = "label_machine_firmversion";
            this.label_machine_firmversion.Size = new System.Drawing.Size(61, 15);
            this.label_machine_firmversion.TabIndex = 31;
            this.label_machine_firmversion.Text = "固件版本：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 600);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 29;
            this.label1.Text = "Language:";
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Language.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Items.AddRange(new object[] {
            "English",
            "简体中文",
            "日本語",
            "Français",
            "Italian",
            "German"});
            this.comboBox_Language.Location = new System.Drawing.Point(91, 595);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(116, 23);
            this.comboBox_Language.TabIndex = 30;
            this.comboBox_Language.SelectedIndexChanged += new System.EventHandler(this.comboBox_Language_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_fan_RPM);
            this.groupBox1.Controls.Add(this.label_laser_power);
            this.groupBox1.Controls.Add(this.label_laser_length);
            this.groupBox1.Controls.Add(this.label_machine_firmversion);
            this.groupBox1.Controls.Add(this.label_machine_mode);
            this.groupBox1.Controls.Add(this.label_power);
            this.groupBox1.Controls.Add(this.label_current);
            this.groupBox1.Controls.Add(this.label_temperature);
            this.groupBox1.Location = new System.Drawing.Point(14, 328);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 232);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "info";
            // 
            // label_fan_RPM
            // 
            this.label_fan_RPM.AutoSize = true;
            this.label_fan_RPM.Location = new System.Drawing.Point(18, 201);
            this.label_fan_RPM.Name = "label_fan_RPM";
            this.label_fan_RPM.Size = new System.Drawing.Size(61, 15);
            this.label_fan_RPM.TabIndex = 34;
            this.label_fan_RPM.Text = "风扇转速：";
            // 
            // label_laser_power
            // 
            this.label_laser_power.AutoSize = true;
            this.label_laser_power.Location = new System.Drawing.Point(18, 177);
            this.label_laser_power.Name = "label_laser_power";
            this.label_laser_power.Size = new System.Drawing.Size(61, 15);
            this.label_laser_power.TabIndex = 33;
            this.label_laser_power.Text = "激光功率：";
            // 
            // label_laser_length
            // 
            this.label_laser_length.AutoSize = true;
            this.label_laser_length.Location = new System.Drawing.Point(18, 153);
            this.label_laser_length.Name = "label_laser_length";
            this.label_laser_length.Size = new System.Drawing.Size(61, 15);
            this.label_laser_length.TabIndex = 32;
            this.label_laser_length.Text = "激光波长：";
            // 
            // label_connection_logo
            // 
            this.label_connection_logo.Image = global::NejeEngraverApp.Properties.Resources.connecting;
            this.label_connection_logo.Location = new System.Drawing.Point(28, 9);
            this.label_connection_logo.Name = "label_connection_logo";
            this.label_connection_logo.Size = new System.Drawing.Size(32, 32);
            this.label_connection_logo.TabIndex = 38;
            // 
            // timer_check_send_pic
            // 
            this.timer_check_send_pic.Interval = 10000;
            this.timer_check_send_pic.Tick += new System.EventHandler(this.timer_check_send_pic_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new ToolStripItem[] {
            this.StatusLabel_state});
            this.statusStrip1.Location = new System.Drawing.Point(0, 629);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1101, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel_state
            // 
            this.StatusLabel_state.Name = "StatusLabel_state";
            this.StatusLabel_state.Size = new System.Drawing.Size(42, 17);
            this.StatusLabel_state.Text = "State : ";
            // 
            // Form_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1101, 651);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label_connection_logo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox_Language);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_software_version);
            this.Controls.Add(this.button_driver);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label_Connection);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = global::NejeEngraverApp.Properties.Form_Loading.Icon;
            this.Margin = new Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "NEJE";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((ISupportInitialize)(this.trackBar_carveTime)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((ISupportInitialize)(this.pictureBox2)).EndInit();
            this.contextMenu1.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBox3)).EndInit();
            ((ISupportInitialize)(this.pictureBox4)).EndInit();
            ((ISupportInitialize)(this.pictureBox5)).EndInit();
            ((ISupportInitialize)(this.pictureBox6)).EndInit();
            ((ISupportInitialize)(this.pictureBox7)).EndInit();
            ((ISupportInitialize)(this.pictureBox8)).EndInit();
            ((ISupportInitialize)(this.pictureBox9)).EndInit();
            ((ISupportInitialize)(this.pictureBox10)).EndInit();
            ((ISupportInitialize)(this.pictureBox11)).EndInit();
            ((ISupportInitialize)(this.pictureBox12)).EndInit();
            ((ISupportInitialize)(this.pictureBox13)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((ISupportInitialize)(this.pictureBox14)).EndInit();
            this.contextMenu2.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBox15)).EndInit();
            ((ISupportInitialize)(this.pictureBox16)).EndInit();
            ((ISupportInitialize)(this.pictureBox17)).EndInit();
            ((ISupportInitialize)(this.pictureBox18)).EndInit();
            ((ISupportInitialize)(this.pictureBox19)).EndInit();
            ((ISupportInitialize)(this.pictureBox20)).EndInit();
            ((ISupportInitialize)(this.pictureBox21)).EndInit();
            ((ISupportInitialize)(this.pictureBox22)).EndInit();
            ((ISupportInitialize)(this.pictureBox23)).EndInit();
            ((ISupportInitialize)(this.pictureBox24)).EndInit();
            ((ISupportInitialize)(this.pictureBox25)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel_Laser_PWM.ResumeLayout(false);
            this.panel_Laser_PWM.PerformLayout();
            ((ISupportInitialize)(this.trackBar_PWM)).EndInit();
            this.panel_control.ResumeLayout(false);
            this.panel_control.PerformLayout();
            ((ISupportInitialize)(this.numericUpDown_times)).EndInit();
            this.panel_direction.ResumeLayout(false);
            this.groupBox_InsertText.ResumeLayout(false);
            this.groupBox_InsertText.PerformLayout();
            this.panel_size.ResumeLayout(false);
            this.panel_size.PerformLayout();
            ((ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
