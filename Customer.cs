using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
    class Customer
    {
        public Customer(string name)
        {
            customerName = name;
        }

        public void addRental(Rental arg) 
        { 
            customerRental.Add(arg);
            arg.getMovie().setRent(false);
        }

        public Boolean removeRental(Rental arg)
        {
            foreach (Rental rental in customerRental)
            {
                if (rental.getMovie() == arg.getMovie())
                {
                    rental.getMovie().setRent(true);
                    customerRental.Remove(rental);
                    return true;
                }
            }
            return false;
        }

        public List<Rental> GetRentals() { return customerRental; }

        public string getName() { return customerName; }

        public string statement()
        {
            double totalAmount = 0.0;
            int frequentRenterPoints = 0;
            StringBuilder result = new StringBuilder();
            StringBuilder newResult = new StringBuilder();

            result.AppendLine("Rental Record for" + getName());
            newResult.AppendLine("-------------------- NEW Rental Receipt --------------------");
            newResult.AppendLine("Customer : " + getName());
            newResult.AppendLine("------------------------------------------------------------");
            newResult.AppendLine(String.Format("{0,-15}|{1,-20}\t|{2,-7}|{3}", "GENRE", "TITLE","PERIOD","PRICE"));
            newResult.AppendLine("------------------------------------------------------------");


            IEnumerator<Rental> enumerator = customerRental.GetEnumerator();

            for (; enumerator.MoveNext();)
            {
                double thisAmount = 0.0;
                Rental each = enumerator.Current;

                switch (each.getMovie().getPriceCode())
                {
                    case Movie.REGULAR:
                        thisAmount += 2.0;
                        if (each.getDaysRented() > 2)
                            thisAmount += (each.getDaysRented() - 2) * 1.5;
                        newResult.Append(String.Format("{0,-15}|", "REGULAR"));
                        break;
                    case Movie.NEW_RELEASE:
                        thisAmount += each.getDaysRented() * 3;
                        newResult.Append(String.Format("{0,-15}|", "NEW RELEASE"));
                        break;

                    case Movie.CHILDRENS:
                        thisAmount += 1.5;
                        if (each.getDaysRented() > 3)
                            thisAmount += (each.getDaysRented() - 3) * 1.5;
                        newResult.Append(String.Format("{0,-15}|", "CHILDRENS"));
                        break;
                    case Movie.EXAMPLE_GENRE:
                        thisAmount += each.getDaysRented() * 2;
                        newResult.Append(String.Format("{0,-15}|", "EXAMPLE GENRE"));
                        break;
                }

                // Add frequent renter points
                frequentRenterPoints++;

                // Add bonus for a two day new release rental
                if ((each.getMovie().getPriceCode() == Movie.NEW_RELEASE)
                        && each.getDaysRented() > 1) frequentRenterPoints++;

                // Show figures for this rental
                result.AppendLine("\t" + each.getMovie().getTitle() + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
                newResult.AppendLine(String.Format("{0,-20}\t|{1,-7}|{2}", each.getMovie().getTitle(), each.getDaysRented().ToString(), thisAmount.ToString()));
            }

            result.AppendLine("Amount owed is " + totalAmount);
            result.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

            newResult.AppendLine("------------------------------------------------------------");
            newResult.AppendLine("Amount owed is " + totalAmount);
            newResult.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

            result.AppendLine(newResult.ToString());

            return result.ToString();
        }

        private string customerName;
        private List<Rental> customerRental = new List<Rental>();
    }
}
