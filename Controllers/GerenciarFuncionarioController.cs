using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinário.Models;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Veterinário.Controllers
{
    public class GerenciarFuncionarioController : Controller
    {
        public IActionResult PageFunc()
        {
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

        [HttpGet]
        public IActionResult Inserir(int rf, String nome, String cargo, String email, String senha)
        {
            senha = GerarHashMd5(senha);

            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();
                string sql = $"INSERT INTO funcionario (nome,cargo,email,senha) VALUES ('{nome}','{cargo}','{email}','{senha}')";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);

                comandoSQL.ExecuteNonQuery();
                int rfInserido = Convert.ToInt32(sqlite_conn.LastInsertRowId);
                sqlite_conn.Close();
                return Json("Registro inserido com sucesso! RF " + rfInserido + " gerado!!!");
            }
            catch (Exception ex)
            {
                return Json("Não foi possível inserir!!!");
            }
        }

        [HttpGet]
        public IActionResult Consultar(int rf, String nome, String cargo, String email, String senha)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"select * from funcionario";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                SQLiteDataReader dr = comandoSQL.ExecuteReader();
                List<Funcionario> listFuncionario = new List<Funcionario>();
                while (dr.Read())
                {
                    Funcionario func = new Funcionario();
                    func.Rf = dr.GetInt32(0);
                    func.Nome = dr.GetString(1);
                    func.Cargo = dr.GetString(2);
                    func.Email = dr.GetString(3);
                    func.Senha = dr.GetString(4);
                    listFuncionario.Add(func);
                }
                sqlite_conn.Close();
                return Json(listFuncionario);
            }
            catch (Exception ex)
            {
                return Json("Não foi possível consultar!!!");
            }
        }

        [HttpGet]
        public IActionResult Alterar(int rf, String nome, String cargo, String email, String senha)
        {
            senha = GerarHashMd5(senha);

            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"UPDATE funcionario set nome='{nome}',cargo='{cargo}',email='{email}',senha='{senha}' where rf={rf}";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                comandoSQL.ExecuteNonQuery();
                sqlite_conn.Close();
                return Json("Registro alterado com sucesso!!!");
            }
            catch (Exception ex)
            {
                return Json("Não foi possível alterar!!!");
            }
        }

        [HttpGet]
        public IActionResult Excluir(int rf, String nome, String cargo, String email, String senha)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"delete from funcionario where rf='{rf}'";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                comandoSQL.ExecuteNonQuery();
                sqlite_conn.Close();
                return Json("Registro excluído com sucesso!!!");
            }
            catch (Exception ex)
            {
                return Json("Não foi possível excluir!!!");
            }
        }

        public SQLiteConnection pegarConexao()
        {
            String stringConnection = "Data Source=veterinario.db; Version = 3; New = True; Compress = True; ";
            return new SQLiteConnection(stringConnection);
        }
    }
}