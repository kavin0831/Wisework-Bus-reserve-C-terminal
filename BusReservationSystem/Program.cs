using System;
using System.Collections.Generic;

namespace BusReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            BusReservationSystem busReservationSystem = new BusReservationSystem();
            busReservationSystem.Run();
        }
    }

    class BusReservationSystem
    {
        private List<Bus> buses;
        private Dictionary<string, List<Bus>> cityBusMapping;
        private Dictionary<string, List<string>> bookedSeats;

        public BusReservationSystem()
        {
            InitializeBuses();
            bookedSeats = new Dictionary<string, List<string>>();
        }

        private void InitializeBuses()
        {
            buses = new List<Bus>
            {
                new Bus("Coimbatore", "Chengalpattu", 10, "6 AM"),
                new Bus("Coimbatore", "Chengalpattu", 10, "5 PM"),
                new Bus("Coimbatore", "Dharmapuri", 10, "6 AM"),
                new Bus("Coimbatore", "Dharmapuri", 10, "5 PM"),
                new Bus("Coimbatore", "Erode", 10, "6 AM"),
                new Bus("Coimbatore", "Erode", 10, "5 PM"),
                new Bus("Coimbatore", "Karur", 10, "6 AM"),
                new Bus("Coimbatore", "Karur", 10, "5 PM"),
                new Bus("Coimbatore", "Nagapattinam", 10, "6 AM"),
                new Bus("Coimbatore", "Nagapattinam", 10, "5 PM"),
                new Bus("Coimbatore", "Nilgiris", 10, "6 AM"),
                new Bus("Coimbatore", "Namakkal", 10, "5 PM"),
                new Bus("Coimbatore", "Theni", 10, "6 AM"),
                new Bus("Coimbatore", "Theni", 10, "5 PM"),
                new Bus("Coimbatore", "Salem", 10, "6 AM"),
                new Bus("Coimbatore", "Salem", 10, "5 PM"),
                new Bus("Coimbatore", "Virudhunagar", 10, "6 AM"),
                new Bus("Coimbatore", "Virudhunagar", 10, "5 PM"),
                new Bus("Coimbatore", "Tiruvannamalai", 10, "6 AM"),
                new Bus("Coimbatore", "Tiruvannamalai", 10, "5 PM"),
                new Bus("Coimbatore", "Thootukudi", 10, "6 AM"),
                new Bus("Coimbatore", "Thootukudi", 10, "5 PM"),
                new Bus("Coimbatore", "Ranipet", 10, "6 AM"),
                new Bus("Coimbatore", "Ranipet", 10, "5 PM"),
                new Bus("Coimbatore", "Perambalur", 10, "6 AM"),
                new Bus("Coimbatore", "Perambalur", 10, "5 PM"),
                new Bus("Coimbatore", "Krishnagiri", 10, "6 AM"),
                new Bus("Coimbatore", "Krishnagiri", 10, "5 PM")
            };

            cityBusMapping = new Dictionary<string, List<Bus>>();
            foreach (var bus in buses)
            {
                if (!cityBusMapping.ContainsKey(bus.To))
                {
                    cityBusMapping[bus.To] = new List<Bus>();
                }
                cityBusMapping[bus.To].Add(bus);
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Bus Reservation System");
                Console.WriteLine("1. Book a Seat");
                Console.WriteLine("2. View Available Buses");
                Console.WriteLine("3. Undo a Ticket");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookSeat();
                        break;
                    case "2":
                        ViewAvailableBuses();
                        break;
                    case "3":
                        UndoBooking();
                        break;
                    case "4":
                        Console.WriteLine("Exiting the system...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void BookSeat()
        {
            Console.WriteLine("Available destinations from Coimbatore:");
            foreach (var city in cityBusMapping.Keys)
            {
                Console.WriteLine($"- {city}");
            }

            Console.Write("Enter your destination city: ");
            string destinationCity = Console.ReadLine();

            if (!cityBusMapping.ContainsKey(destinationCity))
            {
                Console.WriteLine("No buses available to this city.");
                return;
            }

            Console.WriteLine("Available buses:");
            foreach (var bus in cityBusMapping[destinationCity])
            {
                Console.WriteLine($"{bus.BusId}. {bus.To} - {bus.TotalSeats - bus.BookedSeats.Count} seats available at {bus.Timing}");
            }

            Console.Write("Choose a bus number to book: ");
            int busChoice = int.Parse(Console.ReadLine());

            var selectedBus = cityBusMapping[destinationCity].Find(b => b.BusId == busChoice);
            if (selectedBus == null)
            {
                Console.WriteLine("Invalid bus number.");
                return;
            }

            Console.WriteLine("Available seats:");
            selectedBus.DisplayAvailableSeats();

            Console.Write("Choose a seat number to book: ");
            int seatChoice = int.Parse(Console.ReadLine());

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            if (selectedBus.BookSeat(name, seatChoice))
            {
                if (!bookedSeats.ContainsKey(name))
                {
                    bookedSeats[name] = new List<string>();
                }
                bookedSeats[name].Add($"{selectedBus.BusId} - {selectedBus.To} at {selectedBus.Timing}, Seat: {seatChoice}");
                Console.WriteLine($"Seat {seatChoice} booked successfully for {name} on bus to {selectedBus.To} at {selectedBus.Timing}.");
            }
            else
            {
                Console.WriteLine("Seat is not available or does not exist.");
            }
        }

        private void ViewAvailableBuses()
        {
            Console.WriteLine("Available destinations from Coimbatore:");
            foreach (var city in cityBusMapping.Keys)
            {
                Console.WriteLine($"- {city}");
            }

            Console.Write("Enter your destination city: ");
            string destinationCity = Console.ReadLine();

            if (!cityBusMapping.ContainsKey(destinationCity))
            {
                Console.WriteLine("No buses available to this city.");
                return;
            }

            Console.WriteLine("Available buses:");
            foreach (var bus in cityBusMapping[destinationCity])
            {
                Console.WriteLine($"{bus.BusId}. {bus.To} - {bus.TotalSeats - bus.BookedSeats.Count} seats available at {bus.Timing}");
            }
        }

        private void UndoBooking()
        {
            Console.Write("Enter your name to undo booking: ");
            string name = Console.ReadLine();

            if (!bookedSeats.ContainsKey(name) || bookedSeats[name].Count == 0)
            {
                Console.WriteLine("No bookings found for this name.");
                return;
            }

            Console.WriteLine("Your bookings:");
            for (int i = 0; i < bookedSeats[name].Count; i++)
            {
                Console.WriteLine($"{i + 1}. {bookedSeats[name][i]}");
            }

            Console.Write("Select a booking number to undo: ");
            int bookingChoice = int.Parse(Console.ReadLine()) - 1;

            if (bookingChoice < 0 || bookingChoice >= bookedSeats[name].Count)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            string busDetails = bookedSeats[name][bookingChoice];
            var busId = int.Parse(busDetails.Split('-')[0].Trim());

            var bus = buses.Find(b => b.BusId == busId);
            if (bus != null)
            {
                var seatNumber = int.Parse(busDetails.Split(new[] { "Seat: " }, StringSplitOptions.None)[1]);
                bus.CancelSeat(name, seatNumber);
                bookedSeats[name].RemoveAt(bookingChoice);
                Console.WriteLine("Booking undone successfully.");
            }
        }
    }

    class Bus
    {
        public int BusId { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public int TotalSeats { get; private set; }
        public List<string> BookedSeats { get; private set; }
        public string Timing { get; private set; }

        private static int busCounter = 1;
        private List<bool> seatAvailability;

        public Bus(string from, string to, int totalSeats, string timing)
        {
            From = from;
            To = to;
            TotalSeats = totalSeats;
            Timing = timing;
            BusId = busCounter++;
            BookedSeats = new List<string>();
            seatAvailability = new List<bool>(new bool[totalSeats]);
        }

        public void DisplayAvailableSeats()
        {
            Console.WriteLine("Available seats:");
            for (int i = 1; i <= TotalSeats; i++)
            {
                if (!seatAvailability[i - 1])
                {
                    Console.WriteLine($"Seat {i}");
                }
            }
        }

        public bool BookSeat(string name, int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > TotalSeats)
            {
                return false;
            }

            if (!seatAvailability[seatNumber - 1])
            {
                BookedSeats.Add(name);
                seatAvailability[seatNumber - 1] = true;
                return true;
            }
            return false;
        }

        public void CancelSeat(string name, int seatNumber)
        {
            if (seatNumber >= 1 && seatNumber <= TotalSeats)
            {
                BookedSeats.Remove(name);
                seatAvailability[seatNumber - 1] = false;
            }
        }
    }
}
