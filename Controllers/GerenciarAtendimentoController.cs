using Microsoft.AspNetCore.Mvc;
using Sistema_Veterinário.Models;
using System.Data.SQLite;

namespace Sistema_Veterinário.Controllers
{
    public class GerenciarAtendimentoController : Controller
    {
        public IActionResult PageAtend()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inserir(int rc, string nome_res, string nome_pac, string especie, string detalhes, string email, int contato, string data)
        {
            String msg = "";
            try
            {
                SQLiteConnection con = pegarConexao();
                con.Open();
                string sql = $"insert into consulta(nome_res, nome_pac, especie, detalhes, email, contato, data) values('{nome_res}','{nome_pac}','{especie}','{detalhes}','{email}','{contato}','{data}')";

                SQLiteCommand cmd = new SQLiteCommand(sql, con);

                cmd.ExecuteNonQuery();
                con.Close();

                msg = "Consulta agendada com sucesso!";
            }
            catch (Exception e)
            {


                msg = "Não foi possivel agendar a consulta! " + e.Message;
            }
            return Json(msg);
        }
        [HttpGet]
        public IActionResult Consultar(int rc, string nome_res, string nome_pac, string especie, string detalhes, string email, int contato, string data)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"select * from consulta";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                SQLiteDataReader dr = comandoSQL.ExecuteReader();
                List<Consultas> listConsultas = new List<Consultas>();
                while (dr.Read())
                {
                    Consultas cons = new Consultas();
                    cons.Rc = dr.GetInt32(0);
                    cons.Nome_res = dr.GetString(1);
                    cons.Nome_pac = dr.GetString(2);
                    cons.Especie = dr.GetString(3);
                    cons.Detalhes = dr.GetString(4);
                    cons.Email = dr.GetString(5);
                    cons.Contato = dr.GetInt32(6);
                    cons.Data = dr.GetString(7);
                    listConsultas.Add(cons);
                }
                sqlite_conn.Close();
                return Json(listConsultas);
            }
            catch (Exception ex)
            {
                return Json("Não foi possível consultar!!!");
            }
        }

        [HttpGet]
        public IActionResult Alterar(int rc, string nome_res, string nome_pac, string especie, string detalhes, string email, int contato, string data)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"UPDATE consulta set nome_res='{nome_res}',nome_pac='{nome_pac}',especie='{especie}',detalhes='{detalhes}',email='{email}',contato='{contato}',data='{data}' where rc={rc}";

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
        public IActionResult Excluir(int rc, string nome_res, string nome_pac, string especie, string detalhes, string email, int contato, string data)
        {
            SQLiteConnection sqlite_conn;
            try
            {
                sqlite_conn = pegarConexao();
                sqlite_conn.Open();

                string sql = $"delete from consulta where rc='{rc}'";

                SQLiteCommand comandoSQL = new SQLiteCommand(sql, sqlite_conn);
                comandoSQL.ExecuteNonQuery();
                sqlite_conn.Close();
                return Json("Consulta excluída com sucesso!!!");
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
