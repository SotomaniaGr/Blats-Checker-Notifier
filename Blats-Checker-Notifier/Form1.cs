using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;
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
            panelPing.Width = 0;
            panelPort.Width = 0;
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
            if (panelSettings.Height == 563)
            {
                panelSettings.Height = 0;
                panelPing.Width = 0;
                panelPort.Width = 0;
            }
            else
            {
                panelSettings.Height = 563;
            }
        }

        private void btnDark_Click(object sender, EventArgs e)
        {
            dark = true;
            light = false;
            BackColor = SystemColors.WindowFrame;
            panelSettings.BackColor = Color.FromArgb(76, 87, 146);
            panelPing.BackColor = Color.FromArgb(76, 87, 146);
            panelPort.BackColor = Color.FromArgb(76, 87, 146);
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            light = true;
            dark = false;
            BackColor = SystemColors.Control;
            panelSettings.BackColor = SystemColors.InactiveCaption;
            panelPing.BackColor = SystemColors.InactiveCaption;
            panelPort.BackColor = SystemColors.InactiveCaption;
        }

        private void btnSettings_MouseHover(object sender, EventArgs e)
        {
            btnSettings.ForeColor = Color.Red;
        }

        private void btnSettings_MouseLeave(object sender, EventArgs e)
        {
            btnSettings.ForeColor = Color.FromArgb(60, 150, 200);
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            if (panelPing.Width == 1208)
            {
                panelPing.Width = 0;
            }
            else
            {
                panelPing.Width = 1208;
            }
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            if (panelPort.Width == 1208)
            {
                panelPort.Width = 0;
            }
            else
            {
                panelPort.Width = 1208;
            }
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
                    EmailSender();
                }
            }
        }

        #endregion


        #region Tools

        private void EmailSender()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress(txtEmailFrom.Text);

                newMail.To.Add(txtEmailTo.Text);

                newMail.Subject = "My First Email"; // declare the email subject

                newMail.IsBodyHtml = true; newMail.Body = "<h1> This is my first Templated Email in C# </h1>"; // use HTML for the email body


                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential(txtEmailFrom.Text, txtEmailPassFrom.Text);

                client.Send(newMail); // Send the constructed mail
                lblResultPingTool.Text = "An error has been detected on one of your servers. An email was sent successfully.";
            }
            catch (Exception ex)
            {
                lblResultPingTool.Text = (ex.ToString());
            }
        }

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
                if (ex.ErrorCode == 10060)  // Port is unused and could not establish connection 
                    lblResultPingTool.Text = "Checking port " + txtPortTool2.Text + " at " + txtPortTool.Text + ": Port is Closed.";
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



        private void DropDownSelector_SelectedValueChanged(object sender, EventArgs e)
        {
            if (DropDownSelector.SelectedIndex == 0)
            {
                txtPortTool2.Text = "21";
            }
            else if (DropDownSelector.SelectedIndex == 1)
            {
                txtPortTool2.Text = "22";
            }
            else if (DropDownSelector.SelectedIndex == 2)
            {
                txtPortTool2.Text = "23";
            }
            else if (DropDownSelector.SelectedIndex == 3)
            {
                txtPortTool2.Text = "25";
            }
            else if (DropDownSelector.SelectedIndex == 4)
            {
                txtPortTool2.Text = "53";
            }
            else if (DropDownSelector.SelectedIndex == 5)
            {
                txtPortTool2.Text = "80";
            }
            else if (DropDownSelector.SelectedIndex == 6)
            {
                txtPortTool2.Text = "110";
            }
            else if (DropDownSelector.SelectedIndex == 7)
            {
                txtPortTool2.Text = "115";
            }
            else if (DropDownSelector.SelectedIndex == 8)
            {
                txtPortTool2.Text = "143";
            }
            else if (DropDownSelector.SelectedIndex == 9)
            {
                txtPortTool2.Text = "443";
            }
            else if (DropDownSelector.SelectedIndex == 10)
            {
                txtPortTool2.Text = "465";
            }
            else if (DropDownSelector.SelectedIndex == 11)
            {
                txtPortTool2.Text = "993";
            }
            else if (DropDownSelector.SelectedIndex == 12)
            {
                txtPortTool2.Text = "995";
            }
            else if (DropDownSelector.SelectedIndex == 13)
            {
                txtPortTool2.Text = "3306";
            }
            else if (DropDownSelector.SelectedIndex == 14)
            {
                txtPortTool2.Text = "6379";
            }
            else if (DropDownSelector.SelectedIndex == 15)
            {
                txtPortTool2.Text = "5900";
            }
        }

        #endregion


    }
}