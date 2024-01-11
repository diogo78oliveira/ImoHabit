using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ImoHabit.Pages.Clientes
{
    public class QueryModel : PageModel
    {
        public List<QueryInfo> ListQuerys = new List<QueryInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ImoHabit;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Listar todos os clientes que fizeram propostas acima de 175.000,00 e a sua Localidade através do CódigoPostal
                    String sql = @"SELECT Cliente.CL_ID, Cliente.TC_ID, Cliente.CP, CódigoPostal.Localidade, Cliente.Nome, Cliente.Morada,
                            Cliente.Contacto, Cliente.DataNasc , ValorOferta
                                FROM Cliente
                                JOIN Proposta ON Cliente.CL_ID = Proposta.CL_ID
                                JOIN CódigoPostal ON Cliente.CP = CódigoPostal.CP
                                WHERE ValorOferta > 175000.00;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                QueryInfo queryInfo = new QueryInfo();
                                queryInfo.CL_ID = "" + reader.GetInt32(0);
                                queryInfo.TC_ID = "" + reader.GetInt32(1);
                                queryInfo.CP = reader.GetString(2);
                                queryInfo.Localidade = reader.GetString(3);
                                queryInfo.Nome = reader.GetString(4);
                                queryInfo.Morada = reader.GetString(5);
                                queryInfo.Contacto = reader.GetString(6);
                                queryInfo.DataNasc = reader.GetDateTime(7).ToString("dd-MM-yyyy");
                                queryInfo.ValorOferta = "" + reader.GetDecimal(8);

                                ListQuerys.Add(queryInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro:" + ex.ToString());
            }
        }
    }

    public class QueryInfo
    {
        public string CL_ID;
        public string TC_ID;
        public string CP;
        public string Nome;
        public string Morada;
        public string Contacto;
        public string DataNasc;
        public string ValorOferta;
        public string Localidade;
    }
}
