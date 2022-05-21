using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flee.PublicTypes;

namespace NumericalAnalysis
{
    // Crucial Helper class to ensure good performance.
    public class NAFunc
    {
        string funcExpression;
        ExpressionContext context;
        Func<double, double> InternalFunc;
        IGenericExpression<double> eGeneric;

        public NAFunc(string _funcExpression)
        {
            context = new ExpressionContext();
            funcExpression = _funcExpression;
            context.Imports.AddType(typeof(Math));
            context.Variables["x"] = 0d;
            eGeneric = context.CompileGeneric<double>(_funcExpression);
            InternalFunc = new Func<double, double>(x => Evaluate(x));
        }
        public double Evaluate(double x)
        {
            context.Variables["x"] = x;
            return eGeneric.Evaluate();
        }
        public Func<double,double> ToFunc()
        {
            return InternalFunc;
        }
        public void Recompile()
        {
            eGeneric = context.CompileGeneric<double>(funcExpression);
        }
    }
}
