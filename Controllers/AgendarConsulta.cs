using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace Sistema_Veterinário.Controllers
{
    public class AgendarConsulta : Controller
    {
        public IActionResult PageAgenCon()
        {
            return View();
        }

        [HttpGet]
        public IActionResult agendar(string nome_res, string nome_pac, string especie, string detalhes, string email, int contato, string data)
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
        public SQLiteConnection pegarConexao()
        {
            String stringConnection = "Data Source=veterinario.db; Version = 3; New = True; Compress = True; ";
            return new SQLiteConnection(stringConnection);
        }
    }
}
