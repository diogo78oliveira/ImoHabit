using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace ImoHabit.Pages.Clientes
{
    public class CreatClienteModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public string MensagemErro = "";
        public string MensagemSucesso = "";

        private static readonly Random random = new Random();

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ImoHabit;Integrated Security=True;Encrypt=False";

            try
            {
                clienteInfo.CL_ID = GenerateUniqueID(connectionString);

                clienteInfo.TC_ID = Request.Form["TC_ID"];
                clienteInfo.CP = Request.Form["CP"];
                clienteInfo.Nome = Request.Form["Nome"];
                clienteInfo.Morada = Request.Form["Morada"];
                clienteInfo.Contacto = Request.Form["Contacto"];
                clienteInfo.DataNasc = Request.Form["DataNasc"];

                if (clienteInfo.TC_ID.Length == 0
                    || clienteInfo.CP.Length == 0 || clienteInfo.Nome.Length == 0
                    || clienteInfo.Morada.Length == 0 || clienteInfo.Contacto.Length == 0 || clienteInfo.DataNasc.Length == 0)
                {
                    MensagemErro = "Todos os campos têm de ser preenchidos";
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Cliente" +
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

                clienteInfo.CL_ID = ""; clienteInfo.TC_ID = ""; clienteInfo.CP = ""; clienteInfo.Nome = "";
                clienteInfo.Morada = ""; clienteInfo.Contacto = ""; clienteInfo.DataNasc = "";
                MensagemSucesso = "Novo Cliente Adicionado";

                Response.Redirect("/Clientes/Index");
            }
            catch (Exception ex)
            {
                MensagemErro = ex.Message;
                return;
            }
        }

        private string GenerateUniqueID(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT CL_ID FROM Cliente";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                var existingIDs = new HashSet<string>();

                while (reader.Read())
                {
                    existingIDs.Add(reader["CL_ID"].ToString());
                }

                reader.Close();

                string newID = GenerateRandomNumber(10000, 99999).ToString();

                while (existingIDs.Contains(newID))
                {
                    newID = GenerateRandomNumber(10000, 99999).ToString();
                }

                return newID;
            }
        }

        private int GenerateRandomNumber(int min, int max)
        {
            lock (random)
            {
                return random.Next(min, max);
            }
        }
    }
}
