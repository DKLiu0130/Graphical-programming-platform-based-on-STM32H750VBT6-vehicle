using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class ctrData
    {
        public Point location { get; set; }
        public bool isDragging { get; set; } = false;

    }
    //用于纪录tablepanel的信息
    public class tableData : ctrData
    {
        public tableData(int col = 0, int row = 0) : base()
        {
            this.col = col;
            this.row = row;
        }

        public int col { set; get; }
        public int row { set; get; }

    }
    //用于创造控件的按钮数据
    public class creBtnData : ctrData
    {
        //描述控件类型
        enum ctrType
        {
            MaskedTextBook
        }
        public string data { set; get; }
    }
    

}
