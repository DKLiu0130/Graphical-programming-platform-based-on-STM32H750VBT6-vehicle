using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public static class myCtrLib
    {
        public static Form mainForm;
       public static Rectangle Get_Pos(this Control obj,Control ctr=null)
        {
            // 将控件左上角的点转换为屏幕坐标
            Point screenPoint = obj.PointToScreen(Point.Empty);
            // 再将屏幕坐标转换为窗体客户区坐标
            Point formPoint = ctr==null?mainForm.PointToClient(screenPoint):ctr.PointToClient(screenPoint);
            // 返回以转换后的坐标为左上角，控件的尺寸为大小的矩形
            return new Rectangle(formPoint, obj.Size);
        }
        public static List<Control> Get_Fathers(this Control ctr)
        {
            List<Control> list = new List<Control>();
            while (ctr.Parent is TitledPanel)
            {
                list.Add(ctr.Parent);
                ctr = ctr.Parent;
            }
            return list;
        }
        public static int Get_Index(this Control ctr) {

            return (ctr.Parent as TitledPanel).GetRow(ctr);
        }
        public static Point Get_Pos(this Point p, Control ctrl, Form window) {
            Point screenPoint = ctrl.PointToScreen(p);

            Point formPoint = window.PointToClient(screenPoint);

            return formPoint;
        }
        public static void EnableSmoothRounded(this Control ctrl, int radius)
        {


            // 在 Resize 时重建 Region & 重绘
            void Rebuild(object s, EventArgs e)
            {
                // 先绘制 Region（Clip 区域）
                using (var path = BuildRoundedPath(ctrl.ClientRectangle, radius))
                    ctrl.Region = new Region(path);

                // 再触发重绘
                ctrl.Invalidate();
            }

            // Paint 时用抗锯齿填充圆角
            void PaintHandler(object s, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = BuildRoundedPath(ctrl.ClientRectangle, radius))
                using (var brush = new SolidBrush(ctrl.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }

            // 本地方法：生成圆角路径
            static GraphicsPath BuildRoundedPath(Rectangle rect, int r)
            {
                int d = r * 2;
                var path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseAllFigures();
                return path;
            }

            // 绑定事件
            ctrl.Resize += Rebuild;
            ctrl.Paint += PaintHandler;

            // 首次 setup
            Rebuild(ctrl, EventArgs.Empty);
        }

        public static void SetRoundedRegion(this Control ctrl, int radius)
        {
            var bounds = ctrl.ClientRectangle;
            var path = new GraphicsPath();
            int d = radius * 2;

            // 左上、右上、右下、左下 弧
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseAllFigures();

            ctrl.Region = new Region(path);
        }
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseAllFigures();
            return path;
        }
    }
}
