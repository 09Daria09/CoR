using System;
using System.Collections.Generic;

namespace RefactoringGuru.DesignPatterns.ChainOfResponsibility.Conceptual
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        void Handle(Receiver request);
    }
    public abstract class PaymentHandler : IHandler
    {
        protected IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(Receiver request)
        {
            if (_nextHandler != null)
            {
                _nextHandler.Handle(request);
            }
        }
    }

    public class MoneyPaymentHandler : PaymentHandler
    {
        public override void Handle(Receiver request)
        {
            if (request.MoneyTransfer)
            {
                Console.WriteLine("Transfer through money transfer systems\n");
                _nextHandler.Handle(request);
            }
            else
            {
               _nextHandler.Handle(request);
            }
        }
    }

    public class PayPalPaymentHandler : PaymentHandler
    {
        public override void Handle(Receiver request)
        {
            if (request.PayPalTransfer)
            {
                Console.WriteLine("Transfer via paypal\n");
                _nextHandler.Handle(request);
            }
            else
            {
                _nextHandler.Handle(request);
            }
        }
    }

    public class BankPaymentHandler : PaymentHandler
    {
        public override void Handle(Receiver request)
        {
            if (request.BankTransfer)
            {
                Console.WriteLine("Bank transfer\n");
            }
            else
            {
                _nextHandler.Handle(request);
            }
        }
    }

    public class Receiver
    {
        public bool BankTransfer { get; set; }
        public bool MoneyTransfer { get; set; }
        public  bool PayPalTransfer { get; set; }
        public Receiver (bool bt, bool mt, bool ppt)
        {
            BankTransfer = bt;
            MoneyTransfer = mt;
            PayPalTransfer = ppt;
        }
        public void Request(PaymentHandler h, Receiver request)
        {
            h.Handle(request);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var money = new MoneyPaymentHandler();
            var pal = new PayPalPaymentHandler();
            var bank = new BankPaymentHandler();

            money.SetNext(pal).SetNext(bank);

            // Console.WriteLine("Chain: Monkey > Squirrel > Dog\n");
            Receiver receiver = new Receiver(true, false, true);
            receiver.Request(pal, receiver);
            Console.WriteLine();

        }
    }
}
