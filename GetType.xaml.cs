using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
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

namespace ReflectionGen
{
    /// <summary>
    /// Interaction logic for GetType.xaml
    /// </summary>
    public partial class GetType : UserControl
    {
        public GetType()
        {
            InitializeComponent();
            
            Bind();
        }

        public string Directory
        {
            get { return Properties.Settings.Default.Directory; }
            set { Properties.Settings.Default.Directory = value; }
        }

        private void Bind()
        {
            files.ItemsSource = System.IO.Directory.GetFileSystemEntries(Directory).Select(x => new FileInfo(x));

        }

        public void Files_OnSelectionChanged(object c, SelectionChangedEventArgs args)
        {
            if (files.SelectedItem != null)
            {
                var path = files.SelectedItem as System.IO.FileInfo;

                Assembly assembly = Assembly.LoadFile(path.FullName);
                types.ItemsSource = assembly.GetTypes();    
            }
            
        }


        public void Types_OnSelectionChanged(object c, SelectionChangedEventArgs args)
        {
            if (types.SelectedItem != null)
            {
                var path = types.SelectedItem as Type;
                if (path != null)
                {
                    singleType.ItemsSource = path.GetProperties();    
                }
            }
        }

        private void SelectType_OnClick(object sender, RoutedEventArgs e)
        {
            var gen = new GeneralWindow();
            var code = new CodeGenByType();

            gen.Include(code);
            code.DataContext = types.SelectedItem;
            gen.ShowDialog();
        }
    }
}
