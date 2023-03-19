using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VideoRental
{
    public class Menu
    {
        //메뉴 리스트 정보 : 메뉴 관련 변경시 값을 전체 값을 변경을 없애기 위함
        public const int iMainMenu          = 0;
        public const int iAllVideoTitle     = 1;
        public const int iRentalMenu        = 2;
        public const int iReturnMenu        = 3;
        public const int iSaveFile          = 4;
        public const int iReceiptMenu       = 5;
        public const int iRegistCustomer    = 6;
        public const int iRegistMovie       = 7;
        public const int iExite             = 8;


        private int iMenu;
        private int iStartNumber;
        private Boolean bFinish;
        private int iInputPeriod;
        private List<Movie> MoviesList = new List<Movie>();
        private List<Customer> CustomerList = new List<Customer>();
        private Customer CheckCustomer;
        private Movie CheckMovie;
        private InfoFile file;

        public Menu()
        {
            iStartNumber = 0;
            iMenu = iStartNumber;
            bFinish = true;

            file = new InfoFile();
        }

        public void Start()
        {
            InfoRead();

            if(MoviesList.Count == 0 && CustomerList.Count == 0)
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
            }

            while (bFinish)
            {
                switch (iMenu - iStartNumber)
                {
                    case iMainMenu:
                        MainMenu();
                        break;
                    case iAllVideoTitle:
                        AllVideoTitle();
                        break;
                    case iRentalMenu:
                        RentalMenu();
                        break;
                    case iReturnMenu:
                        ReturnMenu();
                        break;
                    case iSaveFile:
                        SaveFile();
                        break;
                    case iReceiptMenu:
                        ReceiptMenu();
                        break;
                    case iRegistCustomer:
                        RegistCustomerMenu();
                        break;
                    case iRegistMovie:
                        RegistMovieMenu();
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
            Console.WriteLine(string.Format("{0} : Print All Video Title", iStartNumber + iAllVideoTitle - 1));
            Console.WriteLine(string.Format("{0} : Rental", iStartNumber + iRentalMenu - 1));                       // Rental 메뉴로 이동
            Console.WriteLine(string.Format("{0} : Return", iStartNumber + iReturnMenu - 1));                       // Return 메뉴로 이동
            Console.WriteLine(string.Format("{0} : Save to File", iStartNumber + iSaveFile - 1));                   // 현재 Rental 한 모든 고객 정보를 영수증 스타일로 파일로 저장
            Console.WriteLine(string.Format("{0} : Receipt", iStartNumber + iReceiptMenu - 1));                     // 고객ID를 입력받아 해당 고객이 대여한 비디오 영수증을 출력
            Console.WriteLine(string.Format("{0} : Regist Customer", iStartNumber + iRegistCustomer - 1));          // 고객ID 추가
            Console.WriteLine(string.Format("{0} : Regist Movie", iStartNumber + iRegistMovie - 1));             // 영화 추가
            Console.WriteLine(string.Format("{0} : Exit", iStartNumber + iExite - 1));
            Console.WriteLine();

            Console.Write("Select Menu: ");
            String input = Console.ReadLine().Trim();
            try
            {
                int inputNum = int.Parse(input) - iStartNumber;
                if (inputNum > iStartNumber + iExite - 1 || inputNum < iStartNumber)
                {
                    Console.WriteLine(string.Format("Please, Enter Only {0}~{1}", iStartNumber, iStartNumber + iExite - 1));
                    Thread.Sleep(2000);
                    return;
                }
                iMenu = inputNum + 1;
            }
            catch
            {
                Console.WriteLine(string.Format("Please, Enter Only Number (%d~%d)", iStartNumber, iStartNumber + iExite - 1));
                Thread.Sleep(500);
            }
            if (iMenu == iExite + iStartNumber)
            {
                InfoSave();
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

            ReturnProcess();

            Continue();

        }

        private void SaveFile()
        {
            Console.WriteLine("---File Save-----");
            file.SaveReceiptFile(CustomerList);

            iMenu = iStartNumber;
            return;
        }

        private void ReceiptMenu()
        {
            Console.WriteLine("---Receipt Menu-----");

            InputCustomerID();

            PrintReceipt();
            
            CheckClear();
            return;
        }

        private void RegistCustomerMenu()
        {
            Console.WriteLine("---Regist Customer Menu-----");

            while(true)
            {
                Boolean CompleteCheck = true;
                Console.Write("Input New Customer ID : ");
                String sInputCustomer = Console.ReadLine().Trim();
                
                if(string.IsNullOrEmpty(sInputCustomer))
                {
                    Console.WriteLine("Please, Enter Customer ID.");
                    CompleteCheck = false;
                }
                else if(sInputCustomer.Length >= "CUSTOMER".Length && sInputCustomer.Substring(0,"CUSTOMER".Length).ToUpper().Equals("CUSTOMER"))
                {
                    Console.WriteLine("\"CUSTOMER\" can't be used  as a Customer ID.");
                    CompleteCheck = false;
                }
                else
                {
                    foreach (Customer cust in CustomerList)
                    {
                        if (sInputCustomer == cust.getName())
                        {
                            Console.WriteLine("Customer ID is already exists.");
                            CompleteCheck = false;
                            break;
                        }
                    }
                }
                if(CompleteCheck)
                {
                    Customer customer = new Customer(sInputCustomer);
                    CustomerList.Add(customer);
                    file.WriteNewCustomer(customer, CustomerList.Count);
                    Console.WriteLine("Customer registration completed.");
                    iMenu = iStartNumber;
                    return;
                }
            }

        }

        private void RegistMovieMenu()
        {
            Console.WriteLine("---Regist Movie Menu-----");

            String sInputVideoTitle;
            int iGenreNumber;
            while (true)
            {
                Boolean TitleCheck = true;
                Console.Write("Input New Movie Title : ");
                sInputVideoTitle = Console.ReadLine();
                foreach(Movie movie in MoviesList)
                {
                    if(sInputVideoTitle.Equals(movie.getTitle()))
                    {
                        Console.WriteLine(String.Format("{0} is already exists.", sInputVideoTitle));
                        TitleCheck = false;
                    }
                }
                if(TitleCheck)
                {
                    break;
                }
            }

            while(true)
            {
                Console.WriteLine("--- Movie Genre List ---");
                for (int iGenre = 0; iGenre < Movie.GENRE_COUNT; iGenre++)
                {
                    Console.WriteLine(String.Format("{0,-2}|{1}", iGenre + iStartNumber, Movie.MovieGenreList[iGenre]));
                }
                Console.Write("Select Genre Number: ");
                String inputGenre = Console.ReadLine().Trim();
                if (int.TryParse(inputGenre, out iGenreNumber))
                {
                    if(iStartNumber <= iGenreNumber && iGenreNumber < Movie.GENRE_COUNT)
                    {
                        Movie movie = new Movie(sInputVideoTitle, iGenreNumber);
                        MoviesList.Add(movie);
                        break;
                    }
                }
            }

            file.WriteMovieInfo(MoviesList);

            while (true)
            {
                Console.Write("Do you want to add more? (Y/N) : ");
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

        private void InputCustomerID()
        {
            if(CheckCustomer != null)
            {
                Console.WriteLine(String.Format("Current Customer ID : {0}", CheckCustomer.getName()));
            }
            while (CheckCustomer == null)
            {
                Console.Write("Input Customer ID : ");
                String sInputCustomer = Console.ReadLine().Trim();
                foreach (Customer cust in CustomerList)
                {
                    if (sInputCustomer == cust.getName())
                    {
                        CheckCustomer = cust;
                        break;
                    }
                }
                if (CheckCustomer == null)
                {
                    Console.WriteLine("Customer ID does not exist.");
                }
            }
        }

        private void InputVideoTitle()
        {
            while (true)
            {
                Console.Write("Input Video Title : ");
                String sInputVideoTitle = Console.ReadLine();
                foreach (Movie movie in MoviesList)
                {
                    if (sInputVideoTitle == movie.getTitle())
                    {
                        CheckMovie = movie;
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
                    CheckClear(false);
                    return;
                }
                else if (Continue == "N" || Continue == "n")
                {
                    CheckClear();
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
            if(CheckMovie.getRentCheck())
            {
                Rental new_rent = new Rental(CheckMovie, iInputPeriod);
                CheckCustomer.addRental(new_rent);
                file.WriteRental(CheckCustomer, new_rent);
            }
            else
            {
                Console.WriteLine(String.Format("Rent Fail : \"{0}\" has already been rented!!", CheckMovie.getTitle()));
            }
        }

        private void ReturnProcess()
        {
            if (CheckCustomer.removeRental(new Rental(CheckMovie, 0)))
            {
                file.ReturnInfo(CheckCustomer);
            }
            else
            {
                Console.WriteLine("This is not a movie that the customer rented!!");
            }
        }

        private void PrintReceipt()
        {
            Console.Write(CheckCustomer.statement());
        }

        private void CheckClear(Boolean customer = true)
        {
            if(customer)
            {
                CheckCustomer = null;
                iMenu = iStartNumber;
            }
            CheckMovie = null;
            iInputPeriod = 0;
        }

        private void InfoSave()
        {
            file.WriteMovieInfo(MoviesList);
            file.WriteCustomerInfo(CustomerList);
        }

        private void InfoRead()
        {
            MoviesList = file.ReadMovieInfo();
            CustomerList = file.ReadCustomerInfo(MoviesList);
        }
    }
}
