using System;
using System.Collections.Generic;
using System.Text;

namespace SolidExample
{
    public interface ISP
    {

    }
    /// <summary>
    /// Simple value object representing a document passed to devices.
    /// </summary>
    public class Document
    {
        public string Content { get; }
        public Document(string content) => Content = content;
        public override string ToString() => Content;
    }

    /* --- BAD: Fat interface (violates Interface Segregation Principle) --- */
    /// <summary>
    /// A fat interface that forces implementers to provide Print, Scan and Fax.
    /// If a device doesn't support scanning or faxing, it must still implement those members,
    /// often throwing NotSupportedException or leaving empty implementations.
    /// This couples clients to methods they don't need.
    /// </summary>
    public interface IAllInOneDevice
    {
        void Print(Document d);
        Document Scan();
        void Fax(Document d);
    }

    /// <summary>
    /// An example implementation of the fat interface that doesn't support scanning.
    /// This demonstrates the problem: methods that are not applicable must still exist.
    /// </summary>
    public class OldAllInOnePrinter : IAllInOneDevice
    {
        public void Print(Document d)
        {
            Console.WriteLine("OldAllInOnePrinter printing: " + d);
        }

        // This device does not support scanning, but must implement Scan().
        public Document Scan()
        {
            // Bad practice: either throw or return null; both are awkward for clients.
            throw new NotSupportedException("This device cannot scan.");
        }

        public void Fax(Document d)
        {
            Console.WriteLine("OldAllInOnePrinter faxing: " + d);
        }
    }

    /* --- GOOD: Interface Segregation Principle applied --- */
    /// <summary>
    /// Role-specific interfaces. Clients depend only on the operations they need.
    /// </summary>
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        Document Scan();
    }

    public interface IFax
    {
        void Fax(Document d);
    }

    /// <summary>
    /// Convenience interface representing a multi-function device by composing small interfaces.
    /// This does not force small clients to depend on all methods; consumers can depend on
    /// `IPrinter` or `IScanner` individually when they only need that capability.
    /// </summary>
    public interface IMultiFunctionDevice : IPrinter, IScanner, IFax
    {
    }

    /// <summary>
    /// A concrete device that only supports printing. It implements only `IPrinter`,
    /// so it is not burdened with irrelevant members.
    /// </summary>
    public class PrinterOnly : IPrinter
    {
        public void Print(Document d)
        {
            Console.WriteLine("PrinterOnly printing: " + d);
        }
    }

    /// <summary>
    /// A full-featured multi-function machine implemented by composing separate modules.
    /// Composition allows reusing small implementations and keeps each class focused.
    /// </summary>
    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;
        private readonly IFax _fax;

        // Inject specific capabilities; any missing capability can be substituted with a stub.
        public MultiFunctionMachine(IPrinter printer, IScanner scanner, IFax fax)
        {
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
            _fax = fax ?? throw new ArgumentNullException(nameof(fax));
        }

        public void Print(Document d) => _printer.Print(d);
        public Document Scan() => _scanner.Scan();
        public void Fax(Document d) => _fax.Fax(d);
    }

    /// <summary>
    /// Small building blocks that can be composed.
    /// </summary>
    public class SimpleScanner : IScanner
    {
        public Document Scan()
        {
            var doc = new Document("Scanned content at " + DateTime.Now);
            Console.WriteLine("SimpleScanner scanned: " + doc);
            return doc;
        }
    }

    public class SimpleFax : IFax
    {
        public void Fax(Document d)
        {
            Console.WriteLine("SimpleFax faxing: " + d);
        }
    }

    /// <summary>
    /// Example usage demonstrating the difference between the fat interface approach
    /// and the Interface Segregation Principle approach.
    /// </summary>
    public static class Examples
    {
        public static void Run()
        {
            Console.WriteLine("=== Demonstrating fat interface (bad) ===");
            IAllInOneDevice old = new OldAllInOnePrinter();
            old.Print(new Document("Invoice #123"));
            try
            {
                // Client calling Scan must be prepared for NotSupportedException.
                old.Scan();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("Caught expected exception: " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("=== Demonstrating ISP (good) ===");
            IPrinter printer = new PrinterOnly();
            printer.Print(new Document("Report Q1"));

            // Compose a multi-function device from focused implementations.
            var mfm = new MultiFunctionMachine(printer, new SimpleScanner(), new SimpleFax());
            mfm.Print(new Document("Contract"));
            var scanned = mfm.Scan();
            mfm.Fax(scanned);
        }
    }
}
