using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    //-----拖拽-----
    //有两套，主窗口的拖拽与panel拖拽

    // 主窗口拖拽，采用的方案和panel不一样，因为panel在button_panel里面
    public class DraggableController_MainForm
    {
        private Control targetControl;
        private Control dragHandle;
        private Point lastMousePos;
        private bool dragging = false;

        public DraggableController_MainForm(Control targetControl, Control dragHandle)
        {
            this.targetControl = targetControl;
            this.dragHandle = dragHandle;

            // 设置拖拽控件的鼠标样式为手型
            dragHandle.Cursor = Cursors.Hand;

            dragHandle.MouseDown += DragHandle_MouseDown;
            dragHandle.MouseMove += DragHandle_MouseMove;
            dragHandle.MouseUp += DragHandle_MouseUp;
            dragHandle.MouseLeave += DragHandle_MouseLeave;
        }

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                lastMousePos = Control.MousePosition;

                // 捕获鼠标，确保即使在控件外也能接收鼠标移动
                dragHandle.Capture = true;
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            Point currentMousePos = Control.MousePosition;
            int deltaX = currentMousePos.X - lastMousePos.X;
            int deltaY = currentMousePos.Y - lastMousePos.Y;

            // 计算新位置
            Point newLocation = new Point(
                targetControl.Left + deltaX,
                targetControl.Top + deltaY
            );

            // 应用新位置
            targetControl.Location = newLocation;

            // 更新最后鼠标位置
            lastMousePos = currentMousePos;

            // 强制立即重绘
            targetControl.Update();
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EndDragging();
            }
        }

        private void DragHandle_MouseLeave(object sender, EventArgs e)
        {
            if (dragging && !dragHandle.ClientRectangle.Contains(dragHandle.PointToClient(Control.MousePosition)))
            {
                EndDragging();
            }
        }

        private void EndDragging()
        {
            dragging = false;
            dragHandle.Capture = false;
        }
    }


    //两个panel拖拽的完美方法，唯一不足是移动会有顿感
    public class DraggableController_Panel_perfect
    {
        private Control targetControl;  // 被拖拽的控件（Panel）
        private Control dragHandle;     // 拖拽手柄
        private Control parentControl;  // 父容器引用
        private Point lastMousePos;     // 上次鼠标位置
        private bool dragging = false;  // 拖拽状态
        private Bitmap dragBuffer;      // 临时缓冲区
        private bool useBuffer = false; // 是否使用缓冲区绘制
        private bool suppressParentPaint = false; // 控制父 Panel 的 Paint
        private DateTime lastUpdate = DateTime.MinValue; // 限制更新频率

        public DraggableController_Panel_perfect(Control targetControl, Control dragHandle)
        {
            this.targetControl = targetControl;
            this.dragHandle = dragHandle;
            this.parentControl = targetControl.Parent;

            // 设置拖拽手柄的鼠标样式为手型
            dragHandle.Cursor = Cursors.Hand;

            // 注册事件
            dragHandle.MouseDown += DragHandle_MouseDown;
            dragHandle.MouseMove += DragHandle_MouseMove;
            dragHandle.MouseUp += DragHandle_MouseUp;
            dragHandle.MouseLeave += DragHandle_MouseLeave;
            targetControl.Paint += TargetControl_Paint;

            // 为父 Panel 注册 Paint 事件
            if (parentControl is Panel parentPanel)
            {
                parentPanel.Paint += ParentControl_Paint;
            }
        }

        private void DragHandle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                lastMousePos = Control.MousePosition;
                dragHandle.Capture = true; // 捕获鼠标
                parentControl.SuspendLayout(); // 暂停父容器布局

                // 创建缓冲区
                dragBuffer = new Bitmap(targetControl.Width, targetControl.Height);
                targetControl.DrawToBitmap(dragBuffer, new Rectangle(0, 0, targetControl.Width, targetControl.Height));
                useBuffer = true;
                suppressParentPaint = true; // 暂停父 Panel 的自定义绘制

                // 隐藏子控件以减少重绘冲突
                foreach (Control ctrl in targetControl.Controls)
                {
                    ctrl.Visible = false;
                }
            }
        }

        private void DragHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            // 限制更新频率（~60fps）
            if ((DateTime.Now - lastUpdate).TotalMilliseconds < 16) return;

            Point currentMousePos = Control.MousePosition;
            int deltaX = currentMousePos.X - lastMousePos.X;
            int deltaY = currentMousePos.Y - lastMousePos.Y;

            // 记录旧位置（用于清除残影）
            Rectangle oldRegion = targetControl.Bounds;

            // 计算新位置
            Point newLocation = new Point(
                targetControl.Left + deltaX,
                targetControl.Top + deltaY
            );

            // 边界检查
            newLocation.X = Math.Max(0, Math.Min(newLocation.X, parentControl.ClientSize.Width - targetControl.Width));
            newLocation.Y = Math.Max(0, Math.Min(newLocation.Y, parentControl.ClientSize.Height - targetControl.Height));

            // 应用新位置
            targetControl.Location = newLocation;

            // 更新最后鼠标位置
            lastMousePos = currentMousePos;

            // 清除旧位置残影：重绘父 Panel 的受影响区域
            Rectangle newRegion = targetControl.Bounds;
            Rectangle unionRegion = Rectangle.Union(oldRegion, newRegion);
            unionRegion.Inflate(10, 10); // 扩展区域以确保覆盖所有残影
            parentControl.Invalidate(unionRegion);
            parentControl.Update(); // 强制父 Panel 更新

            // 触发子 Panel 重绘（使用缓冲区）
            targetControl.Refresh(); // 强制立即重绘

            lastUpdate = DateTime.Now;
        }

        private void DragHandle_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EndDragging();
            }
        }

        private void DragHandle_MouseLeave(object sender, EventArgs e)
        {
            if (dragging && !dragHandle.ClientRectangle.Contains(dragHandle.PointToClient(Control.MousePosition)))
            {
                EndDragging();
            }
        }

        private void EndDragging()
        {
            dragging = false;
            dragHandle.Capture = false;
            parentControl.ResumeLayout();
            useBuffer = false;
            suppressParentPaint = false; // 恢复父 Panel 的绘制

            // 恢复子控件可见性
            foreach (Control ctrl in targetControl.Controls)
            {
                ctrl.Visible = true;
            }

            // 清理缓冲区
            if (dragBuffer != null)
            {
                dragBuffer.Dispose();
                dragBuffer = null;
            }

            // 触发完整重绘
            targetControl.Invalidate(true); // 重绘 Panel 及其子控件
            parentControl.Invalidate(true); // 重绘父容器
            parentControl.Update();
        }

        private void TargetControl_Paint(object sender, PaintEventArgs e)
        {
            if (useBuffer && dragBuffer != null)
            {
                // 优化绘制性能
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;

                // 绘制缓冲区内容，确保覆盖整个 Panel
                e.Graphics.DrawImage(dragBuffer, new Rectangle(0, 0, targetControl.Width, targetControl.Height));
            }
        }

        private void ParentControl_Paint(object sender, PaintEventArgs e)
        {
            if (suppressParentPaint)
            {
                // 在拖拽过程中，仅绘制父 Panel 的背景
                e.Graphics.Clear(parentControl.BackColor);
            }
            // 否则，允许父 Panel 的正常 Paint 逻辑执行
            // 注意：实际的父 Paint 逻辑应由用户定义，这里仅提供钩子
        }

        //解绑!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!拖拽关系
        /// <summary>
        /// 解绑拖拽控制器，移除所有事件绑定并清理资源
        /// </summary>
        public void Dispose()
        {
            // 停止正在进行的拖拽
            if (dragging)
            {
                EndDragging();
            }

            // 移除拖拽手柄的事件绑定
            dragHandle.MouseDown -= DragHandle_MouseDown;
            dragHandle.MouseMove -= DragHandle_MouseMove;
            dragHandle.MouseUp -= DragHandle_MouseUp;
            dragHandle.MouseLeave -= DragHandle_MouseLeave;

            // 移除目标控件的事件绑定
            targetControl.Paint -= TargetControl_Paint;

            // 移除父控件的事件绑定
            if (parentControl is Panel parentPanel)
            {
                parentPanel.Paint -= ParentControl_Paint;
            }

            // 清理缓冲区
            if (dragBuffer != null)
            {
                dragBuffer.Dispose();
                dragBuffer = null;
            }

            // 恢复鼠标样式
            dragHandle.Cursor = Cursors.Default;

            // 清空引用
            targetControl = null;
            dragHandle = null;
            parentControl = null;
        }
    }
}


