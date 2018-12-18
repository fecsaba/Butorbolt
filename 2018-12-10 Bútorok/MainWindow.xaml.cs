using Butorok.Data;
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

namespace _2018_12_10_Bútorok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Butor> butorok = new List<Butor>();

        public MainWindow()
        {
            InitializeComponent();
            cboAlapanyagKeres.ItemsSource = Alapanyag.Select();
            cboAlapanyagKeres.SelectedValuePath = "Id";
            cboAlapanyagKeres.DisplayMemberPath = "Megnevezes";

            grdLista.ItemsSource = butorok;
        }

        private void btnKereses_Click(object sender, RoutedEventArgs e)
        {
            butorok = Butor.Select(txtMegnevezesKeres.Text, 
                                   (int?)cboAlapanyagKeres.SelectedValue);
            grdLista.ItemsSource = butorok;
            grdLista.Items.Refresh();
        }

        private void btnUj_Click(object sender, RoutedEventArgs e)
        {
            var ujButor = new ButorReszletek();
            if (ujButor.ShowDialog() == true)
            {
                butorok.Add(ujButor.Model);
                grdLista.Items.Refresh();
            }
        }

        private void btnModositas_Click(object sender, RoutedEventArgs e)
        {
            if (grdLista.SelectedItem != null)
            {
                var model = (Butor)grdLista.SelectedItem;
                var butorModosit = new ButorReszletek(model);
                if (butorModosit.ShowDialog() == true)
                {
                    int index = butorok.IndexOf(model);
                    butorok[index] = butorModosit.Model;
                    grdLista.Items.Refresh();
                }
            }
        }

        private void btnTorles_Click(object sender, RoutedEventArgs e)
        {
            if (grdLista.SelectedItem != null)
            {
                var model = (Butor)grdLista.SelectedItem;
                var kerdes = string.Format("Biztos szeretné kitörölni a {0} nevű bútort?",
                                           model.Megnevezes);
                if (MessageBox.Show(kerdes, "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Butor.Delete(model);
                        butorok.Remove(model);
                        grdLista.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        
    }
}
