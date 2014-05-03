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
using ReflectionGen.CodeGen;

namespace ReflectionGen
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CodeGenTest_OnClick(object sender, RoutedEventArgs e)
        {
            var gen = new GeneralWindow();
            var code = new CodeGenByType();

            gen.Include(code);
            code.DataContext = typeof (TestClass);
            gen.ShowDialog();
        }
    }
}
