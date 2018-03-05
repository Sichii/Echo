using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAWindower
{
    public partial class MainForm : Form
    {
        private Thread HandleProcesses;
        private List<Process> MasterList => Clients.Select(c => c.Process).ToList();
        private List<ProcessInfo> Clients;

        public MainForm()
        {
            Clients = new List<ProcessInfo>();
            InitializeComponent();

            HandleProcesses = new Thread(() => ProcessHandler());
        }

        private void ProcessHandler()
        {
            while (this != null)
            {
                foreach (Process proc in Process.GetProcessesByName("Darkages.exe"))
                    if (!MasterList.Contains(proc))
                    {
                        ProcessInfo client = new ProcessInfo();
                        client.Process = proc;
                        client.Handle = proc.Handle;

                        Clients.Add(client);
                    }
                Thread.Sleep(10);
            }
        }
    }
}
