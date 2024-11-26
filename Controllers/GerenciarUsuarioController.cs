using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinário.Models;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Veterinário.Controllers
{
    public class GerenciarUsuarioController : Controller
    {
        public IActionResult PageUsu()
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
        public IActionResult Inserir(int ru, String nome, String email, String senha, int contato)
        {
            senha = GerarHashMd5(senha);

            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();
                string sql = $"INSERT INTO usuario (nome,email,senha,contato) VALUES ('{nome}','{email}','{senha}','{contato}')";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);

                comandoSQL.ExecuteNonQuery();
                int rfInserido = Convert.ToInt32(sqlite_conn.LastInsertRowId);
                sqlite_conn.Close();
                return Json("Usuario inserido com sucesso! ");
            }
            catch (Exception ex)
            {
                return Json("Não foi possível inserir!!!");
            }
        }

        [HttpGet]
        public IActionResult Consultar(int ru, String nome, String email, String senha, int contato)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"select * from usuario";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                SQLiteDataReader dr = comandoSQL.ExecuteReader();
                List<Usuario> listUsuario = new List<Usuario>();
                while (dr.Read())
                {
                    Usuario usu = new Usuario();
                    usu.Ru = dr.GetInt32(0);
                    usu.Nome = dr.GetString(1);
                    usu.Email = dr.GetString(2);
                    usu.Senha = dr.GetString(3);
                    usu.Contato = dr.GetInt32(4);
                    listUsuario.Add(usu);
                }
                sqlite_conn.Close();
                return Json(listUsuario);
            }
            catch (Exception ex)
            {
                return Json("Não foi possível consultar!!!");
            }
        }

        [HttpGet]
        public IActionResult Alterar(int ru, String nome, String email, String senha, int contato)
        {
            senha = GerarHashMd5(senha);

            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"UPDATE usuario set nome='{nome}',email='{email}',senha='{senha}',contato='{contato}' where ru='{ru}'";

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
        public IActionResult Excluir(int ru, String nome, String email, String senha, int contato)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"delete from usuario where ru='{ru}'";

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
