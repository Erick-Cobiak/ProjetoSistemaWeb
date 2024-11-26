using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinário.Models;
using System.Data.SQLite;

namespace Sistema_Veterinário.Controllers
{
    public class ExibirProdutosController : Controller
    {
        public IActionResult ExibProd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Consultar(int rp, String produto, String preco)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"select * from produtos";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                SQLiteDataReader dr = comandoSQL.ExecuteReader();
                List<Produtos> listProdutos = new List<Produtos>();
                while (dr.Read())
                {
                    Produtos prod = new Produtos();
                    prod.Rp = dr.GetInt32(0);
                    prod.NomeProduto = dr.GetString(1);
                    prod.Preco = dr.GetString(2);
                    listProdutos.Add(prod);
                }
                sqlite_conn.Close();
                return Json(listProdutos);
            }
            catch (Exception ex)
            {
                return Json("Não foi possível consultar!!!");
            }
        }

        public SQLiteConnection pegarConexao()
        {
            SQLiteConnection sqlite_conn;
            String stringConnection = "Data Source=veterinario.db; Version = 3; New = True; Compress = True; ";
            sqlite_conn = new SQLiteConnection(stringConnection);

            // uma tabela
            sqlite_conn.Open();
            string sql = $"CREATE TABLE IF NOT EXISTS 'produtos'('rp' INTEGER, 'produto' TEXT, 'preco' TEXT, PRIMARY KEY('rp' AUTOINCREMENT))";
            SQLiteCommand comando = new SQLiteCommand(sql, sqlite_conn);
            comando.ExecuteNonQuery();
            sqlite_conn.Close();
            // para cada tabela, abrir e fechar a conexao


            return new SQLiteConnection(stringConnection);

        }
    }
}
