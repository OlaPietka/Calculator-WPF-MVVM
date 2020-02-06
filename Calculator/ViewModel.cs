using System;
using System.Windows.Input;

namespace Calculator
{
    class ViewModel : ViewModelBase
    {
        bool isSumDisplayed = true;      

        double left = 0; 
        double right; 
        string rememberOperator;
        bool firstOperation = true;

        public ViewModel()
        {
            Number = new RelayCommand(
                (parameter) => 
                {
                    if (isSumDisplayed || Equation.Equals("0")) 
                        Equation = (string)parameter;
                    else
                        Equation += parameter;

                    isSumDisplayed = false;             

                    RefreshCanExecutes();
                },
                (obj) =>
                {
                    return isSumDisplayed || Equation.Length < 16;
                });

            Point = new RelayCommand(
                (obj) =>
                {
                    if (Equation.Equals("0"))
                        Equation = "0.";
                    else
                        Equation += ".";

                    isSumDisplayed = false;

                    RefreshCanExecutes();
                },
                (obj) =>
                {
                    return isSumDisplayed || !Equation.Contains(".");
                });

            Operator = new RelayCommand(
               (parameter) =>
               {
                   if (firstOperation)
                   {
                       left = Double.Parse(Equation); 

                       firstOperation = false;
                       rememberOperator = (string)parameter;

                       HistoryEquation += left.ToString() + parameter; 
                   }
                   else 
                   {
                       right = Double.Parse(Equation);

                       HistoryEquation += right.ToString() + parameter;

                       switch (rememberOperator)    
                       {
                           case "+":
                               left += right;
                               break;

                           case "-":
                               left -= right;
                               break;

                           case "*":
                               left *= right;
                               break;

                           case "/":
                               left /= right;
                               break;
                       }
                       rememberOperator = (string)parameter;
                   }


                   Equation = left.ToString();
                   isSumDisplayed = true;

                   RefreshCanExecutes();
               },
               (parameter) =>
               {
                   return !isSumDisplayed;
               });

            Ce = new RelayCommand(
                (obj) =>
                {
                    left = 0;
                    HistoryEquation = "";
                    Equation = "0";
                    isSumDisplayed = false;

                    RefreshCanExecutes();
                });

            C = new RelayCommand(
                (obj) =>
                {
                    right = 0;
                    Equation = "0";
                    isSumDisplayed = false;

                    RefreshCanExecutes();
                },
                (obj) =>
                {
                    return !isSumDisplayed;
                });

            Bcs = new RelayCommand(
                (obj) =>
                {
                    Equation = Equation.Substring(0, Equation.Length - 1);
                    if (Equation.Length == 0)
                    {
                        Equation = "0";
                    }

                    RefreshCanExecutes();
                },
                (obj) =>
                {
                    return !isSumDisplayed && (Equation.Length > 1 || Equation[0] != '0');
                });
        }
        void RefreshCanExecutes()
        {
            ((RelayCommand)Bcs).OnCanExecuteChanged();
            ((RelayCommand)Number).OnCanExecuteChanged();
            ((RelayCommand)Point).OnCanExecuteChanged();
            ((RelayCommand)Operator).OnCanExecuteChanged();
        }
      
        public string Equation
        {
            private set { SetProperty(ref equation, value); }
            get { return equation; }
        }

        public string HistoryEquation
        {
            private set { SetProperty(ref historyEquation, value); }
            get { return historyEquation; }
        }

        public ICommand Number { private set; get; }
        public ICommand Point { private set; get; }
        public ICommand Operator { private set; get; }
        public ICommand Ce { private set; get; }
        public ICommand C { private set; get; }
        public ICommand Bcs { private set; get; }

        public string equation = "0";
        public string historyEquation = "";
    }
}
