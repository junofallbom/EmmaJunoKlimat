
using EmmaJunoKlimat.Repositories;
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

namespace EmmaJunoKlimat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClimateRepository dbClimate = new ClimateRepository();

        Observer observer;
        Observation observation;
        Area area;
        Unit unit;
        Category category;
        
        List<Measurement> measurements = new List<Measurement>();
        List<MeasurementToList> measurementToLists;

        public MainWindow()
        {
            InitializeComponent();

            btnRemoveObs.IsEnabled = false;
            btnSaveNew.IsEnabled = false;
            btnSaveEdit.IsEnabled = false;
            rdbtnSummerSuit.IsEnabled = false;
            rdbtnWinterSuit.IsEnabled = false;

            var area = dbClimate.GetArea();
            lstObservations.ItemsSource = dbClimate.GetObservations();
            cmbCategories.ItemsSource = dbClimate.GetCategory();
            cmbAreas.ItemsSource = dbClimate.GetArea();
            lstObservers.ItemsSource = dbClimate.GetObservers();
            cmbUnit.ItemsSource = dbClimate.GetUnits();

            List<Category> cat = dbClimate.GetCategory();

            measurementToLists = new List<MeasurementToList>();
            rdbtnSummerSuit.Content = cat[16];
            rdbtnWinterSuit.Content = cat[15];
        }

        private void btnPresentObs_Click(object sender, RoutedEventArgs e)
        {
            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = dbClimate.GetObservers();
        }

        private void btnAddObs_Click(object sender, RoutedEventArgs e)
        {
            observer = new Observer();

            observer.Firstname = txtFirstNameNewObs.Text;
            observer.Lastname = txtLastNameNewObs.Text;

            dbClimate.AddObserver(observer);

            lstObservers.ItemsSource = dbClimate.GetObservers();
            lstObservers.Items.Refresh();

            ClearAll();
        }

        private void lstObservers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var test = CheckCategory();
            observer = (Observer)lstObservers.SelectedItem;
            btnRemoveObs.IsEnabled = true;
            btnSaveNew.IsEnabled = true;
            btnSaveEdit.IsEnabled = true;
            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = dbClimate.GetObservationsFromSpecificObserver(observer);

            measurementToLists = new List<MeasurementToList>();

            lstMeasurements.ItemsSource = null;
        }

        private void btnRemoveObs_Click(object sender, RoutedEventArgs e)
        {
          
            observer = (Observer)lstObservers.SelectedItem;
            bool observationMade = dbClimate.CheckObservations(observer);
            if (observationMade == true)
            {
                MessageBox.Show($"{observer} har genomfört klimatobservationer och kan därför inte raderas.");
            }
            else
            {   dbClimate.DeleteObserver(observer);
                MessageBox.Show($"{observer} har raderats från listan över observatörer.");
                
            }
            //UpdateUI();
            lstObservers.ItemsSource = null;
            lstObservers.ItemsSource = dbClimate.GetObservers();
            ClearAll();
        }

        private void btnMakeObervation_Click(object sender, RoutedEventArgs e)
        {
            observation = CreateObservation();

            dbClimate.MakeObservationWithTransaction(observation, measurements);

            measurements = new List<Measurement>();

            lstObservations.ItemsSource = null;
            lstObservations.ItemsSource = dbClimate.GetObservationsFromSpecificObserver(observer);
           
            ClearAll();
        }

        private void lstObservations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnSaveEdit.IsEnabled = true;
        }

        public Observation CreateObservation()
        {
            observer = (Observer)lstObservers.SelectedItem;
            area = (Area)cmbAreas.SelectedItem;

            Geolocation geolocationObservation;
            geolocationObservation = dbClimate.GetGeolocationById(area.Id);

            observation = new Observation
            {
                ObserverId = observer.Id,
                GeolocationId = geolocationObservation.Id,
                CurrentDate = DateTime.Today
            };
            return observation;
        }

        public List<MeasurementToList> SaveMeasurements()
        {
            Measurement measurement;
            category = CheckCategory();

            unit = (Unit)cmbUnit.SelectedItem;

            measurement = new Measurement
            {
                Value = int.Parse(txtValue.Text),
                CategoryID = category.Id
            };

            measurements.Add(measurement);

            category = (Category)cmbCategories.SelectedItem;
            var suit = CheckCategory();
          
            //Hämtar properties från MeasurementsToList-klassen och tilldelar värden
            MeasurementToList listMeash = new MeasurementToList
            {
                Value = measurement.Value,
                Category = category.Name,
                Unit = unit.Abbreviation,
                Suit = CheckSuit(suit.Name, category.Name)
            };

            measurementToLists.Add(listMeash);

            return measurementToLists;
        }

        //Om suit är exakt lika med kategori tex bear bear skrivs inte bear ut två gånger
        private string CheckSuit (string suit, string category)
        {
            if (suit == category)
            {
                return "";         
            }
            return suit;
        }

        private Category CheckCategory()
        {
            if (rdbtnSummerSuit.IsEnabled)
            {
                if (rdbtnSummerSuit.IsChecked == true)
                {
                    return (Category)rdbtnSummerSuit.Content;
                }
                else
                {
                    return (Category)rdbtnWinterSuit.Content;
                }
            }
            else return (Category)cmbCategories.SelectedItem;
        }

        private void btnSaveMeasurements_Click(object sender, RoutedEventArgs e)
        {
            lstMeasurements.ItemsSource = null;
            lstMeasurements.ItemsSource = SaveMeasurements();
            var getCategory = dbClimate.GetCategory();
        }
        
        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbCategories.SelectedItem == null)
            {

            }
            else
            {
                string selectedCategory = cmbCategories.SelectedItem.ToString();

                if (selectedCategory == "Mountain Grouse")
                {
                    rdbtnSummerSuit.IsEnabled = true;
                    rdbtnWinterSuit.IsEnabled = true;
                }
                else
                {
                    rdbtnSummerSuit.IsEnabled = false;
                    rdbtnWinterSuit.IsEnabled = false;
                    rdbtnSummerSuit.IsChecked = false;
                    rdbtnWinterSuit.IsChecked = false;
                }
            }

        }

        private void lstObservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Observation)lstObservations.SelectedItem) == null)
            {

            }
            else
            {
                int observationNr = ((Observation)lstObservations.SelectedItem).Id;

                measurements = dbClimate.GetMeasurementsFromObservation(observationNr);

                measurementToLists = new List<MeasurementToList>();

                foreach (var measurement in measurements)
                {
                    var tempcat = dbClimate.GetSpecificCategory(measurement.CategoryID);
                    var tempcat1 = dbClimate.GetSpecificCategory(tempcat.BaseCategoryID);
                    var tempUnit = dbClimate.GetSpecificUnit(tempcat.UnitId);
                    MeasurementToList meash = new MeasurementToList
                    {
                        Category = tempcat.Name,
                        Suit = tempcat1.ToString(),
                        Value = measurement.Value,
                        Unit = tempUnit.Abbreviation,
                        Id = measurement.Id
                    };
                    measurementToLists.Add(meash);
                }
                lstMeasurements.ItemsSource = null;
                lstMeasurements.ItemsSource = measurementToLists;
            }
        }

        private void btnUpdateCurrentMeash_Click(object sender, RoutedEventArgs e)
        {
            var measurement = (MeasurementToList)lstMeasurements.SelectedItem;

            var dbMeasurement = measurements.Find(x => x.Id == measurement.Id);

            dbMeasurement.Value = int.Parse(txtValue.Text);

            measurement.Value = int.Parse(txtValue.Text);

            int? observationNr = measurement.Id;

            lstMeasurements.ItemsSource = null;
            lstMeasurements.ItemsSource = measurementToLists;     
        }

        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            dbClimate.UpdateMeasurements(measurements);
            ClearAll();
        }

        private void ClearAll()
        {
            txtFirstNameNewObs.Clear();
            txtLastNameNewObs.Clear();
            cmbCategories.SelectedIndex = -1;
            rdbtnSummerSuit.IsChecked = false;
            rdbtnWinterSuit.IsChecked = false;
            txtValue.Clear();
            cmbUnit.SelectedIndex = -1;
            cmbAreas.SelectedIndex = -1;
            lstMeasurements.ItemsSource = null;
        }

        //private void UpdateUI()
        //{
        //    lstObservers.Items.Refresh();
        //    lstMeasurements.Items.Refresh();
        //    lstObservations.Items.Refresh();
        //}
    }
}
