using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Svg;
using System.Drawing.Drawing2D;
using System.Reflection.Metadata;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using SpeechToCodeTest;
using System.Diagnostics;
using WinFormsApp1;

namespace WinFormsApp1
{



    public partial class RadMainForm:RadForm
    {
        private readonly SpeechToCodeConverter converter;
        private bool isRecording = false;
        public struct block
        {
            public int id;
            public string type;
            public string[] expr;
            public int elen;
            public int[] c_id;
            public int clen;
            public int nxt;
            public block()
            {
                id=elen=clen=nxt=0;
                type="";
                expr=new string[2];
                c_id=new int[2];
                expr[0]=expr[1]="";
                c_id[0]=c_id[1]=0;
            }
        };
        private block[] iptblock;
        private Dictionary<int,object> st;
        //private Panel DragPanel;
        private ContextMenuStrip cms;

        public static void link(Control ctr1,Control ctr2)
        {
            TitledPanel p = ctr1.Parent as TitledPanel;
            p.Add(ctr2,p.RowCount-1);
        }

        ////新增的：录音按钮逻辑
        //private async void myButton1_Click(object sender, EventArgs e)
        //{
        //    if (this.enter != null)
        //    {
        //        this.enter.Parent.Controls.Remove(this.enter);
        //        this.enter = null;
        //    }
        //    File.WriteAllText("in1.txt", string.Empty);
        //    isRecording = !isRecording;
        //    btnRecord.Text = isRecording ? "停止录音" : "开始录音";

        //    if (isRecording)
        //    {
        //        await converter.RecordAudioAsync(true, "recorded_audio.wav", UpdateStatus);
        //    }
        //    else
        //    {
        //        await converter.RecordAudioAsync(false, "recorded_audio.wav", UpdateStatus);
        //        await converter.ConvertAudioToInstructionsAsync("recorded_audio.wav", "in1.txt", UpdateStatus);
        //    }
        //    if (!isRecording)
        //    {
        //        Thread.Sleep(2000);
        //        systemcall("logic_to_block.exe");
        //        string[] lines = File.ReadAllLines(@"logic_to_block.out");
        //        if (lines.Length <= 3)
        //        {
        //            MessageBox.Show("空内容或未联网");
        //            return;
        //        }
        //        int len = lines.Length;
        //        int ip = 0;
        //        int n = int.Parse(lines[ip++]);
        //        iptblock = new block[1000];
        //        st = new Dictionary<int, object>();
        //        for (int i = 1; i <= n; i++)
        //        {
        //            iptblock[i].id = int.Parse(lines[ip++]);
        //            iptblock[i].type = lines[ip++];
        //            iptblock[i].elen = int.Parse(lines[ip++]);
        //            iptblock[i].expr = new string[2];
        //            for (int j = 0; j < iptblock[i].elen; j++) iptblock[i].expr[j] = lines[ip++];
        //            iptblock[i].clen = int.Parse(lines[ip++]);
        //            iptblock[i].c_id = new int[2];
        //            for (int j = 0; j < iptblock[i].clen; j++) iptblock[i].c_id[j] = int.Parse(lines[ip++]);
        //            iptblock[i].nxt = int.Parse(lines[ip++]);
        //            if (iptblock[i].clen == 0)
        //            {
        //                SegmentedInputBox sib;
        //                switch (iptblock[i].type)
        //                {
        //                    case "goline":
        //                        sib = Create_Sib("向前直走_cm");
        //                        sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        st.Add(i, sib);
        //                        break;
        //                    case "turn":
        //                        sib = Create_Sib("原地旋转_°");
        //                        sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        st.Add(i, sib);
        //                        break;
        //                    case "circle":
        //                        sib = Create_Sib("以左侧_cm为圆心旋转_°");
        //                        sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        sib.Boxes[1].Text = iptblock[i].expr[1];
        //                        st.Add(i, sib);
        //                        break;
        //                    case "break":
        //                        st.Add(i, Create_Sib("中断"));
        //                        break;
        //                    case "mov":
        //                        sib = Create_Sib("令_为_");
        //                        sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        sib.Boxes[1].Text = iptblock[i].expr[1];
        //                        st.Add(i, sib);
        //                        break;
        //                    case "undefcode":
        //                        st.Add(i, Create_Sib("需要修正的积木块"));
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                TitledPanel tp;
        //                switch (iptblock[i].type)
        //                {
        //                    case "if":
        //                        tp = Create_Panel("如果_");
        //                        tp.sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        st.Add(i, tp);
        //                        break;
        //                    case "while":
        //                        tp = Create_Panel("若_成立则重复执行");
        //                        tp.sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        st.Add(i, tp);
        //                        break;
        //                    case "for":
        //                        tp = Create_Panel("重复执行_次");
        //                        tp.sib.Boxes[0].Text = iptblock[i].expr[0];
        //                        st.Add(i, tp);
        //                        break;
        //                }
        //            }
        //        }

        //        st.Add(0, Create_Panel("程序入口")); iptblock[0].c_id = new int[2]; iptblock[0].c_id[0] = iptblock[0].clen = 1;
        //        for (int i = 0; i <= n; i++)
        //        {
        //            if (st[i] is TitledPanel && iptblock[i].clen != 0)
        //            {
        //                if (iptblock[i].c_id[0] != 0) (st[i] as TitledPanel).Add(st[iptblock[i].c_id[0]] as Control, 1);
        //                if (iptblock[i].clen == 2)
        //                {
        //                    TitledPanel eltp = Create_Panel("否则");
        //                    link(st[i] as Control, eltp as Control);
        //                    if (iptblock[i].c_id[1] != 0) eltp.Add(st[iptblock[i].c_id[1]] as Control, 1);
        //                    if (iptblock[i].nxt != 0) link(eltp as Control, st[iptblock[i].nxt] as Control);
        //                }
        //            }
        //            if (iptblock[i].clen != 2 && i != 0)
        //            {
        //                if (iptblock[i].nxt != 0) link(st[i] as Control, st[iptblock[i].nxt] as Control);
        //            }
        //        }
        //    }
        //}

        //新增：更新状态
        private void UpdateStatus(string message)
        {
            if(status_radLabel.InvokeRequired)
            {
                status_radLabel.Invoke(new Action(() => status_radLabel.Text=message));
            }
            else
            {
                status_radLabel.Text=message;
            }
        }
        ////新增
        //private void Form1_Resize(object sender, EventArgs e)
        //{
        //    myLabel1.Location = new System.Drawing.Point(this.ClientSize.Width - myLabel1.Width - 20, this.ClientSize.Height - myLabel1.Height - 20);
        //}
        //新增
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            converter?.Dispose();
            base.OnFormClosing(e);
        }



        // 用于记录恢复前的窗体大小和位置
        private Rectangle normalBounds;
        //private bool isMaximized = false;


        // 用于拖动窗口
        private bool dragging = false;
        private Point dragStartPoint;


        public TitledPanel enter = null;

        public List<TitledPanel> tps = new List<TitledPanel>();
        //设定可拖拽区
        public Rectangle Dragable;
        public void GetDragable(object sender,EventArgs e)
        {
            Dragable=DragPanel.Get_Pos();

        }

