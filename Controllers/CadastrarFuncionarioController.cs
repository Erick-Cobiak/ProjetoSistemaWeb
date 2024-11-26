using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Veterinário.Controllers
{
    public class CadastrarFuncionarioController : Controller
    {
        public IActionResult CadFunc()
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
        public IActionResult cadastrar(string nome, string cargo, string email, string senha)
        {
            senha = GerarHashMd5(senha);

            String msg = "";
            try
            {
                SQLiteConnection con = pegarConexao();
                con.Open();
                string sql = $"insert into funcionario(nome, cargo, email, senha) values('{nome}','{cargo}','{email}','{senha}')";

                SQLiteCommand cmd = new SQLiteCommand(sql, con);

                cmd.ExecuteNonQuery();
                con.Close();

                msg = "Cadastro realizado com sucesso!";
            }
            catch (Exception e)
            {


                msg = "Não foi possivel realizar o cadastro! " + e.Message;
            }
            return Json(msg);
        }
        public SQLiteConnection pegarConexao()
        {
            String stringConnection = "Data Source=veterinario.db; Version = 3; New = True; Compress = True; ";
            return new SQLiteConnection(stringConnection);
        }
    }
}
