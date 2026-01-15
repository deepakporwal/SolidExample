using System;
using System.Collections.Generic;

// ❌ BAD: Engine directly depends on specific fuel types (violates DIP)
class BadAutomobileExample
{
    class GasolineEngine
    {
        public void Start() => Console.WriteLine("Starting gasoline engine...");
        public void Stop() => Console.WriteLine("Stopping gasoline engine...");
    }

    class DieselEngine
    {
        public void StartDiesel() => Console.WriteLine("Starting diesel engine...");
        public void StopDiesel() => Console.WriteLine("Stopping diesel engine...");
    }

    class Car
    {
        private GasolineEngine gasolineEngine;
        private DieselEngine dieselEngine;
        private string engineType;

        public Car(string type)
        {
            engineType = type;
            if (type == "gasoline")
                gasolineEngine = new GasolineEngine();
            else
                dieselEngine = new DieselEngine();
        }

        public void Drive()
        {
            // ❌ Car is tightly coupled to concrete engine types
            if (engineType == "gasoline")
            {
                gasolineEngine.Start();
                Console.WriteLine("Driving with gasoline...");
                gasolineEngine.Stop();
            }
            else
            {
                dieselEngine.StartDiesel();
                Console.WriteLine("Driving with diesel...");
                dieselEngine.StopDiesel();
            }
        }
    }
}

// ✅ GOOD: Components depend on abstractions (follows DIP)
namespace GoodAutomobileExample
{
    // Abstraction: All engines follow this contract
    interface IEngine
    {
        void Start();
        void Stop();
        string GetFuelType();
    }

    // Low-level modules: Concrete engine implementations
    class GasolineEngine : IEngine
    {
        public void Start() => Console.WriteLine("🔥 Gasoline engine started (8000 RPM)");
        public void Stop() => Console.WriteLine("⛔ Gasoline engine stopped");
        public string GetFuelType() => "Gasoline";
    }

    class DieselEngine : IEngine
    {
        public void Start() => Console.WriteLine("💨 Diesel engine started (6000 RPM)");
        public void Stop() => Console.WriteLine("⛔ Diesel engine stopped");
        public string GetFuelType() => "Diesel";
    }

    class ElectricEngine : IEngine
    {
        public void Start() => Console.WriteLine("⚡ Electric engine activated (silent)");
        public void Stop() => Console.WriteLine("⛔ Electric engine deactivated");
        public string GetFuelType() => "Electricity";
    }

    class HybridEngine : IEngine
    {
        public void Start() => Console.WriteLine("🔋 Hybrid engine started (dual mode)");
        public void Stop() => Console.WriteLine("⛔ Hybrid engine stopped");
        public string GetFuelType() => "Hybrid (Gasoline + Electric)";
    }

    // Abstraction: All cars follow this contract
    interface ITransmission
    {
        void Shift(int gear);
    }

    class AutomaticTransmission : ITransmission
    {
        public void Shift(int gear) => Console.WriteLine($"   Automatic transmission shifted to gear {gear}");
    }

    class ManualTransmission : ITransmission
    {
        public void Shift(int gear) => Console.WriteLine($"   Manual transmission shifted to gear {gear}");
    }

    // High-level module: Car depends ONLY on abstractions
    class Car
    {
        private readonly IEngine engine;
        private readonly ITransmission transmission;
        private readonly string model;

        public Car(string model, IEngine engine, ITransmission transmission)
        {
            this.model = model;
            this.engine = engine;
            this.transmission = transmission;
        }

        public void Drive()
        {
            Console.WriteLine($"\n🚗 {model} - Fuel Type: {engine.GetFuelType()}");
            engine.Start();
            transmission.Shift(1);
            Console.WriteLine("   Accelerating...");
            transmission.Shift(2);
            Console.WriteLine("   Cruising at highway speed");
            transmission.Shift(1);
            Console.WriteLine("   Decelerating...");
            engine.Stop();
        }

        public void DisplaySpecs()
        {
            Console.WriteLine($"Engine: {engine.GetFuelType()} | Transmission: {transmission.GetType().Name}");
        }
    }

    // Manufacturing plant: Creates cars with different configurations
    class CarFactory
    {
        public static Car CreateStandardCar()
            => new Car("Toyota Camry", new GasolineEngine(), new AutomaticTransmission());

        public static Car CreateEconomyCar()
            => new Car("Hyundai i10", new GasolineEngine(), new ManualTransmission());

        public static Car CreatePremiumCar()
            => new Car("Tesla Model S", new ElectricEngine(), new AutomaticTransmission());

        public static Car CreateEcoCar()
            => new Car("Prius", new HybridEngine(), new AutomaticTransmission());
    }

    // Usage example
    class Program
    {
        static void Main()
        {
            Console.WriteLine("========== AUTOMOBILE MANUFACTURING ==========\n");

            // Create different car configurations - ZERO code changes needed
            var cars = new List<Car>
            {
                CarFactory.CreateStandardCar(),
                CarFactory.CreateEconomyCar(),
                CarFactory.CreatePremiumCar(),
                CarFactory.CreateEcoCar()
            };

            foreach (var car in cars)
            {
                car.DisplaySpecs();
                car.Drive();
            }

            // Key benefit: New engine types can be added without modifying Car class
            Console.WriteLine("\n========== FUTURE ENGINE TYPE ==========");
            var hydrogenCar = new Car(
                "Toyota Mirai",
                new HydrogenFuelCell(),
                new AutomaticTransmission()
            );
            hydrogenCar.DisplaySpecs();
            hydrogenCar.Drive();
        }
    }

    // New engine type - ZERO changes to existing code
    class HydrogenFuelCell : IEngine
    {
        public void Start() => Console.WriteLine("🔬 Hydrogen fuel cell activated (zero emissions)");
        public void Stop() => Console.WriteLine("⛔ Hydrogen fuel cell stopped");
        public string GetFuelType() => "Hydrogen";
    }
}