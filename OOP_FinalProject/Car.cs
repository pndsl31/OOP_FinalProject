using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    /// <summary>
    /// represents a single car in the rental fleet
    /// </summary>
    internal class Car
    {
        private string _licensePlate = "";
        private string _manufacturer = "";
        private string _model = "";
        private int _year = 0;
        private string _status = "Available";
        private decimal _dailyRate = 0;

        /// <summary>
        /// initializes a new car with the basic info
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="year"></param>
        /// <param name="dailyRate"></param>
        public Car(string licensePlate, string manufacturer, string model, int year, decimal dailyRate)
        {
            _licensePlate = licensePlate;
            _manufacturer = manufacturer;
            _model = model;
            _year = year;
            _dailyRate = dailyRate;
            _status = "Available";
        }

        public string getLicensePlate() { return _licensePlate; }
        public string getManufacturer() { return _manufacturer; }
        public string getModel() { return _model; }
        public int getYear() { return _year; }
        public decimal getDailyRate() { return _dailyRate; }
        public string getStatus() { return _status; }
        public void setStatus(string status) { _status = status; }

        public string getDisplay()
        {
            return _licensePlate + " - " + _manufacturer + " " + _model + " (" + _year + ")";
        }

        public bool CanBeRented()
        {
            return _status == "Available";
        }
    }
}