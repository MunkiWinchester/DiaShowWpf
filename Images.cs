using System.Windows.Media.Imaging;

namespace DiaShowWpf
{
    public class Images
    {
        public string Main { get; set; }
        public string Left { get; set; }
        public string Left1 { get; set; }
        public string Left2 { get; set; }
        public string Left3 { get; set; }
        public string Left4 { get; set; }
        public string Right { get; set; }
        public string Right1 { get; set; }
        public string Right2 { get; set; }
        public string Right3 { get; set; }
        public string Right4 { get; set; }

        public Images(string main, 
            string left, string left1, string left2, string left3, string left4,
            string right, string right1, string right2, string right3, string right4)
        {
            Main = main;
            Left = left;
            Left1 = left1;
            Left2 = left2;
            Left3 = left3;
            Left4 = left4;
            Right = right;
            Right1 = right1;
            Right2 = right2;
            Right3 = right3;
            Right4 = right4;
        }

        public Images()
        {
        }
    }
}