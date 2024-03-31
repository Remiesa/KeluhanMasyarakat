using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newKeluhanMasyarakat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            koneksi kon = new koneksi();
            kon.login();
        }
    }
}
