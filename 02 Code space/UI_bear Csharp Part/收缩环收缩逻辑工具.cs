using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Svg;

namespace WinFormsApp1
{
    //左上方收缩
    public class CollapsibleDraggablePanel
    {
        private RadPanel targetPanel;
        private Control dragHandle;
        private DraggableController_Panel_perfect draggableController;
        private Size originalSize;
        private List<Control> originalChildren = new List<Control>();
        private bool isCollapsed = false;
        private Bitmap svgBitmap;
        private Point mouseDownLocation; // 记录鼠标按下位置
        private bool isDragging; // 标记是否拖拽
        private System.Windows.Forms.Timer animationTimer; // 动画定时器
        private Size startSize; // 动画起始尺寸
        private Size targetSize; // 动画目标尺寸
        private int animationSteps = 6; // 动画帧数（加快动画）
        private int currentStep = 0; // 当前帧

        //下面的很关键，当panel缩小后，能知道它此刻的尺寸，用于放大逻辑
        public Size ExpandedSize => originalSize.IsEmpty ? targetPanel.Size : originalSize;

        public CollapsibleDraggablePanel(RadPanel panel, Control handle, string svgFilePath)
        {
            targetPanel = panel;
            dragHandle = handle;
            originalSize = panel.Size;

            // 保存子控件
            foreach (Control c in panel.Controls)
            {
                originalChildren.Add(c);
            }

            // 加载SVG图标
            if (File.Exists(svgFilePath))
            {
                svgBitmap = RenderSvgToBitmap(svgFilePath, 32, 32);
            }

            // 设置自定义绘制
            targetPanel.Paint += TargetPanel_Paint;

            // 添加鼠标事件
            targetPanel.MouseDown += TargetPanel_MouseDown;
            targetPanel.MouseMove += TargetPanel_MouseMove;
            targetPanel.Click += TargetPanel_Click;

            // 初始化动画定时器
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 20; // 每20ms一帧，约50fps
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void TargetPanel_Paint(object sender, PaintEventArgs e)
        {
            if (isCollapsed && svgBitmap != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int iconSize = Math.Min(targetPanel.Width, targetPanel.Height) / 2;
                Rectangle iconRect = new Rectangle(
                    (targetPanel.Width - iconSize) / 2,
                    (targetPanel.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );

                g.DrawImage(svgBitmap, iconRect);
            }
        }

        private void TargetPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // 记录鼠标按下位置并重置拖拽状态
            mouseDownLocation = e.Location;
            isDragging = false;
        }

        private void TargetPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // 如果鼠标按下并移动超过5像素，标记为拖拽
            if (e.Button == MouseButtons.Left)
            {
                int dx = Math.Abs(e.X - mouseDownLocation.X);
                int dy = Math.Abs(e.Y - mouseDownLocation.Y);
                if (dx > 5 || dy > 5)
                {
                    isDragging = true;
                }
            }
        }

        private void TargetPanel_Click(object sender, EventArgs e)
        {
            if (isCollapsed && !isDragging)
            {
                // 双重检查鼠标位置
                Point currentMousePos = targetPanel.PointToClient(Cursor.Position);
                int dx = Math.Abs(currentMousePos.X - mouseDownLocation.X);
                int dy = Math.Abs(currentMousePos.Y - mouseDownLocation.Y);
                if (dx < 5 && dy < 5)
                {
                    ToggleCollapse();
                }
            }
        }

        public void ToggleCollapse()
        {
            if (!isCollapsed)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
        }

        private void Collapse()
        {
            isCollapsed = true;

            // 保存当前尺寸
            originalSize = targetPanel.Size;

            // 移除所有子控件
            foreach (var ctrl in originalChildren)
            {
                targetPanel.Controls.Remove(ctrl);
            }

            // 启动缩小动画
            startSize = targetPanel.Size;
            targetSize = new Size(110, 110);
            currentStep = 0;
            animationTimer.Start();

            // 缩小后才能整个为拖拽范围
            draggableController = new DraggableController_Panel_perfect(targetPanel, dragHandle);
        }

        private void Expand()
        {
            isCollapsed = false;

            // 仅启动放大动画，子控件在动画完成后恢复
            startSize = targetPanel.Size;
            targetSize = originalSize;
            currentStep = 0;
            animationTimer.Tag = "expand"; // 标记动画类型
            animationTimer.Start();

            // 放大后解绑拖拽整个区域的关系
            if (draggableController != null)
            {
                draggableController.Dispose();
                draggableController = null;
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentStep++;
            if (currentStep <= animationSteps)
            {
                // 使用线性插值计算当前尺寸
                float t = (float)currentStep / animationSteps;
                int width = (int)(startSize.Width + (targetSize.Width - startSize.Width) * t);
                int height = (int)(startSize.Height + (targetSize.Height - startSize.Height) * t);
                targetPanel.Size = new Size(width, height);
                targetPanel.Invalidate(); // 重绘
            }
            else
            {
                // 动画结束，设置最终尺寸
                targetPanel.Size = targetSize;
                if (animationTimer.Tag?.ToString() == "expand")
                {
                    // 恢复子控件
                    foreach (var ctrl in originalChildren)
                    {
                        if (!targetPanel.Controls.Contains(ctrl))
                        {
                            targetPanel.Controls.Add(ctrl);
                        }
                    }
                }
                targetPanel.Invalidate();
                animationTimer.Stop();
                animationTimer.Tag = null;
            }
        }

        // SVG 渲染方法
        private Bitmap RenderSvgToBitmap(string svgPath, int width, int height)
        {
            try
            {
                var svgDoc = SvgDocument.Open(svgPath);
                svgDoc.Width = width;
                svgDoc.Height = height;
                return svgDoc.Draw();
            }
            catch
            {
                return new Bitmap(width, height); // 错误时返回空 Bitmap
            }
        }
    }


