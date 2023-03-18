using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VideoRental
{
    class InfoFile
    {
        public InfoFile()
        {

        }

        public void SaveReceiptFile(List<Customer> customersList)
        {
            String path = @".\RentalReceiptFile.txt";
            int index = 0;
            foreach (Customer customer in customersList)
            {
                if(index == 0)
                {
                    File.WriteAllText(path, customer.statement());
                    index++;
                }
                else
                {
                    File.AppendAllText(path, customer.statement());
                }
            }
        }
    }
}
