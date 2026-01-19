using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem
{
    /// <summary>
    /// This class manages the collection of cars in the rental group of vehicles
    /// Also handles the loading, saving, searching and filtering of cars
    /// </summary>
    internal class CarManager
    {
        private List<Car> _cars = new List<Car>();
        private string _filePath = "";
        private bool _debugger = false;

        /// <summary>
        /// initializes a new carmanager instance
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="debugger"></param>
        public CarManager(string filePath, bool debugger = false)
        {
            _filePath = filePath;
            _debugger = debugger;
            LoadCars();
        }

        /// <summary>
        /// Loads car data from the CSV file into memory
        /// also creates sample cars if the file doesn't exist
        /// </summary>
        public void LoadCars()
        {
            if (File.Exists(_filePath) == false)
            {
                CreateSampleCars();
                SaveCars();
                return;
            }

            _cars.Clear();
            string[] lines = File.ReadAllLines(_filePath);

            for (int x = 1; x < lines.Length; x++)
            {
                string[] parts = lines[x].Split(',');
                if (parts.Length >= 6)
                {
                    Car car = new Car(
                        parts[0].Trim(),
                        parts[1].Trim(),
                        parts[2].Trim(),
                        int.Parse(parts[3].Trim()),
                        decimal.Parse(parts[4].Trim())
                    );
                    car.setStatus(parts[5].Trim());
                    _cars.Add(car);
                }
            }

            if (_debugger)
            {
                Console.WriteLine("Loaded " + _cars.Count + " cars from file.");
                for (int x = 0; x < 3 && x < _cars.Count; x++)
                {
                    Console.WriteLine(_cars[x].getDisplay());
                }
            }
        }

        /// <summary>
        /// Saves all car data into the csv file
        /// </summary>
        public void SaveCars()
        {
            List<string> lines = new List<string>();
            lines.Add("LicensePlate,Make,Model,Year,DailyRate,Status");

            foreach (Car car in _cars)
            {
                string line = car.getLicensePlate() + "," +
                    car.getManufacturer() + "," +
                    car.getModel() + "," +
                    car.getYear().ToString() + "," +
                    car.getDailyRate().ToString() + "," +
                    car.getStatus();
                lines.Add(line);
            }

            File.WriteAllLines(_filePath, lines);

            if (_debugger)
                Console.WriteLine("Saved " + _cars.Count + " cars to file.");
        }

        /// <summary>
        /// Creates sample car data for initial system setup
        /// </summary>
        private void CreateSampleCars()
        {
            _cars.Add(new Car("ABC123", "Toyota", "Vios", 2022, 2500));
            _cars.Add(new Car("DEF456", "Honda", "Civic", 2023, 3500));
            _cars.Add(new Car("GHI789", "Ford", "Mustang", 2021, 6000));
            _cars.Add(new Car("JKL012", "Chevrolet", "Camaro", 2023, 6500));
        }

        /// <summary>
        /// finds the car by its plate number
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns></returns>
        public Car FindCar(string licensePlate)
        {
            foreach (Car car in _cars)
            {
                if (car.getLicensePlate() == licensePlate)
                    return car;
            }
            return null;
        }

        /// <summary>
        /// gets a list of all cars currently available for rent
        /// </summary>
        /// <returns></returns>
        public List<Car> getAvailableCars()
        {
            List<Car> available = new List<Car>();
            foreach (Car car in _cars)
            {
                if (car.getStatus() == "Available")
                    available.Add(car);
            }
            return available;
        }

        public List<Car> rented()
        {
            List <Car> rented = new List<Car>();
            foreach (Car car in _cars)
            {
                if (car.getStatus() == "Rented")
                    rented.Add(car);
            }
            return rented;
        }

        /// <summary>
        /// Displays all available cars in a formatted list
        /// </summary>
        public void DisplayAvailableCars()
        {
            List<Car> available = getAvailableCars();

            if (available.Count == 0)
            {
                Console.WriteLine("No cars available for rent.");
                return;
            }

            Console.WriteLine("\n===== Available Cars =====");
            for (int x = 0; x < available.Count; x++)
            {
                Console.WriteLine(available[x].getDisplay() + " - ₱" + available[x].getDailyRate() + "/day");
            }
        }

        /// <summary>
        /// displays all cars in the system with their current status
        /// </summary>
        public void DisplayAllCars()
        {
            Console.WriteLine("\n===== All Cars =====");
            for (int x = 0; x < _cars.Count; x++)
            {
                Console.WriteLine(_cars[x].getDisplay() + " - Status: " + _cars[x].getStatus());
            }
        }

        public void displayRentedCars()
        {
            Console.WriteLine("\n ===== Cars Rented =====");
            for (int x = 0; x < rented().Count; x++)
            {
                Console.WriteLine(rented()[x].getDisplay());
            }
        }
    }
}