using Microsoft.SqlServer.Server;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace newKeluhanMasyarakat
{
    internal class koneksi
    {
        MySqlConnection kon = new MySqlConnection("Server=localhost;Database=keluhanmasyarakat;user=root;password=");

        public void openCon()
        {
            try
            {
                kon.Open();
            }
            catch
            {
                MessageBox.Show("Server Error");
            }
        }

        public void closeCon()
        {
            kon.Close();
        }


        public void kotak(int x, int y, int x1, int y1, string subJudul)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("┌");

            Console.SetCursorPosition(x1, y);
            Console.Write("┐");

            Console.SetCursorPosition(x1, y1);
            Console.Write("┘");

            Console.SetCursorPosition(x, y1);
            Console.Write("└");

            for (int i = (x + 1); i < x1; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write("-");

                Console.SetCursorPosition(i, y1);
                Console.Write("-");
            }

            for (int j = (y + 1); j < y1; j++)
            {
                Console.SetCursorPosition(x, j);
                Console.WriteLine("|");

                Console.SetCursorPosition(x1, j);
                Console.WriteLine("|");
            }

            Console.SetCursorPosition(x + 3, y);
            Console.Write(subJudul);
        }

        public void tengah(int t, string isi)
        {
            Console.SetCursorPosition((Console.WindowWidth - isi.Length) / 2, t);
            Console.Write(isi);
        }

        public void menu(string menu1, string menu2, int x, int y, ConsoleColor warna, int getID)
        {
            string[] isi = { menu1, menu2, "Keluar" };

            for (int i = 0; i < isi.Length; i++)
            {
                colorWhite();
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine(isi[i]);
            }

            Console.ForegroundColor = warna;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(isi[0]);

            ConsoleKeyInfo tombol;
            Console.CursorVisible = false;

            int brs = 0;

            do
            {
                tombol = Console.ReadKey();
                switch (tombol.Key)
                {
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(x, y + brs);
                        colorWhite();
                        Console.WriteLine(isi[brs]);
                        if (brs < 2)
                        {
                            brs++;
                        }
                        else
                        {
                            brs = 0;
                        }
                        Console.SetCursorPosition(x, y + brs);
                        Console.ForegroundColor = warna;
                        Console.WriteLine(isi[brs]);
                        break;

                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(x, y + brs);
                        colorWhite();
                        Console.WriteLine(isi[brs]);
                        if (brs > 0)
                        {
                            brs--;
                        }
                        else
                        {
                            brs = 2;
                        }
                        Console.SetCursorPosition(x, y + brs);
                        Console.ForegroundColor = warna;
                        Console.WriteLine(isi[brs]);
                        break;

                    case ConsoleKey.Enter:

                        if (brs == 0)
                        {
                            if (apakahBenarMasyarakat == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("Isi Laporan");
                                Console.CursorVisible = true;
                                Console.SetCursorPosition((Console.WindowWidth - 30) / 2, 8);
                                string laporan = Console.ReadLine();

                                DialogResult result = MessageBox.Show("Apakah anda ingin mengirimkannya?", "Konfirmasi", MessageBoxButtons.YesNo);

                                if (result == DialogResult.Yes)
                                {
                                    kirimLaporan("insert into keluhan VALUES (NULL," + getID + ", '" + laporan + "', 'Diproses', NULL, @date, NULL, NULL)","@date", "Laporan Berhasil Dikirim");
                                    refreshMasyarakat(getID);
                                }
                                else
                                {
                                    refreshMasyarakat(getID);
                                }
                            }
                            else if (apakahBenarPetugas == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("List Laporan Keluhan");
                                Console.SetCursorPosition((Console.WindowWidth) / 2, 8);
                                Console.WriteLine();

                                lihatKeluhanPOVpetugas();

                                Console.CursorVisible = true;
                                Console.SetCursorPosition(25, 10);
                                colorWhite();
                                Console.Write("Masukkan ID : ");
                                string IDkeluhan = Console.ReadLine();

                                DialogResult result = MessageBox.Show("Silahkan pilih opsi", "Konfirmasi", MessageBoxButtons.YesNoCancel);

                                if (result == DialogResult.Yes)
                                {
                                    kirimLaporan("update keluhan SET tanggal_konfirmasi = @date, status = 'Dikonfirmasi' WHERE IDkeluhan = " + Convert.ToInt32(IDkeluhan) + "","@date", "Laporan Dikonfirmasi");
                                    refreshPetugas();
                                }
                                else if (result == DialogResult.No)
                                {
                                    kirimLaporan("delete from keluhan where IDkeluhan = " + Convert.ToInt32(IDkeluhan) + "","", "Laporan Dihapus");
                                    refreshPetugas();
                                }
                                else if (result == DialogResult.Cancel)
                                {
                                    refreshPetugas();

                                }
                            }
                            else if (apakahBenarPemerintah == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("List Laporan Keluhan");
                                Console.SetCursorPosition((Console.WindowWidth) / 2, 8);
                                Console.WriteLine();
                                lihatKeluhanPOVpemerintah();

                                Console.CursorVisible = true;
                                Console.SetCursorPosition(25, 10);
                                colorWhite();
                                Console.Write("Masukkan ID Laporan : ");
                                string IDkeluhan = Console.ReadLine();
                                Console.SetCursorPosition(25, Console.CursorTop);
                                colorWhite();
                                Console.WriteLine("Masukkan Tanggapan :");
                                Console.SetCursorPosition(25, Console.CursorTop);
                                string tanggapan = Console.ReadLine();

                                DialogResult result = MessageBox.Show("Apakah anda ingin mengirim Tanggapan?", "Konfirmasi", MessageBoxButtons.OKCancel);

                                if (result == DialogResult.OK)
                                {
                                    if (tanggapan == "")
                                    {
                                        MessageBox.Show("Laporan Tanggapan tidak boleh kosong");
                                        refreshPemerintah();
                                    }
                                    else
                                    {
                                        kirimLaporan("update keluhan SET tanggapan = '" + tanggapan + "', status = 'Ditanggapi', tanggal_tanggapan = @date WHERE IDkeluhan = " + Convert.ToInt32(IDkeluhan) + "","@date", "Berhasil Mengirim Tanggapan");
                                        refreshPemerintah();

                                    }

                                }
                                else if (result == DialogResult.Cancel)
                                {
                                    refreshPemerintah();
                                }
                            }
                        }
                        else if (brs == 1)
                        {
                            if (apakahBenarMasyarakat == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("List Laporan yang Ditanggapi");
                                Console.SetCursorPosition((Console.WindowWidth) / 2, 8);
                                Console.WriteLine();

                                lihatTanggapan(getID);

                                Console.ReadLine();

                                DialogResult result = MessageBox.Show("Ingin keluar dari 'Lihat Tanggapan'?", "Konfirmasi", MessageBoxButtons.OK);

                                if (result == DialogResult.OK)
                                {
                                    refreshMasyarakat(getID);
                                }
                            }
                            else if (apakahBenarPetugas == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("List History Laporan");
                                Console.SetCursorPosition((Console.WindowWidth) / 2, 8);
                                Console.WriteLine();

                                lihatKeluhanPOVpetugasHistory();

                                Console.ReadLine();


                                DialogResult result = MessageBox.Show("Ingin keluar dari 'History Laporan'?", "Konfirmasi", MessageBoxButtons.OK);
                                if (result == DialogResult.OK)
                                {
                                    refreshPetugas();
                                }
                            }
                            else if (apakahBenarPemerintah == true)
                            {
                                Console.SetCursorPosition(Console.WindowWidth / 2, 5);
                                colorWhite();
                                Console.WriteLine("List History Laporan");
                                Console.SetCursorPosition((Console.WindowWidth) / 2, 8);
                                Console.WriteLine();

                                lihatKeluhanPOVpemerintahHistory();

                                Console.ReadLine();

                                DialogResult result = MessageBox.Show("Ingin keluar dari 'History Laporan'?", "Konfirmasi", MessageBoxButtons.OK);
                                if (result == DialogResult.OK)
                                {
                                    refreshPemerintah();
                                }
                            }
                        }
                        else if (brs == 2)
                        {
                            Console.Clear();
                            Environment.Exit(0);
                        }
                        break;
                }
            } while (brs != 3);

        }
        public void kirimLaporan(string query, string param, string message)
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand(query, kon);
                cmd.Parameters.AddWithValue(param, DateTime.Now);
                cmd.ExecuteNonQuery();
                MessageBox.Show(message);
                closeCon();
            }
            catch
            {
                MessageBox.Show("Laporan Tidak Boleh Kosong");
            }
        }

        public void lihatTanggapan(int getID)
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand("select*from keluhan where status = 'Ditanggapi' AND IDpengguna = " + getID + "", kon);
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Dikirim : " + Convert.ToDateTime(read["tanggal_laporan"]).ToString("dd/MMMM/yyyy"));
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("Keluhan : ");
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine(read["keluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("-----------------");
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Ditanggapi : " + Convert.ToDateTime(read["tanggal_tanggapan"]).ToString("dd/MMMM/yyyy"));
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("Tanggapan : ");
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine(read["tanggapan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine();
                }
                closeCon();
            }
            catch
            {

            }
        }

        public void lihatKeluhanPOVpetugas()
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand("SELECT keluhan.IDkeluhan, keluhan.IDpengguna, keluhan.keluhan, keluhan.status, keluhan.tanggapan, keluhan.tanggal_laporan, pengguna.NIK, pengguna.nama FROM keluhan INNER JOIN pengguna ON keluhan.IDpengguna = pengguna.IDpengguna where status = 'Diproses'", kon);
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("NIK : " + read["NIK"].ToString());

                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Nama : " + read["nama"].ToString());

                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("ID Laporan : " + read["IDkeluhan"].ToString());

                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Dikirim : " + Convert.ToDateTime(read["tanggal_laporan"]).ToString("dd/MMMM/yyyy"));

                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Laporan Keluhan : ");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine(read["keluhan"].ToString());

                    Console.WriteLine();
                }
                closeCon();
            }
            catch
            {

            }
        }

        public void lihatKeluhanPOVpetugasHistory()
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand("SELECT keluhan.IDkeluhan, keluhan.IDpengguna, keluhan.keluhan, keluhan.status, keluhan.tanggapan, keluhan.tanggal_konfirmasi, pengguna.NIK, pengguna.nama FROM keluhan INNER JOIN pengguna ON keluhan.IDpengguna = pengguna.IDpengguna WHERE keluhan.status = 'Dikonfirmasi' OR keluhan.status = 'Ditanggapi'", kon);
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("NIK : " + read["NIK"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Nama : " + read["nama"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("ID Laporan :" + read["IDkeluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Laporan Keluhan : ");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine(read["keluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Status :" + read["status"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Dikonfirmasi : " + Convert.ToDateTime(read["tanggal_konfirmasi"]).ToString("dd/MMMM/yyyy"));
                    Console.WriteLine();
                }
                closeCon();
            }
            catch
            {

            }
        }

        public void lihatKeluhanPOVpemerintah()
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand("SELECT keluhan.IDkeluhan, keluhan.IDpengguna, keluhan.keluhan, keluhan.status, keluhan.tanggapan, pengguna.NIK, pengguna.nama FROM keluhan INNER JOIN pengguna ON keluhan.IDpengguna = pengguna.IDpengguna where keluhan.status = 'Dikonfirmasi'", kon);
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
                    Console.WriteLine("ID Laporan :" + read["IDkeluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Laporan Keluhan :");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine(read["keluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine();
                }
                closeCon();
            }
            catch
            {

            }
        }

        public void lihatKeluhanPOVpemerintahHistory()
        {
            try
            {
                openCon();
                MySqlCommand cmd = new MySqlCommand("select*from keluhan WHERE status = 'Ditanggapi'", kon);
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("ID Laporan : " + read["IDkeluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Dikirim :" + Convert.ToDateTime(read["tanggal_laporan"]).ToString("dd/MMMM/yyyy"));
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Laporan Keluhan : ");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine(read["keluhan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("-----------------");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Tanggal Ditanggapi :" + Convert.ToDateTime(read["tanggal_tanggapan"]).ToString("dd/MMMM/yyyy"));
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine("Laporan Tanggapan : ");
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine(read["tanggapan"].ToString());
                    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
                    Console.WriteLine();
                }
                closeCon();
            }
            catch
            {

            }
        }

        bool apakahBenarMasyarakat = false;
        public void akunMasyarakat(bool status)
        {
            apakahBenarMasyarakat = status;
        }

        bool apakahBenarPetugas = false;

        public void akunPetugas(bool status)
        {
            apakahBenarPetugas = status;
        }

        bool apakahBenarPemerintah = false;

        public void akunPemerintah(bool status)
        {
            apakahBenarPemerintah = status;
        }

        public void refreshMasyarakat(int id)
        {
            Console.Clear();
            kotak(0, 0, Console.WindowWidth - 1, 3, "");
            kotak(0, 4, 20, Console.WindowHeight - 6, "Menu");
            kotak(21, 4, Console.WindowWidth - 1, Console.WindowHeight - 6, "Masyarakat");
            kotak(0, Console.WindowHeight - 5, Console.WindowWidth - 1, Console.WindowHeight - 1, "");

            tengah(1, "Aplikasi Keluhan Masyarakat");
            tengah(2, "Informatika - 1");
            tengah(Console.WindowHeight - 4, "Ricky Ardhi Saputra");
            tengah(Console.WindowHeight - 3, "2023120064");

            menu("Kirim Laporan", "Lihat Tanggapan", 3, 7, ConsoleColor.Green, id);
        }

        public void refreshPetugas()
        {
            Console.Clear();
            kotak(0, 0, Console.WindowWidth - 1, 3, "");
            kotak(0, 4, 20, Console.WindowHeight - 6, "Menu");
            kotak(21, 4, Console.WindowWidth - 1, Console.WindowHeight - 6, "Petugas");
            kotak(0, Console.WindowHeight - 5, Console.WindowWidth - 1, Console.WindowHeight - 1, "");

            tengah(1, "Aplikasi Keluhan Masyarakat");
            tengah(2, "Informatika - 1");
            tengah(Console.WindowHeight - 4, "Ricky Ardhi Saputra");
            tengah(Console.WindowHeight - 3, "2023120064");

            menu("Konfirmasi Laporan", "Lihat History", 3, 7, ConsoleColor.Green, 0);
        }

        public void refreshPemerintah()
        {
            Console.Clear();
            kotak(0, 0, Console.WindowWidth - 1, 3, "");
            kotak(0, 4, 20, Console.WindowHeight - 6, "Menu");
            kotak(21, 4, Console.WindowWidth - 1, Console.WindowHeight - 6, "Pemerintah");
            kotak(0, Console.WindowHeight - 5, Console.WindowWidth - 1, Console.WindowHeight - 1, "");

            tengah(1, "Aplikasi Keluhan Masyarakat");
            tengah(2, "Informatika - 1");
            tengah(Console.WindowHeight - 4, "Ricky Ardhi Saputra");
            tengah(Console.WindowHeight - 3, "2023120064");

            menu("Beri Tanggapan", "Lihat History", 3, 7, ConsoleColor.Green, 0);
        }

        public bool masyarakat = false, petugas = false, pemerintah = false;

        public void login()
        {
            bool login = false;
            string lvl, sambutan = "";
            int getID = 0;

            do
            {
                colorWhite();
                string usr = "username : ";
                colorWhite();
                string pwd = "password : ";
                Console.SetCursorPosition((Console.WindowWidth - usr.Length) / 2, (Console.WindowHeight - 1) / 2);
                Console.Write(usr);
                string username = Console.ReadLine();

                Console.SetCursorPosition((Console.WindowWidth - pwd.Length) / 2, Console.WindowHeight / 2);
                Console.Write(pwd);
                string password = Console.ReadLine();

                try
                {
                    openCon();
                    MySqlCommand cmd = new MySqlCommand("select * from pengguna WHERE username = '" + username + "' AND password = '" + password + "'", kon);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lvl = reader["IDlevel"].ToString();
                        getID = Convert.ToInt32(reader["IDpengguna"]);
                        login = true;
                        if (lvl == "1")
                        {
                            Console.Clear();
                            masyarakat = true;
                            sambutan = "Masyarakat";
                            akunMasyarakat(true);

                        }
                        else if (lvl == "2")
                        {
                            Console.Clear();
                            petugas = true;
                            sambutan = "Petugas";
                            akunPetugas(true);
                        }
                        else if (lvl == "3")
                        {
                            Console.Clear();
                            pemerintah = true;
                            sambutan = "Pemerintah";
                            akunPemerintah(true);
                        }
                    }
                    closeCon();
                }
                catch
                {
                    login = false;
                }

                if (login == false)
                {
                    Console.Clear();
                    Console.SetCursorPosition((Console.WindowWidth - usr.Length) / 2, (Console.WindowHeight - 3) / 2);
                    colorWhite();
                    Console.WriteLine("Data Login Salah");
                }

            } while (login == false);

            Console.Clear();

            kotak(0, 0, Console.WindowWidth - 1, 3, "");
            kotak(0, 4, 20, Console.WindowHeight - 6, "Menu");
            kotak(21, 4, Console.WindowWidth - 1, Console.WindowHeight - 6, sambutan);
            kotak(0, Console.WindowHeight - 5, Console.WindowWidth - 1, Console.WindowHeight - 1, "");

            tengah(1, "Aplikasi Keluhan Masyarakat");
            tengah(2, "Informatika - 1");
            tengah(Console.WindowHeight - 4, "Ricky Ardhi Saputra");
            tengah(Console.WindowHeight - 3, "2023120064");

            if (masyarakat == true)
            {
                menu("Kirim Laporan", "Lihat Tanggapan", 3, 7, ConsoleColor.Green, getID);
            }
            else if (petugas == true)
            {
                menu("Konfirmasi Laporan", "Lihat History", 3, 7, ConsoleColor.Green, getID);
            }
            else if (pemerintah == true)
            {
                menu("Beri Tanggapan", "Lihat History", 3, 7, ConsoleColor.Green, getID);
            }
        }

        private void colorWhite()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
