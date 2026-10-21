using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    internal class mainSystem
    {
        private CarManager _carManager = null;
        private RentalManager _rentalManager = null;
        private MaintenanceManager _maintenanceManager = null;
        private bool _running = true;

        /// <summary>
        /// Initializes a new instance of the mainSystem class
        /// sets up all the managers and starts the main loop
        /// </summary>
        public mainSystem()
        {
            Console.WriteLine("======== Car Rental and Maintenance Management System =========");
            _carManager = new CarManager("cars.csv", false);
            _rentalManager = new RentalManager("rentals.csv");
            _maintenanceManager = new MaintenanceManager("maintenance.csv");

            Run();
        }

        /// <summary>
        /// This is the main program loop that displays the menu and processes all the
        /// user's choices
        /// </summary>
        private void Run()
        {
            while (_running)
            {
                Console.WriteLine("\n======= Menu =======");
                Console.WriteLine("1. View Available Cars");
                Console.WriteLine("2. Rent a Car");
                Console.WriteLine("3. Return a Car & Log Maintenance Needed");
                Console.WriteLine("4. Schedule Maintenance Work");
                Console.WriteLine("5. Complete Maintenance Work");
                Console.WriteLine("6. View Complete Information");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                    MenuViewAvailableCars();
                else if (choice == "2")
                    MenuRentCar();
                else if (choice == "3")
                    MenuReturnCarAndLogMaintenance();
                else if (choice == "4")
                    MenuScheduleMaintenance();
                else if (choice == "5")
                    MenuCompleteMaintenance();
                else if (choice == "6")
                    MenuViewAllData();
                else if (choice == "7")
                {
                    SaveAllData();
                    _running = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Displays all the cars currently available for rental
        /// </summary>
        private void MenuViewAvailableCars()
        {
            _carManager.DisplayAvailableCars();
            Console.WriteLine("\nPress a key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// This handles the process of renting a car to a customer
        /// Contains all the conflict detection for maintenance and existing rentals
        /// </summary>
        private void MenuRentCar()
        {
            _carManager.DisplayAvailableCars();
            Console.Write("\nEnter plate number: ");
            string plate = Console.ReadLine();
            Car car = _carManager.FindCar(plate);

            if (car == null)
            {
                
                
                Console.WriteLine("Car is not found!");
                Console.ReadKey();
                return;
            }
            
            if (car.getStatus() == "Maintenance")
            {
                Console.WriteLine("Car is currently under maintenance!");
                Console.ReadKey();
                return;
            }
            
            if (_maintenanceManager.HasPendingMaintenance(plate))
            {
                Console.WriteLine("Car has pending maintenance that needs to be completed!");
                Console.ReadKey();
                return;
            }
            
            if (_rentalManager.IsCarRented(plate))
            {
                Console.WriteLine("Car is already rented!");
                Console.ReadKey();
                return;
            }
            

            Console.Write("Enter customer name: ");
            string name = Console.ReadLine();
            Console.Write("Enter start date of rental (format: yyyy-mm-dd): ");
            string startDate = Console.ReadLine();
            Console.Write("Enter end date of rental (format: yyyy-mm-dd): ");
            string endDate = Console.ReadLine();

            Rental rental = _rentalManager.CreateRental(plate, name, startDate, endDate, car.getDailyRate());
            car.setStatus("Rented");
            _carManager.SaveCars(); 
            rental.writeReceiptToFile();
            rental.writeRentalSlipToFile();
            rental.printReceipt();
            rental.printRentalSlip();
            Console.WriteLine("\nRental confirmed! Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// This handles returning of a rented car and logging any needed maintenance
        /// </summary>
        private void MenuReturnCarAndLogMaintenance()
        {
            _carManager.displayRentedCars();
            Console.Write("\nEnter plate number of car to return: ");
            string plate = Console.ReadLine();
            
            if (!_rentalManager.IsCarRented(plate))
            {
                Console.WriteLine("This car is not currently rented!");
                Console.ReadKey();
                return;
            }
            
            bool returned = _rentalManager.ReturnCar(plate);
            
            if (returned)
            {
                Console.WriteLine("Car returned.");

                Console.WriteLine("Checking Car Condition.......");
                
                Console.Write("Does this car need maintenance? y/n: ");
                string needsMaintenance = Console.ReadLine().ToUpper();
                
                if (needsMaintenance == "Y")
                {
                    Console.Write("Enter maintenance service: ");
                    string description = Console.ReadLine();
                    Console.Write("Enter mechanic name for maintenance: ");
                    string mechanic = Console.ReadLine();
                    
                    _maintenanceManager.AddRecord(plate, mechanic, description);
                    
                    Car car = _carManager.FindCar(plate);
                    if (car != null)
                    {
                        car.setStatus("Maintenance");
                        _carManager.SaveCars();
                        Console.WriteLine("Maintenance logged and car status set to Maintenance.");
                    }
                }
                else
                {
                    Car car = _carManager.FindCar(plate);
                    if (car != null && car.getStatus() == "Rented")
                    {
                        car.setStatus("Available");
                        _carManager.SaveCars();
                        Console.WriteLine("Car set to Available for rental.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to return car!");
            }
            
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// This schedules maintenance work for a specific car
        /// Contains conflict detection for rented cars and pending maintenance
        /// </summary>
        private void MenuScheduleMaintenance()
        {
            Console.Write("Enter plate number for maintenance work: ");
            string plate = Console.ReadLine();

            Car car = _carManager.FindCar(plate);
            if (car == null)
            {
                Console.WriteLine("Car is not found!");
                Console.ReadKey();
                return;
            }
            
            if (car.getStatus() == "Rented")
            {
                Console.WriteLine("Car is rented. Currently cannot schedule for maintenance!");
                Console.ReadKey();
                return;
            }

            if (_maintenanceManager.HasPendingMaintenance(plate))
            {
                Console.WriteLine("This car already has pending maintenance!");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter mechanic name who will do the maintenance: ");
            string mechanic = Console.ReadLine();

            Console.Write("Enter maintenance service to be done: ");
            string description = Console.ReadLine();

            _maintenanceManager.AddRecord(plate, mechanic, description);
            car.setStatus("Maintenance");
            _carManager.SaveCars(); 
            Console.WriteLine("Maintenance scheduled! Car status set to Maintenance.");
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Marks a pending maintenance record as completed
        /// which also updates car status back to available
        /// </summary>
        private void MenuCompleteMaintenance()
        {
            _maintenanceManager.DisplayPendingMaintenance();
            if (_maintenanceManager.HasAnyPending() == false)
            {
                Console.WriteLine("No pending maintenance to complete.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter maintenance record ID to mark as completed: ");
            string recordId = Console.ReadLine();
            Console.Write("Who completed the work?: ");
            string mechanicCompleted = Console.ReadLine();
            Console.Write("What was actually done?: ");
            string workCompleted = Console.ReadLine();

            bool completed = _maintenanceManager.CompleteMaintenance(recordId, mechanicCompleted, workCompleted);
            if (completed)
            {
                string carPlate = _maintenanceManager.GetCarPlateFromRecord(recordId);
                if (carPlate != null)
                {
                    Car car = _carManager.FindCar(carPlate);
                    if (car != null && car.getStatus() == "Maintenance")
                    {
                        car.setStatus("Available");
                        _carManager.SaveCars(); 
                        Console.WriteLine("Maintenance work completed and logged!");
                        Console.WriteLine("Car " + carPlate + " status set to available.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Record not found or already completed.");
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays all the system data including cars, active rentals, and maintenance records
        /// </summary>
        private void MenuViewAllData()
        {
            _carManager.DisplayAllCars();
            _rentalManager.DisplayActiveRentals();
            _maintenanceManager.DisplayAllMaintenance();
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Saves all the data to their respective csv files
        /// </summary>
        private void SaveAllData()
        {
            _carManager.SaveCars();
            _rentalManager.SaveRentals();
            _maintenanceManager.SaveRecords();
            Console.WriteLine("\nSave Check");
        }
    }
}
