using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using Sistema_Veterinário.Models;

namespace Sistema_Veterinário.Controllers
{
    public class AreaAdmController : Controller
    {
        public IActionResult PageLogin()
        {
            return View();
        }

        public IActionResult PageAdm()
        {
            // aqui
            if (HttpContext.Session.GetString("nome") == null) {  
                return View("PageLogin");
            }
            
            return View();
        }

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));// Converter a String para array de bytes, que é como a biblioteca trabalha.
            StringBuilder sBuilder = new StringBuilder();// Cria-se um StringBuilder para recompôr a string.
            for (int i = 0; i < data.Length; i++)// Loop para formatar cada byte como uma String em hexadecimal
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        [HttpPost]
        public String Logout()
        {
            try
            {
                HttpContext.Session.Remove("nome");
                HttpContext.Session.Clear();
                return "ok";
            }
            catch (Exception ex)
            {
                return "falha: " + ex.Message;
            }

        }

        [HttpPost]
        public string Login(string email, string senha)
        {
            /*
            HttpContext.Session.SetString("nome", "Leandro");
			string resultado = "ok";
            return resultado;
            */


            String resultado = "";
            SQLiteConnection? sqlite_conn = null;
            SQLiteDataReader? dr = null;
            try
            {
                sqlite_conn = Conexao.Nova();
                sqlite_conn.Open();
                senha = GerarHashMd5(senha);
                string sql = $"select * from funcionario where email='{email}' and senha='{senha}'";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                dr = comandoSQL.ExecuteReader();

                if (dr.Read())
                {
                    HttpContext.Session.SetString("nome", (string)dr["nome"]); // LEMBRAR !! IMPORTANTE !!!
                    resultado = "ok";
                }
                else
                {
                    resultado = "falha";
                }
                dr.Close();
                sqlite_conn.Close();

                return resultado;
            }
            catch (Exception ex)
            {
                if (dr != null)
                {
                    dr.Close();
                }
                if (sqlite_conn != null)
                {
                    sqlite_conn.Close();
                }
                return "falha";
            }

        }
    }
}
