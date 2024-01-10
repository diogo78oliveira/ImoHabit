using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;


namespace ImoHabit.Pages.Vendedores
{
    public class CreateVendedorModel : PageModel
    {
        public VendedorInfo vendedorInfo = new VendedorInfo();
        public string MensagemErro = "";
        public string MensagemSucesso = "";
        public ContactoInfo contactoInfo = new ContactoInfo();

        private static readonly Random random = new Random();

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=dbImoHabit;Integrated Security=True;Encrypt=False";

            try
            {
                vendedorInfo.V_ID = GenerateUniqueID(connectionString);

                vendedorInfo.Nome = Request.Form["Nome"];
                vendedorInfo.DataNasc = Request.Form["DataNasc"];

                if (vendedorInfo.Nome.Length == 0 || vendedorInfo.DataNasc.Length == 0)
                {
                    MensagemErro = "Todos os campos têm de ser preenchidos";
                    return;
                }

                InsertVendedor(connectionString);

                MensagemSucesso = "Novo Vendedor Adicionado";

                InsertContacto(connectionString, vendedorInfo.V_ID);
            }
            catch (Exception ex)
            {
                MensagemErro = ex.Message;
                return;
            }

            Response.Redirect("/Vendedores/Index2");
        }

        public void InsertVendedor(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Vendedor" +
                             "(V_ID, Nome, DataNasc) VALUES " +
                             "(@V_ID, @Nome, @DataNasc);";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@V_ID", vendedorInfo.V_ID);
                    command.Parameters.AddWithValue("@Nome", vendedorInfo.Nome);
                    command.Parameters.AddWithValue("@DataNasc", vendedorInfo.DataNasc);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertContacto(string connectionString, string vendedorID)
        {
            try
            {
                contactoInfo.CT_ID = GenerateUniqueContactID(connectionString);

                contactoInfo.V_ID = vendedorID;
                contactoInfo.TC_ID = Request.Form["TC_ID"];
                contactoInfo.ValorContacto = Request.Form["ValorContacto"];

                if (contactoInfo.TC_ID.Length == 0 || contactoInfo.ValorContacto.Length == 0)
                {
                    MensagemErro = "Todos os campos têm de ser preenchidos";
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Contacto" +
                                 "(CT_ID, V_ID, TC_ID, ValorContacto) VALUES " +
                                 "(@CT_ID, @V_ID, @TC_ID, @ValorContacto);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CT_ID", contactoInfo.CT_ID);
                        command.Parameters.AddWithValue("@V_ID", contactoInfo.V_ID);
                        command.Parameters.AddWithValue("@TC_ID", contactoInfo.TC_ID);
                        command.Parameters.AddWithValue("@ValorContacto", contactoInfo.ValorContacto);
                        command.ExecuteNonQuery();
                    }
                }

                contactoInfo.CT_ID = "";
                contactoInfo.TC_ID = "";
                contactoInfo.ValorContacto = "";
                MensagemSucesso = "Novo Contato Adicionado";
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
                string sql = "SELECT V_ID FROM Vendedor";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                var existingIDs = new HashSet<string>();

                while (reader.Read())
                {
                    existingIDs.Add(reader["V_ID"].ToString());
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

        private string GenerateUniqueContactID(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT CT_ID FROM Contacto";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                var existingContactIDs = new HashSet<string>();

                while (reader.Read())
                {
                    existingContactIDs.Add(reader["CT_ID"].ToString());
                }

                reader.Close();

                string newContactID = GenerateRandomNumber(100, 999).ToString();

                while (existingContactIDs.Contains(newContactID))
                {
                    newContactID = GenerateRandomNumber(100, 999).ToString();
                }

                return newContactID;
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
