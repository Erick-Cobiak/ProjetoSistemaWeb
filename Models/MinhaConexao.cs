using System.Data.SQLite;

namespace Sistema_Veterinário.Models
{
    public static class Conexao
    {
        public static SQLiteConnection Nova()
        {
            String stringConnection = "Data Source=veterinario.db; Version = 3; New = True; Compress = True; ";
            return new SQLiteConnection(stringConnection);
        }
    }
}
