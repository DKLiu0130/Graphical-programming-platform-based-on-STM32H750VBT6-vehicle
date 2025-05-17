using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public static class Animator
    {
        public static void AnimatePanel(Control control, Size targetSize, Point targetLocation, int duration = 150)
        {
            const int steps = 20;
            Size startSize = control.Size;
            Point startLocation = control.Location;

            float stepWidth = (targetSize.Width - startSize.Width) / (float)steps;
            float stepHeight = (targetSize.Height - startSize.Height) / (float)steps;
            float stepX = (targetLocation.X - startLocation.X) / (float)steps;
            float stepY = (targetLocation.Y - startLocation.Y) / (float)steps;

            int interval = duration / steps;
            int currentStep = 0;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = interval };
            timer.Tick += (s, e) =>
            {
                if (currentStep >= steps)
                {
                    control.Size = targetSize;
                    control.Location = targetLocation;
                    timer.Stop();
                    timer.Dispose();
                }
                else
                {
                    control.Size = new Size(
                        startSize.Width + (int)(stepWidth * currentStep),
                        startSize.Height + (int)(stepHeight * currentStep));
                    control.Location = new Point(
                        startLocation.X + (int)(stepX * currentStep),
                        startLocation.Y + (int)(stepY * currentStep));
                    currentStep++;
                }
            };
            timer.Start();
        }

        public static void AnimateForm(Form form, Size targetSize, int duration = 150)
        {
            const int steps = 20;
            Size startSize = form.Size;

            float stepWidth = (targetSize.Width - startSize.Width) / (float)steps;
            float stepHeight = (targetSize.Height - startSize.Height) / (float)steps;

            int interval = duration / steps;
            int currentStep = 0;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = interval };
            timer.Tick += (s, e) =>
            {
                if (currentStep >= steps)
                {
                    form.Size = targetSize;
                    timer.Stop();
                    timer.Dispose();
                }
                else
                {
                    form.Size = new Size(
                        startSize.Width + (int)(stepWidth * currentStep),
                        startSize.Height + (int)(stepHeight * currentStep));
                    currentStep++;
                }
            };
            timer.Start();
        }
    }

}