    //右上方收缩
    public class CollapsibleDraggablePanel2
    {
        private RadPanel targetPanel;
        private Control dragHandle;
        private DraggableController_Panel_perfect draggableController;
        private Size originalSize;
        private Point originalLocation;
        private List<Control> originalChildren = new List<Control>();
        private bool isCollapsed = false;
        private Bitmap svgBitmap;
        private Point mouseDownLocation;
        private bool isDragging;
        private System.Windows.Forms.Timer animationTimer;
        private Size startSize;
        private Size targetSize;
        private Point startLocation;
        private Point targetLocation;
        private int animationSteps = 6;
        private int currentStep = 0;

        //同样的，公开此时尺寸
        public Size ExpandedSize => originalSize.IsEmpty ? targetPanel.Size : originalSize;

        public CollapsibleDraggablePanel2(RadPanel panel, Control handle, string svgFilePath)
        {
            targetPanel = panel;
            dragHandle = handle;
            originalSize = panel.Size;
            originalLocation = panel.Location;

            foreach (Control c in panel.Controls)
            {
                originalChildren.Add(c);
            }

            if (File.Exists(svgFilePath))
            {
                svgBitmap = RenderSvgToBitmap(svgFilePath, 32, 32);
            }

            targetPanel.Paint += TargetPanel_Paint;
            targetPanel.MouseDown += TargetPanel_MouseDown;
            targetPanel.MouseMove += TargetPanel_MouseMove;
            targetPanel.Click += TargetPanel_Click;

            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void TargetPanel_Paint(object sender, PaintEventArgs e)
        {
            if (isCollapsed && svgBitmap != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int iconSize = Math.Min(targetPanel.Width, targetPanel.Height) / 2;
                Rectangle iconRect = new Rectangle(
                    (targetPanel.Width - iconSize) / 2,
                    (targetPanel.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );

                g.DrawImage(svgBitmap, iconRect);
            }
        }

        private void TargetPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLocation = e.Location;
            isDragging = false;
        }

        private void TargetPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = Math.Abs(e.X - mouseDownLocation.X);
                int dy = Math.Abs(e.Y - mouseDownLocation.Y);
                if (dx > 5 || dy > 5)
                {
                    isDragging = true;
                }
            }
        }

        private void TargetPanel_Click(object sender, EventArgs e)
        {
            if (isCollapsed && !isDragging)
            {
                Point currentMousePos = targetPanel.PointToClient(Cursor.Position);
                int dx = Math.Abs(currentMousePos.X - mouseDownLocation.X);
                int dy = Math.Abs(currentMousePos.Y - mouseDownLocation.Y);
                if (dx < 5 && dy < 5)
                {
                    ToggleCollapse();
                }
            }
        }

        public void ToggleCollapse()
        {
            if (!isCollapsed)
                Collapse();
            else
                Expand();
        }

        private void Collapse()
        {
            isCollapsed = true;

            originalSize = targetPanel.Size;
            originalLocation = targetPanel.Location;

            foreach (var ctrl in originalChildren)
            {
                targetPanel.Controls.Remove(ctrl);
            }

            startSize = targetPanel.Size;
            startLocation = targetPanel.Location;

            targetSize = new Size(110, 110);
            targetLocation = new Point(
                startLocation.X + (startSize.Width - targetSize.Width), // 向右上角移动
                startLocation.Y
            );

            currentStep = 0;
            animationTimer.Tag = "collapse";
            animationTimer.Start();

            draggableController = new DraggableController_Panel_perfect(targetPanel, dragHandle);
        }

        private void Expand()
        {
            isCollapsed = false;

            startSize = targetPanel.Size;
            startLocation = targetPanel.Location;

            targetSize = originalSize;
            targetLocation = originalLocation;

            currentStep = 0;
            animationTimer.Tag = "expand";
            animationTimer.Start();

            if (draggableController != null)
            {
                draggableController.Dispose();
                draggableController = null;
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentStep++;
            if (currentStep <= animationSteps)
            {
                float t = (float)currentStep / animationSteps;

                int width = (int)(startSize.Width + (targetSize.Width - startSize.Width) * t);
                int height = (int)(startSize.Height + (targetSize.Height - startSize.Height) * t);

                int rightX = startLocation.X + startSize.Width;
                int x = rightX - width;
                int y = startLocation.Y; // Y 保持不变

                targetPanel.Size = new Size(width, height);
                targetPanel.Location = new Point(x, y);

                targetPanel.Invalidate();
            }
            else
            {
                targetPanel.Size = targetSize;

                int rightX = startLocation.X + startSize.Width;
                int x = rightX - targetSize.Width;
                int y = startLocation.Y;

                targetPanel.Location = new Point(x, y);

                if (animationTimer.Tag?.ToString() == "expand")
                {
                    foreach (var ctrl in originalChildren)
                    {
                        if (!targetPanel.Controls.Contains(ctrl))
                        {
                            targetPanel.Controls.Add(ctrl);
                        }
                    }
                }

                targetPanel.Invalidate();
                animationTimer.Stop();
                animationTimer.Tag = null;
            }
        }

        private Bitmap RenderSvgToBitmap(string svgPath, int width, int height)
        {
            try
            {
                var svgDoc = Svg.SvgDocument.Open(svgPath);
                svgDoc.Width = width;
                svgDoc.Height = height;
                return svgDoc.Draw();
            }
            catch
            {
                return new Bitmap(width, height);
            }
        }
    }
}
