using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class ProductoData
    {
        private readonly string conexion;

        public ProductoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<ProductoDetalle>> Lista()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_LISTAR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new ProductoDetalle
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }


        public async Task<ProductoDetalle?> Obtener(int id)
        {
            ProductoDetalle? objeto = null;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_OBTENER", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new ProductoDetalle
                        {
                            Id = Convert.ToInt64(reader["ID"]),
                            NombreProducto = reader["NOMBRE_PRODUCTO"] as string,
                            Precio = reader["PRECIO"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PRECIO"]),
                            Cantidad = reader["CANTIDAD"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CANTIDAD"]),
                            Total = reader["TOTAL"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TOTAL"]),
                            Descripcion = reader["DESCRIPCION"] as string,
                            FechaUltimaModificacion = reader["FECHA_ULTIMA_MODIFICACION"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["FECHA_ULTIMA_MODIFICACION"])
                        };
                    }
                }
            }
            return objeto; // null si no existe
        }

        public async Task<bool> Crear(ProductoDetalle objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_AGREGAR", con);
                cmd.Parameters.AddWithValue("@NombreProducto", (object?)objeto.NombreProducto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@Total", objeto.Total);
                cmd.Parameters.AddWithValue("@Descripcion", (object?)objeto.Descripcion ?? DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task<bool> Editar(ProductoDetalle objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_EDITAR", con);
                cmd.Parameters.AddWithValue("@id", objeto.Id);
                cmd.Parameters.AddWithValue("@nombreProducto", (object?)objeto.NombreProducto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@total", objeto.Total);
                cmd.Parameters.AddWithValue("@descripcion", (object?)objeto.Descripcion ?? DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_ELIMINAR", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
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