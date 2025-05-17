using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


// 尺寸位置更新工具类（UI放大时用到）
namespace WinFormsApp1
{
    public class SizeAndLocationSwitcher
    {
        private readonly Form _form;
        private readonly Dictionary<Control, (Size originalSize, Point originalLocation)> _originalStates = new();
        private readonly Dictionary<Control, (Size expandedSize, Point expandedLocation)> _expandedStates = new();
        private Size _originalFormSize;
        private Size _expandedFormSize;
        private bool _isExpanded = false;

        public SizeAndLocationSwitcher(Form form)
        {
            _form = form;
        }

        public void RegisterControl(Control control, Size expandedSize, Point expandedLocation)
        {
            if (!_originalStates.ContainsKey(control))
            {
                _originalStates[control] = (control.Size, control.Location);
                _expandedStates[control] = (expandedSize, expandedLocation);
            }
        }

        public void SetExpandedFormSize(Size size)
        {
            _originalFormSize = _form.Size;
            _expandedFormSize = size;
        }

        public void Toggle()
        {
            if (!_isExpanded)
            {
                if (_originalFormSize == Size.Empty)
                    _originalFormSize = _form.Size;

                _form.WindowState = FormWindowState.Normal;
                _form.Size = _expandedFormSize;

                foreach (var kvp in _expandedStates)
                {
                    kvp.Key.Size = kvp.Value.expandedSize;
                    kvp.Key.Location = kvp.Value.expandedLocation;
                }

                _form.StartPosition = FormStartPosition.Manual;
                _form.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - _form.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - _form.Height) / 2
                );
            }
            else
            {
                _form.WindowState = FormWindowState.Normal;
                _form.Size = _originalFormSize;

                foreach (var kvp in _originalStates)
                {
                    kvp.Key.Size = kvp.Value.originalSize;
                    kvp.Key.Location = kvp.Value.originalLocation;
                }

                _form.StartPosition = FormStartPosition.Manual;
                _form.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - _form.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - _form.Height) / 2
                );
            }

            _isExpanded = !_isExpanded;
        }

        public void UpdateExpandedState(Control control, Size expandedSize, Point location)
        {
            if (_expandedStates.ContainsKey(control))
            {
                _expandedStates[control] = (expandedSize, location);
            }
        }
    }
}