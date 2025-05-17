using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace WinFormsApp1
{
    static class Setting
    {
        static public Font Font = new Font("JetBrains Mono", 14, FontStyle.Regular);
        static public int RegularHeith = 50;
        static public readonly Padding RegularMargin = new Padding(5, 5, 5, 5);
        static public int RegularWidth = 600;
        static public Color RegularColor = Color.FromArgb(247, 248, 250);
        static public Color BoxColor = Color.FromArgb(247, 248, 250);
        static public Color TitledBoxColor= Color.FromArgb(212, 226, 255);
        static public Color errcolor = Color.FromArgb(255,0,0);
        public class TypeData
        {
            public string Name;//名字
            public bool IsTable;//是否为表
            public int AtrNum;//参数

        }
        static public Dictionary<string, TypeData> InstrToTextMap = new Dictionary<string, TypeData>
        {
            {"if",new TypeData(){Name="如果_",IsTable=true,AtrNum=0} },
            { "while",new TypeData(){Name=" 若_成立则重复执行",IsTable=true,AtrNum=0}},
            { "for",new TypeData(){Name=" 重复执行_次",IsTable=true,AtrNum=0}},
            {"goline" ,new TypeData(){Name="向前走_cm",IsTable=false,AtrNum=1} },
            {"turn",new TypeData(){Name="向左旋转_°",IsTable=false,AtrNum=1} },
            {"break",new TypeData(){Name="中断",IsTable=false,AtrNum=0} },
            {"circle",new TypeData(){Name="以左侧_cm为圆心旋转_°",IsTable=false,AtrNum=2  } },
            {"else",new TypeData(){Name="否则",IsTable=false,AtrNum=0} },
            {"let",new TypeData(){Name="令_为_",IsTable=false,AtrNum=2 } },
        };
        static public Dictionary<string, string> TextToInstr = new Dictionary<string, string> {
            {"如果_","if" },
            {"若_成立则重复执行","while" },
            {"重复执行_次","for" },
            {"令_为_","let" },
            {"向前走_cm","goline" },
            {"向左旋转_°","turn" },
            {"以左侧_cm为圆心旋转_°","circle" },
            {"中断","break" },
            {"否则","else" },
            {"需要修正的积木块","error" },
        };
        public class BlockColorSet
        {
            public Color BoxColor { get; set; }
            public List<Color>? NestedLevels { get; set; }
        }

        public static Dictionary<string, BlockColorSet> ColorMap = new Dictionary<string, BlockColorSet>
        {
        { "如果_", new BlockColorSet {
            BoxColor = Color.FromArgb(253,225,28),//深黄色
            NestedLevels = new List<Color>
            {
                    Color.FromArgb(255,153,22),//*0.9 
                    Color.FromArgb(255,136,20),//*0.8 
                    Color.FromArgb(255,119,17),
                     Color.FromArgb(255,102,15),//*0.6 
                     Color.FromArgb(255,85,12),
                }
            }
        },
        { "若_成立则重复执行", new BlockColorSet {
            BoxColor =  Color.FromArgb(253,225,28),//深黄色
            NestedLevels = new List<Color>
            {
                Color.FromArgb(255,171,25),//橙色
                Color.FromArgb(255,153,22),
                     Color.FromArgb(255,136,20),//*0.8 
                     Color.FromArgb(255,119,17),
                     Color.FromArgb(255,102,15),
                     Color.FromArgb(255,85,12),
            }
        }},
        { "重复执行_次", new BlockColorSet {
            BoxColor = Color.FromArgb(253,225,28),//深黄色
            NestedLevels = new List<Color>
            {
                 Color.FromArgb(255,171,25),//橙色
                 Color.FromArgb(255,153,22),
                 Color.FromArgb(255,136,20),
                 Color.FromArgb(255,119,17),
                 Color.FromArgb(255,102,15),
                 Color.FromArgb(255,85,12),
            }
        }},
        { "否则", new BlockColorSet {

                BoxColor = Color.FromArgb(253,225,28),//深黄色

            NestedLevels = new List<Color>
            {       Color.FromArgb(255,171,25),//橙色
                    Color.FromArgb(255,153,22),
                    Color.FromArgb(255,136,20),
                    Color.FromArgb(255,119,17),
                    Color.FromArgb(255,102,15),
                    Color.FromArgb(255,85,12),

            }
        }},
        { "需要修正的积木块", new BlockColorSet {

                BoxColor = Color.FromArgb(255,0,0),
        }},
        { "向前走_cm", new BlockColorSet {

                BoxColor = Color.FromArgb(76,151,255),
        }},
        { "向左旋转_°", new BlockColorSet {
                BoxColor = Color.FromArgb(76,151,255),
        }},
        { "以左侧_cm为圆心旋转_°", new BlockColorSet {
                BoxColor = Color.FromArgb(76,151,255),
        }},
        { "令_为_", new BlockColorSet {
                BoxColor = Color.FromArgb(62,191,70),
        }},
        { "中断", new BlockColorSet {
                BoxColor = Color.FromArgb(255,0,0),
        }},
        { "程序入口", new BlockColorSet {
                BoxColor =                 Color.FromArgb(253,225,28),//深黄色
                NestedLevels = new List<Color>
            {
                Color.FromArgb(247,252,126),

            }

        }},
    };
        

    }
}
