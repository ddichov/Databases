using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace SuperMarket.Excel.Client
{
    public class ExcelReadController
    {
        private OleDbConnection currentConnection;

        public ExcelReadController()
        {
            this.currentConnection = new OleDbConnection();
        }

        public void SendDataTo()
        {
        }

        public OleDbConnection CurrentConnection
        {
            get
            {
                return this.currentConnection;
            }
        }
    }
}
