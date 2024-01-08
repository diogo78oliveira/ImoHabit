using ImoHabit.Pages.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace ImoHabit.Pages.Vendedores
{
    public class Contacto : PageModel
    {
        public List<ContactoInfo> ListContactos = new List<ContactoInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=dbImoHabit;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Contacto";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContactoInfo contactoInfo = new ContactoInfo();
                                contactoInfo.CT_ID = "" + reader.GetInt32(0);
                                contactoInfo.V_ID = "" + reader.GetInt32(1);
                                contactoInfo.TC_ID = "" + reader.GetInt32(2);
                                contactoInfo.ValorContacto = reader.GetString(3);
                                ListContactos.Add(contactoInfo);

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
    public class ContactoInfo
    {
        public string CT_ID;
        public string V_ID;
        public string TC_ID;
        public string ValorContacto;
    }
}
