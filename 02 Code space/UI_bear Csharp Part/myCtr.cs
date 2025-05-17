using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WinFormsApp1
{
    public interface IMyControl
    {
        ctrData data { set; get; }
    }
    //自定义按钮
    public class myButton : Button, IMyControl
    {
        public myButton() : base()
        {

        }
        private creBtnData _data = new creBtnData();
        public ctrData data
        {
            set { _data = value as creBtnData; }
            get { return _data; }
        }
    }
    //自定义tablepanel
    public class myTabPan : TableLayoutPanel, IMyControl
    {
        private tableData _data = new tableData();
        public ctrData data
        {
            set { _data = value as tableData; }
            get { return _data; }
        }
        public myTabPan() : base()
        {

        }
    }
    public class myLabel : Label, IMyControl {
        public ctrData data { set; get; } = new ctrData();
    }
    public class myTextBox : TextBox, IMyControl
    {
        public ctrData data { set; get; } = new ctrData();
    }
    public class TitledPanel : TableLayoutPanel, IMyControl
    {
        public static int Num = 0;
        public  int nestedLevel = 0;

        public ctrData data { set; get; } = new ctrData();
        public SegmentedInputBox sib;
        public string text;
        
        public float BorderWidth { get; set; } = 0.5F;
        public int borderRadius = 20;


        public TitledPanel(string title)
        {
            SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
            UpdateStyles();

            Padding = new Padding(10, 5, 5, 5);
            BackColor = Setting.ColorMap[title].NestedLevels[nestedLevel];
            text = title;
            AllowDrop = true;            
            ColumnCount = 1;
            Height = Setting.RegularHeith;
            Width = Setting.RegularWidth;
            RowCount = 1;            
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            sib = new SegmentedInputBox(text,true);
            Add(sib);
            this.SetRoundedRegion(20);
        }
        public string toString()
        {
            string info = "";
            return info;
        }
        public void Add(Control ctr,int index=0)
        {
            if(ctr is TitledPanel)//调整嵌套层数
            {
                TitledPanel tp = ctr as TitledPanel;
                tp.nestedLevel=this.nestedLevel + 1;
            }


            List<Control> list = this.Get_Fathers();// 初始化父类
            list.Insert(0, this);
            list.Remove(list.Last());
            int heightAdd;                           //确定高度

            ctr.Location = new Point(Padding.Left,Padding.Top);
            heightAdd= ctr.Height + Padding.Top + Padding.Bottom;


            Height += heightAdd;                //调整父控件
            Width += Padding.Right+Padding.Left+10;
            RowCount++;
            foreach (Control ctrl in this.Controls)
            {
                int currentRow = this.GetRow(ctrl);
                if (currentRow >= index)
                {
                    this.SetRow(ctrl, currentRow + 1);
                }
            }
            Controls.Add(ctr, 0, index);
            RowStyles.Insert(index, new RowStyle(SizeType.Absolute, heightAdd ));

            foreach (Control tp in list)            //调整控件树
            {
                (tp.Parent as TitledPanel).RowStyles[tp.Get_Index()].Height += heightAdd;
                (tp.Parent as TitledPanel).Height += heightAdd;
                (tp.Parent as TitledPanel).Width += Padding.Right+Padding.Left+3;
            }
            ctr.Visible = true;
        }
        public void Remove(Control ctr)
        {
            if (ctr is TitledPanel)//调整嵌套层数
            {
                TitledPanel tp = ctr as TitledPanel;
                tp.nestedLevel = 0;
            }



            int index= this.GetRow(ctr);
            List<Control> list = this.Get_Fathers();//初始化控件树
            list.Insert(0, this);
            list.Remove(list.Last());
            int heightMin;                                  //确定调整高度    

            heightMin= ctr.Height + Padding.Top + Padding.Bottom;                



            Controls.Remove(ctr);          //调整父控件
            this.BringToFront();
            RowCount--;
            Height -= heightMin;
            Width -= Padding.Right + Padding.Left;
            RowStyles.RemoveAt(index);
            foreach (Control ctrl in this.Controls)
            {
                int currentRow = this.GetRow(ctrl);
                if (currentRow > index)
                {
                    this.SetRow(ctrl, currentRow - 1);
                }
            }


            foreach (Control tp in list)                //调整空间书
            {
                (tp.Parent as TitledPanel).RowStyles[tp.Get_Index()].Height -= heightMin;
                (tp.Parent as TitledPanel).Height -= heightMin;
                (tp.Parent as TitledPanel).Width -= Padding.Right+Padding.Left+2;
            }




          
        }
        public int Point_Row(Point point)
        {
            int[] rowHeights = this.GetRowHeights();
            int currentY = 0;
            for (int i = 0; i < rowHeights.Length; i++)
            {
                // 如果当前点的 Y 坐标在当前行范围内，则返回行索引 i
                if (point.Y >= currentY && point.Y < currentY + rowHeights[i])
                {
                    return i;
                }
                currentY += rowHeights[i];
            }
            return -1;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            this.SetRoundedRegion(20);
            BackColor = Setting.ColorMap[text].NestedLevels[nestedLevel];
            //base.OnPaint(e);

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //var heights = this.GetRowHeights();
            //int width = Width - 1;

            //int y = heights[0]+Padding.Top;
            //Point lPoint;
            //Point rPoint;
            //for (int row = 1; row < this.RowCount; row++)
            //{


            //        lPoint = new Point(10, y);
            //        rPoint = new Point(width - 10, y);

            //    using (var pen = new Pen(Color.Black, BorderWidth))
            //    {
            //        e.Graphics.DrawLine(pen, lPoint,rPoint);
            //    }


            //    y += heights[row];
            //}

            //Rectangle rect = new Rectangle(0, 0, this.ClientSize.Width - BorderWidth, this.ClientSize.Height - BorderWidth);

            //e.Graphics.DrawRectangle(pen, rect);
        }
    }
    public class SegmentedInputBox : UserControl,IMyControl
    {
        public static int Num = 0;
        public ctrData data { set; get; } = new ctrData();
        public myTextBox[] Boxes;
        public myLabel[] Separators;
        public int boxWidth = 100;
        public Font font = Setting.Font;
        public Padding Padding = Setting.RegularMargin;
        public string text;
        public Color color;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen pen = new Pen(Color.Gray, 1))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {

                    Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    //e.Graphics.DrawRectangle(pen, rect);
                    // 填充矩形区域
                    e.Graphics.FillRectangle(brush, rect);
                }
            }

        }
        public string toString()
        {
            string info = "";
            for (int i = 0; i < Separators.Length; i++)
            {
                info += Separators[i].Text + "";
                if (i < Boxes.Length)
                {
                    info += Boxes[i].Text + " ";
                }

            }
            return info+"\n";
        }

        public SegmentedInputBox(string text,bool IsTitled=false)
        {
            SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);
            UpdateStyles();

            this.text = text;
            Height = Setting.RegularHeith;
            color = Setting.ColorMap[text].BoxColor;
            string[]infos = text.Split("_");
            int n = infos.Length;//Label个数
            int m =text.Count(c => c == '_');//分隔符个数
            Boxes = new myTextBox[m];
            Separators=new myLabel[n];
            int[]locations= new int[m];//文本框的位置
            List<int> sizes = new List<int>();
            foreach (string info in infos)//label大小
            {
                Size size = TextRenderer.MeasureText(info, font);
                sizes.Add(size.Width);
            }
            for (int i = 0; i < m; i++)
            {
                locations[i] =((i!=0)?(locations[i-1]+boxWidth+sizes[i]) : sizes[0])+Padding.Left;
            }


            for (int i = 0; i < Separators.Length; i++)
            {
                if(i<Boxes.Length)
                {
                    Boxes[i] = new myTextBox()
                    {
                        BackColor = color,
                        Font = font,
                        Size = new Size(boxWidth, Setting.RegularHeith - Padding.Top - Padding.Bottom),
                        Location = new Point(locations[i], Padding.Top),
                        TextAlign = HorizontalAlignment.Center,
                        BorderStyle = BorderStyle.None

                    };

                    this.Controls.Add(Boxes[i]);
                }

                Separators[i] = new myLabel()
                {
                    BackColor = color,
                    FlatStyle = FlatStyle.Flat,
                    Font = font,
                    Text = infos[i],
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Location = i == 0 ? new Point(Padding.Left, Padding.Top) : new Point(Boxes[i - 1].Right + Padding.Left, Padding.Top),
                };
                    
                    this.Controls.Add(Separators[i]);
                
            }
            int maxRight = 0; // 初始化最大右边界

            // 如果 list1 不为空，遍历其中的控件
            if (Boxes.Length != 0)
            {
                foreach (Control ctrl in Boxes)
                {
                    if (ctrl != null)
                        maxRight = Math.Max(maxRight, ctrl.Right);
                }
            }

            // 同理，遍历 list2
            if (Separators.Length!=0)
            {
                foreach (Control ctrl in Separators)
                {
                    if (ctrl != null)
                        maxRight = Math.Max(maxRight, ctrl.Right);
                }
            }
            Width = maxRight + Padding.Right;
            this.SetRoundedRegion(20);
        }
    }
}
