﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDWindowsApp
{
    public partial class SentThree : UserControl
    {
        public SentThree()
        {
            InitializeComponent();
        }

        private void buttonConfirm_Clicked(object sender, EventArgs e)
        {
            AppPanel.sentThreeFrame.Visible = false;
            AppPanel.mainFrame.Visible = true;
        }
    }
}
