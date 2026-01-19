using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CarRentalSystem
{
    /// <summary>
    /// this class represents a single maintenance record in the system
    /// it contains the info about scheduled and completed maintenance work
    /// </summary>
    internal class MaintenanceRecord
    {
        private string _recordId = "";
        private string _licensePlate = "";
        private string _dateScheduled = "";
        private string _mechanicScheduled = "";
        private string _workScheduled = "";
        private bool _completed = false;
        private string _dateCompleted = "";
        private string _mechanicCompleted = "";
        private string _workCompleted = "";

        /// <summary>
        /// Initializes a new maintenance record with the basic information
        /// also generates the unique record ID and sets the scheduled date to today
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <param name="mechanic"></param>
        /// <param name="description"></param>
        public MaintenanceRecord(string licensePlate, string mechanic, string description)
        {
            Random rnd = new Random();
            _recordId = "M" + rnd.Next(1000, 9999).ToString();
            _licensePlate = licensePlate;
            _mechanicScheduled = mechanic;
            _workScheduled = description;
            _dateScheduled = DateTime.Now.ToString("yyyy-MM-dd");
            _completed = false;
            _dateCompleted = "";
            _mechanicCompleted = "";
            _workCompleted = "";
        }

        public string getRecordId() { return _recordId; }
        public string getLicensePlate() { return _licensePlate; }
        public string getDateScheduled() { return _dateScheduled; }
        public string getMechanicScheduled() { return _mechanicScheduled; }
        public string getWorkScheduled() { return _workScheduled; }
        public bool getCompleted() { return _completed; }
        public string getDateCompleted() { return _dateCompleted; }
        public string getMechanicCompleted() { return _mechanicCompleted; }
        public string getWorkCompleted() { return _workCompleted; }

        public void setCompleted(bool completed, string mechanicCompleted, string workCompleted)
        {
            _completed = completed;
            _dateCompleted = DateTime.Now.ToString("yyyy-MM-dd");
            _mechanicCompleted = mechanicCompleted;
            _workCompleted = workCompleted;
        }
    }
}