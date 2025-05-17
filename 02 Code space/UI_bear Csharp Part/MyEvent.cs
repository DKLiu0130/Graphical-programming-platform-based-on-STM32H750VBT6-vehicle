using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Telerik.WinControls.UI;
namespace WinFormsApp1
{
    public partial class RadMainForm
    {
        
        private void Block_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;
            MessageBox.Show(btn.Text);
            Create_Panel( btn.Text);
        }

        private void Text_Click(object sender, EventArgs e)
        {
            RadButton btn = sender as RadButton;

            Create_Sib(btn.Text);
        }


        private SegmentedInputBox Create_Sib( string text)
        {

            
            SegmentedInputBox sib=new SegmentedInputBox(text);
            Point location = this.PointToClient(DragPanel.PointToScreen(new Point(500, 235)));
            sib.Location = location;

            sib.ContextMenuStrip =contextMenuStrip1;
            sib.Name = text + SegmentedInputBox.Num++.ToString();
            
            
            Mysub_Drag(sib);

            
            this.Controls.Add(sib);
            sib.BringToFront();
            return sib;


        }
        private TitledPanel Create_Panel( string text)
        {
            if(text=="程序入口"&&this.enter!=null)
            {
                MessageBox.Show("程序入口已存在！");
                return null;
            }
            TitledPanel tp = new TitledPanel(text);
            Point location = this.PointToClient(DragPanel.PointToScreen(new Point(500, 100)));
            tp.Location = location;
            tp.ContextMenuStrip = contextMenuStrip1;
            tp.Name = text + TitledPanel.Num++.ToString();
            if (text== "程序入口")
            {
                this.enter = tp;
            }

            Mysub_Drag(tp);
            this.Controls.Add(tp);
            this.tps.Add(tp);
            tp.BringToFront();
            return tp;
        }

        private void Mysub_Drag(Control myctr)
        {
            myctr.MouseDown += new MouseEventHandler(Myctr_MouseDown);
            myctr.MouseMove += new MouseEventHandler(Myctr_MouseMove);
            myctr.MouseUp += new MouseEventHandler(Myctr_MouseUp);
            if (myctr.Controls.Count != 0) {
                foreach (Control ctr in myctr.Controls)
                {
                    Mysub_Drag(ctr);
                }
            }
        }
        private void Myctr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragable= DragPanel.Get_Pos();
                Control ctr = sender as Control;
                if (ctr.Parent is SegmentedInputBox && !(ctr is myTextBox) || (ctr.Parent is TitledPanel && (ctr.Parent as TitledPanel).GetRow(ctr) == 0))
                {
                    Myctr_MouseDown((sender as Control).Parent, e);
                }
                else
                {
                    if (ctr is myTextBox)
                    {
                        return;
                    }
                    if (ctr.Parent is TitledPanel)
                    {
                        TitledPanel tp = ctr.Parent as TitledPanel;
                        if (tp.GetRow(ctr) != 0)
                        {
                            tp.Remove(ctr);
                            this.Controls.Add(ctr);
                            ctr.BringToFront();
                        }
                    }
                    IMyControl ic = sender as IMyControl;
                    ic.data.isDragging = true;//记录轨迹
                    ic.data.location = e.Location;

                }
                
            }
            
        }
        private void Myctr_MouseMove(object sender, MouseEventArgs e)
        {
            
            Control ctr = sender as Control;
            if (ctr.Parent is SegmentedInputBox && !(ctr is myTextBox) || (ctr.Parent is TitledPanel && (ctr.Parent as TitledPanel).GetRow(ctr) == 0))
            {
                Myctr_MouseMove((sender as Control).Parent, e);
            }
            else if (sender != null)
            {
                if (ctr is myTextBox)
                {
                    return;
                }
                IMyControl ic = sender as IMyControl;
                if (ic.data.isDragging)
                {

                    // 计算希望移动到的新位置（基于当前控件位置和鼠标移动的差值）
                    int desiredX = ctr.Location.X + (e.X - ic.data.location.X);
                    int desiredY = ctr.Location.Y + (e.Y - ic.data.location.Y);

                    // 限制 newX 和 newY 使得控件不会超出 Dragable 的范围
                    // 假设 Dragable 是一个 Rectangle 类型，代表允许控件移动的区域
                    int clampedX = Math.Max(Dragable.Left, Math.Min(desiredX, Dragable.Right - ctr.Width));
                    int clampedY = Math.Max(Dragable.Top, Math.Min(desiredY, Dragable.Bottom - ctr.Height));
                    //MessageBox.Show(Dragable.Left.ToString()+' '+Dragable.Right.ToString());

                    //MessageBox.Show("begin");
                    ctr.Location = new Point(clampedX, clampedY);
                    //MessageBox.Show("end");
                    Point p = new Point(e.X, e.Y);
                    p = p.Get_Pos(ctr, this);
                    if (p.X >= Dragable.Right)
                    {

                    }
                }
            }
        }
        private void Myctr_MouseUp(object sender, MouseEventArgs e)
        {
            Control ctr = sender as Control;
            if (e.Button == MouseButtons.Left)
            {
                if (ctr.Parent is SegmentedInputBox && !(ctr is myTextBox) || (ctr.Parent is TitledPanel && (ctr.Parent as TitledPanel).GetRow(ctr) == 0))
                {
                    Myctr_MouseUp((sender as Control).Parent, e);
                }
                else if (sender != null)
                {
                    if (ctr is myTextBox)
                    {
                        return;
                    }
                    Point screenPoint = ctr.PointToScreen(new Point(e.X, e.Y));
                    TitledPanel tmp = null;
                    int index = -1;
                    foreach (TitledPanel tp in tps)
                    {
                        Point clientPoint = tp.PointToClient(screenPoint);
                        // 如果释放点在 TitledPanel 的客户区内
                        if (tp.ClientRectangle.Contains(clientPoint) && tp != ctr && !tp.Get_Fathers().Contains(ctr))
                        {
                            if (index == -1 || tp.Get_Fathers().Contains(tmp))
                            {
                                index = tp.Point_Row(clientPoint);
                                tmp = tp;
                            }
                        }
                    }
                    if (index != -1&&index!=0)
                    {

                        if (ctr != this.enter) {
                            //ctr.Visible = false;
                            tmp.Add(ctr, index); }
                    }
                    IMyControl ic = sender as IMyControl;
                    ic.data.isDragging = false;

                    
                }
            }
        }
        


    }
}
