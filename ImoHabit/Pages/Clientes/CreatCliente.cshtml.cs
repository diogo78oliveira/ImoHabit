using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ImoHabit.Pages.Clientes
{
    public class CreatClienteModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public string MensagemErro = "";
        public string MensagemSucesso = "";
        public void OnGet()
        {
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

            if (clienteInfo.CL_ID.Length == 0 || clienteInfo.TC_ID.Length == 0 
                || clienteInfo.CP.Length == 0 || clienteInfo.Nome.Length == 0 
                || clienteInfo.Morada.Length == 0 || clienteInfo.Contacto.Length == 0 || clienteInfo.DataNasc.Length == 0
                )
            {
                MensagemErro = "Todos os campos têm de ser preenchidos";
                return;
            }

            //guardar cliente na base de dados

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=dbImoHabit;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Cliente" +
                                 "(CL_ID, TC_ID, CP, Nome, Morada, Contacto, DataNasc) VALUES " +
                                 "(@CL_ID, @TC_ID, @CP, @Nome, @Morada, @Contacto, @DataNasc);";

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
            catch ( Exception ex )
            {
                MensagemErro= ex.Message;
                return;
            }

            clienteInfo.CL_ID = ""; clienteInfo.TC_ID = ""; clienteInfo.CP = ""; clienteInfo.Nome = "";
            clienteInfo.Morada = ""; clienteInfo.Contacto = ""; clienteInfo.DataNasc = "";
            MensagemSucesso = "Novo Cliente Adicionado";

            Response.Redirect("/Clientes/Index");

        }
    }
}
