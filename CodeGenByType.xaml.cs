using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
    /// Interaction logic for CodeGenByType.xaml
    /// </summary>
    public partial class CodeGenByType : UserControl
    {
        public CodeGenByType()
        {
            InitializeComponent();
        }

        private void INSERT_Click(object sender, RoutedEventArgs e)
        {
            var gen = new CodeGenSQL_INSERT();
            textOut.Text = gen.Generate(DataContext as Type);
        }

        private void SELECT_Click(object sender, RoutedEventArgs e)
        {
            var gen = new CodeGenSQL_SELECT();
            textOut.Text = gen.Generate(DataContext as Type);
        }

        private void UPDATE_Click(object sender, RoutedEventArgs e)
        {
            var gen = new CodeGenSQL_UPDATE();
            textOut.Text = gen.Generate(DataContext as Type);
        }
    }
}
