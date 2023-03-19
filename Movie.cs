using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
    public class Movie
    {
        public const int REGULAR        = 0;
        public const int NEW_RELEASE    = 1;
        public const int CHILDRENS      = 2;
        public const int EXAMPLE_GENRE  = 3;
        public const int CHAPTER_COUNT  = 4;

        public Movie(string title, int priceCode = REGULAR)
        {
            movieTitle = title;
            moviePriceCode = priceCode;
            rentCheck = true;
        }

        public int getPriceCode() { return moviePriceCode; }
        public void setPriceCode(int args) { moviePriceCode = args; }
        public string getTitle() { return movieTitle; }

        public void setRent(Boolean check) { rentCheck = check; }
        public Boolean getRentCheck() { return rentCheck; }

        private string movieTitle;
        private Boolean rentCheck;
        int moviePriceCode;
    }
}
