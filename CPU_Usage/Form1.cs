using R4VMonitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;

namespace CPU_Usage
{
    public partial class Form1 : Form
    {
        private PerformanceCounter cpuCounter;
        private decimal percentFree;
        private decimal percentOccupied;

        public Form1()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            lblRamMax.Text = tot + "MB";
        }

        public string getCurrentCpuUsage()
        {
            return Math.Round(cpuCounter.NextValue()) + "%";
        }
        public void getCurrentCpuTemp()
        {
            //Temperature ct = new Temperature();
            //var lt = ct.Temperatures;
                
            //return ct.CurrentValue + "°C";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

            ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator();

            while (enumerator.MoveNext())
            {
                ManagementBaseObject tempObject = enumerator.Current;
                lblCpuTemp.Text = tempObject["CurrentTemperature"].ToString();
            }
        }

        public string getCurrentGpuUsage()
        {
            return "";
        }

        public string getAvailableRAM()
        {
            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            percentFree = ((decimal)phav / (decimal)tot) * 100;
            percentOccupied = 100 - percentFree;
            
            return Math.Round(percentOccupied) + "%";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblCpuUse.Text = getCurrentCpuUsage();
            //getCurrentCpuTemp();
            //lblCpuTemp.Text = getCurrentCpuTemp();
            //lblGpuUse.Text = getCurrentGpuUsage();
            lblRamUse.Text = getAvailableRAM();
        }
    }
}
