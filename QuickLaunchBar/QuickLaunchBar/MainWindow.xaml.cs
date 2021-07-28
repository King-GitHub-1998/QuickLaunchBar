using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickLaunchBar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 静态属性
        private Rect desktopWorkingArea;
        private System.Windows.Point anchorPoint;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Width / 1.5;
            this.Left = desktopWorkingArea.Width/2 - (this.Width/2);
            this.Top = desktopWorkingArea.Height  - (this.Height / 2);
        }
    }
}