//panel拖拽弃案，没有重影，但是有父类遮挡，问题可能是父类paint
//public class DraggableController_Panel
//{
//    private Control targetControl;  // 被拖拽的控件
//    private Control dragHandle;     // 拖拽手柄
//    private Control parentControl;  // 父容器引用
//    private Point lastMousePos;     // 上次鼠标位置
//    private bool dragging = false;  // 拖拽状态

//    public DraggableController_Panel(Control targetControl, Control dragHandle)
//    {
//        this.targetControl = targetControl;
//        this.dragHandle = dragHandle;
//        this.parentControl = targetControl.Parent; // 获取父容器

//        // 设置拖拽手柄的鼠标样式为手型
//        dragHandle.Cursor = Cursors.Hand;

//        dragHandle.MouseDown += DragHandle_MouseDown;
//        dragHandle.MouseMove += DragHandle_MouseMove;
//        dragHandle.MouseUp += DragHandle_MouseUp;
//        dragHandle.MouseLeave += DragHandle_MouseLeave;
//    }

//    private void DragHandle_MouseDown(object sender, MouseEventArgs e)
//    {
//        if (e.Button == MouseButtons.Left)
//        {
//            dragging = true;
//            lastMousePos = Control.MousePosition;
//            dragHandle.Capture = true; // 捕获鼠标
//            parentControl.SuspendLayout(); // 暂停父容器布局
//        }
//    }

//    private void DragHandle_MouseMove(object sender, MouseEventArgs e)
//    {
//        if (!dragging) return;

//        Point currentMousePos = Control.MousePosition;
//        int deltaX = currentMousePos.X - lastMousePos.X;
//        int deltaY = currentMousePos.Y - lastMousePos.Y;

//        // 计算新位置
//        Point newLocation = new Point(
//            targetControl.Left + deltaX,
//            targetControl.Top + deltaY
//        );

//        // 应用新位置
//        targetControl.Location = newLocation;

//        // 更新最后鼠标位置
//        lastMousePos = currentMousePos;
//        // 注意：这里不再调用 Update()，依赖系统自动重绘
//    }

//    private void DragHandle_MouseUp(object sender, MouseEventArgs e)
//    {
//        if (e.Button == MouseButtons.Left)
//        {
//            EndDragging();
//        }
//    }

//    private void DragHandle_MouseLeave(object sender, EventArgs e)
//    {
//        if (dragging && !dragHandle.ClientRectangle.Contains(dragHandle.PointToClient(Control.MousePosition)))
//        {
//            EndDragging();
//        }
//    }

//    private void EndDragging()
//    {
//        dragging = false;
//        dragHandle.Capture = false;
//        parentControl.ResumeLayout(); // 恢复父容器布局
//        parentControl.Invalidate();   // 强制父容器重绘，清除旧位置图像
//    }
//}

