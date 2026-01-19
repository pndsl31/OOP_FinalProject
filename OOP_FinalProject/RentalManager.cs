using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    /// <summary>
    /// this class manages all the rental transactions in the system
    /// this handles the creation, tracking and return of rentals
    /// </summary>
    internal class RentalManager
    {
        private List<Rental> _rentals = new List<Rental>();
        private string _filePath = "";

        /// <summary>
        /// initializes a new rentalmanager instance
        /// </summary>
        /// <param name="filePath"></param>
        public RentalManager(string filePath)
        {
            _filePath = filePath;
            LoadRentals();
        }

        /// <summary>
        /// Loads all the rental records from the csv file to the program
        /// </summary>
        public void LoadRentals()
        {
            if (File.Exists(_filePath) == false)
                return;

            _rentals.Clear();
            string[] lines = File.ReadAllLines(_filePath);
            for (int x = 1; x < lines.Length; x++)
            {
                string[] parts = lines[x].Split(',');
                if (parts.Length >= 7)
                {
                    // Parse values from CSV
                    string rentalId = parts[0].Trim();
                    string licensePlate = parts[1].Trim();
                    string customerName = parts[2].Trim();
                    string startDate = parts[3].Trim();
                    string endDate = parts[4].Trim();
                    bool isActive = bool.Parse(parts[5].Trim());
                    decimal totalCost = decimal.Parse(parts[6].Trim());
                    
                    Rental rental = new Rental(rentalId, licensePlate, customerName, startDate, endDate, isActive, totalCost);
                    _rentals.Add(rental);
                }
            }
        }

        /// <summary>
        /// saves all rental records to the csv file
        /// </summary>
        public void SaveRentals()
        {
            List<string> lines = new List<string>();
            lines.Add("RentalId,LicensePlate,CustomerName,StartDate,EndDate,IsActive,TotalCost");
            foreach (Rental rental in _rentals)
            {
                string line = rental.getRentalId() + "," +
                    rental.getLicensePlate() + "," +
                    rental.getCustomerName() + "," +
                    rental.getStartDate() + "," +
                    rental.getEndDate() + "," +
                    rental.getIsActive().ToString() + "," +
                    rental.getTotalCost().ToString();
                lines.Add(line);
            }

            File.WriteAllLines(_filePath, lines);
        }

        /// <summary>
        /// creates a new rental transaction and adds it to the system
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <param name="customerName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dailyRate"></param>
        /// <returns></returns>
        public Rental CreateRental(string licensePlate, string customerName, string startDate, string endDate, decimal dailyRate)
        {
            Rental rental = new Rental(licensePlate, customerName, startDate, endDate, dailyRate);
            _rentals.Add(rental);
            SaveRentals(); 
            return rental;
        }

        /// <summary>
        /// checks if a specific car is already rented
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns></returns>
        public bool IsCarRented(string licensePlate)
        {
            foreach (Rental rental in _rentals)
            {
                if (rental.getLicensePlate() == licensePlate && rental.getIsActive() == true)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// marks a rented car as returned
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns></returns>
        public bool ReturnCar(string licensePlate)
        {
            foreach (Rental rental in _rentals)
            {
                if (rental.getLicensePlate() == licensePlate && rental.getIsActive() == true)
                {
                    rental.setIsActive(false);
                    SaveRentals(); 
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// displays all the active rentals which are unreturned
        /// </summary>
        public void DisplayActiveRentals()
        {
            Console.WriteLine("\n== Active Rentals ===");
            bool found = false;
            foreach (Rental rental in _rentals)
            {
                if (rental.getIsActive() == true)
                {
                    Console.WriteLine("ID: " + rental.getRentalId() + " - Car: " + rental.getLicensePlate() + " - Customer: " + rental.getCustomerName());
                    found = true;
                }
            }
            if (found == false)
                Console.WriteLine("No active rentals found.");
        }
    }
}