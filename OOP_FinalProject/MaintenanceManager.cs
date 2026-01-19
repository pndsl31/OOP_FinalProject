using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    /// <summary>
    /// Manages all maintenance records in the program
    /// Also handles loading, saving, adding, completing, and searching, checking, and retrieving info about maintenance records
    /// </summary>
    internal class MaintenanceManager
    {
        private List<MaintenanceRecord> _records = new List<MaintenanceRecord>();
        private string _filePath = "";

        /// <summary>
        /// Initializes a new MaintenanceManager instance
        /// </summary>
        /// <param name="filePath"></param>
        public MaintenanceManager(string filePath)
        {
            _filePath = filePath;
            LoadRecords();
        }

        /// <summary>
        /// Loads the maintenance records from the csv file into the memory
        /// </summary>
        public void LoadRecords()
        {
            if (File.Exists(_filePath) == false)
                return;

            _records.Clear();
            string[] lines = File.ReadAllLines(_filePath);

            for (int x = 1; x < lines.Length; x++)
            {
                string[] parts = lines[x].Split(',');
                if (parts.Length >= 9)
                {
                    MaintenanceRecord record = new MaintenanceRecord(
                        parts[1].Trim(),  // LicensePlate
                        parts[2].Trim(),  // MechanicScheduled
                        parts[3].Trim()   // WorkScheduled
                    );

                    // Load completion info if exists
                    if (bool.Parse(parts[5].Trim()))
                    {
                        record.setCompleted(true, parts[7].Trim(), parts[8].Trim());
                    }

                    _records.Add(record);
                }
            }
        }

        /// <summary>
        /// saves all the records to the csv
        /// </summary>
        public void SaveRecords()
        {
            List<string> lines = new List<string>();
            lines.Add("RecordId,LicensePlate,MechanicScheduled,WorkScheduled,DateScheduled,Completed,DateCompleted,MechanicCompleted,WorkCompleted");

            foreach (MaintenanceRecord record in _records)
            {
                string line = record.getRecordId() + "," +
                    record.getLicensePlate() + "," +
                    record.getMechanicScheduled() + "," +
                    record.getWorkScheduled() + "," +
                    record.getDateScheduled() + "," +
                    record.getCompleted().ToString() + "," +
                    record.getDateCompleted() + "," +
                    record.getMechanicCompleted() + "," +
                    record.getWorkCompleted();
                lines.Add(line);
            }

            File.WriteAllLines(_filePath, lines);
        }

        /// <summary>
        /// adds a new maintenance record to the system
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <param name="mechanic"></param>
        /// <param name="description"></param>
        public void AddRecord(string licensePlate, string mechanic, string description)
        {
            MaintenanceRecord record = new MaintenanceRecord(licensePlate, mechanic, description);
            _records.Add(record);
            SaveRecords(); // Save immediately
        }

        /// <summary>
        /// checks if a specific car has any pending maintenance records
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns></returns>
        public bool HasPendingMaintenance(string licensePlate)
        {
            foreach (MaintenanceRecord record in _records)
            {
                if (record.getLicensePlate() == licensePlate && record.getCompleted() == false)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// checks if there is any pending maintenance in the entire system
        /// </summary>
        /// <returns></returns>
        public bool HasAnyPending()
        {
            foreach (MaintenanceRecord record in _records)
            {
                if (record.getCompleted() == false)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// marks a maintenance record as completed with details
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="mechanicCompleted"></param>
        /// <param name="workCompleted"></param>
        /// <returns></returns>
        public bool CompleteMaintenance(string recordId, string mechanicCompleted, string workCompleted)
        {
            foreach (MaintenanceRecord record in _records)
            {
                if (record.getRecordId() == recordId && record.getCompleted() == false)
                {
                    record.setCompleted(true, mechanicCompleted, workCompleted);
                    SaveRecords(); // Save immediately
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retrieves the license plate associated with a maintenance record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public string GetCarPlateFromRecord(string recordId)
        {
            foreach (MaintenanceRecord record in _records)
            {
                if (record.getRecordId() == recordId)
                    return record.getLicensePlate();
            }
            return null;
        }

        /// <summary>
        /// displays all pending maintenance records in a formatted table
        /// </summary>
        public void DisplayPendingMaintenance()
        {
            Console.WriteLine("\n== Pending Maintenance ==");
            bool found = false;
            foreach (MaintenanceRecord record in _records)
            {
                if (record.getCompleted() == false)
                {
                    Console.WriteLine("ID: " + record.getRecordId() + " - Car: " + record.getLicensePlate() +
                        " - Mechanic: " + record.getMechanicScheduled() +
                        " - Work: " + record.getWorkScheduled());
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("No pending maintenance records.");
        }

        /// <summary>
        /// displays all maintenance records (both pending and completed)
        /// </summary>
        public void DisplayAllMaintenance()
        {
            Console.WriteLine("\n== All Maintenance Records ==");
            bool found = false;
            foreach (MaintenanceRecord record in _records)
            {
                string status;
                if (record.getCompleted())
                    status = "COMPLETED";
                else
                    status = "PENDING";
                
                Console.WriteLine("ID: " + record.getRecordId() + " - Car: " + record.getLicensePlate() +
                    " - Status: " + status + " - Work: " + record.getWorkScheduled());
                found = true;
            }
            if (!found)
                Console.WriteLine("No maintenance records found.");
        }
    }
}