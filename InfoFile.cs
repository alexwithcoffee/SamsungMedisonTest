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
                    case 0:
                        iREGULAR++;
                        key = String.Format("REGULAR{0}", iREGULAR);
                        break;
                    case 1:
                        iNEW_RELEASE++;
                        key = String.Format("NEW_RELEASE{0}", iNEW_RELEASE);
                        break;
                    case 2:
                        iCHILDRENS++;
                        key = String.Format("CHILDRENS{0}", iCHILDRENS);
                        break;
                    case 3:
                        iEXAMPLE_GENRE++;
                        key = String.Format("EXAMPLE_GENRE{0}", iEXAMPLE_GENRE);
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

        //Customer정보 저장
        public void WriteCustomerInfo(List<Customer> CustomerList)
        {
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


                //Customer의 Rental 정보 List 저장
                foreach(Rental rental in custom.GetRentals())
                {
                    iRentCount++;
                    String KeyName = String.Format("{0}_Title{1}", customerName, iRentCount);
                    String sRental_Title = rental.getMovie().getTitle();
                    WriteSection(KeyName, sRental_Title);

                    KeyName = String.Format("{0}_Proid{1}", customerName, iRentCount);
                    String sRental_Proid = rental.getDaysRented().ToString();
                    WriteSection(KeyName, sRental_Proid);
                }

                //Customer의 렌탈 수 저장 : 읽을 때 갯수만큼 읽으면 됨
                WriteSection(customerName, iRentCount.ToString());

            }
            //Customer 인원 저장
            WriteSection("CUSTOMER", iCUSTOMER.ToString());
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
    }
}
