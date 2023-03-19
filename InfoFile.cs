using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace VideoRental
{
    class InfoFile
    {
        public String[] MovieChapterList;
        public InfoFile()
        {
            MovieChapterList = new string[Movie.CHAPTER_COUNT];
            MovieChapterList[Movie.REGULAR] = "REGULAR";
            MovieChapterList[Movie.NEW_RELEASE] = "NEW_RELEASE";
            MovieChapterList[Movie.CHILDRENS] = "CHILDRENS";
            MovieChapterList[Movie.EXAMPLE_GENRE] = "EXAMPLE_GENRE";
        }

        public void SaveReceiptFile(List<Customer> customersList)
        {
            String path = @".\RentalReceiptFile.txt";
            int index = 0;
            foreach (Customer customer in customersList)
            {
                if (index == 0)
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

        //영화 리스트 저장
        public void WriteMovieInfo(List<Movie> moviesList)
        {
            int iREGULAR = 0;
            int iNEW_RELEASE = 0;
            int iCHILDRENS = 0;
            int iEXAMPLE_GENRE = 0;
            foreach (Movie movie in moviesList)
            {
                String value = movie.getTitle();
                String key = "";
                switch (movie.getPriceCode())
                {
                    case Movie.REGULAR:
                        iREGULAR++;
                        key = String.Format("{0}{1}", MovieChapterList[Movie.REGULAR], iREGULAR);
                        break;
                    case Movie.NEW_RELEASE:
                        iNEW_RELEASE++;
                        key = String.Format("{0}{1}", MovieChapterList[Movie.NEW_RELEASE], iNEW_RELEASE);
                        break;
                    case Movie.CHILDRENS:
                        iCHILDRENS++;
                        key = String.Format("{0}{1}", MovieChapterList[Movie.CHILDRENS], iCHILDRENS);
                        break;
                    case Movie.EXAMPLE_GENRE:
                        iEXAMPLE_GENRE++;
                        key = String.Format("{0}{1}", MovieChapterList[Movie.EXAMPLE_GENRE], iEXAMPLE_GENRE);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(key))
                {
                    WriteSection(key, value);
                }
            }

            //읽을 때 장르별 몇개의 영화가 있는지 알기 위해 저장
            WriteSection("REGULAR", iREGULAR.ToString());
            WriteSection("NEW_RELEASE", iNEW_RELEASE.ToString());
            WriteSection("CHILDRENS", iCHILDRENS.ToString());
            WriteSection("EXAMPLE_GENRE", iEXAMPLE_GENRE.ToString());
        }

        //영화 정보 읽기
        public List<Movie> ReadMovieInfo()
        {
            List<Movie> movieList = new List<Movie>();

            for (int iChapter = 0; iChapter < Movie.CHAPTER_COUNT; iChapter++)
            {
                String sReadValue = ReadSection(MovieChapterList[iChapter]);
                int iReadCount;

                if (int.TryParse(sReadValue, out iReadCount))
                {
                    for (int iMovieChpaterNum = 1; iMovieChpaterNum <= iReadCount; iMovieChpaterNum++)
                    {
                        String sReadKey = String.Format("{0}{1}", MovieChapterList[iChapter], iMovieChpaterNum);
                        String sMovieTitle = ReadSection(sReadKey);

                        if (!string.IsNullOrEmpty(sMovieTitle))
                        {
                            Movie movie = new Movie(sMovieTitle, iChapter);
                            movieList.Add(movie);
                        }
                    }
                }
            }

            return movieList;
        }

        //Customer정보 저장
        public void WriteCustomerInfo(List<Customer> CustomerList)
        {
            //Customer 인원 저장
            WriteSection("CUSTOMER", CustomerList.Count.ToString());

            int iCUSTOMER = 0;
            foreach (Customer custom in CustomerList)
            {
                iCUSTOMER++;
                int iRentCount = 0;
                String customerName = custom.getName();
                String key = String.Format("CUSTOMER{0}", iCUSTOMER);

                //CustomerID 저장
                if (!string.IsNullOrEmpty(key))
                {
                    WriteSection(key, customerName);
                }

                //Customer의 렌탈 수 저장 : 읽을 때 갯수만큼 읽으면 됨
                WriteSection(customerName, custom.GetRentals().Count.ToString());

                //Customer의 Rental 정보 List 저장
                foreach (Rental rental in custom.GetRentals())
                {
                    iRentCount++;
                    String KeyName = String.Format("{0}_Title{1}", customerName, iRentCount);
                    String sRental_Title = rental.getMovie().getTitle();
                    WriteSection(KeyName, sRental_Title);

                    KeyName = String.Format("{0}_Proid{1}", customerName, iRentCount);
                    String sRental_Proid = rental.getDaysRented().ToString();
                    WriteSection(KeyName, sRental_Proid);
                }
            }
        }

        public void WriteRental(Customer customer, Rental rental)
        {
            String sCustomerName = customer.getName();
            int iRentNumber = customer.GetRentals().Count;
            //Customer의 렌탈 수 저장 : 읽을 때 갯수만큼 읽으면 됨
            WriteSection(sCustomerName, iRentNumber.ToString());

            String KeyName = String.Format("{0}_Title{1}", sCustomerName, iRentNumber);
            String sRental_Title = rental.getMovie().getTitle();
            WriteSection(KeyName, sRental_Title);

            KeyName = String.Format("{0}_Proid{1}", sCustomerName, iRentNumber);
            String sRental_Proid = rental.getDaysRented().ToString();
            WriteSection(KeyName, sRental_Proid);
        }

        public void ReturnInfo(Customer customer)
        {
            String sCustomerName = customer.getName();
            int iRentCount = 0;
            String KeyName;

            if (int.TryParse(ReadSection(sCustomerName),out iRentCount))
            {
                for(int i = 1; i <= iRentCount; i++)
                {
                    KeyName = String.Format("{0}_Title{1}", sCustomerName, i);
                    RemoveSection(KeyName);

                    KeyName = String.Format("{0}_Proid{1}", sCustomerName, i);
                    RemoveSection(KeyName);
                }
            }

            //Customer의 렌탈 수 저장 : 읽을 때 갯수만큼 읽으면 됨
            WriteSection(sCustomerName, customer.GetRentals().Count.ToString());

            //Customer의 Rental 정보 List 저장
            iRentCount = 0;
            foreach (Rental rental in customer.GetRentals())
            {
                iRentCount++;
                KeyName = String.Format("{0}_Title{1}", sCustomerName, iRentCount);
                String sRental_Title = rental.getMovie().getTitle();
                WriteSection(KeyName, sRental_Title);

                KeyName = String.Format("{0}_Proid{1}", sCustomerName, iRentCount);
                String sRental_Proid = rental.getDaysRented().ToString();
                WriteSection(KeyName, sRental_Proid);
            }
        }

        public List<Customer> ReadCustomerInfo(List<Movie> moviesList)
        {
            List<Customer> customerList = new List<Customer>();

            String sReadValue = ReadSection("CUSTOMER");
            int iReadCount;
            if (int.TryParse(sReadValue, out iReadCount))
            {
                for (int iCustomer = 1; iCustomer <= iReadCount; iCustomer++)
                {
                    String key = String.Format("CUSTOMER{0}", iCustomer);
                    String sCustomerName = ReadSection(key);

                    Customer cCustomer = new Customer(sCustomerName);

                    int iRentalCount;
                    if (int.TryParse(ReadSection(sCustomerName), out iRentalCount))
                    {
                        for (int iRental = 1; iRental <= iRentalCount; iRental++)
                        {
                            key = String.Format("{0}_Title{1}", sCustomerName, iRental);
                            String sRental_Title = ReadSection(key);

                            key = String.Format("{0}_Proid{1}", sCustomerName, iRental);
                            int iRental_Proid;

                            if (int.TryParse(ReadSection(key), out iRental_Proid))
                            {
                                foreach (Movie movie in moviesList)
                                {
                                    if (sRental_Title.Equals(movie.getTitle()))
                                    {
                                        cCustomer.addRental(new Rental(movie, iRental_Proid));
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    customerList.Add(cCustomer);
                }
            }

            return customerList;
        }


        private void WriteSection(String Key, String Value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection cfgColletion = config.AppSettings.Settings;

            foreach (string sKey in cfgColletion.AllKeys)
            {
                if (sKey.Equals(Key))
                {
                    cfgColletion.Remove(sKey);
                    break;
                }
            }
            cfgColletion.Add(Key, Value);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        private String ReadSection(String Key)
        {
            return ConfigurationManager.AppSettings[Key];
        }

        private void RemoveSection(String Key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection cfgColletion = config.AppSettings.Settings;
            cfgColletion.Remove(Key);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

    }
}
