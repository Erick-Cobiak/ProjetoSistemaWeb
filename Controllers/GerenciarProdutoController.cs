using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinário.Models;
using System.Data.SQLite;

namespace Sistema_Veterinário.Controllers
{
    public class GerenciarProdutoController : Controller
    {
        public IActionResult PageProd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inserir(int rp, string produto, string preco)
        {
            String msg = "";
            try
            {
                SQLiteConnection con = pegarConexao();
                con.Open();
                string sql = $"insert into produtos(produto, preco) values('{produto}','{preco}')";

                SQLiteCommand cmd = new SQLiteCommand(sql, con);

                cmd.ExecuteNonQuery();
                con.Close();

                msg = "Produto cadastrado com sucesso!";
            }
            catch (Exception e)
            {


                msg = "Não foi possivel cadastrar o Produto! " + e.Message;
            }
            return Json(msg);
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

        [HttpGet]
        public IActionResult Alterar(int rp, String produto, String preco)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"UPDATE produtos set produto='{produto}',preco='{preco}' where rp={rp}";

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
        public IActionResult Excluir(int rp, String produto, String preco)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"delete from produtos where rp='{rp}'";

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