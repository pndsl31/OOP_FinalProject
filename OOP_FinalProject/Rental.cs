using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    /// <summary>
    /// represents a single rental transaction in the system
    /// this contains all the details about a car rental to a customer
    /// </summary>
    internal class Rental
    {
        private string _rentalId = "";
        private string _licensePlate = "";
        private string _customerName = "";
        private string _startDate = "";
        private string _endDate = "";
        private bool _isActive = false;
        private decimal _totalCost = 0;

        /// <summary>
        /// Initializes a new rental transaction
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <param name="customerName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dailyRate"></param>
        public Rental(string licensePlate, string customerName, string startDate, string endDate, decimal dailyRate)
        {
            Random rnd = new Random();
            _rentalId = "R" + rnd.Next(1000, 9999).ToString();
            _licensePlate = licensePlate;
            _customerName = customerName;
            _startDate = startDate;
            _endDate = endDate;
            _isActive = true;
            int days = (DateTime.Parse(endDate) - DateTime.Parse(startDate)).Days + 1;
            _totalCost = days * dailyRate;
        }

        /// <summary>
        /// Constructor for loading from CSV (with all parameters including ID)
        /// </summary>
        public Rental(string rentalId, string licensePlate, string customerName, string startDate, string endDate, bool isActive, decimal totalCost)
        {
            _rentalId = rentalId;
            _licensePlate = licensePlate;
            _customerName = customerName;
            _startDate = startDate;
            _endDate = endDate;
            _isActive = isActive;
            _totalCost = totalCost;
        }

        public string getRentalId() { return _rentalId; }
        public string getLicensePlate() { return _licensePlate; }
        public string getCustomerName() { return _customerName; }
        public string getStartDate() { return _startDate; }
        public string getEndDate() { return _endDate; }
        public bool getIsActive() { return _isActive; }
        public decimal getTotalCost() { return _totalCost; }
        public void setIsActive(bool active) { _isActive = active; }

        /// <summary>
        /// Writes a receipt to a text file in the Receipts folder
        /// </summary>
        public void writeReceiptToFile()
        {
            string receiptsFolder = "Receipts";
            
            // Create the folder if it doesn't exist
            if (!Directory.Exists(receiptsFolder))
            {
                Directory.CreateDirectory(receiptsFolder);
            }

            string fileName = $"{_rentalId}_Receipt.txt";
            string filePath = Path.Combine(receiptsFolder, fileName);

            string receiptContent = "===== RENTAL RECEIPT =====\n" +
                                   "Rental ID: " + _rentalId + "\n" +
                                   "Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\n" +
                                   "Customer: " + _customerName + "\n" +
                                   "Car: " + _licensePlate + "\n" +
                                   "Period: " + _startDate + " to " + _endDate + "\n" +
                                   "Total Cost: $" + _totalCost + "\n" +
                                   "Thank you!\n" +
                                   "============================";

            File.WriteAllText(filePath, receiptContent);
            Console.WriteLine($"Receipt saved to: {filePath}");
        }

        /// <summary>
        /// Writes a rental slip to a text file in the RentalSlips folder
        /// </summary>
        public void writeRentalSlipToFile()
        {
            string slipsFolder = "RentalSlips";
            
            // Create the folder if it doesn't exist
            if (!Directory.Exists(slipsFolder))
            {
                Directory.CreateDirectory(slipsFolder);
            }

            string fileName = $"{_rentalId}_Slip.txt";
            string filePath = Path.Combine(slipsFolder, fileName);

            string slipContent = "===== RENTAL SLIP =====\n" +
                                "Rental ID: " + _rentalId + "\n" +
                                "Customer: " + _customerName + "\n" +
                                "Vehicle: " + _licensePlate + "\n" +
                                "Pick-up Date: " + _startDate + "\n" +
                                "Return Date: " + _endDate + "\n" +
                                "==========================";

            File.WriteAllText(filePath, slipContent);
            Console.WriteLine($"Rental slip saved to: {filePath}");
        }

        /// <summary>
        /// Prints receipt to console (for backward compatibility)
        /// </summary>
        public void printReceipt()
        {
            Console.WriteLine("\n==== RENTAL RECEIPT =====");
            Console.WriteLine("Rental ID: " + _rentalId);
            Console.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            Console.WriteLine("Customer: " + _customerName);
            Console.WriteLine("Car: " + _licensePlate);
            Console.WriteLine("Period: " + _startDate + " to " + _endDate);
            Console.WriteLine("Total Cost: $" + _totalCost);
            Console.WriteLine("Thank you!");
            Console.WriteLine("==========================");
        }

        /// <summary>
        /// Prints rental slip to console (for backward compatibility)
        /// </summary>
        public void printRentalSlip()
        {
            Console.WriteLine("\n===== RENTAL SLIP =====");
            Console.WriteLine("Rental ID: " + _rentalId);
            Console.WriteLine("Customer: " + _customerName);
            Console.WriteLine("Vehicle: " + _licensePlate);
            Console.WriteLine("Pick-up Date: " + _startDate);
            Console.WriteLine("Return Date: " + _endDate);
            Console.WriteLine("==========================");
        }
    }
}