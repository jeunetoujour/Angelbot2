using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.IO;


namespace AngelBot
{

    public partial class Login : Form
    {
        // String.Format("{0}.{1}", version.FileMajorPart, version.FileMinorPart);

        public static string program = "AB2";
        public string theversion = Assembly.GetExecutingAssembly().GetName().Version.MinorRevision.ToString();
        private int retrys = 0;
        public string test;
        public bool temp;
        private int gropme = 0;
        public string MyConString = "Persist Security Info=False;SERVER=angelsuite.net;" + //this access only has execute rights, we will need to make a new Log file for XRAD
               "DATABASE=angelew3_phbb;" +
               "UID=angelew3_phbb1;" +
               "PASSWORD=[(Lb~BJ[tKnk;Allow User Variables=True;Encrypt=True; Compress=True;Connect Timeout=27;pooling=false"; //4e38f6c

        //MySqlConnection connection;
        //MySqlDataReader Reader;
        string mymac = "Nothing";

        int uid;
        string l_ip;
        string ex_ip;



        public Login()
        {

            InitializeComponent();
            password.Focus();
        }


        public long UnixTimeNow()
        {
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_TimeSpan.TotalSeconds;
        }

        public void GetExternalIp()
        {
            string whatIsMyIp = "http://www.whatismyip.com/automation/n09230945.asp";
            WebClient wc = new WebClient();
            UTF8Encoding utf8 = new UTF8Encoding();
            string requestHtml = "";
            try
            {
                requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));
            }
            catch (WebException we)
            {
                // do something with exception
                MessageBox.Show("Something fucked up yo! " + we.ToString());
            }
            try
            {
                IPAddress externalIp = IPAddress.Parse(requestHtml);
                ex_ip = externalIp.ToString();
            }
            catch (Exception) { ex_ip = "Error"; };
        }

        public bool heartbeat()
        {

            MySqlConnection connection;
            connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            //MySqlCommand infocommand = connection.CreateCommand();
            MySqlDataReader Info1;
           
            if (uid == -1)
            {
                return true;
            }
            try
            {

                connection.Open();
                command.CommandText = "SET @a :=''; CALL AngelInfo1('" + uid.ToString() + "','" + mymac + "' ,@a);SELECT @a;";
                Info1 = command.ExecuteReader();
                if (Info1.Read() != false)
                {
                    string lastmac = Info1.GetString("@a");
                    Info1.Close();
                    if (lastmac == mymac) // Success
                    {
                        command.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 8 + ");";
                        Info1 = command.ExecuteReader();
                        //Info1.Close();
                        if (connection != null || connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                        retrys = 0;
                        return true;
                    }
                    else
                    {
                        command.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 99 + ");";
                        Info1 = command.ExecuteReader();
                        //Info1.Close();
                        if (connection != null || connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                        return false;

                    }
                }
                //Info1.Close();
                if (connection != null || connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            catch (Exception)
            {
                //Info1.Close();
                if (connection != null || connection.State == System.Data.ConnectionState.Open)
                    connection.Close();

                if (retrys > 3)
                {
                    return false;
                }
                retrys++;
            };

            return true;
        }

        void newauth()
        {
            MySqlConnection connection;
            MySqlDataReader Info;
            connection = new MySqlConnection(MyConString);
            MySqlCommand infocommand = connection.CreateCommand();
            IPHostEntry hostInfo = Dns.GetHostEntry("angelsuite.net");
            IPAddress[] address = hostInfo.AddressList;
            if (address[0].ToString() != "69.4.229.168")
            {
                connection.Open();
                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 5 + ");";
                Info = infocommand.ExecuteReader();
                Info.Close();
                connection.Close();
                Environment.Exit(0);
            }
            GetExternalIp();
            //MySqlDataReader Checkgroup;
            connection.Open();
            //MySqlCommand command = connection.CreateCommand();

            if (gropme == 9)
            {
                temp = true;
                test = "temp";
                Utils.SetAppSetting("username", username.Text);

                infocommand.CommandText = "SET @a :=''; CALL AngelInfo1('" + uid.ToString() + "','" + mymac + "' ,@a);SELECT @a;";
                Info = infocommand.ExecuteReader();
                if (Info.Read() != false)
                {
                    if (!Info.IsDBNull(0))
                    {
                        Info.Close();
                        infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                        Info = infocommand.ExecuteReader();
                        Info.Close();

                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 1 + ");";
                        Info = infocommand.ExecuteReader();
                        Info.Close();
                        connection.Close();

                        Form1 f1 = new Form1();
                        f1.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                temp = false;
                test = "pmet";

                infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                Info = infocommand.ExecuteReader();
                Info.Close();

                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 48 + ");";
                Info = infocommand.ExecuteReader();
                Info.Close();
                connection.Close();
                MessageBox.Show("Demo Mode without login hmm");
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
            }


        }

        /*void auth()
        {
            // Initialize the class
            //phpBBCryptoServiceProvider cPhpBB = new phpBBCryptoServiceProvider();
            // Incase your curious. =)
            //string localHash = cPhpBB.phpbb_hash("myPassword");
            // remoteHash is a hash from your SQL database.
            // getSQLhash is a function to query the database and retrieve that hash.
            // Set the result to see if the password matches or not.
            //bool result = cPhpBB.phpbbCheckHash(pword, remotehash);
            MySqlConnection connection;
            MySqlDataReader Info;
            connection = new MySqlConnection(MyConString);
            MySqlCommand infocommand = connection.CreateCommand();
            IPHostEntry hostInfo = Dns.GetHostEntry("angelsuite.net");
            IPAddress[] address = hostInfo.AddressList;
            if (address[0].ToString() != "69.4.229.168")
            {
                connection.Open();
                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 5 + ");";
                Info = infocommand.ExecuteReader();
                Info.Close();
                connection.Close();
                Environment.Exit(0);
            }
            GetExternalIp();
            MySqlDataReader Checkgroup;
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            if (true)
            {// If true, the password matches!
                /*command.CommandText = "SET @a :='';CALL AngelCGroup('" + uid.ToString() + "',@a);SELECT @a;";
                Checkgroup = command.ExecuteReader();

                if (Checkgroup.Read() != false)
                {
                    if (!Checkgroup.IsDBNull(0)) // Yay! They have a record matching group_id = 99
                    {
                        
                if (gropme == 9)
                {

                    temp = true;
                    test = "temp";
                    Utils.SetAppSetting("username", username.Text);

                    infocommand.CommandText = "SET @a :=''; CALL AngelInfo1('" + uid.ToString() + "',@a);SELECT @a;";
                    Info = infocommand.ExecuteReader();
                    if (Info.Read() != false)
                    {
                        if (!Info.IsDBNull(0))
                        {
                            //long toffset = UnixTimeNow() - Convert.ToInt32(Info.GetString("@b"));
                            string lastmac = Info.GetString("@a");
                            if (lastmac == mymac) // Success
                            {
                                Info.Close();
                                infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                                Info = infocommand.ExecuteReader();
                                Info.Close();

                                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 1 + ");";
                                Info = infocommand.ExecuteReader();
                                Info.Close();
                                connection.Close();

                                Form1 f1 = new Form1();
                                f1.Show();
                                this.Hide();
                            }

                            else if (lastmac != mymac) // Failure
                            {
                                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 52 + ");";
                                Info = infocommand.ExecuteReader();
                                Info.Close();
                                MessageBox.Show("Already Logged in!\nPlease wait 1 min before retrying.");
                                connection.Close();
                            }
                            /*else if (lastmac != mymac && toffset >= 60) // Success
                            {
                                Info.Close();
                                infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                                Info = infocommand.ExecuteReader();
                                Info.Close();

                                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 18 + ");";
                                Info = infocommand.ExecuteReader();
                                Info.Close();
                                connection.Close();

                                Form1 f1 = new Form1();
                                f1.Show();
                                this.Hide();
                            }

                        }
                        else
                        {
                            Info.Close();
                            infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                            Info = infocommand.ExecuteReader();
                            Info.Close();

                            infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 0 + ");";
                            Info = infocommand.ExecuteReader();
                            Info.Close();
                            connection.Close();
                            MessageBox.Show("New User!");
                            Form1 f1 = new Form1();
                            f1.Show();
                            this.Hide();
                        }
                    }
                    else
                    {

                        infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                        Info = infocommand.ExecuteReader();
                        Info.Close();
                        connection.Close();
                        MessageBox.Show("You do not have a valid license for this bot.");
                    }


                }
                /* else
                {
                    //Info.Close();
                    Checkgroup.Close();
                    infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
                    Info = infocommand.ExecuteReader();
                    connection.Close();
                    MessageBox.Show("You do not have a valid license for this bot.");
                }

            }
           else
            {
                //Info.Close();
                temp = false;
                test = "pmet";
                //Checkgroup.Close();
                infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                Info = infocommand.ExecuteReader();
                Info.Close();

                infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 48 + ");";
                Info = infocommand.ExecuteReader();
                Info.Close();
                connection.Close();

                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();

            }
        }*/
        /*else // They weren't part of the Customer group - BAD BAD PERSON
        {
            Checkgroup.Close();
            infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 3 + ");";
            Info = infocommand.ExecuteReader();
            connection.Close();
            MessageBox.Show("You do not have a valid license for this bot.");
        }
    }
    else // If false, the password does not!
    {
        //infocommand.CommandText = "CALL AngelInsertLogging('" + username.Text + "','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 9 + ");";
        //Info = infocommand.ExecuteReader();
        connection.Close();
        MessageBox.Show("Authentication failed.");

    }
    }
}*/

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                // Then Enter key was pressed
                loginclick();
            }
        }

        private void loginclick()
        {
            MySqlConnection connection;
            MySqlDataReader Reader;
            connection = new MySqlConnection(MyConString);
            MySqlDataReader Info;
            MySqlCommand infocommand = connection.CreateCommand();
            phpBBCryptoServiceProvider cPhpBB = new phpBBCryptoServiceProvider();


            infocommand.CommandText = "SET @a := ''; SET @b := ''; CALL AngelLogin3('" + username.Text + "',@b,@a);SELECT @a,@b;";
            connection.Open();
            Reader = infocommand.ExecuteReader();
            if (Reader.Read() != false)
            {
                if (!Reader.IsDBNull(0))
                {

                    uid = Convert.ToInt32(Reader.GetString("@a"));
                    string tempy = Reader.GetString("@b");
                    Reader.Close();
                    if (connection != null || connection.State == System.Data.ConnectionState.Open) connection.Close();
                    if (connection != null || connection.State == System.Data.ConnectionState.Open) connection.Dispose();
                    if (cPhpBB.phpbbCheckHash(password.Text, tempy))
                    {
                        gropme = 9;
                        newauth();
                    }
                    else
                    {
                        MessageBox.Show("Authentication Failure");
                    }

                }
                else
                {

                    uid = -1;
                    temp = false;
                    test = "pmet";
                    Reader.Close();
                    /*infocommand.CommandText = "CALL AngelInsert('" + uid.ToString() + "','" + mymac + "','" + UnixTimeNow().ToString() + "');";
                    Info = infocommand.ExecuteReader();
                    Info.Close();
                    */
                    infocommand.CommandText = "CALL AngelInsertLogging('DEMO','" + ex_ip + "','" + l_ip + "','" + mymac + "'," + UnixTimeNow().ToString() + ",'" + program + "'," + theversion + "," + 48 + ");";
                    Info = infocommand.ExecuteReader();
                    Info.Close();
                    if (connection != null || connection.State == System.Data.ConnectionState.Open) connection.Close();

                    MessageBox.Show("Demo Mode");
                    Form1 f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }

            }
            else // If false, the password does not!
            {
                if (connection != null || connection.State == System.Data.ConnectionState.Open) connection.Close();
                MessageBox.Show("Authentication failed.");
            }

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (username.Text != "" || password.Text != "")
                loginclick();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Login_Load(object sender, EventArgs e)
        {

            ManagementObjectSearcher query = null;
            ManagementObjectCollection queryCollection = null;
#if (DEBUG)
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
            this.Hide();
#endif
#if (!kDEBUG)
            password.Focus();
            username.Text = Utils.GetAppSetting("username");
            password.Focus();

            try
            {
                query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");

                queryCollection = query.Get();

                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["MacAddress"] != null)
                    {
                        if (mo["MacAddress"].ToString() != "")
                        {
                            mymac = mo["MacAddress"].ToString();
                        }
                    }
                    if (mo["IPAddress"] != null)
                    {
                        if (mo["IPAddress"].ToString() != "")
                        {
                            string[] addresses = (string[])mo["IPAddress"];
                            l_ip = addresses[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex);
            }
            password.Focus();
#endif
        }

    }
}
