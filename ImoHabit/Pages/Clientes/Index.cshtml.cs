using ImoHabit.Pages.Vendedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace ImoHabit.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        public List<ClienteInfo> ListClientes = new List<ClienteInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=dbImoHabit;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Cliente";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteInfo clienteInfo = new ClienteInfo();
                                clienteInfo.CL_ID = "" + reader.GetInt32(0);
                                clienteInfo.TC_ID = "" + reader.GetInt32(1);
                                clienteInfo.CP = reader.GetString(2);
                                clienteInfo.Nome = reader.GetString(3);
                                clienteInfo.Morada = reader.GetString(4);
                                clienteInfo.Contacto = reader.GetString(5);
                                clienteInfo.DataNasc = reader.GetDateTime(6).ToString("dd-MM-yyyy");                 

                                ListClientes.Add(clienteInfo);


                            }
                        }
                    }
                }
            }
            catch  (Exception ex)
            {
                Console.WriteLine("Erro:" + ex.ToString());
            }
        }
    }
    public class ClienteInfo
    {
        public string CL_ID;
        public string TC_ID;
        public string CP;
        public string Nome;
        public string Morada;
        public string Contacto;
        public string DataNasc;
    }
}
