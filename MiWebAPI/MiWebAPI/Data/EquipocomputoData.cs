using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class EquipoComputoData
    {
        private readonly string conexion;

        public EquipoComputoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        private static EquipoComputo Mapear(SqlDataReader reader)
        {
            return new EquipoComputo
            {
                IdEquipo = Convert.ToInt64(reader["ID_EQUIPO"]),
                NumeroSerie = reader["NUMERO_SERIE"] as string,
                Marca = reader["MARCA"] as string,
                Modelo = reader["MODELO"] as string,
                Procesador = reader["PROCESADOR"] as string,
                MemoriaRamGb = reader["MEMORIA_RAMGB"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MEMORIA_RAMGB"]),
                Activo = reader["ACTIVO"] != DBNull.Value && Convert.ToBoolean(reader["ACTIVO"]),
                FechaRegistro = Convert.ToDateTime(reader["FECHA_REGISTRO"]),
                FechaActualizacion = reader["FECHA_ACTUALIZACION"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["FECHA_ACTUALIZACION"])
            };
        }

        public async Task<List<EquipoComputo>> Lista()
        {
            List<EquipoComputo> lista = new List<EquipoComputo>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.EQUIPO_COMPUTO_CONSULTA", con);
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

        public async Task<EquipoComputo?> Obtener(long idEquipo)
        {
            EquipoComputo? objeto = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.EQUIPO_COMPUTO_CONSULTA_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_equipo", idEquipo);

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

        public async Task<bool> Crear(EquipoComputo objeto)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.EQUIPO_COMPUTO_INSERTAR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numero_serie", (object?)objeto.NumeroSerie ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@marca", (object?)objeto.Marca ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modelo", (object?)objeto.Modelo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@procesador", (object?)objeto.Procesador ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@memoria_ramgb", objeto.MemoriaRamGb);
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

        public async Task<bool> Editar(EquipoComputo objeto)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.EQUIPO_COMPUTO_ACTUALIZAR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_equipo", objeto.IdEquipo);
                cmd.Parameters.AddWithValue("@numero_serie", (object?)objeto.NumeroSerie ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@marca", (object?)objeto.Marca ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modelo", (object?)objeto.Modelo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@procesador", (object?)objeto.Procesador ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@memoria_ramgb", objeto.MemoriaRamGb);
                cmd.Parameters.AddWithValue("@activo", objeto.Activo);
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

        public async Task<bool> Eliminar(long idEquipo)
        {
            bool respuesta;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.EQUIPO_COMPUTO_ELIMINAR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_equipo", idEquipo);
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