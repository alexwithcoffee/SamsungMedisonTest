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

        public Menu()
        {
            iStartNumber = 0;
            iMenu = iStartNumber;
            bFinish = true;
        }

        public void Start()
        {
            while(bFinish)
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
                Thread.Sleep(2000);
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
            iMenu = iStartNumber;
            return;
        }

        private void RentalMenu()
        {
            Console.WriteLine("---Rental Menu-----");

            InputCustomerID();

            InputVideoTitle();

            InputPeriod();

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
            iMenu = iStartNumber;
            return;
        }

        private void InputCustomerID()
        {
            while(true)
            {
                Console.Write("Input Customer ID : ");
                sInputCustomer = Console.ReadLine();
                return;
            }
        }

        private void InputVideoTitle()
        {
            while (true)
            {
                Console.Write("Input Video Title : ");
                sInputVideoTitle = Console.ReadLine();
                return;
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
    }
}
