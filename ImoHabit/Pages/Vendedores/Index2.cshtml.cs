using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ImoHabit.Pages.Vendedores
{
    public class IndexModel2 : PageModel
    {
        public List<VendedorInfo> ListVendedores = new List<VendedorInfo>();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ImoHabit;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Vendedor";

                    
                    if (!string.IsNullOrEmpty(SearchName))
                    {
                        sql += $" WHERE Nome LIKE '%{SearchName}%'";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VendedorInfo vendedorInfo = new VendedorInfo();
                                vendedorInfo.V_ID = "" + reader.GetInt32(0);
                                vendedorInfo.Nome = reader.GetString(1);
                                vendedorInfo.DataNasc = reader.GetDateTime(2).ToString("dd-MM-yyyy");
                                ListVendedores.Add(vendedorInfo);
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

    public class VendedorInfo
    {
        public string V_ID;
        public string Nome;
        public string DataNasc;
    }
}
