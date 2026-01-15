using System;
using System.Collections.Generic;
using System.Text;

namespace SolidExample
{
    /*
    Pseudocode plan (detailed):
    1. Define an interface `IFlyable` with method `Fly()` for types that can fly.
    2. Define an abstract base class `Bird` with common behavior/properties (e.g., Name, Eat()).
    3. Implement `Sparrow` that inherits `Bird` and implements `IFlyable`.
    4. Implement `Penguin` that inherits `Bird` but does NOT implement `IFlyable` (it can swim instead).
    5. Provide a `FlightController` client that depends on `IFlyable` and invokes `Fly()` - demonstrates safe substitution
       for any `IFlyable` implementation.
    6. Provide an `Aviary` client that depends on `Bird` and invokes `Eat()` - both `Sparrow` and `Penguin` can be used.
    7. Add `LSP.Run()` to create instances and demonstrate that code depending on `IFlyable` or `Bird` works correctly
       without forcing non-flying birds to implement `Fly()` (adheres to Liskov Substitution Principle).
    */

    // Interface for flying capability
    public interface IFlyable
    {
        void Fly();
    }

    // Common base class for birds
    public abstract class Bird
    {
        public string Name { get; }

        protected Bird(string name)
        {
            Name = name;
        }

        public virtual void Eat()
        {
            Console.WriteLine($"{Name} is eating.");
        }
    }

    // A bird that can fly
    public class Sparrow : Bird, IFlyable
    {
        public Sparrow(string name) : base(name) { }

        public void Fly()
        {
            Console.WriteLine($"{Name} flaps wings and flies.");
        }
    }

    // A bird that cannot fly (but can swim) - does not implement IFlyable
    public class Penguin : Bird
    {
        public Penguin(string name) : base(name) { }

        public void Swim()
        {
            Console.WriteLine($"{Name} swims in the water.");
        }

        // Penguin uses the base Eat() implementation or can override if needed
    }

    // Client that depends on the flying capability (interface)
    public class FlightController
    {
        // Accept any IFlyable - substitutable implementation is required
        public void SendToFly(IFlyable flyer)
        {
            flyer.Fly();
        }
    }

    // Client that depends on the general Bird abstraction
    public class Aviary
    {
        private readonly IList<Bird> _birds = new List<Bird>();

        public void Add(Bird bird) => _birds.Add(bird);

        public void FeedAll()
        {
            foreach (var bird in _birds)
                bird.Eat();
        }
    }

    public class LSP
    {
        // Demonstration entry for the Liskov Substitution Principle example
        public static void Run()
        {
            var sparrow = new Sparrow("Jack");
            var penguin = new Penguin("Penny");

            // Aviary depends on Bird and works for both Sparrow and Penguin
            var aviary = new Aviary();
            aviary.Add(sparrow);
            aviary.Add(penguin);
            Console.WriteLine("Aviary feeding:");
            aviary.FeedAll();

            // FlightController depends on IFlyable. Penguin is NOT passed here because it does not fly.
            var flightController = new FlightController();
            Console.WriteLine("\nFlight operations:");
            flightController.SendToFly(sparrow);

            // Penguin-specific behavior
            Console.WriteLine("\nPenguin-specific behavior:");
            penguin.Swim();

            // This design adheres to LSP:
            // - Code that expects a Bird can work with any Bird subtype.
            // - Code that expects IFlyable can work with any flying implementation without unexpected behavior.
        }
    }
}