        private void 删除ToolStripMenuItem_MouseDown(object sender,MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                ContextMenuStrip cms = item.Owner as ContextMenuStrip;
                Control clickedControl = cms.SourceControl;//获取右键的对象啊
                if((clickedControl.Parent is TitledPanel))   //删除控件
                {
                    (clickedControl.Parent as TitledPanel).Remove(clickedControl);
                }
                else
                {
                    if(clickedControl==this.enter) this.enter=null;
                    clickedControl.Parent.Controls.Remove(clickedControl);
                }
            }
        }

        private void 复制ToolStripMenuItem_MouseDown(object sender,MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                ContextMenuStrip cms = item.Owner as ContextMenuStrip;
                Control clickedControl = cms.SourceControl;
                if(clickedControl is SegmentedInputBox)
                {
                    Create_Sib(clickedControl.Text);
                }
                else if(clickedControl is TitledPanel)
                {
                    Create_Panel((clickedControl as TitledPanel).text);
                }

            }
        }

        private void menuStrip1_ItemClicked(object sender,ToolStripItemClickedEventArgs e)
        {

        }
        int sensormode = 1;

        void outputblock(ref block[] bt,int bl,ref HashSet<string> st,string filename = "./block_to_logic.in")
        {
            File.WriteAllText(filename,"");
            string output;
            if(filename=="./block_to_logic.in") 
                switch(sensormode)
                {
                case 0:
                File.AppendAllText("./block_to_logic.in","0\n");
                break;
                case 1:
                File.AppendAllText("./block_to_logic.in","1\n");
                break;
                case 2:
                File.AppendAllText("./block_to_logic.in","2\n");
                break;
                }

            File.AppendAllText(filename,bl.ToString()+"\n");
            if(filename=="./block_to_logic.in")
                for(int i = 1;i<=bl;i++)
                {
                    output=bt[i].id.ToString()+' ';
                    output+=bt[i].type+' ';
                    output+=bt[i].elen.ToString()+' ';
                    for(int j = 0;j<bt[i].elen;j++) output+=bt[i].expr[j]+' ';
                    output+=bt[i].clen.ToString()+' ';
                    for(int j = 0;j<bt[i].clen;j++) output+=bt[i].c_id[j].ToString()+' ';
                    output+=bt[i].nxt;
                    File.AppendAllText(filename,output+"\n");
                }
            else
                for(int i = 1;i<=bl;i++)
                {
                    output=bt[i].id.ToString()+'\n';
                    output+=bt[i].type+'\n';
                    output+=bt[i].elen.ToString()+'\n';
                    for(int j = 0;j<bt[i].elen;j++) output+=bt[i].expr[j]+'\n';
                    output+=bt[i].clen.ToString()+'\n';
                    for(int j = 0;j<bt[i].clen;j++) output+=bt[i].c_id[j].ToString()+'\n';
                    output+=bt[i].nxt;
                    File.AppendAllText(filename,output+"\n");
                }
            if(filename=="./block_to_logic.in")
            {
                File.AppendAllText("./block_to_logic.in",st.Count.ToString()+"\n");
                foreach(string s in st) File.AppendAllText("./block_to_logic.in",s+" ");
            }
        }
        int systemcall(string command)
        {
            // 创建一个新的进程
            Process process = new Process();
            process.StartInfo.FileName="cmd.exe";
            process.StartInfo.UseShellExecute=false;
            process.StartInfo.RedirectStandardInput=true;
            process.StartInfo.RedirectStandardOutput=true;
            process.StartInfo.RedirectStandardError=true;
            process.StartInfo.CreateNoWindow=true;

            // 启动进程
            process.Start();

            // 向 CMD 窗口发送输入信息
            process.StandardInput.WriteLine(command+"&exit");
            process.StandardInput.AutoFlush=true;

            // 获取 CMD 窗口的输出信息


            // 等待进程退出
            process.WaitForExit();
            int ans = process.ExitCode;
            process.Close();
            return 0;
        }
        //private void 导出文件ToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        int x = 0;
        //        int bl = 0;
        //        if (this.enter == null) return;
        //        block[] bt = new block[10000];
        //        HashSet<string> st = new HashSet<string>(); ;
        //        bt[0].expr = new string[2];
        //        bt[0].c_id = new int[2];
        //        build(this.enter, ref bt, ref bl, 0, ref st);
        //        outputblock(ref bt, bl, ref st);
        //        systemcall("block_to_logic.exe");
        //        string content = File.ReadAllText(@"./block_to_logic.out");
        //        if (content == "" || content[0] == ' ')
        //        {
        //            if (content == "") MessageBox.Show("空内容", "错误");
        //            else MessageBox.Show(content, "错误");
        //        }
        //        else
        //        {
        //            systemcall("logic_to_code.exe");
        //        }
        //    }
        //}
        string trans(string s)
        {
            string ans = "";
            if(s==""||s==null) return "???";
            int len = s.Length;
            for(int i = 0;i<len;i++) if(s[i]!=' '&&s[i]!='\n'&&s[i]!='r') ans+=s[i];
            return ans;
        }
        private void build(TitledPanel con,ref block[] bt,ref int id,int f,ref HashSet<string> st)
        {
            int lastid = 0;
            for(int i = 0;i<con.RowCount;i++)
            {
                Control ctrl = con.GetControlFromPosition(0,i);

                if(ctrl is SegmentedInputBox)
                {
                    SegmentedInputBox sib = ctrl as SegmentedInputBox;

                    if(Setting.TextToInstr.ContainsKey(sib.text))
                    {
                        switch(Setting.TextToInstr[sib.text])
                        {
                        case "error":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="error";
                        bt[id].elen=0;
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        case "goline":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="goline";
                        bt[id].elen=1;
                        bt[id].expr[0]=trans(sib.Boxes[0].Text);
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        case "turn":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="turn";
                        bt[id].elen=1;
                        string ans = trans(sib.Boxes[0].Text);
                        bt[id].expr[0]=ans;
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        case "circle":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="circle";
                        bt[id].elen=2;
                        bt[id].expr[0]=trans(sib.Boxes[0].Text);
                        bt[id].expr[1]=trans(sib.Boxes[1].Text);
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        case "break":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="break";
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        case "let":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        bt[id].type="mov";
                        bt[id].elen=2;
                        bt[id].expr[0]=trans(sib.Boxes[0].Text);
                        bt[id].expr[1]=trans(sib.Boxes[1].Text);
                        st.Add(bt[id].expr[0]);
                        if(lastid!=0) bt[lastid].nxt=id;
                        else bt[f].c_id[bt[f].clen++]=id;
                        lastid=id;
                        break;
                        }
                    }
                }
                else if(ctrl is TitledPanel)
                {
                    TitledPanel tp = ctrl as TitledPanel;
                    if(Setting.TextToInstr.ContainsKey(tp.text))
                    {
                        switch(Setting.TextToInstr[tp.text])
                        {
                        case "if":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        int t = id;
                        bt[t].type="if";
                        bt[t].elen=1;
                        bt[t].expr[0]=trans(tp.sib.Boxes[0].Text);
                        if(lastid!=0) bt[lastid].nxt=t;
                        else bt[f].c_id[bt[f].clen++]=t;
                        build(ctrl as TitledPanel,ref bt,ref id,t,ref st);
                        bt[t].clen=1;
                        lastid=t;
                        break;
                        case "while":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        t=id;
                        bt[t].type="while";
                        bt[t].elen=1;
                        bt[t].expr[0]=trans(tp.sib.Boxes[0].Text);
                        if(lastid!=0) bt[lastid].nxt=t;
                        else bt[f].c_id[bt[f].clen++]=t;
                        build(ctrl as TitledPanel,ref bt,ref id,t,ref st);
                        bt[t].clen=1;
                        lastid=t;
                        break;

                        case "else":
                        if(lastid!=0&&bt[lastid].type=="if")
                        {
                            build(ctrl as TitledPanel,ref bt,ref id,lastid,ref st);
                            bt[lastid].clen=2;
                        }
                        else { while(true) ; }
                        break;
                        case "for":
                        bt[++id].id=id;
                        bt[id].expr=new string[2];
                        bt[id].c_id=new int[2];
                        bt[id].expr[0]=bt[id].expr[1]="";
                        bt[id].c_id[0]=bt[id].c_id[1]=0;
                        t=id;
                        bt[t].type="for";
                        bt[t].elen=1;
                        bt[t].expr[0]=trans(tp.sib.Boxes[0].Text);
                        if(lastid!=0) bt[lastid].nxt=t;
                        else bt[f].c_id[bt[f].clen++]=t;
                        build(ctrl as TitledPanel,ref bt,ref id,t,ref st);
                        bt[t].clen=1;
                        lastid=t;
                        break;
                        }
                    }

                }
            }
        }

        private void Read_String(string[] lines,TitledPanel main,ref int i)
        {

            while(i<lines.Length)
            {
                string line = lines[i];
                string[] word = line.Split(' ');
                if(word[0]=="}")
                {
                    i++;
                    return;
                }
                else if(!Setting.InstrToTextMap.ContainsKey(word[0]))
                {
                    MessageBox.Show("文件错误"+word[0]);
                    i=line.Length;
                    return;
                }
                else if(Setting.InstrToTextMap[word[0]].IsTable)
                {
                    Setting.TypeData td = Setting.InstrToTextMap[word[0]];
                    TitledPanel tp = Create_Panel(td.Name);
                    main.Add(tp,main.ColumnCount);
                    i++;
                    Read_String(lines,tp,ref i);
                }
                else if(!Setting.InstrToTextMap[word[0]].IsTable)
                {
                    Setting.TypeData td = Setting.InstrToTextMap[word[0]];

                    SegmentedInputBox sib = Create_Sib(td.Name);
                    for(int j = 0;j<td.AtrNum;j++)
                    {
                        if(j+1>=word.Length) sib.Boxes[j].Text="";
                        else
                            sib.Boxes[j].Text=word[j+1];
                    }
                    main.Add(sib,main.ColumnCount);
                    i++;
                }
            }
        }

        private void Close_Win(object sender,EventArgs e)
        {
            this.Close();
        }

        private void 文件ToolStripMenuItem_Click(object sender,EventArgs e)
        {

        }


        private void btnMinimize_Click(object sender,EventArgs e)
        {
            this.WindowState=FormWindowState.Minimized;
        }

        // 实现拖动逻辑
        //private void TitlePanel_MouseDown(object sender,MouseEventArgs e)
        //{
        //    if(e.Button==MouseButtons.Left)
        //    {
        //        dragging=true;
        //        // 使用屏幕坐标记录拖动起点
        //        dragStartPoint=TitlePanel.PointToScreen(e.Location);
        //    }
        //}


        //private void TitlePanel_MouseMove(object sender,MouseEventArgs e)
        //{
        //    if(dragging)
        //    {
        //        Point currentScreenPos = TitlePanel.PointToScreen(e.Location);
        //        int offsetX = currentScreenPos.X-dragStartPoint.X;
        //        int offsetY = currentScreenPos.Y-dragStartPoint.Y;
        //        this.Location=new Point(this.Location.X+offsetX,this.Location.Y+offsetY);
        //        dragStartPoint=currentScreenPos; // 更新起点
        //    }
        //}

        //private void CustomForm_Resize(object sender, EventArgs e)
        //{
        //    int btnWidth = btnClose.Width;
        //    btnClose.Location = new Point(TitlePanel.Width - btnWidth, 0);
        //    btnMinimize.Location = new Point(TitlePanel.Width - 2 * btnWidth, 0);
        //    btnMaximize.Location = new Point(TitlePanel.Width - 3 * btnWidth, 0);
        //}
        //private void TitlePanel_MouseUp(object sender,MouseEventArgs e)
        //{
        //    dragging=false;
        //}

        //private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //}

        //private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    TitledPanel titledPanel = Create_Panel("导入模块");
        //    string[] lines = File.ReadAllLines("in2.txt");
        //    int i = 0;
        //    Read_String(lines, titledPanel, ref i);
        //}

        private void myLabel1_Click(object sender,EventArgs e)
        {

        }

        //private void sub_button_Click(object sender, EventArgs e)
        //{
        //    switch (sub_button.Text)
        //    {
        //        //case "摄像头禁用":
        //        //sub_button.Text="红绿灯模式";
        //        //break;
        //        //case "红绿灯模式":
        //        //sub_button.Text="数字识别模式";
        //        //break;
        //        //case "数字识别模式":
        //        //sub_button.Text="摄像头禁用";
        //        //break;

        //        //case "摄像头禁用":
        //        //sub_button.Text="红绿灯模式";
        //        //break;
        //        case "红绿灯模式":
        //            sub_button.Text = "数字识别模式";
        //            break;
        //        case "数字识别模式":
        //            sub_button.Text = "红绿灯模式";
        //            break;
        //    }

        //}


        //------------------------------------------------------------------------------------------------//

        //窗体变换工具
        private SizeAndLocationSwitcher sizeSwitcher;

        public RadMainForm()
        {
            InitializeComponent();

            converter=new SpeechToCodeConverter("il.txt");

            this.FormBorderStyle=FormBorderStyle.None; // 设置窗体无边框

            // 设置窗体圆角（通过创建圆角区域，不影响控件边框）
            this.Region=Region.FromHrgn(CreateRoundRectRgn(0,0,this.Width,this.Height,20,20));

            // 设置窗体双缓冲
            this.DoubleBuffered=true; // 启用双缓冲，减少闪烁            // 配置RadPanel样式
            ConfigureScrollRadPanel(scroll_radPanel);
            ConfigureBottomRadPanel(DragPanel);
            ConfigureLeftRadPanel(left_radPanel);
            ConfigureRightRadPanel(right_radPanel);
            scroll_radPanel.RootElement.EnableElementShadow=false;
            scroll_radPanel.RootElement.ClipDrawing=true; // 超出部分剪裁
            EnableDoubleBufferingRecursive(left_radPanel);
            EnableDoubleBufferingRecursive(right_radPanel);

            // 滑动条
            ConfigureScrollBar(radRightScrollBar,true);
            radButtonScrollBar.BringToFront();
            ConfigureScrollBar(radButtonScrollBar,false);
            radRightScrollBar.BringToFront();
            SetupScrolling();// 控制滑动 // 配置RadPanel样式
            ConfigureScrollRadPanel(scroll_radPanel);
            ConfigureBottomRadPanel(DragPanel);
            ConfigureLeftRadPanel(left_radPanel);
            ConfigureRightRadPanel(right_radPanel);
            scroll_radPanel.RootElement.EnableElementShadow=false;
            scroll_radPanel.RootElement.ClipDrawing=true; // 超出部分剪裁
            EnableDoubleBufferingRecursive(left_radPanel);
            EnableDoubleBufferingRecursive(right_radPanel);

            // 滑动条
            ConfigureScrollBar(radRightScrollBar,true);
            radButtonScrollBar.BringToFront();
            ConfigureScrollBar(radButtonScrollBar,false);
            radRightScrollBar.BringToFront();
            SetupScrolling();// 控制滑动           

            // 主窗口支持拖拽
            new DraggableController_MainForm(this,MoveRadPanel0);

            // 提前定义初始化放大工具
            sizeSwitcher=new SizeAndLocationSwitcher(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // 调用基类的OnLoad方法

            //-----初始绘制-----
            //RadPanel
            ConfigureScrollRadPanel(scroll_radPanel);
            ConfigureBottomRadPanel(DragPanel);
            ConfigureLeftRadPanel(left_radPanel);
            ConfigureRightRadPanel(right_radPanel);
            scroll_radPanel.RootElement.EnableElementShadow=false;
            scroll_radPanel.RootElement.ClipDrawing=true; // 超出部分剪裁
            EnableDoubleBufferingRecursive(left_radPanel);
            EnableDoubleBufferingRecursive(right_radPanel);
            //滑动条
            ConfigureScrollBar(radRightScrollBar,true);
            radButtonScrollBar.BringToFront();
            ConfigureScrollBar(radButtonScrollBar,false);
            radRightScrollBar.BringToFront();
            SetupScrolling();// 控制滑动
            //右上方三按钮 最小化 最大化 关闭
            //ConfigureMinimizeRadButton(minimize_radButton,"..\\..\\..\\图标\\最小化.svg");
            //ConfigureMaximizeRadButton(maximize_radButton,"..\\..\\..\\图标\\最大化.svg");
            //ConfigureCloseRadButton(close_radButton,"..\\..\\..\\图标\\关闭.svg");

            ConfigureMinimizeRadButton(minimize_radButton,"图标\\最小化.svg");
            ConfigureMaximizeRadButton(maximize_radButton,"图标\\最大化.svg");
            ConfigureCloseRadButton(close_radButton,"图标\\关闭.svg");

            //左上方四按钮
            ConfigureRadButtonStyle(radButton1);
            ConfigureRadButtonStyle(radButton2);
            ConfigureRadButtonStyle(radButton3);
            ConfigureRadButtonStyle(radButton4);
            //分隔线
            ConfigureSeparatorStyle(function_radSeparator);
            ConfigureSeparatorStyle(module_radSeparator);
            ConfigureSeparatorStyle(voice_radSeparator);
            ConfigureSeparatorStyle(status_radSeparator);
            //左侧胶囊
            //ConfigureCapsuleRadButton(create_enter,"..\\..\\..\\图标\\入口.svg","程序入口",50);
            //ConfigureCapsuleRadButton(CapsuleRadButton2,"..\\..\\..\\图标\\直行.svg","向前走____cm",20);
            //ConfigureCapsuleRadButton(CapsuleRadButton3,"..\\..\\..\\图标\\原地左转.svg","原地左转____°",20);
            //ConfigureCapsuleRadButton(CapsuleRadButton4,"..\\..\\..\\图标\\法线左转.svg","以左侧__cm为圆心旋转___°",20);
            //ConfigureCapsuleRadButton(CapsuleRadButton6,"..\\..\\..\\图标\\if.svg","如果____",50);
            //ConfigureCapsuleRadButton(CapsuleRadButton5,"..\\..\\..\\图标\\赋值.svg","令____为____",20);
            //ConfigureCapsuleRadButton(CapsuleRadButton8,"..\\..\\..\\图标\\循环次数.svg","重复执行__次",20);
            //ConfigureCapsuleRadButton(CapsuleRadButton7,"..\\..\\..\\图标\\else.svg","否则",70);
            //ConfigureCapsuleRadButton(CapsuleRadButton9,"..\\..\\..\\图标\\while循环.svg","若____成立   则重复执行",40);
            //ConfigureCapsuleRadButton(CapsuleRadButton10,"..\\..\\..\\图标\\跳出循环.svg","中断",70);

            ConfigureCapsuleRadButton(create_enter,"图标\\入口.svg","程序入口",50);
            ConfigureCapsuleRadButton(CapsuleRadButton2,"图标\\直行.svg","向前走____cm",20);
            ConfigureCapsuleRadButton(CapsuleRadButton3,"图标\\原地左转.svg","原地左转____°",20);
            ConfigureCapsuleRadButton(CapsuleRadButton4,"图标\\法线左转.svg","以左侧__cm为圆心旋转___°",20);
            ConfigureCapsuleRadButton(CapsuleRadButton6,"图标\\if.svg","如果____",50);
            ConfigureCapsuleRadButton(CapsuleRadButton5,"图标\\赋值.svg","令____为____",20);
            ConfigureCapsuleRadButton(CapsuleRadButton8,"图标\\循环次数.svg","重复执行__次",20);
            ConfigureCapsuleRadButton(CapsuleRadButton7,"图标\\else.svg","否则",70);
            ConfigureCapsuleRadButton(CapsuleRadButton9,"图标\\while循环.svg","若____成立   则重复执行",40);
            ConfigureCapsuleRadButton(CapsuleRadButton10,"图标\\跳出循环.svg","中断",70);
            //收缩环
            ConfigureShrinkRadButton(ShrinkRadButton1);
            ConfigureShrinkRadButton(ShrinkRadButton2);
            //拖拽条.
            ConfigureMoveRadPanel_top(MoveRadPanel0);
            ConfigureMoveRadPanel(MoveRadPanel1);
            ConfigureMoveRadPanel(MoveRadPanel2);
            //右侧胶囊    
            //ConfigureCapsuleRadButton(CapsuleRadButton11, "..\\..\\..\\图标\\红绿灯.svg", "红绿灯模式", 40);
            //ConfigureCapsuleRadButton(CapsuleRadButton12, "..\\..\\..\\图标\\数字识别.svg", "数字识别模式", 30);

            //ConfigureCapsuleRadButton(CapsuleRadButton11,"..\\..\\..\\图标\\红绿灯.svg","红绿灯模式",40,() => sensormode==2);
            //ConfigureCapsuleRadButton(CapsuleRadButton12,"..\\..\\..\\图标\\数字识别.svg","数字识别模式",30,() => sensormode==1);
            //ConfigureCapsuleRadButton(CapsuleRadButton13,"..\\..\\..\\图标\\麦克风.svg","开始录音",50);

            ConfigureCapsuleRadButton(CapsuleRadButton11,"图标\\红绿灯.svg","红绿灯模式",40,() => sensormode==2);
            ConfigureCapsuleRadButton(CapsuleRadButton12,"图标\\数字识别.svg","数字识别模式",30,() => sensormode==1);
            ConfigureCapsuleRadButton(CapsuleRadButton13,"图标\\麦克风.svg","开始录音",50);
            //文本容器
            ConfigureTextRadPanel(text_radPanel);
            //文本框
            ConfigureRadTextBox(radTextBox);

            //收缩环的缩小逻辑我们放到放大button塑形里面，因为收缩涉及到了尺寸的变化，
            //而放大button里面的尺寸变化是我们需要的，所以放在一起


            //-----放大按钮相关联逻辑注册-----

            // 主窗体放大后尺寸
            sizeSwitcher.SetExpandedFormSize(new Size(2850,1700));
            //右上三按钮
            sizeSwitcher.RegisterControl(minimize_radButton,new Size(100,100),new Point(2549,0));
            sizeSwitcher.RegisterControl(maximize_radButton,new Size(100,100),new Point(2649,0));
            sizeSwitcher.RegisterControl(close_radButton,new Size(100,100),new Point(2749,0));
            //左上四按钮
            sizeSwitcher.RegisterControl(radButton1,new Size(240,105),new Point(30,23));
            sizeSwitcher.RegisterControl(radButton2,new Size(240,105),new Point(285,23));
            sizeSwitcher.RegisterControl(radButton3,new Size(240,105),new Point(540,23));
            sizeSwitcher.RegisterControl(radButton4,new Size(240,105),new Point(795,23));
            //顶部拖拽条
            sizeSwitcher.RegisterControl(MoveRadPanel0,new Size(700,25),new Point(1140,25));
            //裁剪panel
            sizeSwitcher.RegisterControl(scroll_radPanel,new Size(2800,1530),new Point(25,150));
            //滑动条
            sizeSwitcher.RegisterControl(radRightScrollBar,new Size(12,1400),new Point(2780,70));
            sizeSwitcher.RegisterControl(radButtonScrollBar,new Size(2700,12),new Point(28,1510));
            //两个panel的放大逻辑我们放到了对 maximize_radButton塑性的逻辑里面
            //底层容器panel不用变，因为尺寸一开始就设定了，只要裁剪panel能正常裁剪即可



            //-----双缓冲-----           
            EnableDoubleBuffering(ShrinkRadButton1);
            EnableDoubleBuffering(ShrinkRadButton2);
            EnableDoubleBuffering(MoveRadPanel1);
            EnableDoubleBuffering(MoveRadPanel2);
            EnableDoubleBuffering(text_radPanel);
            EnableDoubleBuffering(radTextBox);
            EnableDoubleBuffering(create_enter);
            EnableDoubleBuffering(CapsuleRadButton2);
            EnableDoubleBuffering(CapsuleRadButton3);
            EnableDoubleBuffering(CapsuleRadButton4);
            EnableDoubleBuffering(CapsuleRadButton5);
            EnableDoubleBuffering(CapsuleRadButton6);
            EnableDoubleBuffering(CapsuleRadButton7);
            EnableDoubleBuffering(CapsuleRadButton8);
            EnableDoubleBuffering(CapsuleRadButton9);
            EnableDoubleBuffering(CapsuleRadButton10);
            EnableDoubleBuffering(CapsuleRadButton11);
            EnableDoubleBuffering(CapsuleRadButton12);
            EnableDoubleBuffering(CapsuleRadButton13);
            // 确保拖拽目标的父容器也启用双缓冲
            EnableDoubleBuffering(left_radPanel.Parent);
            EnableDoubleBuffering(right_radPanel.Parent);

            // 允许拖动            
            new DraggableController_Panel_perfect(left_radPanel,MoveRadPanel1);
            new DraggableController_Panel_perfect(right_radPanel,MoveRadPanel2);
        }

        // 导入GDI32函数用于创建圆角矩形区域（region圆角用到）
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect,int nTopRect,
            int nRightRect,int nBottomRect,int nWidthEllipse,int nHeightEllipse);

        // 创建圆角矩形路径的辅助方法（paint圆角用到）
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectPath(Rectangle rect,int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath(); // 创建图形路径
            int diameter = radius*2; // 计算直径
            path.StartFigure(); // 开始绘制图形
            // 添加四个角的圆弧
            path.AddArc(rect.X,rect.Y,diameter,diameter,180,90);
            path.AddArc(rect.Right-diameter,rect.Y,diameter,diameter,270,90);
            path.AddArc(rect.Right-diameter,rect.Bottom-diameter,diameter,diameter,0,90);
            path.AddArc(rect.X,rect.Bottom-diameter,diameter,diameter,90,90);
            path.CloseFigure(); // 闭合图形
            return path;
        }

        // 一、配置最底层裁剪panel
        private void ConfigureScrollRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent;
            panel.RootElement.BackColor=Color.Transparent;
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed;

            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1);
                int radius = 20;

                using(SolidBrush brush = new SolidBrush(Color.FromArgb(178,255,255,255)))
                using(Pen borderPen = new Pen(Color.Gray,1))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path);
                    g.DrawPath(borderPen,path);
                }
            };

            panel.AutoScroll=false; // 禁止系统滚动

            panel.AutoScrollMargin=new Size(0,0);//不要加尺寸，不然就多出来系统滚动界面
            panel.AutoScrollMinSize=new Size(0,0);
            panel.Padding=new Padding(0);
        }

        // 二、配置bottom_RadPanel控件样式
        private void ConfigureBottomRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框           

            DragPanel.Parent=scroll_radPanel;//规定从属关系

            panel.Margin=new Padding(0);
            panel.Padding=new Padding(0);
            panel.Dock=DockStyle.None; // 禁止自动布局
            panel.Location=new Point(0,0); //固定在最左上角（取消下移）

            //！！！取消绘制的原因在于重复导致了背景色不一，由于我们禁用了，就直接让滑动层绘制，这一层不绘制
        }

        // 三、收缩环塑性，它的收缩逻辑放在了放大按钮的塑性里面
        private void ConfigureShrinkRadButton(RadButton button)
        {
            // 设置按钮背景透明（多层透明设置确保完全透明）
            button.BackColor=Color.Transparent; // 设置控件背景透明 
            button.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            button.ButtonElement.BackColor=Color.Transparent; // 设置按钮元素背景透明

            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed; // 隐藏填充
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed; // 隐藏边框
            button.Image=null; // 清空默认图标
            button.Text=""; // 清空默认文本


            // 定义按钮状态
            bool isHovered = false;
            bool isPressed = false;

            // 定义外观常量
            int outerRadius = Math.Min(button.Width,button.Height)/2-9; // 外圆半径
            int ringThickness = 6; // 圆环厚度
            int innerRadius = outerRadius-ringThickness; // 内圆半径
            Color normalColor = Color.FromArgb(255,255,255); // 正常状态圆环颜色（白色）
            Color hoverColor = Color.FromArgb(255,255,255); // 悬停状态圆环颜色（白色）
            Color pressedColor = Color.FromArgb(255,248,38); // 按下状态圆环颜色（深青色）
            Color glowColor = Color.FromArgb(128,255,251,110); // 悬停时的发光颜色
            int glowThickness = 12; // 发光效果厚度

            // 创建圆环的 Region
            Region GetRingRegion()
            {
                GraphicsPath outerPath = new GraphicsPath();
                Rectangle outerRect = new Rectangle(
                    button.Width/2-outerRadius,
                    button.Height/2-outerRadius,
                    outerRadius*2,
                    outerRadius*2);
                outerPath.AddEllipse(outerRect); // 添加外圆

                GraphicsPath innerPath = new GraphicsPath();
                Rectangle innerRect = new Rectangle(
                    button.Width/2-innerRadius,
                    button.Height/2-innerRadius,
                    innerRadius*2,
                    innerRadius*2);
                innerPath.AddEllipse(innerRect); // 添加内圆

                Region region = new Region(outerPath);
                region.Exclude(innerPath); // 排除内圆，形成圆环
                return region;
            }

            // 创建发光效果的 Region
            Region GetGlowRegion()
            {
                GraphicsPath outerPath = new GraphicsPath();
                int glowOuterRadius = outerRadius+glowThickness/2;
                Rectangle outerGlowRect = new Rectangle(
                    button.Width/2-glowOuterRadius,
                    button.Height/2-glowOuterRadius,
                    glowOuterRadius*2,
                    glowOuterRadius*2);
                outerPath.AddEllipse(outerGlowRect); // 添加外圆

                GraphicsPath innerPath = new GraphicsPath();
                int glowInnerRadius = innerRadius-glowThickness/2;
                Rectangle innerGlowRect = new Rectangle(
                    button.Width/2-glowInnerRadius,
                    button.Height/2-glowInnerRadius,
                    glowInnerRadius*2,
                    glowInnerRadius*2);
                innerPath.AddEllipse(innerGlowRect); // 添加内圆

                Region region = new Region(outerPath);
                region.Exclude(innerPath); // 排除内圆，形成发光圆环
                return region;
            }

            // 自定义绘制事件
            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=SmoothingMode.AntiAlias; // 启用抗锯齿
                //g.Clear(Color.Transparent); // 清除绘制区域为透明   这句话一定要删！！！！！不然会有黑背景

                // 绘制发光效果（仅在悬停和按下时）
                if(isHovered)
                {
                    using(Region glowRegion = GetGlowRegion())
                    using(SolidBrush glowBrush = new SolidBrush(glowColor))
                    {
                        g.FillRegion(glowBrush,glowRegion); // 填充发光环
                    }
                }

                // 绘制圆环
                using(Region ringRegion = GetRingRegion())
                using(SolidBrush ringBrush = new SolidBrush(isPressed ? pressedColor : (isHovered ? hoverColor : normalColor)))
                {
                    g.FillRegion(ringBrush,ringRegion); // 填充圆环
                }
            };

            // 鼠标事件处理
            button.MouseEnter+=(s,e) =>
            {
                isHovered=true;
                button.Invalidate(); // 触发重绘
            };

            button.MouseLeave+=(s,e) =>
            {
                isHovered=false;
                isPressed=false;
                button.Invalidate(); // 触发重绘
            };

            button.MouseDown+=(s,e) =>
            {
                if(e.Button==MouseButtons.Left)
                {
                    isPressed=true;
                    button.Invalidate(); // 触发重绘
                }
            };

            button.MouseUp+=(s,e) =>
            {
                if(e.Button==MouseButtons.Left)
                {
                    isPressed=false;
                    button.Invalidate(); // 触发重绘
                }
            };

        }


        // 四1、底部滑动条
        private void ConfigureScrollBar(RadScrollBar scrollBar,bool isVertical)
        {
            scrollBar.Parent=scroll_radPanel;

            scrollBar.ThemeName=string.Empty;
            scrollBar.ThemeClassName=string.Empty;

            scrollBar.MinThumbLength=30;
            scrollBar.Width=isVertical ? 6 : scrollBar.Width;
            scrollBar.Height=!isVertical ? 6 : scrollBar.Height;

            // 设置轨道颜色
            var trackColor = ColorTranslator.FromHtml("#F0F0F0");

            var fill = scrollBar.ScrollBarElement.FindDescendant<Telerik.WinControls.Primitives.FillPrimitive>();
            if(fill!=null)
            {
                fill.BackColor=trackColor;
                fill.GradientStyle=Telerik.WinControls.GradientStyles.Solid;
                fill.Visibility=Telerik.WinControls.ElementVisibility.Visible;
            }

            var border = scrollBar.ScrollBarElement.FindDescendant<Telerik.WinControls.Primitives.BorderPrimitive>();
            if(border!=null)
            {
                border.ForeColor=Color.Transparent;
                border.BackColor=Color.Transparent;
                border.Visibility=Telerik.WinControls.ElementVisibility.Collapsed;
            }

            // 设置滑块样式
            var thumbNormalColor = ColorTranslator.FromHtml("#6E8DB8"); // 普通状态：深蓝色 接近中央三竖线
            var thumbHoverColor = ColorTranslator.FromHtml("#7A93AC");  // 悬停/点击状态：灰蓝色

            var thumb = scrollBar.ScrollBarElement.ThumbElement;

            // 设置滑块的基础颜色
            thumb.BackColor=thumbNormalColor;
            thumb.Shape=new Telerik.WinControls.RoundRectShape(4); // 圆角矩形，保持简约

            // 通过 FillPrimitive 设置滑块填充样式
            var thumbFill = thumb.FindDescendant<Telerik.WinControls.Primitives.FillPrimitive>();
            if(thumbFill!=null)
            {
                thumbFill.BackColor=thumbNormalColor;
                thumbFill.GradientStyle=Telerik.WinControls.GradientStyles.Solid;
            }

            // 移除滑块边框
            var thumbBorder = thumb.FindDescendant<Telerik.WinControls.Primitives.BorderPrimitive>();
            if(thumbBorder!=null)
            {
                thumbBorder.Visibility=Telerik.WinControls.ElementVisibility.Collapsed;
            }

            //// 隐藏滑块中央 Grip（三条竖线）   找不到！！！
            //var grip = thumb.FindDescendant<Telerik.WinControls.UI.ScrollBarGripElement>();
            //if (grip != null)
            //{
            //    grip.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            //}           

            // 设置两侧三角形按钮（FirstButton 和 SecondButton）
            var firstButton = scrollBar.ScrollBarElement.FirstButton;
            var secondButton = scrollBar.ScrollBarElement.SecondButton;

            // 配置第一个按钮（左/上箭头）
            if(firstButton!=null)
            {
                var firstButtonFill = firstButton.FindDescendant<Telerik.WinControls.Primitives.FillPrimitive>();
                if(firstButtonFill!=null)
                {
                    firstButtonFill.BackColor=thumbNormalColor;
                    firstButtonFill.GradientStyle=Telerik.WinControls.GradientStyles.Solid;
                }

                var firstButtonBorder = firstButton.FindDescendant<Telerik.WinControls.Primitives.BorderPrimitive>();
                if(firstButtonBorder!=null)
                {
                    firstButtonBorder.Visibility=Telerik.WinControls.ElementVisibility.Collapsed;
                }

                var firstButtonArrow = firstButton.FindDescendant<Telerik.WinControls.Primitives.ArrowPrimitive>();
                if(firstButtonArrow!=null)
                {
                    firstButtonArrow.ForeColor=ColorTranslator.FromHtml("#6E8DB8"); //和左侧块相同
                }

                firstButton.MouseEnter+=(s,e) =>
                {
                    if(firstButtonFill!=null) firstButtonFill.BackColor=thumbHoverColor;
                };
                firstButton.MouseLeave+=(s,e) =>
                {
                    if(firstButtonFill!=null) firstButtonFill.BackColor=thumbNormalColor;
                };
                firstButton.MouseDown+=(s,e) =>
                {
                    if(firstButtonFill!=null) firstButtonFill.BackColor=thumbHoverColor;
                };
                firstButton.MouseUp+=(s,e) =>
                {
                    if(firstButtonFill!=null) firstButtonFill.BackColor=thumbHoverColor;
                };
            }

            // 配置第二个按钮（右/下箭头）
            if(secondButton!=null)
            {
                var secondButtonFill = secondButton.FindDescendant<Telerik.WinControls.Primitives.FillPrimitive>();
                if(secondButtonFill!=null)
                {
                    secondButtonFill.BackColor=thumbNormalColor;
                    secondButtonFill.GradientStyle=Telerik.WinControls.GradientStyles.Solid;
                }

                var secondButtonBorder = secondButton.FindDescendant<Telerik.WinControls.Primitives.BorderPrimitive>();
                if(secondButtonBorder!=null)
                {
                    secondButtonBorder.Visibility=Telerik.WinControls.ElementVisibility.Collapsed;
                }

                var secondButtonArrow = secondButton.FindDescendant<Telerik.WinControls.Primitives.ArrowPrimitive>();
                if(secondButtonArrow!=null)
                {
                    secondButtonArrow.ForeColor=ColorTranslator.FromHtml("#6E8DB8");//和右侧块相同
                }

                secondButton.MouseEnter+=(s,e) =>
                {
                    if(secondButtonFill!=null) secondButtonFill.BackColor=thumbHoverColor;
                };
                secondButton.MouseLeave+=(s,e) =>
                {
                    if(secondButtonFill!=null) secondButtonFill.BackColor=thumbNormalColor;
                };
                secondButton.MouseDown+=(s,e) =>
                {
                    if(secondButtonFill!=null) secondButtonFill.BackColor=thumbHoverColor;
                };
                secondButton.MouseUp+=(s,e) =>
                {
                    if(secondButtonFill!=null) secondButtonFill.BackColor=thumbHoverColor;
                };
            }

            // 滑块的鼠标交互事件
            thumb.MouseEnter+=(s,e) =>
            {
                thumb.BackColor=thumbHoverColor;
                if(thumbFill!=null) thumbFill.BackColor=thumbHoverColor;
            };
            thumb.MouseLeave+=(s,e) =>
            {
                thumb.BackColor=thumbNormalColor;
                if(thumbFill!=null) thumbFill.BackColor=thumbNormalColor;
            };
            thumb.MouseDown+=(s,e) =>
            {
                thumb.BackColor=thumbHoverColor;
                if(thumbFill!=null) thumbFill.BackColor=thumbHoverColor;
            };
            thumb.MouseUp+=(s,e) =>
            {
                thumb.BackColor=thumbHoverColor;
                if(thumbFill!=null) thumbFill.BackColor=thumbHoverColor;
            };
        }


        // 四2、滑动条的滑动逻辑
        private void SetupScrolling()
        {
            // 设置右侧垂直滚动条(radRightScrollBar)的属性

            // 设置滚动条的最小值为0（滚动到最顶部）
            radRightScrollBar.Minimum=0;

            // 设置滚动条的最大值：
            // 计算内容高度(button_radPanel.Height)与可视区域高度(scroll_radPanel.Height)的差值
            // 使用Math.Max确保最小值为0（当内容高度小于可视区域高度时）
            radRightScrollBar.Maximum=Math.Max(DragPanel.Height-scroll_radPanel.Height,0);

            // 设置鼠标滚轮或箭头按钮的小步滚动量为10像素
            radRightScrollBar.SmallChange=10;

            // 设置点击滚动条空白区域或PageUp/PageDown键的大步滚动量为50像素
            radRightScrollBar.LargeChange=50;

            // 设置底部水平滚动条(radButtonScrollBar)的属性

            // 设置滚动条的最小值为0（滚动到最左侧）
            radButtonScrollBar.Minimum=0;

            // 设置滚动条的最大值：
            // 计算内容宽度(button_radPanel.Width)与可视区域宽度(scroll_radPanel.Width)的差值
            // 使用Math.Max确保最小值为0（当内容宽度小于可视区域宽度时）
            radButtonScrollBar.Maximum=Math.Max(DragPanel.Width-scroll_radPanel.Width,0);

            // 设置鼠标滚轮或箭头按钮的小步滚动量为10像素
            radButtonScrollBar.SmallChange=10;

            // 设置点击滚动条空白区域或PageUp/PageDown键的大步滚动量为50像素
            radButtonScrollBar.LargeChange=50;

            // 为右侧垂直滚动条添加值改变事件处理
            radRightScrollBar.ValueChanged+=(s,e) =>
            {
                // 当滚动条值改变时，调整button_radPanel的Top位置
                // 使用负值是因为要向上移动内容面板以显示下方内容
                DragPanel.Top=-radRightScrollBar.Value;
            };

            // 为底部水平滚动条添加值改变事件处理
            radButtonScrollBar.ValueChanged+=(s,e) =>
            {
                // 当滚动条值改变时，调整button_radPanel的Left位置
                // 使用负值是因为要向左移动内容面板以显示右侧内容
                DragPanel.Left=-radButtonScrollBar.Value;
            };
        }



        // 五、left_RadPanel
        private void ConfigureLeftRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框          

            // 自定义绘制事件
            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1); // 定义矩形区域
                int radius = 20; // 圆角半径

                // 使用完全非透明绿色画刷和灰色边框笔
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(209,238,226)))//浅绿
                using(Pen borderPen = new Pen(Color.Gray,1))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形
                    g.DrawPath(borderPen,path); // 绘制圆角矩形边框
                }
            };
        }

        // 六、right_RadPanel
        private void ConfigureRightRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框

            // 自定义绘制事件
            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1); // 定义矩形区域
                int radius = 20; // 圆角半径

                // 使用完全非透明画刷和灰色边框笔
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(207,217,250)))//浅紫
                using(Pen borderPen = new Pen(Color.Gray,1))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形
                    g.DrawPath(borderPen,path); // 绘制圆角矩形边框
                }
            };
        }

        // 七、文本框容器
        private void ConfigureTextRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框          

            // 自定义绘制事件
            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1); // 定义矩形区域
                int radius = 20; // 圆角半径

                // 使用完全画刷和灰色边框笔
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(255,255,255)))//浅绿
                using(Pen borderPen = new Pen(Color.Gray,1))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形
                    //g.DrawPath(borderPen, path); // 绘制圆角矩形边框
                }
            };
        }


        // 八、左上方四个RadButton（四个通用）
        private void ConfigureRadButtonStyle(RadButton button)
        {
            button.BackColor=Color.Transparent; // 设置背景透明
            button.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            button.ButtonElement.BackColor=Color.Transparent; // 设置按钮元素背景透明

            // 隐藏默认的填充和边框       
            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed;
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed;

            // 状态变量（用于跟踪鼠标悬停和按下状态）
            bool isHovered = false;
            bool isPressed = false;

            // 事件处理：鼠标进入
            button.MouseEnter+=(s,e) => { isHovered=true; button.Invalidate(); };
            // 事件处理：鼠标离开
            button.MouseLeave+=(s,e) => { isHovered=false; button.Invalidate(); };
            // 事件处理：鼠标按下
            button.MouseDown+=(s,e) => { isPressed=true; button.Invalidate(); };
            // 事件处理：鼠标释放
            button.MouseUp+=(s,e) => { isPressed=false; button.Invalidate(); };

            // 自定义绘制事件
            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,button.Width-1,button.Height-1); // 定义矩形区域
                int radius = 20; // 圆角半径

                Color borderColor = Color.Gray; // 默认边框颜色
                Color fillColor = Color.FromArgb(178,255,255,255); // 默认填充颜色
                int borderWidth = 1; // 默认边框宽度

                //文本框变换
                if(isHovered)
                {
                    borderColor=Color.SteelBlue; // 悬停时边框颜色
                    fillColor=Color.FromArgb(200,230,240,250); // 悬停时填充颜色
                }
                if(isPressed)
                {
                    borderWidth=2; // 按下时边框加粗
                    fillColor=Color.FromArgb(220,200,220,240); // 按下时填充颜色
                }

                // 使用动态颜色的画刷和边框笔
                using(SolidBrush brush = new SolidBrush(fillColor))
                using(Pen borderPen = new Pen(borderColor,borderWidth))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形
                    g.DrawPath(borderPen,path); // 绘制圆角矩形边框
                }

                //文字变换
                Color textColor = Color.FromArgb(30,30,30); // 默认：接近黑色但保持柔和
                if(isHovered)
                    textColor=Color.FromArgb(50,110,180); // 悬停：稍亮的蓝色
                if(isPressed)
                    textColor=Color.FromArgb(80,80,80); // 按下：深灰

                //文字paint（由于前面禁止了引擎，需要自己绘制，不然默认灰）
                TextRenderer.DrawText(
                    g,
                    button.Text,
                    button.Font,
                    button.ClientRectangle,
                    textColor,
                    TextFormatFlags.SingleLine|TextFormatFlags.HorizontalCenter|TextFormatFlags.VerticalCenter|TextFormatFlags.NoPadding // 立体效果
                );
            };
        }

        //九、右一最小化图标塑性---------------------------------------------
        private void ConfigureMinimizeRadButton(RadButton button,string svgFilePath)
        {
            button.BackColor=Color.Transparent;
            button.RootElement.BackColor=Color.Transparent;
            button.ButtonElement.BackColor=Color.Transparent;
            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed;
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed;

            bool isHovered = false;
            bool isPressed = false;

            Bitmap svgBitmap = null;
            if(File.Exists(svgFilePath))
            {
                svgBitmap=RenderSvgToBitmap(svgFilePath,32,32);
            }

            button.MouseEnter+=(s,e) => { isHovered=true; button.Invalidate(); };
            button.MouseLeave+=(s,e) => { isHovered=false; button.Invalidate(); };
            button.MouseDown+=(s,e) => { isPressed=true; button.Invalidate(); };
            button.MouseUp+=(s,e) => { isPressed=false; button.Invalidate(); };

            button.Click+=(s,e) =>
            {
                var form = button.FindForm();
                if(form!=null)
                {
                    form.WindowState=FormWindowState.Minimized;
                }
            };

            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Color fillColor = Color.Transparent;
                if(isPressed)
                    fillColor=Color.FromArgb(100,Color.Gray);
                else if(isHovered)
                    fillColor=Color.FromArgb(60,Color.Gray);

                using(SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillRectangle(brush,button.ClientRectangle);
                }

                if(svgBitmap!=null)
                {
                    int iconSize = (int)(Math.Min(button.Width,button.Height)*0.35);
                    Rectangle iconRect = new Rectangle(
                        (button.Width-iconSize)/2,
                        (button.Height-iconSize)/2,
                        iconSize,
                        iconSize
                    );
                    g.DrawImage(svgBitmap,iconRect);
                }
            };
        }


        //十、右二最大化图标塑性--------------------------------------------- 
        // ！！当前的问题在于如果缩小之后，放大窗口，panel无法根据当前的位置来更新在大窗口的尺寸！！！！！
        private bool isMaximized = false; // 记录是放大状态还是缩小状态
        private void ConfigureMaximizeRadButton(RadButton button,string svgFilePath)
        {
            button.BackColor=Color.Transparent;
            button.RootElement.BackColor=Color.Transparent;
            button.ButtonElement.BackColor=Color.Transparent;
            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed;
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed;

            //--------------------收缩环逻辑--------------------begin
            CollapsibleDraggablePanel collapsiblePanel1 = new CollapsibleDraggablePanel(left_radPanel,left_radPanel,"图标\\代码块.svg");
            ShrinkRadButton1.Click+=(s,ev) =>
            {
                collapsiblePanel1.ToggleCollapse();
            };

            CollapsibleDraggablePanel2 collapsiblePanel2 = new CollapsibleDraggablePanel2(right_radPanel,right_radPanel,"图标\\说话.svg");
            ShrinkRadButton2.Click+=(s,ev) =>
            {
                collapsiblePanel2.ToggleCollapse();
            };
            //--------------------收缩环逻辑--------------------end


            var currentExpandedSize = collapsiblePanel1.ExpandedSize; //加个属性返回当前“展开尺寸”
            //var currentLocation = left_radPanel.Location;
            ////sizeSwitcher.UpdateExpandedState(left_radPanel, currentExpandedSize, currentLocation);
            sizeSwitcher.RegisterControl(left_radPanel,currentExpandedSize,new Point(30,40));
            var currentExpandedSize2 = collapsiblePanel2.ExpandedSize;
            //var currentLocation2 = right_radPanel.Location;
            ////sizeSwitcher.UpdateExpandedState(right_radPanel, currentExpandedSize2, currentLocation2);
            sizeSwitcher.RegisterControl(right_radPanel,currentExpandedSize2,new Point(2300,40));

            //------------------------------------------------

            bool isHovered = false;
            bool isPressed = false;

            Bitmap svgBitmap = null;
            if(File.Exists(svgFilePath))
            {
                svgBitmap=RenderSvgToBitmap(svgFilePath,32,32);
            }

            button.MouseEnter+=(s,e) => { isHovered=true; button.Invalidate(); };
            button.MouseLeave+=(s,e) => { isHovered=false; button.Invalidate(); };
            button.MouseDown+=(s,e) => { isPressed=true; button.Invalidate(); };
            button.MouseUp+=(s,e) => { isPressed=false; button.Invalidate(); };

            button.Click+=async (s,e) =>
            {
                isMaximized=!isMaximized;// 切换状态

                var form = button.FindForm();
                if(form!=null)
                {
                    // 第一步：先隐藏左右面板
                    left_radPanel.Visible=false;
                    right_radPanel.Visible=false;

                    // 小间隔，等待系统处理隐藏（避免重影）
                    await Task.Delay(50);

                    // 执行尺寸与位置切换
                    if(isMaximized)
                    {
                        // 放大状态
                        sizeSwitcher.UpdateExpandedState(left_radPanel,collapsiblePanel1.ExpandedSize,new Point(30,40));
                        sizeSwitcher.UpdateExpandedState(right_radPanel,collapsiblePanel2.ExpandedSize,new Point(2300,40));
                    }
                    else
                    {
                        // 缩小状态
                        sizeSwitcher.UpdateExpandedState(left_radPanel,collapsiblePanel1.ExpandedSize,new Point(30,40));
                        sizeSwitcher.UpdateExpandedState(right_radPanel,collapsiblePanel2.ExpandedSize,new Point(1000,40));
                    }

                    //开始尺寸切换（含放大/缩小动画）
                    sizeSwitcher.Toggle();

                    // 最后显示左右面板
                    left_radPanel.Visible=true;
                    right_radPanel.Visible=true;
                }
            };

            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Color fillColor = Color.Transparent;
                if(isPressed)
                    fillColor=Color.FromArgb(100,Color.SteelBlue);
                else if(isHovered)
                    fillColor=Color.FromArgb(60,Color.SteelBlue);

                using(SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillRectangle(brush,button.ClientRectangle);
                }

                if(svgBitmap!=null)
                {
                    int iconSize = (int)(Math.Min(button.Width,button.Height)*0.35);
                    Rectangle iconRect = new Rectangle(
                        (button.Width-iconSize)/2,
                        (button.Height-iconSize)/2,
                        iconSize,
                        iconSize
                    );
                    g.DrawImage(svgBitmap,iconRect);
                }
            };
        }

        //十一、右三关闭图标塑性---------------------------------------------
        private void ConfigureCloseRadButton(RadButton button,string svgFilePath)
        {
            button.BackColor=Color.Transparent;
            button.RootElement.BackColor=Color.Transparent;
            button.ButtonElement.BackColor=Color.Transparent;
            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed;
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed;

            bool isHovered = false;
            bool isPressed = false;

            Bitmap svgBitmap = null;
            if(File.Exists(svgFilePath))
            {
                svgBitmap=RenderSvgToBitmap(svgFilePath,32,32);
            }

            button.MouseEnter+=(s,e) => { isHovered=true; button.Invalidate(); };
            button.MouseLeave+=(s,e) => { isHovered=false; button.Invalidate(); };
            button.MouseDown+=(s,e) => { isPressed=true; button.Invalidate(); };
            button.MouseUp+=(s,e) => { isPressed=false; button.Invalidate(); };

            button.Click+=(s,e) =>
            {
                var form = button.FindForm();
                if(form!=null)
                {
                    form.Close();
                }
            };

            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Color fillColor = Color.Transparent;
                if(isPressed)
                    fillColor=Color.FromArgb(150,Color.Red);
                else if(isHovered)
                    fillColor=Color.FromArgb(100,Color.Red);

                using(SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillRectangle(brush,button.ClientRectangle);
                }

                if(svgBitmap!=null)
                {
                    int iconSize = (int)(Math.Min(button.Width,button.Height)*0.35);
                    Rectangle iconRect = new Rectangle(
                        (button.Width-iconSize)/2,
                        (button.Height-iconSize)/2,
                        iconSize,
                        iconSize
                    );
                    g.DrawImage(svgBitmap,iconRect);
                }
            };
        }


        private void radButton1_Click(object sender,EventArgs e)
        {
            MessageBox.Show("填写的表达式中支持的运算符：\r\n数学运算符：+ - * / %\r\n比较运算符：< > == <= >= !=\r\n逻辑运算符：! && ||\r\n位运算符： ~ ^ & | << >> \r\n为保证正确编译与烧录，请将keil以及st-link unity程序的完整路径分别填写在config.txt的第1、2行中\r\n文件保存读取位置为projectfile.txt ","帮助");
        }

        // 十二、panel内分隔线
        // （还有白底问题！！！！！！！！！！！！）
        private void ConfigureSeparatorStyle(RadSeparator separator)
        {
            // 设置完全透明背景（解决白底问题）
            separator.BackColor=Color.Transparent; // 控件背景透明
            separator.SeparatorElement.BackColor=Color.Transparent; // 分隔线元素背景透明
            separator.SeparatorElement.ForeColor=Color.Transparent; // 前景色透明
            separator.SeparatorElement.GradientStyle=GradientStyles.Solid; // 使用纯色样式
            separator.SeparatorElement.DrawFill=false; // 禁止绘制填充
            separator.SeparatorElement.DrawBorder=false; // 禁止绘制边框

            // 自定义绘制事件
            separator.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 启用抗锯齿

                // 使用灰色画笔绘制线条
                using(Pen pen = new Pen(Color.FromArgb(255,255,255),1)) // #BBBBBB颜色，1像素粗细
                {
                    int y = separator.Height/2; // 计算垂直居中位置
                    g.DrawLine(pen,0,y,separator.Width,y); // 从左侧到右侧绘制横线
                }
            };
        }

        // 十三、囊样式按钮
        // 第三参数为显示文本，第四参数为图标文字距（使用SVG图标）
        private void ConfigureCapsuleRadButton(RadButton button,string svgFilePath,string displayText,int imag_to_text)
        {
            // 设置按钮背景透明（三层透明设置确保完全透明）
            button.BackColor=Color.Transparent;            // 设置控件背景透明
            button.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            button.ButtonElement.BackColor=Color.Transparent; // 设置按钮元素背景透明

            // 禁用Telerik默认渲染元素（避免与自定义绘制冲突）
            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed; // 隐藏按钮填充元素
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed; // 隐藏按钮边框元素
            button.Image=null; // 清空默认图标设置（避免自动渲染）
            button.Text="";    // 清空默认文本（避免自动渲染）

            // 按钮交互状态跟踪变量
            bool isHovered = false; // 鼠标悬停状态标志
            bool isPressed = false; // 鼠标按下状态标志

            // 鼠标事件处理
            button.MouseEnter+=(s,e) =>
            {
                isHovered=true;    // 标记悬停状态
                button.Invalidate(); // 触发重绘
            };
            button.MouseLeave+=(s,e) =>
            {
                isHovered=false;   // 清除悬停状态
                button.Invalidate();
            };
            button.MouseDown+=(s,e) =>
            {
                isPressed=true;    // 标记按下状态
                button.Invalidate();
            };
            button.MouseUp+=(s,e) =>
            {
                isPressed=false;   // 清除按下状态
                button.Invalidate();
            };

            // 按钮显示内容配置           
            // 按钮显示文本内容前置了，可以多个公用
            Font font = new Font("阿里巴巴普惠体 3.0",12f,FontStyle.Regular); // 文本字体设置
            Color textColor = Color.Black; // 文本颜色

            // SVG 图标 Bitmap 渲染，把SVG图片渲染成 Bitmap，不用PNG在于一旦缩小它的锯齿太严重了
            Bitmap svgBitmap = null;
            if(File.Exists(svgFilePath))
            {
                svgBitmap=RenderSvgToBitmap(svgFilePath,32,32); // 渲染为 32×32 图标
            }

            // 自定义绘制事件
            button.Paint+=(s,e) =>
            {
                // 获取绘图对象并设置抗锯齿
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 计算绘制区域（留出1像素边框）
                Rectangle rect = new Rectangle(0,0,button.Width-1,button.Height-1);
                int radius = button.Height/2; // 圆角半径（胶囊效果）

                // 根据状态设置背景色
                Color fillColor = Color.FromArgb(166,255,255,255); // 默认状态：65%透明白色
                if(isHovered)
                    fillColor=Color.FromArgb(200,230,240,255); // 悬停状态：浅蓝色
                if(isPressed)
                    fillColor=Color.FromArgb(220,210,230,250); // 按下状态：更深蓝色

                // 绘制圆角背景
                using(SolidBrush bgBrush = new SolidBrush(fillColor)) // 创建背景画刷
                using(var path = GetRoundedRectPath(rect,radius))    // 获取圆角矩形路径
                {
                    g.FillPath(bgBrush,path); // 填充背景
                }

                /* 图标绘制部分 */
                int iconSize = 32; // 图标尺寸（更小的现代风格）
                                   // 计算图标位置（垂直居中）
                Rectangle iconRect = new Rectangle(
                    30,                                   // X坐标：左侧30像素
                    (button.Height-iconSize)/2,       // Y坐标：垂直居中
                    iconSize,                             // 宽度
                    iconSize                              // 高度
                );

                if(svgBitmap!=null)
                {
                    g.DrawImage(svgBitmap,iconRect);
                }

                /* 文本绘制部分 */
                int textX = iconRect.Right+imag_to_text; // 文本起始X坐标（图标右侧 imag_to_text 像素）
                Rectangle textRect = new Rectangle(
                    textX,                      // X坐标
                    0,                          // Y坐标
                    button.Width-textX-10,  // 宽度（右侧留10像素边距）
                    button.Height               // 高度
                );
                using(SolidBrush textBrush = new SolidBrush(textColor)) // 创建文本画刷
                {
                    // 设置文本格式（垂直居中，左对齐）
                    StringFormat sf = new StringFormat
                    {
                        LineAlignment=StringAlignment.Center, // 垂直居中
                        Alignment=StringAlignment.Near        // 水平左对齐
                    };
                    // 绘制文本
                    g.DrawString(displayText,font,textBrush,textRect,sf);
                }
            };
        }

        private void ConfigureCapsuleRadButton(RadButton button,string svgFilePath,string displayText,int imag_to_text,Func<bool> isSelected)
        {
            // 设置按钮背景透明（三层透明设置确保完全透明）
            button.BackColor=Color.Transparent;
            button.RootElement.BackColor=Color.Transparent;
            button.ButtonElement.BackColor=Color.Transparent;

            button.ButtonElement.ButtonFillElement.Visibility=ElementVisibility.Collapsed;
            button.ButtonElement.BorderElement.Visibility=ElementVisibility.Collapsed;
            button.Image=null;
            button.Text="";

            bool isHovered = false;
            bool isPressed = false;

            button.MouseEnter+=(s,e) => { isHovered=true; button.Invalidate(); };
            button.MouseLeave+=(s,e) => { isHovered=false; button.Invalidate(); };
            button.MouseDown+=(s,e) => { isPressed=true; button.Invalidate(); };
            button.MouseUp+=(s,e) => { isPressed=false; button.Invalidate(); };

            // 渲染 SVG 图标
            Bitmap svgBitmap = null;
            if(File.Exists(svgFilePath))
            {
                svgBitmap=RenderSvgToBitmap(svgFilePath,32,32);
            }

            // 自定义绘制
            button.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0,0,button.Width-1,button.Height-1);
                int radius = button.Height/2;

                Color fillColor = Color.FromArgb(166,255,255,255); // 默认背景
                if(isHovered)
                    fillColor=Color.FromArgb(200,230,240,255);
                if(isPressed)
                    fillColor=Color.FromArgb(220,210,230,250);

                using(SolidBrush bgBrush = new SolidBrush(fillColor))
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(bgBrush,path);
                }

                // 图标绘制
                int iconSize = 32;
                Rectangle iconRect = new Rectangle(
                    30,
                    (button.Height-iconSize)/2,
                    iconSize,
                    iconSize
                );
                if(svgBitmap!=null)
                {
                    g.DrawImage(svgBitmap,iconRect);
                }

                // 文字样式根据是否选中变化
                bool selected = isSelected(); // 获取是否选中状态
                Font font = new Font("阿里巴巴普惠体 3.0",12f,selected ? FontStyle.Bold : FontStyle.Regular);
                Color textColor = selected ? Color.Black : Color.LightGray;

                int textX = iconRect.Right+imag_to_text;
                Rectangle textRect = new Rectangle(textX,0,button.Width-textX-10,button.Height);
                using(SolidBrush textBrush = new SolidBrush(textColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        LineAlignment=StringAlignment.Center,
                        Alignment=StringAlignment.Near
                    };
                    g.DrawString(displayText,font,textBrush,textRect,sf);
                }
            };
        }

        //十四、最顶部的滑动条（与panel内滑动条的区别在于颜色更深了）
        private void ConfigureMoveRadPanel_top(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框          

            // 自定义绘制事件
            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1);
                int radius = panel.Height/2; // 圆角半径（胶囊效果）

                // 使用画刷
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(160,150,150,150)))//灰色           
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形                   
                }
            };
        }

        //十五、panel内的两个滑动条
        private void ConfigureMoveRadPanel(RadPanel panel)
        {
            panel.BackColor=Color.Transparent; // 设置背景透明
            panel.RootElement.BackColor=Color.Transparent; // 设置根元素背景透明
            panel.PanelElement.PanelBorder.Visibility=ElementVisibility.Collapsed; // 隐藏面板边框          

            // 自定义绘制事件
            panel.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics; // 获取绘图对象
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 设置抗锯齿

                Rectangle rect = new Rectangle(0,0,panel.Width-1,panel.Height-1);
                int radius = panel.Height/2; // 圆角半径（胶囊效果）

                // 使用画刷
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(160,200,200,200)))//灰色           
                using(var path = GetRoundedRectPath(rect,radius))
                {
                    g.FillPath(brush,path); // 填充圆角矩形                   
                }
            };
        }

        //十六、右panel内文本框
        private void ConfigureRadTextBox(RadTextBox textBox)
        {
            // 背景透明
            textBox.BackColor=Color.Transparent;
            textBox.RootElement.BackColor=Color.Transparent;
            textBox.TextBoxElement.BackColor=Color.Transparent;

            // 禁用默认边框和填充
            textBox.TextBoxElement.Fill.Visibility=ElementVisibility.Collapsed;
            textBox.TextBoxElement.Border.Visibility=ElementVisibility.Collapsed;

            // 基础属性
            textBox.Multiline=true;
            textBox.ReadOnly=true;
            textBox.ScrollBars=ScrollBars.Vertical;
            textBox.Size=new Size(340,350);

            // 鼠标状态
            bool isHovered = false;
            bool isPressed = false;

            textBox.MouseEnter+=(s,e) => { isHovered=true; textBox.Invalidate(); };
            textBox.MouseLeave+=(s,e) => { isHovered=false; textBox.Invalidate(); };
            textBox.MouseDown+=(s,e) => { isPressed=true; textBox.Invalidate(); };
            textBox.MouseUp+=(s,e) => { isPressed=false; textBox.Invalidate(); };

            // 自定义绘制
            textBox.Paint+=(s,e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0,0,textBox.Width-1,textBox.Height-1);
                int radius = 20;

                // 状态变化颜色
                Color fillColor = Color.FromArgb(160,255,255,255); // 默认：半透明白色
                if(isHovered)
                    fillColor=Color.FromArgb(180,240,245,255); // 悬停：浅蓝白
                if(isPressed)
                    fillColor=Color.FromArgb(200,220,230,255); // 按下：更深

                //using (SolidBrush brush = new SolidBrush(fillColor))
                //using (var path = GetRoundedRectPath(rect, radius))
                //{
                //    g.FillPath(brush, path);
                //}
            };
        }






        // SVG使用方法，要先转
        // 加载 SVG 文件为 Bitmap，由于直接使用 RadSvgImage 里面的库函数不完善，我只能用第三方库Svg
        Bitmap RenderSvgToBitmap(string svgPath,int width,int height)
        {
            var svgDoc = SvgDocument.Open(svgPath);
            svgDoc.Width=width;
            svgDoc.Height=height;
            return svgDoc.Draw();
        }



        // 开启双缓冲的帮助方法
        public static void EnableDoubleBuffering(Control ctrl)
        {
            if(ctrl==null) return;
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty("DoubleBuffered",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
            aProp?.SetValue(ctrl,true,null);
        }
        private void EnableDoubleBufferingRecursive(Control control)
        {
            if(control is RadControl||control is Panel||control is Button||control is Label)
            {
                control.GetType().GetProperty("DoubleBuffered",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(control,true,null);
            }

            // 递归对子控件也启用
            foreach(Control child in control.Controls)
            {
                EnableDoubleBufferingRecursive(child);
            }
        }

        private void button_radPanel_Paint(object sender,PaintEventArgs e)
        {

        }

        private void RadMainForm_Load(object sender,EventArgs e)
        {

        }

        private void CapsuleRadButton1_Click(object sender,EventArgs e)
        {
            Create_Panel("程序入口");
        }

        private void CapsuleRadButton2_Click(object sender,EventArgs e)
        {
            Create_Sib("向前走_cm");
        }

        private void CapsuleRadButton3_Click(object sender,EventArgs e)
        {
            Create_Sib("向左旋转_°");
        }

        private void CapsuleRadButton4_Click(object sender,EventArgs e)
        {
            Create_Sib("以左侧_cm为圆心旋转_°");
        }

        private void CapsuleRadButton5_Click(object sender,EventArgs e)
        {
            Create_Sib("令_为_");
        }

        private void CapsuleRadButton6_Click(object sender,EventArgs e)
        {
            Create_Panel("如果_");
        }

        private void CapsuleRadButton7_Click(object sender,EventArgs e)
        {
            Create_Panel("否则");
        }

        private void CapsuleRadButton8_Click(object sender,EventArgs e)
        {
            Create_Panel("重复执行_次");
        }

        private void CapsuleRadButton9_Click(object sender,EventArgs e)
        {
            Create_Panel("若_成立则重复执行");
        }

        private void CapsuleRadButton10_Click(object sender,EventArgs e)
        {
            Create_Sib("中断");
        }
        private void savefile()
        {
            int x = 0;
            int bl = 0;
            if(this.enter==null) return;
            block[] bt = new block[10000];
            HashSet<string> st = new HashSet<string>(); ;
            bt[0].expr=new string[2];
            bt[0].c_id=new int[2];
            build(this.enter,ref bt,ref bl,0,ref st);
            outputblock(ref bt,bl,ref st,"projectfile.txt");
            MessageBox.Show("保存成功");
        }
        private void readfile()
        {
            if(this.enter!=null)
            {
                this.enter.Parent.Controls.Remove(this.enter);
                tps.Remove(this.enter);
                this.enter=null;
            }
            string[] lines = File.ReadAllLines(@"projectfile.txt");
            int len = lines.Length;
            int ip = 0;
            int n = int.Parse(lines[ip++]);
            iptblock=new block[1000];
            st=new Dictionary<int,object>();
            for(int i = 1;i<=n;i++)
            {
                iptblock[i].id=int.Parse(lines[ip++]);
                iptblock[i].type=lines[ip++];
                iptblock[i].elen=int.Parse(lines[ip++]);
                iptblock[i].expr=new string[2];
                for(int j = 0;j<iptblock[i].elen;j++)
                {
                    iptblock[i].expr[j]=lines[ip++];
                    if(iptblock[i].expr[j]=="???") iptblock[i].expr[j]="";
                }
                iptblock[i].clen=int.Parse(lines[ip++]);
                iptblock[i].c_id=new int[2];
                for(int j = 0;j<iptblock[i].clen;j++) iptblock[i].c_id[j]=int.Parse(lines[ip++]);
                iptblock[i].nxt=int.Parse(lines[ip++]);
                if(iptblock[i].clen==0)
                {
                    SegmentedInputBox sib;
                    switch(iptblock[i].type)
                    {
                    case "goline":
                    sib=Create_Sib("向前走_cm");
                    sib.Boxes[0].Text=iptblock[i].expr[0];
                    st.Add(i,sib);
                    break;
                    case "turn":
                    sib=Create_Sib("向左旋转_°");
                    sib.Boxes[0].Text=iptblock[i].expr[0];
                    st.Add(i,sib);
                    break;
                    case "circle":
                    sib=Create_Sib("以左侧_cm为圆心旋转_°");
                    sib.Boxes[0].Text=iptblock[i].expr[0];
                    sib.Boxes[1].Text=iptblock[i].expr[1];
                    st.Add(i,sib);
                    break;
                    case "break":
                    st.Add(i,Create_Sib("中断"));
                    break;
                    case "mov":
                    sib=Create_Sib("令_为_");
                    sib.Boxes[0].Text=iptblock[i].expr[0];
                    sib.Boxes[1].Text=iptblock[i].expr[1];
                    st.Add(i,sib);
                    break;
                    case "undefcode":
                    st.Add(i,Create_Sib("需要修正的积木块"));
                    break;
                    }
                }
                else
                {
                    TitledPanel tp;
                    switch(iptblock[i].type)
                    {
                    case "if":
                    tp=Create_Panel("如果_");
                    tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                    st.Add(i,tp);
                    break;
                    case "while":
                    tp=Create_Panel("若_成立则重复执行");
                    tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                    st.Add(i,tp);
                    break;
                    case "for":
                    tp=Create_Panel("重复执行_次");
                    tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                    st.Add(i,tp);
                    break;
                    }
                }
            }
            st.Add(0,Create_Panel("程序入口")); iptblock[0].c_id=new int[2]; iptblock[0].c_id[0]=iptblock[0].clen=1;
            for(int i = 0;i<=n;i++)
            {
                if(st[i] is TitledPanel&&iptblock[i].clen!=0)
                {
                    if(iptblock[i].c_id[0]!=0) (st[i] as TitledPanel).Add(st[iptblock[i].c_id[0]] as Control,1);
                    if(iptblock[i].clen==2)
                    {
                        TitledPanel eltp = Create_Panel("否则");
                        link(st[i] as Control,eltp as Control);
                        if(iptblock[i].c_id[1]!=0) eltp.Add(st[iptblock[i].c_id[1]] as Control,1);
                        if(iptblock[i].nxt!=0) link(eltp as Control,st[iptblock[i].nxt] as Control);
                    }
                }
                if(iptblock[i].clen!=2&&i!=0)
                {
                    if(iptblock[i].nxt!=0) link(st[i] as Control,st[iptblock[i].nxt] as Control);
                }
            }
        }
        private async void CapsuleRadButton13_Click(object sender,EventArgs e)
        {
            if(this.enter!=null)
            {
                MessageBox.Show("请先删除旧积木");
                return;
            }
            File.WriteAllText("in1.txt",string.Empty);
            isRecording=!isRecording;
            if(isRecording)
            {
                //ConfigureCapsuleRadButton(CapsuleRadButton13,"..\\..\\..\\图标\\麦克风.svg","    ",50);
                //ConfigureCapsuleRadButton(CapsuleRadButton13,"..\\..\\..\\图标\\麦克风.svg","停止录音",50);

                ConfigureCapsuleRadButton(CapsuleRadButton13,"图标\\麦克风.svg","    ",50);
                ConfigureCapsuleRadButton(CapsuleRadButton13,"图标\\麦克风.svg","停止录音",50);
            }
            else
            {
                //ConfigureCapsuleRadButton(CapsuleRadButton13,"..\\..\\..\\图标\\麦克风.svg","    ",50);
                //ConfigureCapsuleRadButton(CapsuleRadButton13,"..\\..\\..\\图标\\麦克风.svg","开始录音",50);

                ConfigureCapsuleRadButton(CapsuleRadButton13,"图标\\麦克风.svg","    ",50);
                ConfigureCapsuleRadButton(CapsuleRadButton13,"图标\\麦克风.svg","开始录音",50);
            }
            //CapsuleRadButton13.Text=isRecording ? "停止录音" : "开始录音";

            //return;

            if(isRecording)
            {
                await converter.RecordAudioAsync(true,"recorded_audio.wav",UpdateStatus);
            }
            else
            {
                await converter.RecordAudioAsync(false,"recorded_audio.wav",UpdateStatus);

                string speechText = await converter.RecognizeSpeechOnlyAsync("recorded_audio.wav",UpdateStatus);
                radTextBox.Text=speechText;

                await converter.ConvertAudioToInstructionsAsync("recorded_audio.wav","in1.txt",UpdateStatus);
            }
            if(!isRecording)
            {
                Thread.Sleep(2000);
                systemcall("logic_to_block.exe");
                string[] lines = File.ReadAllLines(@"logic_to_block.out");
                if(lines.Length<=3)
                {
                    MessageBox.Show("空内容或未联网");
                    return;
                }
                int len = lines.Length;
                int ip = 0;
                int n = int.Parse(lines[ip++]);
                iptblock=new block[1000];
                st=new Dictionary<int,object>();
                for(int i = 1;i<=n;i++)
                {
                    iptblock[i].id=int.Parse(lines[ip++]);
                    iptblock[i].type=lines[ip++];
                    iptblock[i].elen=int.Parse(lines[ip++]);
                    iptblock[i].expr=new string[2];
                    for(int j = 0;j<iptblock[i].elen;j++) iptblock[i].expr[j]=lines[ip++];
                    iptblock[i].clen=int.Parse(lines[ip++]);
                    iptblock[i].c_id=new int[2];
                    for(int j = 0;j<iptblock[i].clen;j++) iptblock[i].c_id[j]=int.Parse(lines[ip++]);
                    iptblock[i].nxt=int.Parse(lines[ip++]);
                    if(iptblock[i].clen==0)
                    {
                        SegmentedInputBox sib;
                        switch(iptblock[i].type)
                        {
                        case "goline":
                        sib=Create_Sib("向前走_cm");
                        sib.Boxes[0].Text=iptblock[i].expr[0];
                        st.Add(i,sib);
                        break;
                        case "turn":
                        sib=Create_Sib("向左旋转_°");
                        sib.Boxes[0].Text=iptblock[i].expr[0];
                        st.Add(i,sib);
                        break;
                        case "circle":
                        sib=Create_Sib("以左侧_cm为圆心旋转_°");
                        sib.Boxes[0].Text=iptblock[i].expr[0];
                        sib.Boxes[1].Text=iptblock[i].expr[1];
                        st.Add(i,sib);
                        break;
                        case "break":
                        st.Add(i,Create_Sib("中断"));
                        break;
                        case "mov":
                        sib=Create_Sib("令_为_");
                        sib.Boxes[0].Text=iptblock[i].expr[0];
                        sib.Boxes[1].Text=iptblock[i].expr[1];
                        st.Add(i,sib);
                        break;
                        case "undefcode":
                        st.Add(i,Create_Sib("需要修正的积木块"));
                        break;
                        }
                    }
                    else
                    {
                        TitledPanel tp;
                        switch(iptblock[i].type)
                        {
                        case "if":
                        tp=Create_Panel("如果_");
                        tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                        st.Add(i,tp);
                        break;
                        case "while":
                        tp=Create_Panel("若_成立则重复执行");
                        tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                        st.Add(i,tp);
                        break;
                        case "for":
                        tp=Create_Panel("重复执行_次");
                        tp.sib.Boxes[0].Text=iptblock[i].expr[0];
                        st.Add(i,tp);
                        break;
                        }
                    }
                }

                st.Add(0,Create_Panel("程序入口")); iptblock[0].c_id=new int[2]; iptblock[0].c_id[0]=iptblock[0].clen=1;
                for(int i = 0;i<=n;i++)
                {
                    if(st[i] is TitledPanel&&iptblock[i].clen!=0)
                    {
                        if(iptblock[i].c_id[0]!=0) (st[i] as TitledPanel).Add(st[iptblock[i].c_id[0]] as Control,1);
                        if(iptblock[i].clen==2)
                        {
                            TitledPanel eltp = Create_Panel("否则");
                            link(st[i] as Control,eltp as Control);
                            if(iptblock[i].c_id[1]!=0) eltp.Add(st[iptblock[i].c_id[1]] as Control,1);
                            if(iptblock[i].nxt!=0) link(eltp as Control,st[iptblock[i].nxt] as Control);
                        }
                    }
                    if(iptblock[i].clen!=2&&i!=0)
                    {
                        if(iptblock[i].nxt!=0) link(st[i] as Control,st[iptblock[i].nxt] as Control);
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender,EventArgs e)
        {
            //if(e.Button==MouseButtons.Left)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                ContextMenuStrip cms = item.Owner as ContextMenuStrip;
                Control clickedControl = cms.SourceControl;//获取右键的对象啊
                if((clickedControl.Parent is TitledPanel))   //删除控件
                {
                    (clickedControl.Parent as TitledPanel).Remove(clickedControl);
                }
                else
                {
                    if(clickedControl==this.enter) this.enter=null;
                    if(clickedControl is TitledPanel) tps.Remove(clickedControl as TitledPanel);
                    clickedControl.Parent.Controls.Remove(clickedControl);
                }
            }
        }

        private void radButton4_Click(object sender,EventArgs e)
        {
            //if(e.Button==MouseButtons.Left)
            {
                int x = 0;
                int bl = 0;
                if(this.enter==null) return;
                block[] bt = new block[10000];
                HashSet<string> st = new HashSet<string>(); ;
                bt[0].expr=new string[2];
                bt[0].c_id=new int[2];
                build(this.enter,ref bt,ref bl,0,ref st);
                outputblock(ref bt,bl,ref st);
                systemcall("block_to_logic.exe");
                string content = File.ReadAllText(@"./block_to_logic.out");
                if(content==""||content[0]==' ')
                {
                    //this.enter.sib.BackColor=Setting.errcolor;
                    if(content=="") MessageBox.Show("空内容","错误");
                    else MessageBox.Show(content,"错误");
                    //this.enter.sib.BackColor=Setting.BoxColor;

                }
                else
                {
                    systemcall("logic_to_code.exe");
                    //File.Copy("freertos.c","..\\..\\..\\..\\STMH750VBT6_Celebright_V3.1\\STMH750VBT6_Celebright_V2\\Core\\Src\\freertos.c",overwrite: true);

                    File.Copy("freertos.c","STMH750VBT6_Celebright_V3.1\\STMH750VBT6_Celebright_V2\\Core\\Src\\freertos.c",overwrite: true);
                    keiling();
                }
            }
        }
        private void keiling()
        {
            //Console.WriteLine("Keil 开始编译!");
            //MessageBox.Show("Keil 开始编译!");
            // Keil UV4.exe 路径
            try
            {
                string[] lines = File.ReadAllLines(@"./config.txt");

                //string keilPath = @"D:\keil\UV4\UV4.exe";
                string keilPath = lines[0];
                // Keil项目文件路径
                string projectPath = @"STMH750VBT6_Celebright_V3.1\STMH750VBT6_Celebright_V2\MDK-ARM\STMH750VBT6_Celebright_V2.uvprojx";

                // 1. 编译Keil项目
                Process compileProcess = new Process();
                compileProcess.StartInfo.FileName=keilPath;
                compileProcess.StartInfo.Arguments=$"-b \"{projectPath}\""; // 批处理模式，自动编译
                compileProcess.StartInfo.WindowStyle=ProcessWindowStyle.Hidden; // 隐藏窗口
                compileProcess.StartInfo.CreateNoWindow=true; // 不创建新窗口
                compileProcess.StartInfo.UseShellExecute=false; // 不使用系统外壳
                compileProcess.Start();
                compileProcess.WaitForExit(); // 等待编译完成

                //Console.WriteLine("Keil 编译完成!");
                //MessageBox.Show("Keil 编译完成!");
                //return;

                // 2. 烧录 (使用 ST-Link CLI 工具)
                // ST-Link命令行工具路径
                string stLinkPath = lines[1];
                //string stLinkPath = @"D:\st_link_unity\ST-LINK Utility\ST-LINK_CLI.exe";
                // 固件文件路径
                string firmwarePath = @"STMH750VBT6_Celebright_V3.1\STMH750VBT6_Celebright_V2\MDK-ARM\STMH750VBT6_Celebright_V2\STMH750VBT6_Celebright_V2.hex";

                Process flashProcess = new Process();
                flashProcess.StartInfo.FileName=stLinkPath;
                flashProcess.StartInfo.Arguments=$"-c SWD -P \"{firmwarePath}\" 0x08000000";
                flashProcess.StartInfo.WindowStyle=ProcessWindowStyle.Hidden; // 隐藏窗口
                flashProcess.StartInfo.CreateNoWindow=true; // 不创建新窗口
                flashProcess.StartInfo.UseShellExecute=false; // 不使用系统外壳
                flashProcess.Start();
                flashProcess.WaitForExit(); // 等待烧录完成

                //Console.WriteLine("烧录完成!");
                MessageBox.Show("烧录完成！");
            }
            catch
            {
                MessageBox.Show("请检查config.txt文件中文件路径正确性","错误");
            }
            
        }

        private void CapsuleRadButton11_Click(object sender,EventArgs e)
        {
            // 如果当前已经是模式2，再点一下就切换（变为1）
            sensormode=(sensormode==2) ? 1 : 2;
            CapsuleRadButton11.Invalidate();
            CapsuleRadButton12.Invalidate();
        }

        private void CapsuleRadButton12_Click(object sender,EventArgs e)
        {
            // 如果当前已经是模式1，再点一下就切换（变为2）
            sensormode=(sensormode==1) ? 2 : 1;
            CapsuleRadButton11.Invalidate();
            CapsuleRadButton12.Invalidate();
        }

        private void radTextBox_TextChanged(object sender,EventArgs e)
        {

        }

        private void radButton2_Click(object sender,EventArgs e)
        {
            readfile();
        }

        private void radButton3_Click(object sender,EventArgs e)
        {
            savefile();
        }
    }
}
