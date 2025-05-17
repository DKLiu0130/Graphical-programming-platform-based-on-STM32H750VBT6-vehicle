using System.Drawing.Text;


namespace WinFormsApp1
{

    partial class RadMainForm
    {
        PrivateFontCollection pfc = new PrivateFontCollection(); // using System.Drawing.Text;

        /// <summary>
        /// 添加第三方字体
        /// </summary>
        void AddPrivateFont()
        {
            try
            {
                // 假设字体文件位于项目下的 Fonts 文件夹
                string fontDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts");

                string[] fontNames = { "AlibabaPuHuiTi-3-45-Light.ttf", "AlibabaPuHuiTi-3-65-Medium.ttf" };

                foreach (var fontName in fontNames)
                {
                    string fontPath = Path.Combine(fontDir, fontName);
                    if (File.Exists(fontPath))
                    {
                        pfc.AddFontFile(fontPath);
                    }
                    else
                    {
                        MessageBox.Show($"字体文件未找到: {fontPath}", "错误");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载字体失败: {ex.Message}", "错误");
            }
        }



        private System.ComponentModel.IContainer components = null;
        
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components=new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadMainForm));
            radButton1=new Telerik.WinControls.UI.RadButton();
            radButton2=new Telerik.WinControls.UI.RadButton();
            radButton3=new Telerik.WinControls.UI.RadButton();
            radButton4=new Telerik.WinControls.UI.RadButton();
            minimize_radButton=new Telerik.WinControls.UI.RadButton();
            maximize_radButton=new Telerik.WinControls.UI.RadButton();
            close_radButton=new Telerik.WinControls.UI.RadButton();
            MoveRadPanel0=new Telerik.WinControls.UI.RadPanel();
            right_radPanel=new Telerik.WinControls.UI.RadPanel();
            text_radPanel=new Telerik.WinControls.UI.RadPanel();
            radTextBox=new Telerik.WinControls.UI.RadTextBox();
            ShrinkRadButton2=new Telerik.WinControls.UI.RadButton();
            MoveRadPanel2=new Telerik.WinControls.UI.RadPanel();
            CapsuleRadButton12=new Telerik.WinControls.UI.RadButton();
            status_radLabel=new Telerik.WinControls.UI.RadLabel();
            status_radSeparator=new Telerik.WinControls.UI.RadSeparator();
            statustext_radLabel=new Telerik.WinControls.UI.RadLabel();
            radSeparator5=new Telerik.WinControls.UI.RadSeparator();
            radSeparator6=new Telerik.WinControls.UI.RadSeparator();
            radSeparator7=new Telerik.WinControls.UI.RadSeparator();
            CapsuleRadButton13=new Telerik.WinControls.UI.RadButton();
            voice_radSeparator=new Telerik.WinControls.UI.RadSeparator();
            voice_radLabel=new Telerik.WinControls.UI.RadLabel();
            radSeparator3=new Telerik.WinControls.UI.RadSeparator();
            radSeparator2=new Telerik.WinControls.UI.RadSeparator();
            radSeparator1=new Telerik.WinControls.UI.RadSeparator();
            CapsuleRadButton11=new Telerik.WinControls.UI.RadButton();
            module_radSeparator=new Telerik.WinControls.UI.RadSeparator();
            modle_radLabel=new Telerik.WinControls.UI.RadLabel();
            left_radPanel=new Telerik.WinControls.UI.RadPanel();
            ShrinkRadButton1=new Telerik.WinControls.UI.RadButton();
            MoveRadPanel1=new Telerik.WinControls.UI.RadPanel();
            CapsuleRadButton10=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton9=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton8=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton7=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton6=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton5=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton4=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton3=new Telerik.WinControls.UI.RadButton();
            CapsuleRadButton2=new Telerik.WinControls.UI.RadButton();
            create_enter=new Telerik.WinControls.UI.RadButton();
            function_radSeparator=new Telerik.WinControls.UI.RadSeparator();
            function_radLabel=new Telerik.WinControls.UI.RadLabel();
            DragPanel=new Telerik.WinControls.UI.RadPanel();
            scroll_radPanel=new Telerik.WinControls.UI.RadPanel();
            radRightScrollBar=new Telerik.WinControls.UI.RadVScrollBar();
            radButtonScrollBar=new Telerik.WinControls.UI.RadHScrollBar();
            contextMenuStrip1=new ContextMenuStrip(components);
            删除ToolStripMenuItem=new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)radButton1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButton2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButton3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButton4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)minimize_radButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maximize_radButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)close_radButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel0).BeginInit();
            ((System.ComponentModel.ISupportInitialize)right_radPanel).BeginInit();
            right_radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)text_radPanel).BeginInit();
            text_radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radTextBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ShrinkRadButton2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)status_radLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)status_radSeparator).BeginInit();
            ((System.ComponentModel.ISupportInitialize)statustext_radLabel).BeginInit();
            statustext_radLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radSeparator5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)voice_radSeparator).BeginInit();
            ((System.ComponentModel.ISupportInitialize)voice_radLabel).BeginInit();
            voice_radLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radSeparator3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)module_radSeparator).BeginInit();
            ((System.ComponentModel.ISupportInitialize)modle_radLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)left_radPanel).BeginInit();
            left_radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ShrinkRadButton1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)create_enter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)function_radSeparator).BeginInit();
            ((System.ComponentModel.ISupportInitialize)function_radLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DragPanel).BeginInit();
            DragPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scroll_radPanel).BeginInit();
            scroll_radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radRightScrollBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButtonScrollBar).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radButton1
            // 
            radButton1.BackColor=Color.White;
            radButton1.ForeColor=Color.Black;
            radButton1.Location=new Point(21,9);
            radButton1.Name="radButton1";
            radButton1.Size=new Size(90,40);
            radButton1.TabIndex=3;
            radButton1.Text="帮助";
            radButton1.Click+=radButton1_Click;
            // 
            // radButton2
            // 
            radButton2.BackColor=Color.White;
            radButton2.ForeColor=Color.Black;
            radButton2.Location=new Point(117,9);
            radButton2.Name="radButton2";
            radButton2.Size=new Size(90,40);
            radButton2.TabIndex=4;
            radButton2.Text="读取";
            radButton2.Click+=radButton2_Click;
            // 
            // radButton3
            // 
            radButton3.BackColor=Color.White;
            radButton3.ForeColor=Color.Black;
            radButton3.Location=new Point(213,9);
            radButton3.Name="radButton3";
            radButton3.Size=new Size(90,40);
            radButton3.TabIndex=5;
            radButton3.Text="保存";
            radButton3.Click+=radButton3_Click;
            // 
            // radButton4
            // 
            radButton4.BackColor=Color.White;
            radButton4.ForeColor=Color.Black;
            radButton4.Location=new Point(309,9);
            radButton4.Name="radButton4";
            radButton4.Size=new Size(90,40);
            radButton4.TabIndex=6;
            radButton4.Text="运行";
            radButton4.Click+=radButton4_Click;
            // 
            // minimize_radButton
            // 
            minimize_radButton.Location=new Point(895,0);
            minimize_radButton.Name="minimize_radButton";
            minimize_radButton.Size=new Size(50,50);
            minimize_radButton.TabIndex=7;
            minimize_radButton.Text=" ";
            // 
            // maximize_radButton
            // 
            maximize_radButton.Location=new Point(945,0);
            maximize_radButton.Name="maximize_radButton";
            maximize_radButton.Size=new Size(50,50);
            maximize_radButton.TabIndex=8;
            maximize_radButton.Text=" ";
            // 
            // close_radButton
            // 
            close_radButton.Location=new Point(994,0);
            close_radButton.Name="close_radButton";
            close_radButton.Size=new Size(50,50);
            close_radButton.TabIndex=8;
            close_radButton.Text=" ";
            // 
            // MoveRadPanel0
            // 
            MoveRadPanel0.BackColor=Color.Silver;
            MoveRadPanel0.Location=new Point(400,9);
            MoveRadPanel0.Name="MoveRadPanel0";
            MoveRadPanel0.Size=new Size(277,12);
            MoveRadPanel0.TabIndex=10;
            // 
            // right_radPanel
            // 
            right_radPanel.BackColor=Color.FromArgb(207,217,250);
            right_radPanel.Controls.Add(text_radPanel);
            right_radPanel.Controls.Add(ShrinkRadButton2);
            right_radPanel.Controls.Add(MoveRadPanel2);
            right_radPanel.Controls.Add(CapsuleRadButton12);
            right_radPanel.Controls.Add(status_radLabel);
            right_radPanel.Controls.Add(status_radSeparator);
            right_radPanel.Controls.Add(statustext_radLabel);
            right_radPanel.Controls.Add(CapsuleRadButton13);
            right_radPanel.Controls.Add(voice_radSeparator);
            right_radPanel.Controls.Add(voice_radLabel);
            right_radPanel.Controls.Add(CapsuleRadButton11);
            right_radPanel.Controls.Add(module_radSeparator);
            right_radPanel.Controls.Add(modle_radLabel);
            right_radPanel.Location=new Point(785,3);
            right_radPanel.Name="right_radPanel";
            right_radPanel.Size=new Size(227,563);
            right_radPanel.TabIndex=3;
            // 
            // text_radPanel
            // 
            text_radPanel.BackColor=Color.White;
            text_radPanel.Controls.Add(radTextBox);
            text_radPanel.Location=new Point(28,280);
            text_radPanel.Name="text_radPanel";
            text_radPanel.Size=new Size(174,184);
            text_radPanel.TabIndex=11;
            // 
            // radTextBox
            // 
            radTextBox.BackColor=Color.FromArgb(255,192,192);
            radTextBox.Location=new Point(3,3);
            radTextBox.Name="radTextBox";
            radTextBox.Size=new Size(140,20);
            radTextBox.TabIndex=8;
            radTextBox.Text="等待录音";
            radTextBox.TextChanged+=radTextBox_TextChanged;
            // 
            // ShrinkRadButton2
            // 
            ShrinkRadButton2.Location=new Point(190,7);
            ShrinkRadButton2.Name="ShrinkRadButton2";
            ShrinkRadButton2.Size=new Size(25,25);
            ShrinkRadButton2.TabIndex=14;
            // 
            // MoveRadPanel2
            // 
            MoveRadPanel2.BackColor=Color.Silver;
            MoveRadPanel2.Location=new Point(18,552);
            MoveRadPanel2.Name="MoveRadPanel2";
            MoveRadPanel2.Size=new Size(194,7);
            MoveRadPanel2.TabIndex=10;
            // 
            // CapsuleRadButton12
            // 
            CapsuleRadButton12.BackColor=Color.White;
            CapsuleRadButton12.ForeColor=Color.Black;
            CapsuleRadButton12.Location=new Point(28,111);
            CapsuleRadButton12.Name="CapsuleRadButton12";
            CapsuleRadButton12.Size=new Size(174,44);
            CapsuleRadButton12.TabIndex=5;
            CapsuleRadButton12.Text="Lover";
            CapsuleRadButton12.Click+=CapsuleRadButton12_Click;
            // 
            // status_radLabel
            // 
            status_radLabel.Font=new Font("Microsoft Sans Serif",15F,FontStyle.Regular,GraphicsUnit.Point,0);
            status_radLabel.Location=new Point(46,520);
            status_radLabel.Name="status_radLabel";
            status_radLabel.Size=new Size(145,27);
            status_radLabel.TabIndex=6;
            status_radLabel.Text="录音准备就绪";
            // 
            // status_radSeparator
            // 
            status_radSeparator.Location=new Point(13,506);
            status_radSeparator.Name="status_radSeparator";
            status_radSeparator.Size=new Size(211,10);
            status_radSeparator.TabIndex=3;
            // 
            // statustext_radLabel
            // 
            statustext_radLabel.Controls.Add(radSeparator5);
            statustext_radLabel.Controls.Add(radSeparator6);
            statustext_radLabel.Controls.Add(radSeparator7);
            statustext_radLabel.Font=new Font("Microsoft Sans Serif",15F,FontStyle.Regular,GraphicsUnit.Point,0);
            statustext_radLabel.Location=new Point(15,475);
            statustext_radLabel.Name="statustext_radLabel";
            statustext_radLabel.Size=new Size(100,27);
            statustext_radLabel.TabIndex=5;
            statustext_radLabel.Text="实时状态";
            // 
            // radSeparator5
            // 
            radSeparator5.Location=new Point(0,32);
            radSeparator5.Name="radSeparator5";
            radSeparator5.Size=new Size(211,10);
            radSeparator5.TabIndex=4;
            // 
            // radSeparator6
            // 
            radSeparator6.Location=new Point(0,35);
            radSeparator6.Name="radSeparator6";
            radSeparator6.Size=new Size(211,10);
            radSeparator6.TabIndex=4;
            // 
            // radSeparator7
            // 
            radSeparator7.Location=new Point(3,38);
            radSeparator7.Name="radSeparator7";
            radSeparator7.Size=new Size(211,10);
            radSeparator7.TabIndex=4;
            // 
            // CapsuleRadButton13
            // 
            CapsuleRadButton13.BackColor=Color.White;
            CapsuleRadButton13.ForeColor=Color.Black;
            CapsuleRadButton13.Location=new Point(28,222);
            CapsuleRadButton13.Name="CapsuleRadButton13";
            CapsuleRadButton13.Size=new Size(174,44);
            CapsuleRadButton13.TabIndex=5;
            CapsuleRadButton13.Text="Lover";
            CapsuleRadButton13.Click+=CapsuleRadButton13_Click;
            // 
            // voice_radSeparator
            // 
            voice_radSeparator.Location=new Point(13,202);
            voice_radSeparator.Name="voice_radSeparator";
            voice_radSeparator.Size=new Size(211,10);
            voice_radSeparator.TabIndex=3;
            // 
            // voice_radLabel
            // 
            voice_radLabel.Controls.Add(radSeparator3);
            voice_radLabel.Controls.Add(radSeparator2);
            voice_radLabel.Controls.Add(radSeparator1);
            voice_radLabel.Font=new Font("Microsoft Sans Serif",15F,FontStyle.Regular,GraphicsUnit.Point,0);
            voice_radLabel.Location=new Point(15,174);
            voice_radLabel.Name="voice_radLabel";
            voice_radLabel.Size=new Size(100,27);
            voice_radLabel.TabIndex=3;
            voice_radLabel.Text="语音输入";
            // 
            // radSeparator3
            // 
            radSeparator3.Location=new Point(0,32);
            radSeparator3.Name="radSeparator3";
            radSeparator3.Size=new Size(211,10);
            radSeparator3.TabIndex=4;
            // 
            // radSeparator2
            // 
            radSeparator2.Location=new Point(0,35);
            radSeparator2.Name="radSeparator2";
            radSeparator2.Size=new Size(211,10);
            radSeparator2.TabIndex=4;
            // 
            // radSeparator1
            // 
            radSeparator1.Location=new Point(3,38);
            radSeparator1.Name="radSeparator1";
            radSeparator1.Size=new Size(211,10);
            radSeparator1.TabIndex=4;
            // 
            // CapsuleRadButton11
            // 
            CapsuleRadButton11.BackColor=Color.White;
            CapsuleRadButton11.ForeColor=Color.Black;
            CapsuleRadButton11.Location=new Point(28,52);
            CapsuleRadButton11.Name="CapsuleRadButton11";
            CapsuleRadButton11.Size=new Size(174,44);
            CapsuleRadButton11.TabIndex=4;
            CapsuleRadButton11.Text="Lover";
            CapsuleRadButton11.Click+=CapsuleRadButton11_Click;
            // 
            // module_radSeparator
            // 
            module_radSeparator.Location=new Point(12,31);
            module_radSeparator.Name="module_radSeparator";
            module_radSeparator.Size=new Size(211,10);
            module_radSeparator.TabIndex=3;
            // 
            // modle_radLabel
            // 
            modle_radLabel.Font=new Font("Microsoft Sans Serif",15F,FontStyle.Regular,GraphicsUnit.Point,0);
            modle_radLabel.Location=new Point(12,3);
            modle_radLabel.Name="modle_radLabel";
            modle_radLabel.Size=new Size(100,27);
            modle_radLabel.TabIndex=2;
            modle_radLabel.Text="模式选择";
            // 
            // left_radPanel
            // 
            left_radPanel.BackColor=Color.FromArgb(209,238,226);
            left_radPanel.Controls.Add(ShrinkRadButton1);
            left_radPanel.Controls.Add(MoveRadPanel1);
            left_radPanel.Controls.Add(CapsuleRadButton10);
            left_radPanel.Controls.Add(CapsuleRadButton9);
            left_radPanel.Controls.Add(CapsuleRadButton8);
            left_radPanel.Controls.Add(CapsuleRadButton7);
            left_radPanel.Controls.Add(CapsuleRadButton6);
            left_radPanel.Controls.Add(CapsuleRadButton5);
            left_radPanel.Controls.Add(CapsuleRadButton4);
            left_radPanel.Controls.Add(CapsuleRadButton3);
            left_radPanel.Controls.Add(CapsuleRadButton2);
            left_radPanel.Controls.Add(create_enter);
            left_radPanel.Controls.Add(function_radSeparator);
            left_radPanel.Controls.Add(function_radLabel);
            left_radPanel.Location=new Point(14,3);
            left_radPanel.Name="left_radPanel";
            left_radPanel.Size=new Size(217,563);
            left_radPanel.TabIndex=2;
            // 
            // ShrinkRadButton1
            // 
            ShrinkRadButton1.ForeColor=Color.White;
            ShrinkRadButton1.Location=new Point(178,9);
            ShrinkRadButton1.Name="ShrinkRadButton1";
            ShrinkRadButton1.Size=new Size(25,25);
            ShrinkRadButton1.TabIndex=13;
            // 
            // MoveRadPanel1
            // 
            MoveRadPanel1.BackColor=Color.Silver;
            MoveRadPanel1.Location=new Point(9,552);
            MoveRadPanel1.Name="MoveRadPanel1";
            MoveRadPanel1.Size=new Size(194,7);
            MoveRadPanel1.TabIndex=9;
            // 
            // CapsuleRadButton10
            // 
            CapsuleRadButton10.BackColor=Color.White;
            CapsuleRadButton10.ForeColor=Color.Black;
            CapsuleRadButton10.Location=new Point(21,502);
            CapsuleRadButton10.Name="CapsuleRadButton10";
            CapsuleRadButton10.Size=new Size(174,44);
            CapsuleRadButton10.TabIndex=12;
            CapsuleRadButton10.Text="Lover";
            CapsuleRadButton10.Click+=CapsuleRadButton10_Click;
            // 
            // CapsuleRadButton9
            // 
            CapsuleRadButton9.BackColor=Color.White;
            CapsuleRadButton9.ForeColor=Color.Black;
            CapsuleRadButton9.Location=new Point(21,452);
            CapsuleRadButton9.Name="CapsuleRadButton9";
            CapsuleRadButton9.Size=new Size(174,44);
            CapsuleRadButton9.TabIndex=11;
            CapsuleRadButton9.Text="Lover";
            CapsuleRadButton9.Click+=CapsuleRadButton9_Click;
            // 
            // CapsuleRadButton8
            // 
            CapsuleRadButton8.BackColor=Color.White;
            CapsuleRadButton8.ForeColor=Color.Black;
            CapsuleRadButton8.Location=new Point(21,402);
            CapsuleRadButton8.Name="CapsuleRadButton8";
            CapsuleRadButton8.Size=new Size(174,44);
            CapsuleRadButton8.TabIndex=10;
            CapsuleRadButton8.Text="Lover";
            CapsuleRadButton8.Click+=CapsuleRadButton8_Click;
            // 
            // CapsuleRadButton7
            // 
            CapsuleRadButton7.BackColor=Color.White;
            CapsuleRadButton7.ForeColor=Color.Black;
            CapsuleRadButton7.Location=new Point(21,352);
            CapsuleRadButton7.Name="CapsuleRadButton7";
            CapsuleRadButton7.Size=new Size(174,44);
            CapsuleRadButton7.TabIndex=9;
            CapsuleRadButton7.Text="Lover";
            CapsuleRadButton7.Click+=CapsuleRadButton7_Click;
            // 
            // CapsuleRadButton6
            // 
            CapsuleRadButton6.BackColor=Color.White;
            CapsuleRadButton6.ForeColor=Color.Black;
            CapsuleRadButton6.Location=new Point(21,302);
            CapsuleRadButton6.Name="CapsuleRadButton6";
            CapsuleRadButton6.Size=new Size(174,44);
            CapsuleRadButton6.TabIndex=8;
            CapsuleRadButton6.Text="Lover";
            CapsuleRadButton6.Click+=CapsuleRadButton6_Click;
            // 
            // CapsuleRadButton5
            // 
            CapsuleRadButton5.BackColor=Color.White;
            CapsuleRadButton5.ForeColor=Color.Black;
            CapsuleRadButton5.Location=new Point(21,252);
            CapsuleRadButton5.Name="CapsuleRadButton5";
            CapsuleRadButton5.Size=new Size(174,44);
            CapsuleRadButton5.TabIndex=7;
            CapsuleRadButton5.Text="Lover";
            CapsuleRadButton5.Click+=CapsuleRadButton5_Click;
            // 
            // CapsuleRadButton4
            // 
            CapsuleRadButton4.BackColor=Color.White;
            CapsuleRadButton4.ForeColor=Color.Black;
            CapsuleRadButton4.Location=new Point(21,202);
            CapsuleRadButton4.Name="CapsuleRadButton4";
            CapsuleRadButton4.Size=new Size(174,44);
            CapsuleRadButton4.TabIndex=6;
            CapsuleRadButton4.Text="Lover";
            CapsuleRadButton4.Click+=CapsuleRadButton4_Click;
            // 
            // CapsuleRadButton3
            // 
            CapsuleRadButton3.BackColor=Color.White;
            CapsuleRadButton3.ForeColor=Color.Black;
            CapsuleRadButton3.Location=new Point(21,152);
            CapsuleRadButton3.Name="CapsuleRadButton3";
            CapsuleRadButton3.Size=new Size(174,44);
            CapsuleRadButton3.TabIndex=5;
            CapsuleRadButton3.Text="Lover";
            CapsuleRadButton3.Click+=CapsuleRadButton3_Click;
            // 
            // CapsuleRadButton2
            // 
            CapsuleRadButton2.BackColor=Color.White;
            CapsuleRadButton2.ForeColor=Color.Black;
            CapsuleRadButton2.Location=new Point(21,102);
            CapsuleRadButton2.Name="CapsuleRadButton2";
            CapsuleRadButton2.Size=new Size(174,44);
            CapsuleRadButton2.TabIndex=4;
            CapsuleRadButton2.Text="Lover";
            CapsuleRadButton2.Click+=CapsuleRadButton2_Click;
            // 
            // create_enter
            // 
            create_enter.BackColor=Color.White;
            create_enter.ForeColor=Color.Black;
            create_enter.Location=new Point(21,52);
            create_enter.Name="create_enter";
            create_enter.Size=new Size(174,44);
            create_enter.TabIndex=3;
            create_enter.Text="程序入口";
            create_enter.Click+=CapsuleRadButton1_Click;
            // 
            // function_radSeparator
            // 
            function_radSeparator.Location=new Point(3,31);
            function_radSeparator.Name="function_radSeparator";
            function_radSeparator.Size=new Size(211,15);
            function_radSeparator.TabIndex=2;
            // 
            // function_radLabel
            // 
            function_radLabel.Font=new Font("Microsoft Sans Serif",15F,FontStyle.Regular,GraphicsUnit.Point,134);
            function_radLabel.Location=new Point(9,7);
            function_radLabel.Name="function_radLabel";
            function_radLabel.Size=new Size(77,27);
            function_radLabel.TabIndex=1;
            function_radLabel.Text="功能块";
            // 
            // DragPanel
            // 
            DragPanel.BackColor=Color.White;
            DragPanel.Controls.Add(left_radPanel);
            DragPanel.Controls.Add(right_radPanel);
            DragPanel.Location=new Point(4,55);
            DragPanel.Name="DragPanel";
            DragPanel.Size=new Size(3000,1500);
            DragPanel.TabIndex=1;
            DragPanel.Paint+=button_radPanel_Paint;
            // 
            // scroll_radPanel
            // 
            scroll_radPanel.BackColor=Color.White;
            scroll_radPanel.Controls.Add(radRightScrollBar);
            scroll_radPanel.Controls.Add(radButtonScrollBar);
            scroll_radPanel.Location=new Point(4,55);
            scroll_radPanel.Name="scroll_radPanel";
            scroll_radPanel.Size=new Size(1034,595);
            scroll_radPanel.TabIndex=4;
            // 
            // radRightScrollBar
            // 
            radRightScrollBar.Location=new Point(1025,10);
            radRightScrollBar.Name="radRightScrollBar";
            radRightScrollBar.Size=new Size(6,555);
            radRightScrollBar.TabIndex=4;
            // 
            // radButtonScrollBar
            // 
            radButtonScrollBar.Location=new Point(14,586);
            radButtonScrollBar.Name="radButtonScrollBar";
            radButtonScrollBar.Size=new Size(1003,6);
            radButtonScrollBar.TabIndex=4;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize=new Size(24,24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 删除ToolStripMenuItem });
            contextMenuStrip1.Name="contextMenuStrip1";
            contextMenuStrip1.Size=new Size(101,26);
            // 
            // 删除ToolStripMenuItem
            // 
            删除ToolStripMenuItem.Name="删除ToolStripMenuItem";
            删除ToolStripMenuItem.Size=new Size(100,22);
            删除ToolStripMenuItem.Text="删除";
            删除ToolStripMenuItem.Click+=删除ToolStripMenuItem_Click;
            // 
            // RadMainForm
            // 
            AutoScaleBaseSize=new Size(7,17);
            AutoScaleDimensions=new SizeF(7F,17F);
            AutoScaleMode=AutoScaleMode.Font;
            BackColor=SystemColors.ActiveCaption;
            BackgroundImage=(Image)resources.GetObject("$this.BackgroundImage");
            ClientSize=new Size(1050,650);
            Controls.Add(MoveRadPanel0);
            Controls.Add(close_radButton);
            Controls.Add(maximize_radButton);
            Controls.Add(minimize_radButton);
            Controls.Add(radButton4);
            Controls.Add(radButton3);
            Controls.Add(radButton2);
            Controls.Add(radButton1);
            Controls.Add(DragPanel);
            Controls.Add(scroll_radPanel);
            FormBorderStyle=FormBorderStyle.None;
            Margin=new Padding(2);
            Name="RadMainForm";
            Text="RadMainForm";
            Load+=RadMainForm_Load;
            ((System.ComponentModel.ISupportInitialize)radButton1).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButton2).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButton3).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButton4).EndInit();
            ((System.ComponentModel.ISupportInitialize)minimize_radButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)maximize_radButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)close_radButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel0).EndInit();
            ((System.ComponentModel.ISupportInitialize)right_radPanel).EndInit();
            right_radPanel.ResumeLayout(false);
            right_radPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)text_radPanel).EndInit();
            text_radPanel.ResumeLayout(false);
            text_radPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)radTextBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ShrinkRadButton2).EndInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel2).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton12).EndInit();
            ((System.ComponentModel.ISupportInitialize)status_radLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)status_radSeparator).EndInit();
            ((System.ComponentModel.ISupportInitialize)statustext_radLabel).EndInit();
            statustext_radLabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radSeparator5).EndInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator6).EndInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator7).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton13).EndInit();
            ((System.ComponentModel.ISupportInitialize)voice_radSeparator).EndInit();
            ((System.ComponentModel.ISupportInitialize)voice_radLabel).EndInit();
            voice_radLabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radSeparator3).EndInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator2).EndInit();
            ((System.ComponentModel.ISupportInitialize)radSeparator1).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton11).EndInit();
            ((System.ComponentModel.ISupportInitialize)module_radSeparator).EndInit();
            ((System.ComponentModel.ISupportInitialize)modle_radLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)left_radPanel).EndInit();
            left_radPanel.ResumeLayout(false);
            left_radPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ShrinkRadButton1).EndInit();
            ((System.ComponentModel.ISupportInitialize)MoveRadPanel1).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton10).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton9).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton8).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton7).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton6).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton5).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton4).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton3).EndInit();
            ((System.ComponentModel.ISupportInitialize)CapsuleRadButton2).EndInit();
            ((System.ComponentModel.ISupportInitialize)create_enter).EndInit();
            ((System.ComponentModel.ISupportInitialize)function_radSeparator).EndInit();
            ((System.ComponentModel.ISupportInitialize)function_radLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)DragPanel).EndInit();
            DragPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scroll_radPanel).EndInit();
            scroll_radPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radRightScrollBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButtonScrollBar).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            ResumeLayout(false);
        }

        #endregion
        internal Telerik.WinControls.UI.RadButton radButton1;
        internal Telerik.WinControls.UI.RadButton radButton2;
        internal Telerik.WinControls.UI.RadButton radButton3;
        internal Telerik.WinControls.UI.RadButton radButton4;
        internal Telerik.WinControls.UI.RadButton minimize_radButton;
        internal Telerik.WinControls.UI.RadButton maximize_radButton;
        internal Telerik.WinControls.UI.RadButton close_radButton;
        internal Telerik.WinControls.UI.RadPanel MoveRadPanel0;
        internal Telerik.WinControls.UI.RadPanel right_radPanel;
        internal Telerik.WinControls.UI.RadPanel MoveRadPanel2;
        internal Telerik.WinControls.UI.RadTextBox radTextBox;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton12;
        internal Telerik.WinControls.UI.RadLabel status_radLabel;
        internal Telerik.WinControls.UI.RadSeparator status_radSeparator;
        internal Telerik.WinControls.UI.RadLabel statustext_radLabel;
        internal Telerik.WinControls.UI.RadSeparator radSeparator5;
        internal Telerik.WinControls.UI.RadSeparator radSeparator6;
        internal Telerik.WinControls.UI.RadSeparator radSeparator7;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton13;
        internal Telerik.WinControls.UI.RadSeparator voice_radSeparator;
        internal Telerik.WinControls.UI.RadLabel voice_radLabel;
        internal Telerik.WinControls.UI.RadSeparator radSeparator3;
        internal Telerik.WinControls.UI.RadSeparator radSeparator2;
        internal Telerik.WinControls.UI.RadSeparator radSeparator1;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton11;
        internal Telerik.WinControls.UI.RadSeparator module_radSeparator;
        internal Telerik.WinControls.UI.RadLabel modle_radLabel;
        internal Telerik.WinControls.UI.RadPanel left_radPanel;
        internal Telerik.WinControls.UI.RadPanel MoveRadPanel1;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton10;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton9;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton8;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton7;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton6;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton5;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton4;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton3;
        internal Telerik.WinControls.UI.RadButton CapsuleRadButton2;
        internal Telerik.WinControls.UI.RadButton create_enter;
        internal Telerik.WinControls.UI.RadSeparator function_radSeparator;
        internal Telerik.WinControls.UI.RadLabel function_radLabel;
        internal Telerik.WinControls.UI.RadPanel DragPanel;
        internal Telerik.WinControls.UI.RadPanel scroll_radPanel;
        internal Telerik.WinControls.UI.RadVScrollBar radRightScrollBar;
        internal Telerik.WinControls.UI.RadHScrollBar radButtonScrollBar;
        internal Telerik.WinControls.UI.RadButton ShrinkRadButton1;
        internal Telerik.WinControls.UI.RadButton ShrinkRadButton2;
        internal Telerik.WinControls.UI.RadPanel text_radPanel;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 删除ToolStripMenuItem;
    }
}