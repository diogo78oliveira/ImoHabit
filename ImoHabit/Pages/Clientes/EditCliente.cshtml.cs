using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ImoHabit.Pages.Clientes
{
    public class EditClienteModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public string MensagemErro = "";
        public string MensagemSucesso = "";
        public void OnGet()
        {
            string CL_ID = Request.Query["CL_ID"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ImoHabit;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Cliente WHERE CL_ID=@CL_ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CL_ID", CL_ID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clienteInfo.CL_ID = "" + reader.GetInt32(0);
                                clienteInfo.TC_ID = "" + reader.GetInt32(1);
                                clienteInfo.CP = reader.GetString(2);
                                clienteInfo.Nome = reader.GetString(3);
                                clienteInfo.Morada = reader.GetString(4);
                                clienteInfo.Contacto = reader.GetString(5);
                                clienteInfo.DataNasc = reader.GetDateTime(6).ToString("yyyy-MM-dd");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MensagemErro= ex.Message;   
            }
        }

        public void OnPost() 
        {
            clienteInfo.CL_ID = Request.Form["CL_ID"];
            clienteInfo.TC_ID = Request.Form["TC_ID"];
            clienteInfo.CP = Request.Form["CP"];
            clienteInfo.Nome = Request.Form["Nome"];
            clienteInfo.Morada = Request.Form["Morada"];
            clienteInfo.Contacto = Request.Form["Contacto"];
            clienteInfo.DataNasc = Request.Form["DataNasc"];

            if (clienteInfo.TC_ID.Length == 0
                || clienteInfo.CP.Length == 0 || clienteInfo.Nome.Length == 0
                || clienteInfo.Morada.Length == 0 || clienteInfo.Contacto.Length == 0 || clienteInfo.DataNasc.Length == 0
                )
            {
                MensagemErro = "Todos os campos têm de ser preenchidos";
                return;
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ImoHabit;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Cliente " +
                                 "SET CL_ID=@CL_ID, TC_ID=@TC_ID, CP=@CP, Nome=@Nome, Morada=@Morada, Contacto=@Contacto, DataNasc=@DataNasc " +
                                  "WHERE CL_ID=@CL_ID";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CL_ID", clienteInfo.CL_ID);
                        command.Parameters.AddWithValue("@TC_ID", clienteInfo.TC_ID);
                        command.Parameters.AddWithValue("@CP", clienteInfo.CP);
                        command.Parameters.AddWithValue("@Nome", clienteInfo.Nome);
                        command.Parameters.AddWithValue("@Morada", clienteInfo.Morada);
                        command.Parameters.AddWithValue("@Contacto", clienteInfo.Contacto);
                        command.Parameters.AddWithValue("@DataNasc", clienteInfo.DataNasc);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MensagemErro = ex.Message;
                return;
            }

             Response.Redirect("/Clientes/Index");
        }
    }
}
