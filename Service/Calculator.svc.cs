using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Calculator : ICalculator
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        #region Common Methods

        /// <summary>
        /// проверка соединения
        /// </summary>
        /// <returns> OK </returns>
        public string TestConnection()
        {
            return "OK";
        }

        #endregion

        #region Arithmetic

        /// <summary>
        /// сложение
        /// </summary>
        /// <param name="a"> слагаемое 1 </param>
        /// <param name="b"> слагаемое 2 </param>
        /// <returns> сумма </returns>
        public double Addition(double a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// вычитание
        /// </summary>
        /// <param name="a"> уменьшаемое </param>
        /// <param name="b"> вычитаемое </param>
        /// <returns> разность </returns>
        public double Subtraction(double a, double b)
        {
            return a - b;
        }

        /// <summary>
        /// умножение
        /// </summary>
        /// <param name="a"> множитель 1 </param>
        /// <param name="b"> множитель 2 </param>
        /// <returns> произведение </returns>
        public double Multiplication(double a, double b)
        {
            return a * b;
        }

        /// <summary>
        /// деление
        /// </summary>
        /// <param name="a"> делимое </param>
        /// <param name="b"> делитель </param>
        /// <returns> частное </returns>
        public double Division(double a, double b)
        {
            return a / b;
        }

        #endregion
    }
}
