using System;
using System.Collections.Generic;
using System.Text;

namespace SolidExample
{
    public class Birds
    {
        public virtual void Fly()
        {
            Console.WriteLine("This bird can fly.");
        }
    }

    class Obstrich : Birds
    {
        public override void Fly()
        {
            throw new NotImplementedException("Ostriches cannot fly.");
        }
    }

    class Sparrow : Birds
    {
        public override void Fly()
        {
            Console.WriteLine("Sparrow is flying.");
        }
    }

    public static void Main(){
        Birds myBird = new Obstrich();
        myBird.Fly(); // This will throw an exception, violating LSP

        Birds mySparrow = new Sparrow();
        mySparrow.Fly(); // This works fines
    }


    // this is the correct way to implement LSP
    public class Birds2 
    {
        public virtual void Eat()
        {
            Console.WriteLine("This bird is eating.");
        }
    }
    interface IFlyable
    {
        void Fly();
    }

    class Ostrich2 : Birds2
    {
        // Ostrich does not implement IFlyable
        public void Eat()
        {
            Console.WriteLine("Ostrich is eating.");
        }
    }

    class Sparrow2 : Birds2, IFlyable
    {
        public void Fly()
        {
            Console.WriteLine("Sparrow is flying.");
        }

        public override void Eat()
        {
            Console.WriteLine("Sparrow is eating.");
        }
    }

    public static void Main2(){
        Birds2 myBird = new Ostrich2();
        myBird.Eat(); // This works fine

        Birds2 mySparrow = new Sparrow2();
        mySparrow.Eat(); // This works fine

        IFlyable flyingBird = new Sparrow2();
        flyingBird.Fly(); // This works fine
    }

    /// <summary>
    /// The Liskov Substitution Principle (LSP) states that objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program.
    /// 
}
