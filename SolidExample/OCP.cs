using System;
using System.Collections.Generic;

namespace SolidExample
{
    public static class OCP
    {
        /*
         PSEUDOCODE / DETAILED PLAN:
         - Define an abstraction `IShape` exposing `Area()` so behavior is accessed through an interface.
         - Implement concrete shapes: `Rectangle`, `Circle`, `Triangle` each providing their own `Area()` computation.
         - Implement `AreaCalculator` which accepts `IEnumerable<IShape>` and sums areas.
           - This class is closed for modification (no changes needed here) but open for extension:
             new shapes that implement `IShape` can be added without touching `AreaCalculator`.
         - Provide a `Demo()` method that constructs a list of shapes (including a newly added shape),
           computes total area and prints results to demonstrate the Open/Closed Principle.
        */

        // Abstraction
        public interface IShape
        {
            double Area();
        }

        // Concrete implementations - can be extended with new shapes without changing AreaCalculator
        public sealed class Rectangle : IShape
        {
            public double Width { get; }
            public double Height { get; }

            public Rectangle(double width, double height) => (Width, Height) = (width, height);

            public double Area() => Width * Height;

            public override string ToString() => $"Rectangle({Width}×{Height})";
        }

        public sealed class Circle : IShape
        {
            public double Radius { get; }

            public Circle(double radius) => Radius = radius;

            public double Area() => Math.PI * Radius * Radius;

            public override string ToString() => $"Circle(r={Radius})";
        }

        // New shape added without changing AreaCalculator
        public sealed class Triangle : IShape
        {
            public double @Base { get; }
            public double Height { get; }

            public Triangle(double @base, double height) => (Base, Height) = (@base, height);

            public double Area() => 0.5 * Base * Height;

            public override string ToString() => $"Triangle(base={Base}, h={Height})";
        }

        // Consumer that depends on the abstraction (IShape) — follows OCP
        public sealed class AreaCalculator
        {
            public double TotalArea(IEnumerable<IShape> shapes)
            {
                if (shapes == null) throw new ArgumentNullException(nameof(shapes));
                double total = 0;
                foreach (var s in shapes) total += s.Area();
                return total;
            }
        }

        // Demo showing how to extend with a new shape without modifying AreaCalculator
        public static void Demo()
        {
            var shapes = new List<IShape>
            {
                new Rectangle(3, 4),
                new Circle(2),
                new Triangle(3, 4) // added later; no changes required in AreaCalculator
            };

            var calc = new AreaCalculator();

            Console.WriteLine("Shapes and areas:");
            foreach (var s in shapes)
            {
                Console.WriteLine($" - {s}: {s.Area():F2}");
            }

            Console.WriteLine($"Total area = {calc.TotalArea(shapes):F2}");
        }
    }
}
