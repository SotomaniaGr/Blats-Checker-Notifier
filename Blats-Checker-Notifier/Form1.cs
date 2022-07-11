using System.Net.NetworkInformation;
using System.Net;
namespace Blats_Checker_Notifier
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
        int minutes = 0;
        int seconds = 60;
        public bool dark, light;
        public Form1()
        {
            InitializeComponent();
            panelSettings.Height = 0;
            InitTimer();
            InitTimer2();
        }

        #region Form Gui Settings
        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Red;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.FromArgb(60, 150, 200);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMinimize_MouseHover(object sender, EventArgs e)
        {
            BtnMinimize.ForeColor = Color.Gray;
        }

        private void BtnMinimize_MouseLeave(object sender, EventArgs e)
        {
            BtnMinimize.ForeColor = Color.FromArgb(60, 150, 200);
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (panelSettings.Height == 449)
            {
                panelSettings.Height = 0;
            }
            else
            {
                panelSettings.Height = 449;
            }
        }

        private void btnDark_Click(object sender, EventArgs e)
        {
            dark = true;
            light = false;
            BackColor = SystemColors.WindowFrame;
            panelSettings.BackColor = Color.FromArgb(76, 87, 146);
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            light = true;
            dark = false;
            BackColor = SystemColors.Control;
            panelSettings.BackColor = SystemColors.InactiveCaption;
        }

        private void btnSettings_MouseHover(object sender, EventArgs e)
        {
            btnSettings.ForeColor = Color.Red;
        }

        private void btnSettings_MouseLeave(object sender, EventArgs e)
        {
            btnSettings.ForeColor = Color.FromArgb(60, 150, 200);
        }

        #endregion


        #region Checker & Timers

        public void InitTimer()
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 300000; // in miliseconds
            timer1.Start();
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            ScriptChecker();
            InitTimer2();
        }

        public void ScriptChecker()
        {
            //edw tha mpoun ta scripts
        }

        public void InitTimer2()
        {

            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 1000; // in miliseconds
            timer2.Start();
        }

        public void timer2_Tick(object sender, EventArgs e)
        {
            CountdownTimer();
        }

        public void CountdownTimer()
        {

            seconds--;
            if (minutes < 9)
            {
                lblChecker.Text = "Next check in: 0" + minutes + ":" + seconds;
            }
            else
            {
                lblChecker.Text = "Next check in: " + minutes + ":" + seconds;
            }


            if (seconds < 10)
            {
                lblChecker.Text = "Next check in: 0" + minutes + ":0" + seconds;
            }
            if (seconds == 0)
            {
                if (minutes > 0)
                {
                    minutes--;
                    seconds = 60;
                }
                else
                {
                    timer2.Stop();
                    lblChecker.Text = ("Status OK!");
                }
            }
        }

        #endregion

        #region Tools

        private void btnPingTool_Click(object sender, EventArgs e)
        {
            try
            {
                Ping p = new Ping();
                PingReply r;
                string s;
                s = txtPingTool.Text;
                r = p.Send(s);
                if (r.Status == IPStatus.Success)
                {
                    lblResultPingTool.Text = "Ping to " + s.ToString() + "[" + r.Address.ToString() + "]" + " Successful"
                       + " Response delay = " + r.RoundtripTime.ToString() + " ms" + "\n";
                }
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(txtPingTool.Text) || txtPingTool.Text == "")
                    lblResultPingTool.Text = "You need to enter ip or hostname. IP field cannot be blank.";
                else
                    lblResultPingTool.Text = "IP or Hostname is not reachable.";
            }

        }

        private void btnPortTool_Click(object sender, EventArgs e)
        {
            try
            {
                string hostname = txtPortTool.Text;
                int portno = int.Parse(txtPortTool2.Text);
                IPAddress ipa = (IPAddress)Dns.GetHostAddresses(hostname)[0];

                System.Net.Sockets.Socket sock = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                sock.Connect(ipa, portno);
                if (sock.Connected == true)  // Port is in use and connection is successful
                    lblResultPingTool.Text = "Checking port "+ txtPortTool2.Text + " at " + txtPortTool.Text + ": Port is Open!";
                sock.Close();

            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.ErrorCode == 10061)  // Port is unused and could not establish connection 
                    lblResultPingTool.Text = lblResultPingTool.Text = "Checking port " + txtPortTool2.Text + " at " + txtPortTool.Text + ": Port is Closed.";
                else
                    lblResultPingTool.Text = "Error occured with ip/hostname or port.";
            }
            catch
            {
                if (string.IsNullOrWhiteSpace(txtPortTool.Text) || txtPortTool.Text == "")
                    lblResultPingTool.Text = "You need to enter ip or hostname. IP field cannot be blank.";
                else if (string.IsNullOrWhiteSpace(txtPortTool2.Text) || txtPortTool2.Text == "")
                    lblResultPingTool.Text = "You need to enter port number. Port field cannot be blank.";
            }
        }

        #endregion


    }
}