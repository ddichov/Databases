using System;
using System.Data.Linq;
using System.Linq;

namespace Northwind.Data
{
    // 08. By inheriting the Employee entity class create a class which allows employees 
    // to access their corresponding territories as property of type EntitySet<T>.
    public partial class Employee
    {
        public EntitySet<Territory> Teritory
        {
            get
            {
                var teritotyes = new EntitySet<Territory>();
                teritotyes.AddRange(this.Teritory);
                return teritotyes;
            }
        }
    }
}