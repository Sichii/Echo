using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAWindower
{
    public partial class MainForm : Form
    {
        private bool Active = true;
        private Thread HandleProcesses;
        private List<int> MasterList => Clients.Select(c => c.Proc.Id).ToList();
        private List<Client> Clients;

        public MainForm()
        {
            Clients = new List<Client>();
            InitializeComponent();

            HandleProcesses = new Thread(new ThreadStart(ProcessHandler));
            //HandleProcesses.Start();
        }

        private void ProcessHandler()
        {
            while (Active)
            {
                while (this == null)
                    Thread.Sleep(10);
                //for each open process named Darkages.exe
                foreach (Process proc in Process.GetProcessesByName("Darkages"))
                    //if that process hasnt been handled yet
                    if (!MasterList.Contains(proc.Id))
                    {
                        //see if dawnd needs to be written to the base directory
                        string dawnd = $@"{proc.MainModule.FileName.Replace("Darkages.exe", "")}dawnd.dll";
                        if (!File.Exists(dawnd))
                            File.WriteAllBytes(dawnd, Properties.Resources.dawnd);

                        //create a client out of this darkages process
                        Client client = new Client(this, proc.Id);
                        //inject dawnd into da
                        bool x = client.InjectDLL();
                        //create a dwmapi thumbnail for the client
                        client.Thumb.SetClient(client);
                        //add client to the master list
                        Clients.Add(client);
                    }

                Thread.Sleep(10);
            }
            return;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Active = false;
        }
    }
}
