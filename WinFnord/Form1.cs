using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DemonStar.FnordLib;

namespace DemonStar.WinFnord
{
    
    public partial class Form1 : Form
    {
        // Create a new Fnorder to generate messages
        Fnorder fnord = new Fnorder();

        // Create a variable to count the number of ticks
        int ticks = 0;

        // Timer target and set it to a default of 5 matching the defualt value
        // of the TrackBar
        int targetTime = 5;

        // Random to get new message interval when random intervals are requested
        Random r = new Random();

        private string[] titles = new string[] {
                "Message from the Illuminati:",
                "Fnord!"
            };

        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            // Change the minutes label and message interval according to the TrackBar
            minutesLabel.Text = trackBar1.Value.ToString();
            targetTime = trackBar1.Value;
        }
        
        private void randomFreq_CheckedChanged(object sender, EventArgs e)
        {
            if (randomFreq.Checked == true)
            {
                // Disable the TrackBar and its friends
                trackBar1.Enabled = false;
                label2.Enabled = false;
                minutesLabel.Enabled = false;
                // Get a random number betwen 5 and 31 to use as the new message interval
                targetTime = r.Next(5, 31);
            }
            else
            {
                // Enable the TrackBar his friends
                trackBar1.Enabled = true;
                label2.Enabled = true;
                minutesLabel.Enabled = false;
                // message interval now controlled by the TrackBar
                targetTime = trackBar1.Value;
            }

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks += 1;
            // Check if the modulus of ticks / 10 is zero so that ten message
            // balloons don't get displayed in one second when the message interval
            // value is met
            if ((ticks % 10 == 0) && (ticks / 600 == targetTime))
            {
                ticks = 0;
                displayMessageBalloon();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // I can't wait! Display a message NOW!
            displayMessageBalloon();
        }

        private void displayMessageBalloon()
        {
            string tempMessage = fnord.GetMessage();
            // Check if appendFnord is checked and handle
            if (appendFnord.Checked == true)
                tempMessage = tempMessage + " Fnord.";

            //notifyIcon1.BalloonTipTitle = "Message from the Illuminati:";
            notifyIcon1.BalloonTipTitle = titles[r.Next(titles.Length)];
            // Set balloon text to message from fnorder
            notifyIcon1.BalloonTipText = tempMessage;
            // OS enforces minimum and maximum timeouts for message balloons.
            // Minimum is apperantly 10 seconds so that's what is set here.
            // Desired value is really 5 seconds.
            // http://msdn2.microsoft.com/en-us/library/ms160064(vs.80).aspx
            notifyIcon1.ShowBalloonTip(10000);

            // Check if user wants random message intervals and if so, set a new interval
            if (randomFreq.Checked == true)
                targetTime = r.Next(5, 31);
        }
    }
}