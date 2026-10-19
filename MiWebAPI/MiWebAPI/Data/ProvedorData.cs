using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class ProvedorData
    {
        private readonly string conexion;

        public ProvedorData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        private static Provedor Mapear(SqlDataReader reader)
        {
            return new Provedor
            {
                Id = Convert.ToInt64(reader["id"]),
                Nombre = reader["nombre"] as string,
                ApellidoPaterno = reader["apellido_paterno"] as string,
                ApellidoMaterno = reader["apellido_materno"] as string,
                Rfc = reader["rfc"] as string,
                Correo = reader["correo"] as string,
                Credito = reader["credito"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["credito"]),
                NombreBanco = reader["nombre_banco"] as string,
                FechaUltimaModificacion = reader["fecha_ultima_modificacion"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["fecha_ultima_modificacion"])
            };
        }

        public async Task<List<Provedor>> Lista()
        {
            List<Provedor> lista = new List<Provedor>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PROVEDOR_SELECCIONA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(Mapear(reader));
                    }
                }
            }
            return lista;
        }

        public async Task<Provedor?> Obtener(long id)
        {
            Provedor? objeto = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PROVEDOR_SELECCIONA_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        objeto = Mapear(reader);
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Provedor objeto)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PROVEDOR_AGREGA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", (object?)objeto.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@apellido_paterno", (object?)objeto.ApellidoPaterno ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@apellido_materno", (object?)objeto.ApellidoMaterno ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@rfc", (object?)objeto.Rfc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@correo", (object?)objeto.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@credito", objeto.Credito);
                cmd.Parameters.AddWithValue("@nombre_banco", (object?)objeto.NombreBanco ?? DBNull.Value);
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Provedor objeto)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PROVEDOR_ACTUALIZA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", objeto.Id);
                cmd.Parameters.AddWithValue("@nombre", (object?)objeto.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@apellido_paterno", (object?)objeto.ApellidoPaterno ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@apellido_materno", (object?)objeto.ApellidoMaterno ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@rfc", (object?)objeto.Rfc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@correo", (object?)objeto.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@credito", objeto.Credito);
                cmd.Parameters.AddWithValue("@nombre_banco", (object?)objeto.NombreBanco ?? DBNull.Value);
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(long id)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PROVEDOR_ELIMINA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
