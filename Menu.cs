using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VideoRental
{
    public class Menu
    {
        private int iMenu;
        private int iStartNumber;
        private Boolean bFinish;
        private String sInputCustomer;
        private String sInputVideoTitle;
        private int iInputPeriod;
        private List<Movie> MoviesList = new List<Movie>();
        private List<Customer> CustomerList = new List<Customer>();
        private Customer CheckCustomer;
        private Movie CheckMovie;

        public Menu()
        {
            iStartNumber = 0;
            iMenu = iStartNumber;
            bFinish = true;
        }

        public void Start()
        {
            Movie regular1 = new Movie("일반 1", Movie.REGULAR);
            MoviesList.Add(regular1);
            Movie regular2 = new Movie("일반 2", Movie.REGULAR);
            MoviesList.Add(regular2);
            Movie newRelease1 = new Movie("신작 1", Movie.NEW_RELEASE);
            MoviesList.Add(newRelease1);
            Movie newRelease2 = new Movie("신작 2", Movie.NEW_RELEASE);
            MoviesList.Add(newRelease2);
            Movie children1 = new Movie("어린이 1", Movie.CHILDRENS);
            MoviesList.Add(children1);
            Movie children2 = new Movie("어린이 2", Movie.CHILDRENS);
            MoviesList.Add(children2);
            Customer customer = new Customer("고객");
            CustomerList.Add(customer);

            customer.addRental(new Rental(regular1, 2));
            customer.addRental(new Rental(regular2, 3));
            customer.addRental(new Rental(newRelease1, 1));
            customer.addRental(new Rental(newRelease2, 2));
            customer.addRental(new Rental(children1, 3));
            customer.addRental(new Rental(children2, 4));

            while (bFinish)
            {
                switch (iMenu-iStartNumber)
                {
                    case 0:
                        MainMenu();
                        break;
                    case 1:
                        AllVideoTitle();
                        break;
                    case 2:
                        RentalMenu();
                        break;
                    case 3:
                        ReturnMenu();
                        break;
                    case 4:
                        SaveFile();
                        break;
                    case 5:
                        ReceiptMenu();
                        break;
                    default:
                        MainMenu();
                        break;
                }
            }
            
        }

        public void MainMenu()
        {
            Console.WriteLine("--Main Menu---");
            string test = string.Format("%d : Print All Video Title", iStartNumber);
            Console.WriteLine(string.Format("{0} : Print All Video Title",iStartNumber));
            Console.WriteLine(string.Format("{0} : Rental", iStartNumber+1));            // Rental 메뉴로 이동
            Console.WriteLine(string.Format("{0} : Return", iStartNumber+2));            // Return 메뉴로 이동
            Console.WriteLine(string.Format("{0} : Save to File", iStartNumber+3));      // 현재 Rental 한 모든 고객 정보를 영수증 스타일로 파일로 저장
            Console.WriteLine(string.Format("{0} : Receipt", iStartNumber+4));           // 고객ID를 입력받아 해당 고객이 대여한 비디오 영수증을 출력
            Console.WriteLine(string.Format("{0} : Exit", iStartNumber+5));
            Console.WriteLine();

            Console.Write("Select Menu: ");
            String input = Console.ReadLine().Trim();
            try
            {
                int inputNum = int.Parse(input) - iStartNumber;
                if (inputNum > 5)
                {
                    Console.WriteLine(string.Format("Please, Enter Only {0}~{1}", iStartNumber, iStartNumber + 5));
                    Thread.Sleep(2000);
                    return;
                }
                iMenu = inputNum + 1;
            }
            catch
            {
                Console.WriteLine(string.Format("Please, Enter Only Number (%d~%d)", iStartNumber, iStartNumber + 5));
                Thread.Sleep(500);
            }
            if (iMenu == 6 + iStartNumber)
            {
                bFinish = false;
            }
            return;
        }

        private void AllVideoTitle()
        {
            Console.WriteLine("---Video Title-----");
            Console.WriteLine("Number |  Title");

            int index = 0;
            foreach (Movie movie in MoviesList)
            {
                index++;
                Console.WriteLine(String.Format("{0,-5}  |  {1}", index.ToString(), movie.getTitle()));
            }
            iMenu = iStartNumber;
            return;
        }

        private void RentalMenu()
        {
            Console.WriteLine("---Rental Menu-----");

            InputCustomerID();

            InputVideoTitle();

            InputPeriod();

            RentalProcess();

            Continue();

        }

        private void ReturnMenu()
        {
            Console.WriteLine("---Return Menu-----");

            InputCustomerID();
            
            InputVideoTitle();

            Continue();

        }

        private void SaveFile()
        {
            Console.WriteLine("---File Save-----");
            iMenu = iStartNumber;
            return;
        }

        private void ReceiptMenu()
        {
            Console.WriteLine("---Receipt Menu-----");

            InputCustomerID();
            PrintReceipt();
            iMenu = iStartNumber;
            return;
        }

        private void InputCustomerID()
        {
            Boolean check = false;
            while (!check)
            {
                Console.Write("Input Customer ID : ");
                sInputCustomer = Console.ReadLine().Trim();
                foreach (Customer cust in CustomerList)
                {
                    if (sInputCustomer == cust.getName())
                    {
                        CheckCustomer = cust;
                        check = true;
                        break;
                    }
                }
                if (CheckCustomer != null)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Customer ID does not exist.");
                }
            }
        }

        private void InputVideoTitle()
        {
            Boolean check = false;
            while (!check)
            {
                Console.Write("Input Video Title : ");
                sInputVideoTitle = Console.ReadLine();
                foreach (Movie movie in MoviesList)
                {
                    if (sInputVideoTitle == movie.getTitle())
                    {
                        CheckMovie = movie;
                        check = true;
                        break;
                    }
                }

                if (CheckMovie != null)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Video Title does not exist.");
                }
            }
        }

        private void InputPeriod()
        {
            while (true)
            {
                Console.Write("Input Period : ");
                String inputPeriod = Console.ReadLine().Trim();
                try
                {
                    iInputPeriod = int.Parse(inputPeriod);
                    return;
                }
                catch
                {
                    Console.WriteLine("Please, Enter Only Number");
                    Thread.Sleep(500);
                }
            }
        }

        private void Continue()
        {
            while (true)
            {
                Console.Write("Continue? (Y/N) : ");
                String Continue = Console.ReadLine();

                if (Continue == "Y" || Continue == "y")
                {
                    return;
                }
                else if (Continue == "N" || Continue == "n")
                {
                    iMenu = iStartNumber;
                    return;
                }
                else
                {
                    Console.WriteLine("Please, Enter Only Y or N");
                }
            }
        }

        private void RentalProcess()
        {
            CheckCustomer.addRental(new Rental(CheckMovie, iInputPeriod));
            CheckClear();
        }

        private void PrintReceipt()
        {
            Console.Write(CheckCustomer.statement());
            CheckClear();
        }

        private void CheckClear()
        {
            CheckCustomer = null;
            CheckMovie = null;
            iInputPeriod = 0;
        }
    }
}
