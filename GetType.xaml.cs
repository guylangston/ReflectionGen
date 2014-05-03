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
using ReflectionGen.CodeGen;

namespace ReflectionGen
{
    public class TypeModel
    {
        public TypeModel(Type type)
        {
            this.type = type;
        }

        readonly Type type;

        public string NameSpace { get { return type.Namespace; } }
        public string Name { get { return TypeHelper.ToFriendlyCSharp(type); } }

        

        public Type GetData()
        {
            return type;
        }
    }

    public class PropModel
    {
        private readonly PropertyInfo prop;

        public PropModel(PropertyInfo prop)
        {
            this.prop = prop;
        }

        public string Name { get { return prop.Name; } }
        public string PropertyType { get { return TypeHelper.ToFriendlyCSharp(prop.PropertyType); } }
    }

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
                types.ItemsSource = assembly.GetTypes().OrderBy(x => x.Namespace).ThenBy(x=>x.Name).Select(x => new TypeModel(x));    
            }
            
        }


        public void Types_OnSelectionChanged(object c, SelectionChangedEventArgs args)
        {
            if (types.SelectedItem != null)
            {
                var path = types.SelectedItem as TypeModel;
                if (path != null)
                {
                    singleType.ItemsSource = path.GetData().GetProperties().Select(x=>new PropModel(x));    
                }
            }
        }

        private void SelectType_OnClick(object sender, RoutedEventArgs e)
        {
            var gen = new GeneralWindow();
            var code = new CodeGenByType();

            gen.Include(code);
            code.DataContext = (types.SelectedItem as TypeModel).GetData();
            gen.ShowDialog();
        }
    }
}
