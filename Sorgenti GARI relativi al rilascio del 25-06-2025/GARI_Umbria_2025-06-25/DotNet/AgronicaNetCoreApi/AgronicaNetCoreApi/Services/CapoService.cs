namespace AgronicaNetCoreApi.Services
{
    public class CapoService
    {
        public CapoService(IServiceProvider provider)
        {

        }
        /*
        public List<GiacenzeZoo> CaricaGiacenzeZoo(long codiceStalla, DateTime dataRiferimento, string codCausale)
        {
            List<GiacenzeZoo> listaGiacenze = new List<GiacenzeZoo>();

            try
            {
                using (SqlConnection cn = new SqlConnection(Connessione))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_GiacenzeZoo", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodiceStalla", codiceStalla);
                        cmd.Parameters.AddWithValue("@DataRiferimento", dataRiferimento);
                        cmd.Parameters.AddWithValue("@CodCausale", codCausale);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var giacenza = new GiacenzeZoo
                                {
                                    ID = dr.GetInt32(dr.GetOrdinal("ID")),
                                    CodiceStalla = dr.GetInt64(dr.GetOrdinal("CodiceStalla")),
                                    DataRiferimento = dr.GetDateTime(dr.GetOrdinal("DataRiferimento")),
                                    CodCausale = dr.GetString(dr.GetOrdinal("CodCausale")),
                                    Quantita = dr.GetInt32(dr.GetOrdinal("Quantita"))
                                };
                                listaGiacenze.Add(giacenza);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
                // Puoi anche loggare l'errore o rilanciarlo
            }

            return listaGiacenze;
        }
    */
    }
}
