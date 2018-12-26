using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace func_calc_terver.ViewModels
{
    class MainViewModel:ViewModelBase
    {
        public bool IsAvailable { get; set; } = true;
        public bool IsUnavailable => !IsAvailable;
        public string Function { get; set; } = "-x^2/2+10*x-49";
        public double Min { get; set; } = 9;
        public double Max { get; set; } = 12;

        public string In { get; set; } =
            "0,141758477\r\n0,419385357\r\n0,288308359\r\n0,381847591\r\n0,059022797\r\n0,0990936\r\n0,088900418\r\n0,434949797\r\n0,309244057\r\n0,353617969\r\n0,402050844\r\n0,365123447\r\n0,392162847\r\n0,323587756\r\n0,491866817";
        public string Out { get; set; }
        public double Eps { get; set; } = 0.001;

        private Expression _expression;

        public ICommand CalculateCommand=>new DelegateCommand(() =>
        {
            Out = "";
            IsAvailable = false;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                    _expression = new Expression(Function);
                    _expression.addArguments(new Argument("x", 0));
                    var nums = In.Split('\n');
                    
                    foreach (var num in nums)
                    {
                        string res = "";
                        
                        for (var i = Min; i <= Max; i += Eps)
                        {

                            _expression.setArgumentValue("x", i);
                            var a = _expression.calculate();
                            if (Math.Abs(a - Convert.ToDouble(num.Replace(",","."))) < Eps)
                                res = i.ToString();
                        }
                        if (res.Length == 0) res = "null";


                            Application.Current.Dispatcher.Invoke(() => Out += res + '\n');
                    }
                    }
                    catch (Exception e)
                    {
                    }
                    Application.Current.Dispatcher.Invoke(() => IsAvailable=true);
                });


                //Expression.addArguments(new Argument("x",));


        });
    }
}
