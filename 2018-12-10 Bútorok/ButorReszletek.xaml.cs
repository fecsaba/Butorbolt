using Butorok.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2018_12_10_Bútorok
{
    /// <summary>
    /// Új bútor rögzítése => üres ctor
    /// Bútor adatainak módosítása => ctor Model paraméterrel
    /// </summary>
    public partial class ButorReszletek : Window
    {
        private int? id = null;  //null érték esetén új rögzítés, különben módosítás
        public Butor Model { get; private set; }

        public ButorReszletek()
        {
            InitializeComponent();
            cboAlapanyag.ItemsSource = Alapanyag.Select();
            cboAlapanyag.SelectedValuePath = "Id";
            cboAlapanyag.DisplayMemberPath = "Megnevezes";
        }

        public ButorReszletek(Butor model) : this()
        {
            id = model.Id;
            txtMegnevezes.Text = model.Megnevezes;
            cboAlapanyag.SelectedValue = model.AlapanyagId;
            txtMeret.Text = model.Meret;
            txtAr.Text = model.Ar.ToString();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (kotelezoMezoEllenorzes())
            {
                try
                {
                    Model = new Butor()
                    {
                        Megnevezes = txtMegnevezes.Text.Trim(),
                        Meret = txtMeret.Text.Trim(),
                        AlapanyagId = (int)cboAlapanyag.SelectedValue,
                        Ar = txtAr.Text == "" ? null : (decimal?)Convert.ToDecimal(txtAr.Text)
                    };
                    if (id == null)  //új bútor
                    {
                        Model = Butor.Insert(Model);
                    }
                    else
                    {
                        Model.Id = (int)id;
                        Model = Butor.Update(Model);
                    }
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    Model = null;
                    MessageBox.Show(ex.Message, "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool kotelezoMezoEllenorzes()
        {
            bool rendben = true;
            if (txtMegnevezes.Text.Trim() == "")
            {
                rendben = false;
                txtMegnevezes.BorderBrush = Brushes.Red;
            }
            if (cboAlapanyag.SelectedValue == null)
            {
                rendben = false;
                //TODO: színezés, jelzés
                cboAlapanyag.IsDropDownOpen = true;
                grdComboboxHatter.Fill = Brushes.Red;
            }
            if (txtAr.Text != "" && !decimal.TryParse(txtAr.Text, out decimal temp))
            {
                rendben = false;
                txtAr.BorderBrush = Brushes.Red;
            }
            return rendben;
        }

        private void btnMegsem_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).BorderBrush = Brushes.Silver;
        }

        private void CboAlapanyag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            grdComboboxHatter.Fill = Brushes.White;

        }
    }
}
